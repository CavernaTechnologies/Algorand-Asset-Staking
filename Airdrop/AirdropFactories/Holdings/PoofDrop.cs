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
                "GEN3ZPDZQ32IZTTKBKSGTOJZODD2XXTGZ2I3NCRFJWKS2U2DVMYR5EKT2Q",
                "GNP7NPQR23MAPPJYM37ET7U2ZQLQZ4OC4ANMTF2VXATIVNEF5XWW4WUFAY",
                "NTAFYECB4OSPIGLFCNTXHQXEZ3EV2MOR6Z5HLEOCVGXRT75YBME5KLFPDI",
                "UN7I77ZLE4STXMHP4BCDWSBTS4XPB5YDU33CYQ6UNWLWEB6ZXZZ5ZF3BWU",
                "WINDLCAIJNVVFS62RAR5VKCXDAMOBDBX7PTFPBER3NQVURDNVUJABEHZ34"
            };
            this.RevokedAssets = new ulong[] {};
            this.RevokedAddresses = new string[]
            {
                "LUNDIW5HP5KWUG6JTXHMDB5XPNXQ43OIBXET3RTBXNVJISQUTBKWONTGNI",
                "UB6E3ACF7MHSAY5LRRNUHMW55IAQGGWOVADTVDNMF7JL7CA25GO7V4F2MI"
            };
            this.AssetValue = 5;
        }
    }
}
