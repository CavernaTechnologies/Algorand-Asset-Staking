using System;
using System.Collections.Generic;
using System.Text;

namespace Airdrop
{
    public class AirdropModifier
    {
        public string Address { get; set; }
        public ulong DropAssetId { get; set; }
        public ulong SourceAssetId { get; set; }
        public ulong NumberOfSourceAsset { get; set; }
        public double Value { get; set; }

        public AirdropModifier(string address, ulong dropAssetId, ulong sourceAssetId, double value, ulong numberOfSourceAsset)
        {
            this.Address = address;
            this.DropAssetId = dropAssetId;
            this.SourceAssetId = sourceAssetId;
            this.NumberOfSourceAsset = numberOfSourceAsset;
            this.Value = value;
        }

        public double GetModifier()
        {
            return this.Value * this.NumberOfSourceAsset;
        }
    }
}
