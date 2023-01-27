using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class GooseHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public GooseHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 751294723;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "GOOSE4NW53JZLCG6NX37WWPUAOOYSAHPVPPEMBI7FGZZBZ4OET27JV4O3U",
                "GOOSECHXVEKJ4SO43NTW5HXOIGLFGC2SQDAVWQGJCN576ODJ5SECV6MUOM",
                "GOOSE7PN4S366W5LLQ3TRO4BCB2C66VBSMMXRVAWAPGZJHJYR34VNK2AU4",
                "GOOSEOQSO2XM54KNCN2ESH3A7VCHRFSCFMACE24QLBUGCR256JOGRCYSSI",
                "GOOSEKPIKOZPEPBMFO7TRRR2EPXLWKOIBLKXJKXWMK2J56SOXWRC3FLNSU" };
            this.AssetValue = 6;

            this.SearchAlandia = true;
        }
    }
}
