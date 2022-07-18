﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class PyreneesHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public PyreneesHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 765722712;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "IEQGNR7DPQ26ZY7A22VHRTLHDL2PZ3XD5AUMMEEPYB6DUL6FSRN2NUUWPE" };
            this.RevokedAddresses = new string[] { "IEQGNR7DPQ26ZY7A22VHRTLHDL2PZ3XD5AUMMEEPYB6DUL6FSRN2NUUWPE" };
            this.RevokedAssets = new ulong[] { };
            this.AssetValue = 1;
            //this.SearchRand = true;
        }
    }
}
