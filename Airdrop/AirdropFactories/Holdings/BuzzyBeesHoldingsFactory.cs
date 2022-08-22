using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
            };
            this.RevokedAssets = new ulong[]
            {
            };
            this.RevokedAddresses = new string[]
            {
                "HNYWPQPSKMYGVOY2HTZQGMD5JUANMVGIOFPALVZCC6YGFHOWAYWQCC2AJE"
            };
            this.AssetValue = 10;
            this.SearchAlandia = true;
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[] { "buzzy-bees" };
        }
    }
}
