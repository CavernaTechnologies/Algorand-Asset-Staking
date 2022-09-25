using Algorand.V2.Indexer.Model;
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
    public class DrakkHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        private readonly ICosmos cosmos;

        public DrakkHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ICosmos cosmos, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 560039769;
            this.Decimals = 6;
            this.CreatorAddresses = new string[] { "UXXYI4CPUIZ27UTWNL42VO7EG5LQGRQHLNKD2JIPVHEBZ7T7JXYOHAGW4A" };
            this.RevokedAddresses = new string[] { "7VGVH2G7R7MM7HNRV6AFIC7HP3WXIK6CZ42GGSK6LRRMREGNOQXRATBMLA", "UXXYI4CPUIZ27UTWNL42VO7EG5LQGRQHLNKD2JIPVHEBZ7T7JXYOHAGW4A", "B6PH4HYVR6MJOOQMFFJ3VFNHJO3NTBGHQBCFOHT4KKI6CGWI3DW2AZBMNM", "5MJJG6FPU43HRHGOMHWZT5ZGNCUD7DHICWF45GJVIEZDRQKS3M7KWMJWAQ" };
            this.cosmos = cosmos;
            this.SearchRand = true;
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            IEnumerable<AssetValue> assets = await cosmos.GetAssetValues("Drakk");

            Dictionary<ulong, ulong> assetValues = assets.ToDictionary(av => av.AssetId, av => (ulong) (av.Value * Math.Pow(10, this.Decimals)));
            assetValues[572918686] = 1;
            assetValues[776390976] = 1;
            assetValues[776405295] = 1;

            return assetValues;
        }
    }
}
