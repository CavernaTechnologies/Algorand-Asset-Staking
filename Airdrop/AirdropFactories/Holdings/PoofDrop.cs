using System;
using System.Collections.Generic;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class PoofDrop : HoldingsAirdropFactory
    {
        public PoofDrop(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 658399558;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "CONEZ362OXBZTZJNAZVPME4JNH77TAZ74LAJGSIEKYISOX3DH5MVYAQUF4",
                "OT6BNH6RTODT2ZMRIJOXTGPRQ4YUSTRSOZSPSQ2XDSLR4IBBZRZPADROCI",
            };
            this.RevokedAssets = new ulong[] {};
            this.RevokedAddresses = new string[]
            {
                "LUNDIW5HP5KWUG6JTXHMDB5XPNXQ43OIBXET3RTBXNVJISQUTBKWONTGNI",
                "CONEZ362OXBZTZJNAZVPME4JNH77TAZ74LAJGSIEKYISOX3DH5MVYAQUF4",
                "OT6BNH6RTODT2ZMRIJOXTGPRQ4YUSTRSOZSPSQ2XDSLR4IBBZRZPADROCI"
            };
            this.AssetValue = 5;
        }
    }
}
