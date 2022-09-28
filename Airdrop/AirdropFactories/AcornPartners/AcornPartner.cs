using Algorand.V2.Algod.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.AcornPartners
{
    public abstract class AcornPartner
    {
        public IIndexerUtils IndexerUtils { get; }
        public IAlgodUtils AlgodUtils { get; }

        public AcornPartner(IIndexerUtils indexerUtils, IAlgodUtils algodUtils)
        {
            this.DropAssetId = 226265212;
            this.Decimals = 0;
            this.IndexerUtils = indexerUtils;
            this.AlgodUtils = algodUtils;
        }

        public ulong DropAssetId { get; set; }
        public ulong Decimals { get; set; }
        public ulong NumberOfWinners { get; set; }
        public ulong TotalWinnings { get; set; }
        public ulong[] RevokedAssets { get; set; }
        public string[] CreatorAddresses { get; set; }
        public string[] RevokedAddresses { get; set; }
        public string AssetPrefix { get; set; }

        public virtual async Task FetchAirdropUnitCollections(AirdropUnitCollectionManager airdropManager, IEnumerable<Account> accounts)
        {
            HashSet<ulong> assetIds = await FetchAssetIds();
            List<(Account, ulong)> eligibleWinners = new List<(Account, ulong)>();

            foreach (Account account in accounts)
            {
                if (!this.RevokedAddresses.Contains(account.Address))
                {
                    eligibleWinners.AddRange(this.GetEligibleWinners(account, assetIds));
                }
            }

            System.Random random = new System.Random();
            ulong i = 0;

            while (i < this.NumberOfWinners && eligibleWinners.Count > 0)
            {
                int winnerIndex = random.Next(eligibleWinners.Count);

                (Account, ulong) winnerInfo = eligibleWinners.ElementAt(winnerIndex);

                Account winner = winnerInfo.Item1;
                ulong winningAssetId = winnerInfo.Item2;

                eligibleWinners.RemoveAt(winnerIndex);

                airdropManager.AddAirdropUnit(new AirdropUnit(
                    winner.Address,
                    this.DropAssetId,
                    winningAssetId,
                    this.TotalWinnings / this.NumberOfWinners,
                    numberOfSourceAsset: 1));

                i++;
            }
        }

        public virtual async Task<HashSet<ulong>> FetchAssetIds()
        {
            HashSet<ulong> assetIds = new HashSet<ulong>();

            foreach (string creatorAddress in this.CreatorAddresses)
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null && this.AssetPrefix != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index) && asset.Params?.UnitName != null && asset.Params.UnitName.StartsWith(this.AssetPrefix))
                        {
                            assetIds.Add(asset.Index);
                        }
                    }
                }
                else if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetIds.Add(asset.Index);
                        }
                    }
                }
                else if (this.AssetPrefix != null)
                {
                    foreach (var asset in assets)
                    {
                        if (asset.Params?.UnitName != null && asset.Params.UnitName.StartsWith(this.AssetPrefix))
                        {
                            assetIds.Add(asset.Index);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetIds.Add(asset.Index);
                    }
                }
            }

            return assetIds;
        }

        public async Task<IEnumerable<Account>> FetchAccounts()
        {
            IEnumerable<string> addresses = await this.IndexerUtils.GetWalletAddresses(this.DropAssetId);

            ConcurrentBag<Account> accounts = new ConcurrentBag<Account>();

            Parallel.ForEach<string>(addresses, new ParallelOptions { MaxDegreeOfParallelism = 10 }, address =>
            {
                var account = this.AlgodUtils.GetAccount(address).Result;
                accounts.Add(account);
            });

            return accounts;
        }

        public virtual IEnumerable<(Account, ulong)> GetEligibleWinners(Account account, HashSet<ulong> assetIds)
        {
            List<(Account, ulong)> eligibleAssets = new List<(Account, ulong)>();

            if (account.Assets != null)
            {
                foreach (AssetHolding assetHolding in account.Assets)
                {
                    if (assetIds.Contains(assetHolding.AssetId) && assetHolding.Amount > 0)
                    {
                        for (ulong i = 0; i < assetHolding.Amount; i++)
                        {
                            eligibleAssets.Add((account, assetHolding.AssetId));
                        }
                    }
                }
            }

            return eligibleAssets;
        }
    }
}
