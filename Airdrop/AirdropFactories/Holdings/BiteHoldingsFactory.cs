using Algorand.V2.Algod.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class BiteHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public BiteHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 875415272;
            this.Decimals = 0;
            this.CreatorAddresses = new string[]
            {
                "GNP7NPQR23MAPPJYM37ET7U2ZQLQZ4OC4ANMTF2VXATIVNEF5XWW4WUFAY",
                "GNF5FYIJBZX7U7PHGOVLOM7R6GGV77QX3AJYJVWHHQJBIUUXQBQGNNARKQ",
            };
            this.SearchAlgox = true;
            this.SearchRand = true;
            this.SearchAlandia = true;
            this.AlgoxCollectionNames = new string[]
            {
                "goodnight-family",
                "pop-y-collection"
            };
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, 333);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, 333);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, 666);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, 666);
                    }
                }
            }

            return assetValues;
        }
    }
}
