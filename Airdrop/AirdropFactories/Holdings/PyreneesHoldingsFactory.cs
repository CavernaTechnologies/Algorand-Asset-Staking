using Algorand.V2.Algod.Model;
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
    public class PyreneesHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public PyreneesHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 765722712;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "IEQGNR7DPQ26ZY7A22VHRTLHDL2PZ3XD5AUMMEEPYB6DUL6FSRN2NUUWPE", "PUPSJCLH66O6L6BNBT3OTVECXH3QXRJ5F2RKDIC4XQBMHYH2WGQUB6MKUY" };
            this.RevokedAddresses = new string[] { "IEQGNR7DPQ26ZY7A22VHRTLHDL2PZ3XD5AUMMEEPYB6DUL6FSRN2NUUWPE", "PUPSJCLH66O6L6BNBT3OTVECXH3QXRJ5F2RKDIC4XQBMHYH2WGQUB6MKUY" };
            this.RevokedAssets = new ulong[] { };
            this.AssetValue = 200;
            this.SearchRand = true;
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            ulong pixValue = 200;

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, pixValue);
                }
            }

            ulong wuValue = 20;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, wuValue);
                }
            }

            assetValues.Remove(1054691020);

            return assetValues;
        }
    }
}
