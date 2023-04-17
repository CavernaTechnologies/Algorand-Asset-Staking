using Algorand.V2.Algod.Model;
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
    public class AlgaeHoldingsFactory : HoldingsAirdropFactory
    {
        public AlgaeHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 1067414308;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "I36V2745GNHNEE6CXZSAB76DCRTFHN7RLBRFWVZ6DAXUF6PFSMHZDQK57I",
                "YRBKG74MVJUVFR7BKOMA4YNCAG4L3BJVOVIP7QMDPOVOJBGCLXZHSJPGRY",
            };
            this.RevokedAssets = new ulong[]
            {
            };
            this.RevokedAddresses = new string[]
            {
                "I36V2745GNHNEE6CXZSAB76DCRTFHN7RLBRFWVZ6DAXUF6PFSMHZDQK57I",
                "YRBKG74MVJUVFR7BKOMA4YNCAG4L3BJVOVIP7QMDPOVOJBGCLXZHSJPGRY",
            };
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            ulong seafolk = 30;
            ulong divers = 45;
            ulong seaking = 60;
            ulong seagod = 80;

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, seafolk);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, seafolk);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, seagod);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, seagod);
                    }
                }
            }

            // divers

            assetValues[783352241] = divers;
            assetValues[783354264] = divers;
            assetValues[983605646] = divers;
            assetValues[790580532] = divers;
            assetValues[795190359] = divers;
            assetValues[802786345] = divers;
            assetValues[807087063] = divers;
            assetValues[807087733] = divers;
            assetValues[819146646] = divers;
            assetValues[825663969] = divers;
            assetValues[829446758] = divers;
            assetValues[829520128] = divers;
            assetValues[833371174] = divers;
            assetValues[838837889] = divers;
            assetValues[847625759] = divers;
            assetValues[852098424] = divers;
            assetValues[856980215] = divers;
            assetValues[862868961] = divers;
            assetValues[862869944] = divers;
            assetValues[862871675] = divers;
            assetValues[870824942] = divers;
            assetValues[871192217] = divers;
            assetValues[871260292] = divers;
            assetValues[878280465] = divers;
            assetValues[885338388] = divers;
            assetValues[895438987] = divers;
            assetValues[901528233] = divers;
            assetValues[910146198] = divers;
            assetValues[917727050] = divers;
            assetValues[917728592] = divers;
            assetValues[917729797] = divers;
            assetValues[923497994] = divers;
            assetValues[923501295] = divers;
            assetValues[925792934] = divers;
            assetValues[928472270] = divers;
            assetValues[937532153] = divers;
            assetValues[946337356] = divers;
            assetValues[948074381] = divers;
            assetValues[955101547] = divers;
            assetValues[962265895] = divers;
            assetValues[972518199] = divers;
            assetValues[978524136] = divers;

            //seaking

            assetValues[960197363] = seaking;
            assetValues[811853429] = seaking;
            assetValues[831365384] = seaking;
            assetValues[840049404] = seaking;
            assetValues[854044378] = seaking;
            assetValues[876734213] = seaking;
            assetValues[901531822] = seaking;
            assetValues[932225948] = seaking;
            assetValues[985566007] = seaking;
            assetValues[986439702] = seaking;
            assetValues[988238364] = seaking;

            return assetValues;
        }
    }
}
