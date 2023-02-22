using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class RaccoonHoldingsFactory : HoldingsAirdropFactory
    {
        public RaccoonHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 1022062201;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "6CNV3Q2CZM75OCVOCI32MVFCTL3ZY6ZXUKQBCVPE4NXMPMQQBCXHWUAD7M" };
            this.AssetValue = 20;
        }
    }
}
