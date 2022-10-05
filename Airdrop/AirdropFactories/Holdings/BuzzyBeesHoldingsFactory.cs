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
    public class BuzzyBeesHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public BuzzyBeesHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 790791967;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "BEEE3WGLXN6QD62D3LVB67DF5LESMIS6FVD4QTMOV3325OM24CDQKBWI6U",
                "HKBG45ZMBYAJPDRI4GZJI4HWB7ZEKPHH6EETDQ563KTMNUTOGTR7CZZPEI"
            };
            this.RevokedAssets = new ulong[]
            {
            };
            this.RevokedAddresses = new string[]
            {
                "HNYWPQPSKMYGVOY2HTZQGMD5JUANMVGIOFPALVZCC6YGFHOWAYWQCC2AJE",
                "HKBG45ZMBYAJPDRI4GZJI4HWB7ZEKPHH6EETDQ563KTMNUTOGTR7CZZPEI"
            };
            this.AssetValue = 10;
            this.SearchAlandia = true;
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[] { "buzzy-bees", "buzzy-bees-hive-keepers" };
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, this.AssetValue);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 2);
                }
            }

            return assetValues;
        }
    }
}
