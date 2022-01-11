﻿using Algorand.V2.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Airdrop.AirdropFactories.Liquidity
{
    public class RaptorLiquidityFactory : ILiquidityAirdropFactory
    {
        public long AssetId { get; set; }
        public long Decimals { get; set; }
        public string CreatorWallet { get; set; }
        public long LiquidityAssetId { get; set; }
        public string LiquidityWallet { get; set; }
        public long LiquidityMinimum { get; set; }
        public long DropTotal { get; set; }
        public long DropMinimum { get; set; }

        private readonly IAlgoApi api;

        public RaptorLiquidityFactory(IAlgoApi api)
        {
            this.AssetId = 426980914;
            this.Decimals = 2;
            this.CreatorWallet = "SBKN5JI72DS4USUUIFO3MMNZLPVDKERA2D3HOPZMSXAK5VBBEM364TGS3A";
            this.LiquidityAssetId = 428917669;
            this.LiquidityWallet = "AIFU57RAPPX672WDLAUGZE46677XNGYQEX2KQM54DJZ4HUTQ264M2TALQA";
            this.LiquidityMinimum = 400000;
            this.DropTotal = 20000000;
            this.DropMinimum = 0;
            this.api = api;
        }

        public Task<IEnumerable<AirdropAmount>> FetchAirdropAmounts()
        {
            List<string> walletAddresses = this.FetchWalletAddresses().ToList();
            walletAddresses.Remove(this.LiquidityWallet);
            walletAddresses.Remove(this.CreatorWallet);

            IEnumerable<(string, long)> liquidityAmounts = this.GetLiquidityAmounts(walletAddresses);

            long liquidityTotal = liquidityAmounts.Sum(la => la.Item2);

            List<AirdropAmount> airdropAmounts = new List<AirdropAmount>();

            foreach ((string, long) liquidityAmount in liquidityAmounts)
            {
                long dropAmount = (long)(this.DropTotal * ((double)liquidityAmount.Item2 / (double)liquidityTotal));
                if (dropAmount > DropMinimum)
                {
                    airdropAmounts.Add(new AirdropAmount(liquidityAmount.Item1, this.AssetId, dropAmount));
                }
            }

            return Task.FromResult<IEnumerable<AirdropAmount>>(airdropAmounts);
        }

        public IEnumerable<(string, long)> GetLiquidityAmounts(IEnumerable<string> walletAddresses)
        {
            ConcurrentBag<(string, long)> liquidityAmounts = new ConcurrentBag<(string, long)>();

            //Parallel.ForEach(walletAddresses, new ParallelOptions { MaxDegreeOfParallelism = 10 }, walletAddress =>
            foreach (string walletAddress in walletAddresses)
            {
                Account account = api.GetAccountByAddress(walletAddress);
                ulong? liquidityAmount = this.GetLiquidityAssetAmount(account.Assets);

                if (liquidityAmount.HasValue && (long)liquidityAmount.Value > this.LiquidityMinimum)
                {
                    long weekLowAmount = api.GetAssetLowest(walletAddress, this.LiquidityAssetId, (long)liquidityAmount.Value, DateTime.Now.AddDays(-14));
                    liquidityAmounts.Add((walletAddress, weekLowAmount));
                }
            }

            return liquidityAmounts;
        }

        public ulong? GetLiquidityAssetAmount(IEnumerable<AssetHolding> assetHoldings)
        {
            foreach (AssetHolding assetHolding in assetHoldings)
            {
                if (assetHolding.AssetId.HasValue &&
                    assetHolding.AssetId.Value == this.LiquidityAssetId &&
                    assetHolding.Amount.HasValue &&
                    assetHolding.Amount > 0)
                {
                    return assetHolding.Amount;
                }
            }

            return null;
        }

        public IEnumerable<string> FetchWalletAddresses()
        {
            IEnumerable<string> walletAddresses = this.api.GetWalletAddressesWithAsset(this.LiquidityAssetId, this.AssetId);

            return walletAddresses;
        }
    }
}