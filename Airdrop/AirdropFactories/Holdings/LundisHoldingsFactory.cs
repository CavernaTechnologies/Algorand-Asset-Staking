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
    public class LundisHoldingsFactory : HoldingsAirdropFactory
    {
        public LundisHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils) : base(indexerUtils, algodUtils)
        {
            this.DropAssetId = 658399558;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "PFDZQWMRT2KTJTB3VUGDOILNCNSB63ILZL4XMBHYECBOV24LGTID4YRFPM",
                "7PVEEP2CS77VJEYZGW2IIGZ63P5CO557XRNKBRPTIREKLET7A4G62W4CQA",
                "TXZ3AKZLIKNNT3OBQOMTSYWC7AK7CIVSZZIDCSTONXNTX44LQIRU6ELFDA",
                "UB6E3ACF7MHSAY5LRRNUHMW55IAQGGWOVADTVDNMF7JL7CA25GO7V4F2MI",
                "ONYCV746UXUU337323KNMNCAMZ5YLC6U5KLTOAFD5AVDDRMYVDOJHXMSVQ",
                "KEFVKQAEKQBFHS5UJMC3JL7OSODFIEBYRAYXDXL5UL4WEBHSVMB2EBITP4"
            };
            this.RevokedAssets = new ulong[]
            {
                654561741,
                660004771,
                676186493
            };
            this.RevokedAddresses = new string[]
            {
                "LUNDIW5HP5KWUG6JTXHMDB5XPNXQ43OIBXET3RTBXNVJISQUTBKWONTGNI",
                "UB6E3ACF7MHSAY5LRRNUHMW55IAQGGWOVADTVDNMF7JL7CA25GO7V4F2MI"
            };
            this.AssetValue = 10;
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            ulong tier3 = 1250;
            ulong tier2 = 500;
            ulong tier1 = 200;
            ulong notier = 75;
            ulong statepoof = 30;

            foreach (string creatorAddress in this.CreatorAddresses.Take(3))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, notier);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, notier);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(3).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, statepoof);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, statepoof);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(4).Take(2))
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

            //tier3 Assets

            assetValues[698467453] = tier3;
            assetValues[698480336] = tier3;
            assetValues[698488421] = tier3;
            assetValues[698494814] = tier3;
            assetValues[698501141] = tier3;
            
            assetValues[698507762] = tier3;
            assetValues[698517704] = tier3;
            assetValues[698526609] = tier3;
            assetValues[698547096] = tier3;
            assetValues[698657815] = tier3;

            assetValues[698686953] = tier3;
            assetValues[698533302] = tier3;
            assetValues[704110764] = tier3;
            assetValues[704128153] = tier3;
            assetValues[708924844] = tier3;
            
            assetValues[708965058] = tier3;
            assetValues[708948058] = tier3;
            assetValues[708969228] = tier3;
            assetValues[709016655] = tier3;
            assetValues[721243915] = tier3;

            assetValues[721176809] = tier3;
            assetValues[721195331] = tier3;
            assetValues[721204629] = tier3;
            assetValues[721225594] = tier3;
            assetValues[721235002] = tier3;

            assetValues[721322235] = tier3;
            assetValues[739133725] = tier3;
            assetValues[737750442] = tier3;
            assetValues[737756951] = tier3;
            assetValues[737762728] = tier3;

            assetValues[737769615] = tier3;
            assetValues[737778187] = tier3;
            assetValues[737784336] = tier3;
            assetValues[756545371] = tier3;
            assetValues[762227028] = tier3;

            assetValues[762232337] = tier3;
            assetValues[762205371] = tier3;
            assetValues[762236867] = tier3;
            assetValues[762213644] = tier3;
            assetValues[762221228] = tier3;

            assetValues[773081051] = tier3;
            assetValues[773114659] = tier3;
            assetValues[773085590] = tier3;
            assetValues[785720995] = tier3;
            assetValues[798634710] = tier3;

            assetValues[798642989] = tier3;
            assetValues[798638793] = tier3;
            assetValues[798830332] = tier3;
            assetValues[799367574] = tier3;
            assetValues[806274588] = tier3;

            assetValues[809304789] = tier3;
            assetValues[819535209] = tier3;
            assetValues[819551980] = tier3;
            assetValues[821756365] = tier3;
            assetValues[835274204] = tier3;

            assetValues[833585953] = tier3;
            assetValues[835493754] = tier3;
            assetValues[837340710] = tier3;
            assetValues[837345892] = tier3;
            assetValues[838391718] = tier3;

            assetValues[838148671] = tier3;
            assetValues[839144345] = tier3;
            assetValues[798830332] = tier3;
            assetValues[862386572] = tier3;
            assetValues[862487649] = tier3;

            assetValues[862493977] = tier3;
            assetValues[862388352] = tier3;
            assetValues[863335775] = tier3;
            assetValues[862381532] = tier3;
            assetValues[867796704] = tier3;

            assetValues[879781046] = tier3;
            assetValues[931133436] = tier3;
            assetValues[1002815902] = tier3;
            assetValues[1002826253] = tier3;
            assetValues[1007584453] = tier3;

            assetValues[1013341476] = tier3;

            //tier2 Assets

            assetValues[749820378] = tier2;
            assetValues[750010095] = tier2;
            assetValues[750142642] = tier2;
            assetValues[750934239] = tier2;
            assetValues[751553397] = tier2;

            assetValues[751865805] = tier2;
            assetValues[752200818] = tier2;
            assetValues[752945710] = tier2;
            assetValues[752948741] = tier2;
            assetValues[752961942] = tier2;

            assetValues[753873322] = tier2;
            assetValues[773072296] = tier2;
            assetValues[773063834] = tier2;
            assetValues[781054741] = tier2;
            assetValues[793585294] = tier2;

            assetValues[804514128] = tier2;
            assetValues[811020055] = tier2;
            assetValues[812192148] = tier2;
            assetValues[812194599] = tier2;
            assetValues[819515831] = tier2;

            assetValues[819518789] = tier2;
            assetValues[835364862] = tier2;
            assetValues[835366017] = tier2;
            assetValues[835370994] = tier2;
            assetValues[835367463] = tier2;

            assetValues[835371877] = tier2;

            //110 Assets

            assetValues[684438622] = tier1;
            assetValues[687582413] = tier1;
            assetValues[687573278] = tier1;
            assetValues[687578619] = tier1;
            assetValues[687591588] = tier1;

            assetValues[687610963] = tier1;
            assetValues[687614444] = tier1;
            assetValues[687597589] = tier1;
            assetValues[697453848] = tier1;
            assetValues[689652667] = tier1;

            assetValues[713165860] = tier1;
            assetValues[713170980] = tier1;
            assetValues[713173804] = tier1;
            assetValues[741026307] = tier1;
            assetValues[752998901] = tier1;

            assetValues[753002063] = tier1;
            assetValues[756517824] = tier1;
            assetValues[756520037] = tier1;
            assetValues[760062139] = tier1;
            assetValues[760057914] = tier1;

            assetValues[781050181] = tier1;
            assetValues[793590644] = tier1;
            assetValues[826108477] = tier1;

            // 5 assets

            assetValues[929928221] = 5;
            assetValues[929928230] = 5;
            assetValues[929928238] = 5;
            assetValues[929928239] = 5;
            assetValues[929928241] = 5;
            assetValues[929928242] = 5;
            assetValues[929928277] = 5;
            assetValues[929928321] = 5;
            assetValues[929928359] = 5;
            assetValues[929928396] = 5;
            assetValues[903239263] = 5;
            assetValues[907245578] = 5;
            assetValues[960332297] = 5;
            assetValues[971517283] = 5;
            assetValues[971518096] = 5;

            return assetValues;
        }
    }
}
