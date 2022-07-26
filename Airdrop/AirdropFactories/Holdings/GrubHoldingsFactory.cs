using Algorand.V2.Algod.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Cosmos;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class GrubHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        private readonly ICosmos cosmos;

        public GrubHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ICosmos cosmos, IConfiguration config, IHttpClientFactory httpClient) : base(indexerUtils, algodUtils, config, httpClient.CreateClient())
        {
            this.cosmos = cosmos;
            this.DropAssetId = 787168529;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "PXGKTWC67EQAJJZX2J22WU6PWUGJGOXNF3Q3KDABHLXTP6P336G4CMF2WI",
                "PXGKO3GQXYKAVNKOFKFIMZLZJE3I3VHYALIU7MHT3CELM4I4JJC4XVNY7M",
                "WISEUJP5PIFAOWYDEVIDWQLKEQFTOSG3ADHE7NJYEAQNFXV5I2R7YLLBQQ",
                "DDYUZA46ZXAJICQMSYIKG3SWNKZND6C5VEH57YVJAV47NDYM3BMDKAC3LQ",
            };
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[]
            {
                "pixel-geko",
                "gekofam",
                "99-wise-uncles",
            };
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            ulong pixValue = 36;

            foreach (string creatorAddress in this.CreatorAddresses.Take(2))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, pixValue);
                }
            }

            ulong wuValue = 69;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(2).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, wuValue);
                }
            }

            IEnumerable<AssetValue> values = await cosmos.GetAssetValues("Gekofam");

            foreach (AssetValue av in values)
            {
                assetValues.Add(av.AssetId, av.Value);
            }

            return assetValues;
        }
    }
}
