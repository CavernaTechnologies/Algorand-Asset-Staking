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
    public class AlgoleaguesHoldingsFactory : HoldingsAirdropFactory
    {
        public AlgoleaguesHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 445905873;
            this.Decimals = 6;
            this.CreatorAddresses = new string[]
            {
                "CUBSX2LHQRFGLE3UNT5ZHNJCXS4AJKCC5OGL6GYK7FBYTTWEBDM4RFB5NI",
                "PARDGF6H4Z2GNRFG2DNN4TNOWNHU3EMH6OLHZXPVEDN74YNE7LHVIIFKH4",
                "PARDFYNXQV6QXUODZXPWGN5VNSIN3YXF7RVCD5ZSLHF4RWO24TYWYOG2JQ"
            };
            this.RevokedAddresses = new string[]
            {
                "CUBSX2LHQRFGLE3UNT5ZHNJCXS4AJKCC5OGL6GYK7FBYTTWEBDM4RFB5NI",
                "PARDGF6H4Z2GNRFG2DNN4TNOWNHU3EMH6OLHZXPVEDN74YNE7LHVIIFKH4",
                "PARDFYNXQV6QXUODZXPWGN5VNSIN3YXF7RVCD5ZSLHF4RWO24TYWYOG2JQ"
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
                    assetValues.Add(asset.Index, 40_000_000);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 50_000_000);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(2).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 100_000_000);
                }
            }

            return assetValues;
        }
    }
}
