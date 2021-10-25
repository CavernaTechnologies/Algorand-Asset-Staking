﻿using Algorand.V2.Model;
using System;
using System.Collections.Generic;

namespace Airdrop
{
    public abstract class AirdropFactory
    {
        public long AssetId { get; set; }
        public long Decimals { get; set; }

        public AirdropFactory(long assetId, long decimals = 0)
        {
            AssetId = assetId;
            Decimals = decimals;
        }

        public abstract IDictionary<long, long> GetAssetValues();
        public abstract IEnumerable<string> FetchWalletAddresses();
        public abstract long GetAssetHoldingsAmount(IEnumerable<AssetHolding> assetHoldings, IDictionary<long, long> assetValues);
        public abstract IEnumerable<AirdropAmount> FetchAirdropAmounts(IDictionary<long, long> assetValues);
    }
}
