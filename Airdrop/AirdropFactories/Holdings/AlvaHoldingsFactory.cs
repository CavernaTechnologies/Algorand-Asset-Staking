using Algorand.V2.Algod.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class AlvaHoldingsFactory : HoldingsAirdropFactory
    {
        public AlvaHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 553615859;
            this.Decimals = 2;
            this.CreatorAddresses = new string[] { "ALVA7QT5JWKXMWGNYL3JYFTCFFCYJVUIFZAD4S7AKFW5M7OI6Q7X3EAGFY" };
            this.RevokedAddresses = new string[] { "ALVA7QT5JWKXMWGNYL3JYFTCFFCYJVUIFZAD4S7AKFW5M7OI6Q7X3EAGFY",
                                                   "BKOSO3RMXW6XIB7TQWOPIEUSW2Q4PY2DKCMQO433WHLKFV22ZXEPQGQDKU" };
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<int, ulong> powerToValue = new Dictionary<int, ulong>();
            powerToValue[11] = 200;
            powerToValue[12] = 300;
            powerToValue[13] = 800;
            powerToValue[14] = 2000;
            powerToValue[15] = 4500;
            powerToValue[16] = 10000;
            powerToValue[17] = 22500;
            powerToValue[18] = 47500;

            ulong foilValue = 55000;


            ulong[] endingDigits;
            DateTime t = DateTime.Now;
            if (t.DayOfWeek == DayOfWeek.Sunday)
            {
                endingDigits = new ulong[] { 0, 1 };
            } 
            else if (t.DayOfWeek == DayOfWeek.Tuesday)
            {
                endingDigits = new ulong[] { 2, 3, 4 };
            }
            else if (t.DayOfWeek == DayOfWeek.Thursday)
            {
                endingDigits = new ulong[] { 5, 6, 7 };
            }
            else if (t.DayOfWeek == DayOfWeek.Saturday)
            {
                endingDigits = new ulong[] { 8, 9 };
            }
            else
            {
                endingDigits = new ulong[] { };
            }


            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            var txns = await IndexerUtils.GetTransactions(this.CreatorAddresses[0], txType: Algorand.V2.Indexer.Model.TxType.Acfg);
            
            foreach (var txn in txns)
            {
                if (txn?.CreatedAssetIndex != null)
                {
                    ulong id = (ulong)txn.CreatedAssetIndex;

                    if (endingDigits.Contains(id % 10))
                    {
                        if (txn?.Note != null)
                        {
                            try
                            {
                                dynamic obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(txn.Note));

                                if (obj.standard == "arc69")
                                {
                                    int power = (int)obj.properties.Power;
                                    string background = (string)obj.properties.Background;
                                    if (power > 10)
                                    {
                                        if (background.StartsWith("Foil"))
                                        {
                                            assetValues[id] = foilValue;
                                        }
                                        else
                                        {
                                            assetValues[id] = powerToValue[power];
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Failed on " + id);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Failed on " + id);
                            }
                        }
                    }

                }
            }

            return assetValues;
        }

        public async Task<IDictionary<ulong, double>> FetchModifiers()
        {
            Dictionary<ulong, double> modifiers = new Dictionary<ulong, double>();
            modifiers[555593804] = .025;

            var txns = await IndexerUtils.GetTransactions(this.CreatorAddresses[0], txType: Algorand.V2.Indexer.Model.TxType.Acfg);

            foreach (var txn in txns)
            {
                if (txn?.CreatedAssetIndex != null)
                {
                    if (txn?.Note != null)
                    {
                        ulong id = (ulong)txn.CreatedAssetIndex;
                        dynamic obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(txn.Note));
                        try
                        {
                            if (obj.standard == "arc69")
                            {
                                string background = (string)obj.properties.Background;
                                if (background.StartsWith("Foil"))
                                {
                                    modifiers[id] = .005;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Failed on " + id);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failed on " + id);
                        }
                    }
                }
            }

            return modifiers;
        }

        public void AddAssetsInAccount(AirdropUnitCollectionManager collectionManager, Account account, IDictionary<ulong, ulong> assetValues, IDictionary<ulong, double> modifiers)
        {
            IEnumerable<AssetHolding> assetHoldings = account.Assets;

            if (assetHoldings != null)
            {
                foreach (AssetHolding asset in assetHoldings)
                {
                    ulong sourceAssetId = asset.AssetId;
                    ulong numberOfSourceAsset = asset.Amount;

                    if (modifiers.ContainsKey(sourceAssetId) && numberOfSourceAsset > 0)
                    {
                        double value = modifiers[sourceAssetId];
                        collectionManager.AddModifier(new AirdropModifier(
                            account.Address,
                            this.DropAssetId,
                            sourceAssetId,
                            value,
                            numberOfSourceAsset: numberOfSourceAsset));
                    }
                    
                    if (assetValues.ContainsKey(sourceAssetId) && numberOfSourceAsset > 0)
                    {
                        ulong assetValue = assetValues[sourceAssetId];
                        collectionManager.AddAirdropUnit(new AirdropUnit(
                            account.Address,
                            this.DropAssetId,
                            sourceAssetId,
                            assetValue,
                            numberOfSourceAsset: numberOfSourceAsset,
                            isMultiplied: true));
                    }
                }
            }
        }

        public override async Task<IEnumerable<AirdropUnitCollection>> FetchAirdropUnitCollections()
        {
            IDictionary<ulong, ulong> assetValues = await FetchAssetValues();
            IDictionary<ulong, double> modifiers = await FetchModifiers();
            IEnumerable<Account> accounts = await FetchAccounts();

            AirdropUnitCollectionManager collectionManager = new AirdropUnitCollectionManager();

            Parallel.ForEach(accounts, new ParallelOptions { MaxDegreeOfParallelism = 10 }, account =>
            {
                AddAssetsInAccount(collectionManager, account, assetValues, modifiers);
            });

            return collectionManager.GetAirdropUnitCollections();
        }
    }
}
