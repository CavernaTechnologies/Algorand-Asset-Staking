using Algorand.V2.Algod.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class YarnHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public YarnHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 878951062;
            this.Decimals = 0;
            this.CreatorAddresses = new string[]
            {
                "45LDVA6A44QD2PNWNAPGGDQESXNOY36HJC6UZXZNMIAYLXUYD4DGRAMNNA",
                "FROGJWNVWICMFTAGNCLGAI5UNXJEEFCUDNPBD2U6VWTATXANHXB6BHRW2M",
                "FROGOHZ3D5GHBKPDDQWKT5RWABZGY3VD6D3UNG7YLTABKDDLJJD4E257HA",
                "SNAILSURAATOMSYJ36S7ZVZ3OD5USXU7BHHZFEB5QOXEDDTQH2MOO3REAI",
                "PIXYN3736RN7XS7ZA354R33RTDUTPRAZ2YMGU3V72I3EVDY62O3TDK43X4",
            };
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[]
            {
                "knit-heads",
                "lil-knits"
            };
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    if (asset.Params.UnitName != null && asset.Params.UnitName.StartsWith("KH"))
                    {
                        assetValues.Add(asset.Index, 300);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    if (asset.Params.UnitName != null && asset.Params.UnitName.StartsWith("frogBOT"))
                    {
                        assetValues.Add(asset.Index, 5);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 69);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(2).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 333);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(3).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 55);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(4).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 55);
                }
            }

            return assetValues;
        }
    }
}
