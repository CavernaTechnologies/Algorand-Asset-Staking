using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Cosmos;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class HootAirdropFactory : HoldingsAirdropFactory
    {
        public ICosmos Cosmos { get; }
        public HootAirdropFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ICosmos cosmos) : base(indexerUtils, algodUtils)
        {
            this.Cosmos = cosmos;
            this.DropAssetId = 797382233;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "OWLETILXSEP4INXUQ3XTKRJX4ATYBHOFRQ6THSTCGS7ZFCINAEL4B4F7CQ",
                "AOWLLUX3BBLDV6KUZYQ7FBZTIWGWRRJO6B5XL2DFQ6WLITHUK26OO7IGMI",
                "AOWL3ZOC55JJDB5LU5CCIVK3WITVXMVZU6PV2CQQOLXUNCTOJQ6VJCPYAU",
                "AOWLDAJSAIHNR4J2TKB4U32Z73O7W52JGJUGUR5RW6ZH46G3ET4IFNNGQ4",
                "OWLETS3KHONGNL5V2WGRY7FLOUE7NGC3CXA7PY3PJZXSXMBMFICGBUJGQA"
            };
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            ulong val = 12;
            foreach (string addr in this.CreatorAddresses.Take(1))
            {
                var acc = await AlgodUtils.GetAccount(addr);

                foreach (var asset in acc.CreatedAssets)
                {
                    assetValues[asset.Index] = val;
                }
            }

            val = 5;
            foreach (string addr in this.CreatorAddresses.Skip(1).Take(3))
            {
                var acc = await AlgodUtils.GetAccount(addr);

                foreach (var asset in acc.CreatedAssets)
                {
                    assetValues[asset.Index] = val;
                }
            }

            val = 3;
            foreach (string addr in this.CreatorAddresses.Skip(4).Take(1))
            {
                var acc = await AlgodUtils.GetAccount(addr);

                foreach (var asset in acc.CreatedAssets)
                {
                    assetValues[asset.Index] = val;
                }
            }

            var avs = await Cosmos.GetAssetValues("Hoot");
            foreach (var assetValue in avs)
            {
                assetValues[assetValue.AssetId] = assetValue.Value;
            }

            return assetValues;
        }
    }
}
