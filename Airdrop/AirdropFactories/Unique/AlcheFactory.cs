using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Unique
{
    public class AlcheFactory
    {
        public IAlgodUtils AlgodUtils { get; }
        public IIndexerUtils IndexerUtils { get; }
        public ulong DropAssetId { get; set; }
        public ulong StakeFlagId { get; set; }
        public ulong Decimals { get; set; }

        private readonly AlcheSet[] sets;

        private readonly string[] ignoredWallets;

        public AlcheFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils)
        {
            this.IndexerUtils = indexerUtils;
            this.AlgodUtils = algodUtils;
            this.DropAssetId = 310014962;
            this.StakeFlagId = 320570576;
            this.Decimals = 0;

            ignoredWallets = new string[] {
                "LEMO5ZPXGACO25UN4GFHWFRHP2MZNJTJL7OV3HYJU7KZF2TL7CHWZIYEWU",
                "5W3QB7A7BFX2MD7XRMD3FLYEBS4AMOVFHRL5QAOP4QQC227L2GVIJIWNNM",
                "C5UMHCZBPPVFUSPHCZT6YYFXB6IXTF7Z57JQ7SHUGIC5PBU7BSLNPKSSSY",
                "OJGTHEJ2O5NXN7FVXDZZEEJTUEQHHCIYIE5MWY6BEFVVLZ2KANJODBOKGA",
            };

            sets = new AlcheSet[]
            {
                // Set 1
                new AlcheSet(50_000, new ulong[]{315166675},  new ulong[]{313419037, 313421275}, new ulong[]{306729637, 306186552, 306189097, 306191511}, new ulong[]{306181364, 306184765, 306188337, 306190826}, new ulong[]{306180273, 306183859, 306187531, 306190133}),
                // Set 2
                new AlcheSet(50_000, new ulong[]{337245090},  new ulong[]{337243065, 337244072}, new ulong[]{337226686, 337230496, 337233127, 337239113}, new ulong[]{337225085, 337228921, 337231891, 337238291}, new ulong[]{332014800, 332016564, 332017894, 332018856}),
                // Set 3
                new AlcheSet(50_000, new ulong[]{400926043},  new ulong[]{400920947, 400924498}, new ulong[]{400878709, 400885791, 400889657, 400894271}, new ulong[]{400877134, 400883933, 400887826, 400891886}, new ulong[]{395700430, 395706101, 395702497, 395703386}),
                // Set 4
                new AlcheSet(50_000, new ulong[]{527486409},  new ulong[]{527483715, 527485015}, new ulong[]{527475282, 527477069, 527479654, 527481591}, new ulong[]{509842608, 509844088, 509848775, 509850827}, new ulong[]{490139078, 490141855, 493271743, 490146814}),
                // Set 5
                new AlcheSet(50_000, new ulong[]{744540333},  new ulong[]{744538073, 744539419}, new ulong[]{744528583, 744530969, 744533302, 744536686}, new ulong[]{744527932, 744530060, 744532520, 744535776}, new ulong[]{744527019, 744551347, 744531764, 744534630}),
            };
        }

        public async Task<IEnumerable<AirdropUnitCollection>> FetchAirdropUnitCollections()
        {
            IEnumerable<string> wallets = await IndexerUtils.GetWalletAddressesIntersect(new ulong[] { this.DropAssetId, this.StakeFlagId });
            wallets = wallets.Except(ignoredWallets);

            AirdropUnitCollectionManager collectionManager = new AirdropUnitCollectionManager();

            foreach (var set in this.sets)
            {
                Dictionary<string, Holdings> holdings = new Dictionary<string, Holdings>();

                foreach (var wallet in wallets)
                {
                    holdings[wallet] = new Holdings(wallet);
                }

                foreach (ulong id in set.Legendaries)
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

                foreach (ulong id in set.Epics)
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

                foreach (ulong id in set.Rares)
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

                foreach (ulong id in set.Uncommons)
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

                foreach (ulong id in set.Commons)
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

                double commonVal = (double)set.TotalDrop / (numberCommon + 2 * numberUncommon + 4 * numberRare + 16 * numberEpic + 32 * numberLegendary);
                double uncommonVal = 2 * commonVal;
                double rareVal = 4 * commonVal;
                double epicVal = 16 * commonVal;
                double legendaryVal = 32 * commonVal;

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
            }

            return collectionManager.GetAirdropUnitCollections();
        }
    }

    class AlcheSet
    {
        public ulong TotalDrop { get; }
        public ulong[] Legendaries { get; }
        public ulong[] Epics { get; }
        public ulong[] Rares { get; }
        public ulong[] Uncommons { get; }
        public ulong[] Commons { get; }

        public AlcheSet(ulong totalDrop, ulong[] legendaries, ulong[] epics, ulong[] rares, ulong[] uncommons, ulong[] commons)
        {
            TotalDrop = totalDrop;
            Legendaries = legendaries;
            Epics = epics;
            Rares = rares;
            Uncommons = uncommons;
            Commons = commons;
        }
    }
}
