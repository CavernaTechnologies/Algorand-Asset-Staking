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
    public class BrontoHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        public BrontoHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 875537962;
            this.Decimals = 0;
            this.CreatorAddresses = new string[]
            {
                "DHJYS3DXY6IERYVFS3UV2WJS64UO4FJG2NUKUN6OWPWFDW4UABX2KE7FEE",
                "2KQZYAT2HV4FIVXZIP7UEEJEF7KB4U2B47L7KHSEMULG2NHR47QIZUUS24",
                "EYERHFMTLLBVVDV4RKGKSTMWU4QZ7IG5HUEXQYO3UVX4ABQ5MDNJIKN7HQ",
                "5EIFWC25ABTARJL3G4PFPPJVYLR4OIUX3NYTUDSEGW4UFKJY4KMFXOGBPE"
            };
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.SearchAlandia = true;
            this.AlgoxCollectionNames = new string[] { "dino-eye", "brontos-eye", "raptor-eye", "raptoreye-2" };
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

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
                            assetValues.Add(asset.Index, 10);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, 10);
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
                            assetValues.Add(asset.Index, 20);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, 20);
                    }
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(2).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                if (this.RevokedAddresses != null)
                {
                    foreach (var asset in assets)
                    {
                        if (!this.RevokedAssets.Contains(asset.Index))
                        {
                            assetValues.Add(asset.Index, 125);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, 125);
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
                            assetValues.Add(asset.Index, 250);
                        }
                    }
                }
                else
                {
                    foreach (var asset in assets)
                    {
                        assetValues.Add(asset.Index, 250);
                    }
                }
            }

            return assetValues;
        }
    }
}
