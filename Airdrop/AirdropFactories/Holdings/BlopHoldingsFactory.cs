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
    public class BlopHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public BlopHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 896650094;
            this.Decimals = 2;
            this.AssetValue = 10000;
            this.CreatorAddresses = new string[] { "U4WMQCXM7655FIJS6AYBKC6A3R74X2XZ3T4SPJV52BQVJ5WF7HTYKGQQ74" };
            this.RevokedAddresses = new string[] { "U4WMQCXM7655FIJS6AYBKC6A3R74X2XZ3T4SPJV52BQVJ5WF7HTYKGQQ74" };
            this.RevokedAssets = new ulong[] { };
            this.SearchAlandia = true;
            this.SearchAlgox = true;
            this.SearchRand = true;
            this.AlgoxCollectionNames = new string[] { "alg-octopus" };
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            foreach (string creatorAddress in this.CreatorAddresses)
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, this.AssetValue);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, this.AssetValue);
                    }
                }
            }

            assetValues[668390783] = 20000;
            assetValues[714850998] = 20000;
            assetValues[732602696] = 20000;
            assetValues[753868606] = 20000;
            assetValues[783095260] = 20000;
            assetValues[794300275] = 20000;
            assetValues[813827809] = 20000;
            assetValues[851894366] = 20000;
            assetValues[851895857] = 20000;
            assetValues[854327860] = 20000;
            assetValues[854437765] = 20000;
            assetValues[857226544] = 20000;
            assetValues[857281826] = 20000;
            assetValues[861125213] = 20000;
            assetValues[864390995] = 20000;
            assetValues[870037060] = 20000;
            assetValues[886495739] = 20000;
            assetValues[889533367] = 20000;
            assetValues[995505599] = 20000;
            assetValues[995504808] = 20000;
            assetValues[986277663] = 20000;
            assetValues[982523796] = 20000;
            assetValues[979548161] = 20000;
            assetValues[966134707] = 20000;
            assetValues[963784801] = 20000;
            assetValues[956469717] = 20000;
            assetValues[956467978] = 20000;
            assetValues[950661430] = 20000;
            assetValues[939703432] = 20000;
            assetValues[939702320] = 20000;
            assetValues[932723083] = 20000;
            assetValues[929060665] = 20000;
            assetValues[929059760] = 20000;
            assetValues[921198891] = 20000;
            assetValues[921196949] = 20000;
            assetValues[919965273] = 20000;
            assetValues[912906887] = 20000;
            assetValues[908977667] = 20000;
            assetValues[903131053] = 20000;

            assetValues[827254442] = 30000;
            assetValues[908977667] = 30000;

            return assetValues;
        }
    }
}
