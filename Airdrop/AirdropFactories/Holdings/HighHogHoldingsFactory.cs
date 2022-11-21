using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class HighHogHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public HighHogHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 844772414;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "25YRTO46IOKSN66T3GTSFUUYXS7HDCFA4DRKUYZ6RDNKYXUDZ5GYARZ4QE" };
            this.AssetValue = 5000;
            this.SearchRand = true;
            this.SearchAlandia = true;
            this.SearchAlgox= true;
            this.AlgoxCollectionNames = new string[]
            {
                "high-hogs-reborn",
            };
        }
    }
}
