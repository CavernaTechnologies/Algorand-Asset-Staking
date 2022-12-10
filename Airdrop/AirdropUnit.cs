namespace Airdrop
{
    public class AirdropUnit
    {
        public string Address { get; set; }
        public ulong DropAssetId { get; set; }
        public ulong SourceAssetId { get; set; }
        public ulong NumberOfSourceAsset { get; set; }
        public double Value { get; set; }
        public bool IsMultiplied { get; }
        public double Total { get => this.GetTotal(); }

        public AirdropUnit(string address, ulong dropAssetId, ulong sourceAssetId, double value, ulong numberOfSourceAsset = 1, bool isMultiplied = false)
        {
            this.Address = address;
            this.DropAssetId = dropAssetId;
            this.SourceAssetId = sourceAssetId;
            this.NumberOfSourceAsset = numberOfSourceAsset;
            this.Value = value;
            this.IsMultiplied = isMultiplied;
        }

        public double GetTotal()
        {
            if (this.IsMultiplied)
            {
                return this.NumberOfSourceAsset * this.Value;
            }
            else
            {
                return this.Value;
            }
        }
    }
}
