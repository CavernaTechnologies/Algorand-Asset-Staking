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
    public class GrubHoldingsFactory : ExchangeHoldingsAirdropFactory
    {
        private readonly ICosmos cosmos;

        public GrubHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, ICosmos cosmos, IConfiguration config, IHttpClientFactory httpClient) : base(indexerUtils, algodUtils, config, httpClient.CreateClient())
        {
            this.cosmos = cosmos;
            this.DropAssetId = 787168529;
            this.Decimals = 0;
            this.CreatorAddresses = new string[] {
                "PXGKTWC67EQAJJZX2J22WU6PWUGJGOXNF3Q3KDABHLXTP6P336G4CMF2WI",
                "PXGKO3GQXYKAVNKOFKFIMZLZJE3I3VHYALIU7MHT3CELM4I4JJC4XVNY7M",

                "WISEUJP5PIFAOWYDEVIDWQLKEQFTOSG3ADHE7NJYEAQNFXV5I2R7YLLBQQ",

                "K7ZA75X6HZDN5ENOWQYT4Y7PVM6T7YYPKVOI6ZT6H3TEHCA3MBNPODN2TI",

                "DDYUZA46ZXAJICQMSYIKG3SWNKZND6C5VEH57YVJAV47NDYM3BMDKAC3LQ",
                "PCWJPIR4RJMJZNV2WVXIS6UMIBCQ36VYMLFWQROVFXTZGCFG6TYLT4RWVI",

                "QTZS73C5IW2LF76EPTD6G5MVCXYKS5WD3AQI2WOMQ3TXEZWFYIR3VJOAEI",
                "NIQSYSUHA7BW7CVC5HNRJOHFZHJYSAFA6JMLJJD4LV4S4CODGPUUX6HOCA",
                "NCQFFPYBTWTGGOTECZIWNI5RUIVELYEUKV4HN24DEJUWKPRVIN3CG2VSGI",

                "MNXQMK77MONDGLZQOYQD4F3HGRG6L56FPRNQY3IM2SZOVQWYT33NSWO3LE",
                "AZCR3TDUWKS2DJS3AWYQ6I7LUA2LRT4CFR2U5WZGXWMZV3RNJURPZYFWBQ",

                "Y6BWPPICS2HYP2KRVZCDASKJHJLBU7EZCA2JFSLMPYGC6FPDLRMF7UMVUQ",

                "LOVEOJUJL4GDH7VLETAHG3BOFE4UDTMLEDMBYLQPICWCWCVTFKCQJJCEDE",
            };
            this.SearchRand = true;
            this.SearchAlandia = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[]
            {
                "pixel-geko",
                "99-wise-uncles",
                "moofys",
                "gekofam",
                "mia",
                "doofy",
                "dooofy",
                "algo-rpg-npc",
                "99-foolish-uncles",
                "from-algorand-with-love",
            };
        }

        public async override Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            ulong pixValue = 72;

            foreach (string creatorAddress in this.CreatorAddresses.Take(2))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, pixValue);
                }
            }

            ulong wuValue = 99;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(2).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, wuValue);
                }
            }

            ulong moofyValue = 48;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(3).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, moofyValue);
                }
            }

            ulong doofyValue = 8;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(6).Take(3))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, doofyValue);
                }
            }

            ulong rpgValue = 12;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(9).Take(2))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, rpgValue);
                }
            }

            ulong foolishValue = 22;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(11).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, foolishValue);
                }
            }

            ulong loveValue = 44;

            foreach (string creatorAddress in this.CreatorAddresses.Skip(12).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;

                foreach (var asset in assets.Where(a => a.Params.Total == 1))
                {
                    assetValues.Add(asset.Index, loveValue);
                }
            }

            IEnumerable<AssetValue> values = await cosmos.GetAssetValues("Gekofam", "mia");

            foreach (AssetValue av in values)
            {
                assetValues.Add(av.AssetId, av.Value);
            }

            return assetValues;
        }
    }
}
