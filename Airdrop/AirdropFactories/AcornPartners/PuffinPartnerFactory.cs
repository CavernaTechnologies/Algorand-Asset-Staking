using Algorand.V2.Indexer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.AcornPartners
{
    public class PuffinPartnerFactory : AcornPartner
    {
        public PuffinPartnerFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ulong totalWinnings) : base(indexerUtils, algodUtils)
        {
            this.CreatorAddresses = new string[] {
                "PUFFS6OCX2BNOWY3BK6K6SY2Z7M7AFR2EC4PP6YOSATRXPF2DAC76DSLAI",
                "PUFFNIY3XM2D445BNI5WNKZCKRA5OU3Y7FODNHJTYTIRGWSDA3J3MNZZBI",
                "VX7HS4LUA6HOAMOIK4J2Z2MWJPLDADBXVLXHOVOPFO2CGXBIP2LLXZC4R4",
                "X5GD3CDDSXZCIAPKKXQOWDTHDN32NP6KAZOZXFFRE625TPUND33ZCOIJCM"};
            this.NumberOfWinners = 250;
            this.TotalWinnings = totalWinnings;
            this.RevokedAddresses = new string[] {
                "PUFFS6OCX2BNOWY3BK6K6SY2Z7M7AFR2EC4PP6YOSATRXPF2DAC76DSLAI",
                "PUFFNIY3XM2D445BNI5WNKZCKRA5OU3Y7FODNHJTYTIRGWSDA3J3MNZZBI",
                "VX7HS4LUA6HOAMOIK4J2Z2MWJPLDADBXVLXHOVOPFO2CGXBIP2LLXZC4R4",
                "X5GD3CDDSXZCIAPKKXQOWDTHDN32NP6KAZOZXFFRE625TPUND33ZCOIJCM"
            };
        }
    }
}
