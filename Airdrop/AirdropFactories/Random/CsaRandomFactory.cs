using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Random
{
    public class CsaRandomFactory : RandomAirdropFactory
    {
        public CsaRandomFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) :base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 226265212;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "ACORNOX3WJKR2GX63IJGPC2MV6E7GRNBBEE22BXQ6H7HFMEXLUL3RSEBLY" };
            this.NumberOfWinners = 10;
            this.TotalWinnings = 250000;
        }
    }
}
