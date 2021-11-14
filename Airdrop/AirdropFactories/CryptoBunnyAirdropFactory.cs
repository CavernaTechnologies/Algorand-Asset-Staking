﻿using Algorand.V2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Util;
using Util.Cosmos;

namespace Airdrop.AirdropFactories
{
    public class CryptoBunnyAirdropFactory : IAirdropFactory
    {
        public long AssetId { get; set; }
        public long Decimals { get; set; }
        private readonly IAlgoApi api;
        private readonly ICosmos cosmos;

        public CryptoBunnyAirdropFactory(IAlgoApi api, ICosmos cosmos)
        {
            this.AssetId = 329532956;
            this.Decimals = 0;
            this.api = api;
            this.cosmos = cosmos;
        }
        public IEnumerable<RetrievedAsset> CheckAssets()
        {
            IEnumerable<AssetHolding> assetHoldings = this.api.GetAssetsByAddress("BNYSETPFTL2657B5RCSW64A3M766GYBVRV5ALOM7F7LIRUZKBEOGF6YSO4");

            List<long> assetIds = assetHoldings.ToList().ConvertAll<long>(ah => ah.AssetId.Value);
            IEnumerable<Asset> assets = this.api.GetAssetById(assetIds);
            List<RetrievedAsset> retrievedAssets = new List<RetrievedAsset>();

            foreach (Asset asset in assets)
            {
                if (asset.Params.UnitName.StartsWith("BNYL") || asset.Params.UnitName.StartsWith("BNYO"))
                {
                    retrievedAssets.Add(new RetrievedAsset(asset.Params.Name, asset.Params.UnitName, asset.Index.Value));
                }
            }

            return retrievedAssets;
        }

        public IEnumerable<AirdropAmount> FetchAirdropAmounts(IDictionary<long, long> assetValues)
        {
            List<AirdropAmount> airdropAmounts = new List<AirdropAmount>();
            IEnumerable<string> walletAddresses = this.FetchWalletAddresses();

            foreach (string walletAddress in walletAddresses)
            {
                IEnumerable<AssetHolding> assetHoldings = this.api.GetAssetsByAddress(walletAddress);
                long amount = this.GetAssetHoldingsAmount(assetHoldings, assetValues);
                if (amount > 0)
                {
                    airdropAmounts.Add(new AirdropAmount(walletAddress, this.AssetId, amount));
                }
            }

            return airdropAmounts;
        }

        public IEnumerable<string> FetchWalletAddresses()
        {
            IEnumerable<string> walletAddresses = this.api.GetWalletAddressesWithAsset(this.AssetId);

            return walletAddresses;
        }

        public async Task<IDictionary<long, long>> GetAssetValues()
        {
            IEnumerable<AssetValue> assets = await cosmos.GetAssetValues("CryptoBunny");

            Dictionary<long, long> assetValues = assets.ToDictionary(av => av.AssetId, av => av.Value);

            return assetValues;
        }

        public long GetAssetHoldingsAmount(IEnumerable<AssetHolding> assetHoldings, IDictionary<long, long> assetValues)
        {
            long airdropAmount = 0;

            foreach (AssetHolding miniAssetHolding in assetHoldings)
            {
                if (miniAssetHolding.AssetId.HasValue &&
                    miniAssetHolding.Amount.HasValue &&
                    miniAssetHolding.Amount > 0 &&
                    assetValues.ContainsKey(miniAssetHolding.AssetId.Value))
                {
                    airdropAmount += (long)miniAssetHolding.Amount.Value;
                }
            }

            return airdropAmount;
        }
    }
}
