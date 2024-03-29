﻿using Algorand.V2.Algod.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Unique
{
    public class RugAirdropFactory
    {
        public IAlgodUtils AlgodUtils { get; }
        public IIndexerUtils IndexerUtils { get; }
        public string[] CreatorAddresses { get; }
        public WinningItem[] WinningItems { get; }

        public RugAirdropFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils)
        {
            this.IndexerUtils = indexerUtils;
            this.AlgodUtils = algodUtils;

            this.CreatorAddresses = new string[] { "RUGSVETYW67CNQB5ZBJXYRZR5ZS5TXCQWFRACISKGEJ5N6YXYKTGJUTGRM", "RUG2PYQAYXNDR4UWUEBUX5NMDRS2VS73IY2F6LDEFHWXAHZRCU7FHTE3IE" };
            this.WinningItems = new WinningItem[]
            {
                new WinningItem("DougDoug", 2123661, 0, 2, 1),
                new WinningItem("Yieldly", 226701642, 6, 4, 2500000000),
                new WinningItem("AlgoScout Token", 569120128, 6, 10, 500000000),
                new WinningItem("Anything", 564993115, 10, 3, 2000000000),
                new WinningItem("LOUDefi", 457819394, 6, 10, 10000000000),
                new WinningItem("Coffee Beans", 473180477, 0, 1, 1),
                new WinningItem("Rug Pull", 404399153, 0, 10, 1000),
                new WinningItem("PonziRand", 401983071, 6, 10, 1000000000),
                new WinningItem("AlgoDoggo", 352658929, 0, 5, 25000),
                new WinningItem("Motherload", 230764428, 0, 10, 10000)
             };
        }

        public async Task<IEnumerable<AirdropUnitCollection>> FetchAirdropUnitCollections()
        {
            HashSet<ulong> assetIds = await FetchAssetIds();
            IEnumerable<Account> accounts = await FetchAccounts();
            List<(Account, ulong)> eligibleWinners = new List<(Account, ulong)>();
            AirdropUnitCollectionManager airdropManager = new AirdropUnitCollectionManager();

            foreach (Account account in accounts)
            {
                eligibleWinners.AddRange(GetEligibleWinners(account, assetIds));
            }

            ulong totalWinners = (ulong)this.WinningItems.Sum(wi => (double)wi.NumberOfWinners);

            System.Random random = new System.Random();
            ulong i = 0;

            List<(Account, ulong)> winners = new List<(Account, ulong)>();

            while (i < totalWinners && eligibleWinners.Count > 0)
            {
                int winnerIndex = random.Next(eligibleWinners.Count);

                winners.Add(eligibleWinners.ElementAt(winnerIndex));

                eligibleWinners.RemoveAt(winnerIndex);

                i++;
            }

            foreach (WinningItem winningItem in this.WinningItems)
            {
                for (ulong j = 0; j < winningItem.NumberOfWinners; j++)
                {
                    (Account, ulong) winner = winners.ElementAt(0);
                    winners.RemoveAt(0);

                    Account winningAccount = winner.Item1;
                    ulong winningAsset = winner.Item2;

                    airdropManager.AddAirdropUnit(new AirdropUnit(
                        winningAccount.Address,
                        winningItem.AssetId,
                        winningAsset,
                        winningItem.ValueWon));
                }
            }

            return airdropManager.GetAirdropUnitCollections();
        }

        public async Task<HashSet<ulong>> FetchAssetIds()
        {
            HashSet<ulong> assetIds = new HashSet<ulong>();

            foreach (string creatorAddress in this.CreatorAddresses)
            {
                IEnumerable<Asset> assets = (await AlgodUtils.GetAccount(creatorAddress)).CreatedAssets;

                foreach (Asset asset in assets)
                {
                    assetIds.Add(asset.Index);
                }
            }

            return assetIds;
        }

        public async Task<IEnumerable<Account>> FetchAccounts()
        {
            IEnumerable<string> addresses = await this.IndexerUtils.GetWalletAddressesIntersect(this.WinningItems.Select(wi => wi.AssetId));

            ConcurrentBag<Account> accounts = new ConcurrentBag<Account>();

            Parallel.ForEach<string>(addresses, new ParallelOptions { MaxDegreeOfParallelism = 10 }, address =>
            {
                var account = this.AlgodUtils.GetAccount(address).Result;
                accounts.Add(account);
            });

            return accounts;
        }

        public IEnumerable<(Account, ulong)> GetEligibleWinners(Account account, HashSet<ulong> assetIds)
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

    class AccountEqualityComparer : IEqualityComparer<Account>
    {
        public bool Equals(Account x, Account y)
        {
            return x.Address == y.Address;
        }

        public int GetHashCode(Account x)
        {
            return x.Address.GetHashCode();
        }
    }

    public class WinningItem
    {
        public WinningItem(string name, ulong assetId, ulong decimals, ulong numberOfWinners, ulong valueWon)
        {
            Name = name;
            AssetId = assetId;
            Decimals = decimals;
            NumberOfWinners = numberOfWinners;
            ValueWon = valueWon;
        }

        public string Name { get; }
        public ulong AssetId { get; }
        public ulong Decimals { get; }
        public ulong NumberOfWinners { get; }
        public ulong ValueWon { get; }

    }
}
