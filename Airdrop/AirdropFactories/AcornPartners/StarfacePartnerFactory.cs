﻿using Algorand.V2.Indexer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.AcornPartners
{
    public class StarfacePartnerFactory : AcornPartner
    {
        public StarfacePartnerFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ulong totalWinnings) : base(indexerUtils, algodUtils)
        {
            this.CreatorAddresses = new string[] {
                "63CO7UJGTZIRIY6GDJRUEB7UYCH6BDVROMM3M5RPKST2PRET3WTJIR2VXY"};
            this.NumberOfWinners = 5;
            this.TotalWinnings = totalWinnings;
        }
    }
}
