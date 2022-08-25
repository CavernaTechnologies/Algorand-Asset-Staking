using System.Collections.Concurrent;

namespace Airdrop
{
    public class AirdropUnitCollection
    {
        public string Wallet { get; }
        public ulong DropAssetId { get; }
        public ulong Total { get => this.GetTotal(); }
        public ConcurrentBag<AirdropUnit> airdropUnits;
        public ConcurrentBag<AirdropModifier> modifiers;

        public AirdropUnitCollection(string wallet, ulong dropAssetId)
        {
            this.Wallet = wallet;
            this.DropAssetId = dropAssetId;
            this.airdropUnits = new ConcurrentBag<AirdropUnit>();
            this.modifiers = new ConcurrentBag<AirdropModifier>();
        }

        public AirdropUnitCollection((string, ulong) infoTuple) : this(infoTuple.Item1, infoTuple.Item2) { }

        public AirdropUnitCollection(string wallet, ulong dropAssetId, AirdropUnit airdropUnit) : this(wallet, dropAssetId)
        {
            this.AddAirdropUnit(airdropUnit);
        }

        public AirdropUnitCollection((string, ulong) infoTuple, AirdropUnit airdropUnit) : this(infoTuple.Item1, infoTuple.Item2, airdropUnit) { }

        public void AddAirdropUnit(AirdropUnit airdropUnit)
        {
            this.airdropUnits.Add(airdropUnit);
        }

        public void AddModifier(AirdropModifier modifier)
        {
            this.modifiers.Add(modifier);
        }

        public ulong GetTotal()
        {
            ulong total = 0;

            foreach (AirdropUnit airdropUnit in this.airdropUnits)
            {
                total += airdropUnit.GetTotal();
            }

            double modTotal = 0;

            foreach (AirdropModifier modifier in this.modifiers)
            {
                modTotal += modifier.GetModifier();
            }

            if (modTotal != 0)
            {
                total = (ulong)(total * (1 + modTotal));
            }

            return total;
        }

        public int Count()
        {
            return this.airdropUnits.Count;
        }

        public override string ToString()
        {
            string collectionBreakdown = $"{this.Wallet} : {this.DropAssetId} : {this.Total}";

            foreach (AirdropModifier modifier in this.modifiers)
            {
                collectionBreakdown += $"\n\tModifier: {modifier.SourceAssetId} : {modifier.NumberOfSourceAsset} : {modifier.GetModifier()}";
            }

            foreach (AirdropUnit airdropUnit in this.airdropUnits)
            {
                collectionBreakdown += $"\n\t{airdropUnit.SourceAssetId} : {airdropUnit.NumberOfSourceAsset} : {airdropUnit.Total}";
            }

            return collectionBreakdown;
        }
    }
}
