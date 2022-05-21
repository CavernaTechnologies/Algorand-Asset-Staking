﻿using Algorand.V2.Algod.Model;
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
            };
            this.RevokedAssets = new ulong[]
            {
                654561741,
                660004771,
                676186493
            };
            this.AssetValue = 50;
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

            //750 Assets

            assetValues[698467453] = 750;
            assetValues[698480336] = 750;
            assetValues[698488421] = 750;
            assetValues[698494814] = 750;
            assetValues[698501141] = 750;
            
            assetValues[698507762] = 750;
            assetValues[698517704] = 750;
            assetValues[698526609] = 750;
            assetValues[698547096] = 750;
            assetValues[698657815] = 750;

            assetValues[698686953] = 750;
            assetValues[698533302] = 750;
            assetValues[704110764] = 750;
            assetValues[704128153] = 750;
            assetValues[708924844] = 750;
            
            assetValues[708965058] = 750;
            assetValues[708948058] = 750;
            assetValues[708969228] = 750;
            assetValues[709016655] = 750;
            assetValues[721243915] = 750;

            assetValues[721176809] = 750;
            assetValues[721195331] = 750;
            assetValues[721204629] = 750;
            assetValues[721225594] = 750;
            assetValues[721235002] = 750;

            assetValues[721322235] = 750;
            assetValues[739133725] = 750;
            assetValues[737750442] = 750;
            assetValues[737756951] = 750;
            assetValues[737762728] = 750;

            assetValues[737769615] = 750;
            assetValues[737778187] = 750;
            assetValues[737784336] = 750;

            //110 Assets

            assetValues[684438622] = 110;
            assetValues[687582413] = 110;
            assetValues[687573278] = 110;
            assetValues[687578619] = 110;
            assetValues[687591588] = 110;

            assetValues[687610963] = 110;
            assetValues[687614444] = 110;
            assetValues[687597589] = 110;
            assetValues[697453848] = 110;
            assetValues[689652667] = 110;

            assetValues[713165860] = 110;
            assetValues[713170980] = 110;
            assetValues[713173804] = 110;
            assetValues[741026307] = 110;

            return assetValues;
        }
    }
}