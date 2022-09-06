using System;
using System.Collections.Generic;
using System.Text;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Random
{
    public class PixRandomFactory : RandomAirdropFactory
    {
        public PixRandomFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 226265212;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "PIXSQBMSKAK4ZB4SEZJEOOMB72RI75CNW2DCQ3BCFHQWK3SIQYUB2G5HGA" };
            this.NumberOfWinners = 200;
            this.TotalWinnings = 250000;
        }
    }
}
