using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class BallstarHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public BallstarHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 913136336;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "CDTWDUSOWMATKF4ZJEHPCVHR2F2LABO44YV36IE5UERRLUQGI2QF2N7J5Q" };
            this.RevokedAddresses = new string[] { "CDTWDUSOWMATKF4ZJEHPCVHR2F2LABO44YV36IE5UERRLUQGI2QF2N7J5Q" };
            this.RevokedAssets = new ulong[] { };
            this.SearchAlandia = true;
            this.SearchRand = true;
            this.AssetValue = 1000;
        }
    }
}
