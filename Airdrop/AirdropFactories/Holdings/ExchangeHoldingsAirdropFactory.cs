using Algorand.V2.Algod.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public abstract class ExchangeHoldingsAirdropFactory : HoldingsAirdropFactory
    {
        public HttpClient HttpClient { get; }
        public bool SearchRand { get; set; }
        public bool SearchAlgox { get; set; }
        public bool SearchAlandia { get; set; }
        public string[] AlgoxCollectionNames { get; set; }
        private string randToken;

        public ExchangeHoldingsAirdropFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, HttpClient httpClient) : base(indexerUtils, algodUtils)
        {
            this.HttpClient = httpClient;
            this.HttpClient.DefaultRequestHeaders.Add("user-agent", "cavernatech/1.0.0");
            this.randToken = config["randToken"];
        }

        public override async Task<IEnumerable<AirdropUnitCollection>> FetchAirdropUnitCollections()
        {
            IDictionary<ulong, ulong> assetValues = await FetchAssetValues();
            IEnumerable<Account> accounts = await FetchAccounts();
            IDictionary<string, List<(ulong, ulong)>> randAccounts = null; 
            IDictionary<string, List<(ulong, ulong)>> algoxAccounts = null;
            IDictionary<string, List<(ulong, ulong)>> alandiaAccounts = null;
            if (SearchRand)
            {
                randAccounts = await FetchRandAccounts();
            }
            if (SearchAlgox)
            {
                algoxAccounts = await FetchAlgoxAccounts();
            }
            if (SearchAlandia)
            {
                alandiaAccounts = await FetchAlandiaAccounts();
            }

            AirdropUnitCollectionManager collectionManager = new AirdropUnitCollectionManager();

            Parallel.ForEach(accounts, new ParallelOptions { MaxDegreeOfParallelism = 10 }, account =>
            {
                AddAssetsInAccount(collectionManager, account, assetValues);

                string address = account.Address;

                if (SearchRand && randAccounts != null && randAccounts.ContainsKey(address))
                {
                    AddAssetsInList(collectionManager, address, randAccounts[address], assetValues);
                }
                if (SearchAlgox && algoxAccounts != null && algoxAccounts.ContainsKey(address))
                {
                    AddAssetsInList(collectionManager, address, algoxAccounts[address], assetValues);
                }
                if (SearchAlandia && alandiaAccounts != null && alandiaAccounts.ContainsKey(address))
                {
                    AddAssetsInList(collectionManager, address, alandiaAccounts[address], assetValues);
                }
            });

            return collectionManager.GetAirdropUnitCollections();
        }

        public void AddAssetsInList(AirdropUnitCollectionManager collectionManager, string address, IEnumerable<(ulong, ulong)> sourceAssets, IDictionary<ulong, ulong> assetValues)
        {
            if (sourceAssets != null)
            {
                foreach ((ulong, ulong) sourceAssetInfo in sourceAssets)
                {
                    ulong sourceAssetId = sourceAssetInfo.Item1;
                    ulong numberOfSourceAsset = sourceAssetInfo.Item2;

                    if (assetValues.ContainsKey(sourceAssetId))
                    {
                        ulong sourceAssetValue = assetValues[sourceAssetId];
                        collectionManager.AddAirdropUnit(new AirdropUnit(
                            address,
                            this.DropAssetId,
                            sourceAssetId,
                            sourceAssetValue,
                            numberOfSourceAsset: numberOfSourceAsset,
                            isMultiplied: true));
                    }
                }
            }
        }

        public async Task<Dictionary<string, List<(ulong, ulong)>>> FetchAlandiaAccounts()
        {
            Dictionary<string, List<(ulong, ulong)>> alandiaBorrowers = new Dictionary<string, List<(ulong, ulong)>>();

            foreach (string creatorAddress in this.CreatorAddresses)
            {
                string alandiaEndpoint = "https://alandia.io/api/cached_asset/creator/" + creatorAddress;

                string jsonResponse = await HttpClient.GetStringAsync(alandiaEndpoint);
                List<AlandiaListing> listings = JsonConvert.DeserializeObject<List<AlandiaListing>>(jsonResponse);

                foreach (AlandiaListing listing in listings)
                {
                    if (alandiaBorrowers.ContainsKey(listing.SellerAddress))
                    {
                        alandiaBorrowers[listing.SellerAddress].Add((listing.AssetId, 1));
                    }
                    else
                    {
                        alandiaBorrowers[listing.SellerAddress] = new List<(ulong, ulong)>() { (listing.AssetId, 1) };
                    }
                }
            }

            return alandiaBorrowers;
        }


        public async Task<Dictionary<string, List<(ulong, ulong)>>> FetchRandAccounts()
        {
            Dictionary<string, List<(ulong, ulong)>> randSellers = new Dictionary<string, List<(ulong, ulong)>>();

            foreach (string creatorAddress in this.CreatorAddresses)
            {
                string randEndpoint = "https://www.randswap.com/v1/listings/creator/" + creatorAddress + "?token=" + randToken;

                string jsonResponse = await HttpClient.GetStringAsync(randEndpoint);
                List<RandListing> listings = JsonConvert.DeserializeObject<List<RandListing>>(jsonResponse);

                foreach (RandListing listing in listings)
                {
                    if (randSellers.ContainsKey(listing.SellerAddress))
                    {
                        randSellers[listing.SellerAddress].Add((listing.AssetId, 1));
                    }
                    else
                    {
                        randSellers[listing.SellerAddress] = new List<(ulong, ulong)>() { (listing.AssetId, 1) };
                    }
                }
            }

            return randSellers;
        }

        public async Task<Dictionary<string, List<(ulong, ulong)>>> FetchAlgoxAccounts()
        {
            Dictionary<string, List<(ulong, ulong)>> sellers = new Dictionary<string, List<(ulong, ulong)>>();

            foreach (string collection in this.AlgoxCollectionNames)
            {
                string endpoint = "https://api.algoxnft.com/v1/collections/" + collection + "/buy-it-now-listings";

                string jsonResponse = await HttpClient.GetStringAsync(endpoint);
                List<AlgoxListing> listings = JsonConvert.DeserializeObject<List<AlgoxListing>>(jsonResponse);

                foreach (AlgoxListing listing in listings)
                {
                    if (sellers.ContainsKey(listing.SellerAddress))
                    {
                        sellers[listing.SellerAddress].Add((listing.AssetId, 1));
                    }
                    else
                    {
                        sellers[listing.SellerAddress] = new List<(ulong, ulong)>() { (listing.AssetId, 1) };
                    }
                }
            }

            return sellers;
        }
    }

    class AlandiaListing
    {
        [JsonProperty("asset_id")]
        public ulong AssetId { get; set; }
        [JsonProperty("wallet")]
        public string SellerAddress { get; set; }
    }

    class RandListing
    {
        [JsonProperty("assetId")]
        public ulong AssetId { get; set; }
        [JsonProperty("sellerAddress")]
        public string SellerAddress { get; set; }
    }

    class AlgoxListing
    {
        [JsonProperty("asset_id")]
        public ulong AssetId { get; set; }
        [JsonProperty("seller_wallet_address")]
        public string SellerAddress { get; set; }
    }
}
