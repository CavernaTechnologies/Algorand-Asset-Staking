using System;
using System.Collections.Generic;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class MagoHoldingsFactory : HoldingsAirdropFactory
    {
        public MagoHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 982060282;
            this.Decimals = 2;
            this.CreatorAddresses = new string[]
            {
                "MAGOFPV7QCCUNUVVQB656QADKX56XZANO4HP6GENZHIGKVOI6I5QYU3XU4"
            };
            this.RevokedAddresses = new string[]
            {
                "MAGOFPV7QCCUNUVVQB656QADKX56XZANO4HP6GENZHIGKVOI6I5QYU3XU4"
            };
            this.RevokedAssets = new ulong[] { };
            this.AssetValue = 400;
        }
    }
}
