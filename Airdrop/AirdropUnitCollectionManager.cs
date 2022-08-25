using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Airdrop
{
    public class AirdropUnitCollectionManager
    {
        public ConcurrentDictionary<(string, ulong), AirdropUnitCollection> collectionDict;

        public AirdropUnitCollectionManager()
        {
            this.collectionDict = new ConcurrentDictionary<(string, ulong), AirdropUnitCollection>();
        }

        public void AddAirdropUnit(AirdropUnit airdropUnit)
        {
            (string, ulong) key = (airdropUnit.Address, airdropUnit.DropAssetId);

            if (this.collectionDict.ContainsKey(key))
            {
                this.collectionDict[key].AddAirdropUnit(airdropUnit);
            }
            else
            {
                this.collectionDict[key] = new AirdropUnitCollection(key, airdropUnit);
            }
        }

        public void AddModifier(AirdropModifier modifier)
        {
            (string, ulong) key = (modifier.Address, modifier.DropAssetId);

            if (this.collectionDict.ContainsKey(key))
            {
                this.collectionDict[key].AddModifier(modifier);
            } 
            else
            {
                AirdropUnitCollection col = new AirdropUnitCollection(key);
                col.AddModifier(modifier);
                this.collectionDict[key] = col;
            }
        }

        public IEnumerable<string> GetWalletAddresses()
        {
            return this.collectionDict.Keys.Select(k => k.Item1).Distinct();
        }

        public IEnumerable<ulong> GetAssetIds(string address)
        {
            return this.collectionDict.Keys.Where(k => k.Item1 == address).Select(k => k.Item2);
        }

        public AirdropUnitCollection GetAirdropUnitCollection(string address, ulong assetId)
        {
            return this.collectionDict.GetValueOrDefault((address, assetId));
        }

        public IEnumerable<AirdropUnitCollection> GetAirdropUnitCollections()
        {
            return this.collectionDict.Values.Where(c => c.GetTotal() > 0);
        }
    }

}
