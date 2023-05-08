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

            assetValues[1062726124] = 5;
            assetValues[1062726143] = 5;
            assetValues[1062726279] = 5;
            assetValues[1062726463] = 5;
            assetValues[1062727747] = 5;
            assetValues[1062727760] = 5;
            assetValues[1062727775] = 5;
            assetValues[1062746526] = 5;
            assetValues[1062746555] = 5;
            assetValues[1062746764] = 5;
            assetValues[1062746886] = 5;
            assetValues[1062746998] = 5;
            assetValues[1062747173] = 5;
            assetValues[1062747634] = 5;
            assetValues[1062747636] = 5;
            assetValues[1062747826] = 5;
            assetValues[1062748006] = 5;
            assetValues[1062748204] = 5;
            assetValues[1062758966] = 5;
            assetValues[1062759069] = 5;
            assetValues[1062759587] = 5;
            assetValues[1062759961] = 5;
            assetValues[1062776418] = 5;
            assetValues[1062776677] = 5;
            assetValues[1062776679] = 5;

            return assetValues;
        }
    }
}
