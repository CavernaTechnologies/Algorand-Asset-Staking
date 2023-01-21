using System;
using System.Collections.Generic;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class BeaverHoldingsFactory : HoldingsAirdropFactory
    {
        public BeaverHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 1013257855;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "QPXO7WASOG52K5LQ6UVA4R3ZWEBAZ3QEF6YJOICEGW3WVICY52FYVS7J6Y" };
            this.RevokedAddresses = new string[] { "QPXO7WASOG52K5LQ6UVA4R3ZWEBAZ3QEF6YJOICEGW3WVICY52FYVS7J6Y" };
            this.RevokedAssets = new ulong[] { };
            this.AssetValue = 10;
        }
    }
}
