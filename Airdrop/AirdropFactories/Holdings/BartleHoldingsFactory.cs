using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class BartleHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public BartleHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 829992960;
            this.Decimals = 6;
            this.CreatorAddresses = new string[] {
                "SSPOU535FKPYNKR3OYYVU67EPWO7AEEA55WOG3AIWH2KEXEMJ4R3RUKYJQ",
            };
            this.RevokedAssets = new ulong[]
            {
            };
            this.RevokedAddresses = new string[]
            {
                "SSPOU535FKPYNKR3OYYVU67EPWO7AEEA55WOG3AIWH2KEXEMJ4R3RUKYJQ"
            };
            this.AssetValue = 1000_000_000;
            this.SearchAlandia = true;
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[] { "bartle-doo-derp-squad" };
        }
    }
}
