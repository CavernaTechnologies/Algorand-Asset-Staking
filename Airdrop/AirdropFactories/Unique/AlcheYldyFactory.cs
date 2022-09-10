using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Unique
{
    public class AlcheYldyFactory
    {
        public IAlgodUtils AlgodUtils { get; }
        public IIndexerUtils IndexerUtils { get; }
        public ulong DropAssetId { get; set; }
        public ulong Decimals { get; set; }

        public ulong TotalDrop { get; set; }

        private readonly ulong[] legendary = new ulong[] { 798987161 };
        private readonly double legendarySplit = 0.615;
        private readonly ulong[] epic = new ulong[] { 798986475, 798985842 };
        private readonly double epicSplit = 0.28;
        private readonly ulong[] rare = new ulong[] { 798985107, 798982872, 798980408, 798978192 };
        private readonly double rareSplit = 0.06;
        private readonly ulong[] uncommon = new ulong[] { 798984317, 798982205, 798979660, 798977534 };
        private readonly double uncommonSplit = 0.03;
        private readonly ulong[] common = new ulong[] { 798983583, 798981060, 798978892, 798976821 };
        private readonly double commonSplit = 0.015;

        public AlcheYldyFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils)
        {
            this.IndexerUtils = indexerUtils;
            this.AlgodUtils = algodUtils;
            this.DropAssetId = 226701642;
            this.Decimals = 6;
            this.TotalDrop = 750_000_000_000;
        }

        public async Task<IEnumerable<AirdropUnitCollection>> FetchAirdropUnitCollections()
        {
            IEnumerable<string> wallets = await IndexerUtils.GetWalletAddressesIntersect(new ulong[] { this.DropAssetId, 310014962, 320570576 });
            Dictionary<string, Holdings> holdings = new Dictionary<string, Holdings>();

            foreach (var wallet in wallets)
            {
                holdings[wallet] = new Holdings(wallet);
            }

            foreach (ulong id in this.legendary)
            {
                var balances = await IndexerUtils.GetBalances(id);
                foreach (var balance in balances)
                {
                    if (holdings.ContainsKey(balance.Address) && balance.Amount > 0)
                    {
                        holdings[balance.Address].Legendaries[id] = balance.Amount;
                    }
                }
            }

            foreach (ulong id in this.epic)
            {
                var balances = await IndexerUtils.GetBalances(id);
                foreach (var balance in balances)
                {
                    if (holdings.ContainsKey(balance.Address) && balance.Amount > 0)
                    {
                        holdings[balance.Address].Epics[id] = balance.Amount;
                    }
                }
            }

            foreach (ulong id in this.rare)
            {
                var balances = await IndexerUtils.GetBalances(id);
                foreach (var balance in balances)
                {
                    if (holdings.ContainsKey(balance.Address) && balance.Amount > 0)
                    {
                        holdings[balance.Address].Rares[id] = balance.Amount;
                    }
                }
            }

            foreach (ulong id in this.uncommon)
            {
                var balances = await IndexerUtils.GetBalances(id);
                foreach (var balance in balances)
                {
                    if (holdings.ContainsKey(balance.Address) && balance.Amount > 0)
                    {
                        holdings[balance.Address].Uncommons[id] = balance.Amount;
                    }
                }
            }

            foreach (ulong id in this.common)
            {
                var balances = await IndexerUtils.GetBalances(id);
                foreach (var balance in balances)
                {
                    if (holdings.ContainsKey(balance.Address) && balance.Amount > 0)
                    {
                        holdings[balance.Address].Commons[id] = balance.Amount;
                    }
                }
            }

            ulong numberLegendary = 0;
            ulong numberEpic = 0;
            ulong numberRare = 0;
            ulong numberUncommon = 0;
            ulong numberCommon = 0;

            foreach (var holding in holdings.Values)
            {
                foreach (var num in holding.Legendaries.Values)
                {
                    numberLegendary += num;
                }
                foreach (var num in holding.Epics.Values)
                {
                    numberEpic += num;
                }
                foreach (var num in holding.Rares.Values)
                {
                    numberRare += num;
                }
                foreach (var num in holding.Uncommons.Values)
                {
                    numberUncommon += num;
                }
                foreach (var num in holding.Commons.Values)
                {
                    numberCommon += num;
                }
            }

            ulong legendaryVal = (ulong)((this.TotalDrop * legendarySplit) / numberLegendary);
            ulong epicVal = (ulong)((this.TotalDrop * epicSplit) / numberEpic);
            ulong rareVal = (ulong)((this.TotalDrop * rareSplit) / numberRare);
            ulong uncommonVal = (ulong)((this.TotalDrop * uncommonSplit) / numberUncommon);
            ulong commonVal = (ulong)((this.TotalDrop * commonSplit) / numberCommon);

            AirdropUnitCollectionManager collectionManager = new AirdropUnitCollectionManager();

            foreach (var holding in holdings.Values)
            {
                foreach (var pair in holding.Legendaries)
                {
                    collectionManager.AddAirdropUnit(new AirdropUnit(holding.Wallet, this.DropAssetId, pair.Key, legendaryVal, pair.Value, true));
                }
                foreach (var pair in holding.Epics)
                {
                    collectionManager.AddAirdropUnit(new AirdropUnit(holding.Wallet, this.DropAssetId, pair.Key, epicVal, pair.Value, true));
                }
                foreach (var pair in holding.Rares)
                {
                    collectionManager.AddAirdropUnit(new AirdropUnit(holding.Wallet, this.DropAssetId, pair.Key, rareVal, pair.Value, true));
                }
                foreach (var pair in holding.Uncommons)
                {
                    collectionManager.AddAirdropUnit(new AirdropUnit(holding.Wallet, this.DropAssetId, pair.Key, uncommonVal, pair.Value, true));
                }
                foreach (var pair in holding.Commons)
                {
                    collectionManager.AddAirdropUnit(new AirdropUnit(holding.Wallet, this.DropAssetId, pair.Key, commonVal, pair.Value, true));
                }
            }

            return collectionManager.GetAirdropUnitCollections();
        }
    }

    class Holdings
    {
        public string Wallet { get; }
        public Dictionary<ulong, ulong> Legendaries { get;  }
        public Dictionary<ulong, ulong> Epics { get; }
        public Dictionary<ulong, ulong> Rares { get; }
        public Dictionary<ulong, ulong> Uncommons { get; }
        public Dictionary<ulong, ulong> Commons { get; }

        public Holdings(string wallet)
        {
            this.Wallet = wallet;
            this.Legendaries = new Dictionary<ulong, ulong>();
            this.Epics = new Dictionary<ulong, ulong>();
            this.Rares = new Dictionary<ulong, ulong>();
            this.Uncommons = new Dictionary<ulong, ulong>();
            this.Commons = new Dictionary<ulong, ulong>();
        }
    }
}
