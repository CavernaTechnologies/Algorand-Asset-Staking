using Algorand.V2.Indexer.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Cosmos;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Holdings
{
    public class NanaHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        private readonly ICosmos cosmos;

        public NanaHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ICosmos cosmos, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 418706707;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] { "NV7D4EFKO5FRXEHRVMEP3LDG6IACFQVJXYYG6KJAGXW2JRBKW3Y7UNQE2Y" };
            this.cosmos = cosmos;
            this.SearchRand = true;
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            IEnumerable<AssetValue> values = await cosmos.GetAssetValues("NanaCoin");

            Dictionary<ulong, ulong> assetValues = values.ToDictionary(av => av.AssetId, av => av.Value);

            //Add bananaBIRD assets
            var account = await AlgodUtils.GetAccount("APEJUA6GF5PWA5GNR7DPEOILKFAMC6ES4H2DO3YMVEDZWBDZIEWZGKZMR4");

            foreach (var asset in account.CreatedAssets)
            {
                assetValues.Add(asset.Index, 20);
            }

            return assetValues;
        }
    }
}
