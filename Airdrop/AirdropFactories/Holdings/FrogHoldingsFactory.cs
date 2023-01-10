using System;
using System.Collections.Generic;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class FrogHoldingsFactory : HoldingsAirdropFactory
    {
        public FrogHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 973917383;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "ITWKSVOUWA64AW7AEYB6QIYVYRP7I75QDRNVUNXO7PIK4AIHDXGYILRO54" };
            this.RevokedAddresses = new string[] { "ITWKSVOUWA64AW7AEYB6QIYVYRP7I75QDRNVUNXO7PIK4AIHDXGYILRO54" };
            this.RevokedAssets = new ulong[] { };
            this.AssetValue = 4;
        }
    }
}
