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
    public class CreamHoldingsFactory : ExchangeHoldingsAirdropFactory
    {

        public CreamHoldingsFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils, IConfiguration config, IHttpClientFactory httpClientFactory) : base(indexerUtils, algodUtils, config, httpClientFactory.CreateClient())
        {
            this.DropAssetId = 895780292;
            this.Decimals = 0;
            this.CreatorAddresses = new string[]
            {
                "C3GRANDYJXZRIGSDKTQUBRTHP53NPSPOWV2325JO6FSUE2M7XPKOZ7IID4",
                "I57HTTFYXHHAI7XXMEDJ6EKVML6XUNILFVEN2HSQQVGMX56ZUWWQFWUJUY",
                "RP3UUCLLANRVDXCKKVTBV5BY5OPNGFWB5YEIKK7KTR6M7CQAMIB5MV4SRQ",
                "CONEZ362OXBZTZJNAZVPME4JNH77TAZ74LAJGSIEKYISOX3DH5MVYAQUF4"
            };
            this.RevokedAddresses = new string[]
            {
                "C3GRANDYJXZRIGSDKTQUBRTHP53NPSPOWV2325JO6FSUE2M7XPKOZ7IID4",
                "I57HTTFYXHHAI7XXMEDJ6EKVML6XUNILFVEN2HSQQVGMX56ZUWWQFWUJUY",
                "RP3UUCLLANRVDXCKKVTBV5BY5OPNGFWB5YEIKK7KTR6M7CQAMIB5MV4SRQ",
                "CONEZ362OXBZTZJNAZVPME4JNH77TAZ74LAJGSIEKYISOX3DH5MVYAQUF4"
            };
            this.SearchAlandia = true;
            this.SearchRand = true;
            this.SearchAlgox = true;
            this.AlgoxCollectionNames = new string[] { "randy-cones", "randy-pixels", "8-bit-cones" };
        }

        public override async Task<IDictionary<ulong, ulong>> FetchAssetValues()
        {
            Dictionary<ulong, ulong> assetValues = new Dictionary<ulong, ulong>();

            foreach (string creatorAddress in this.CreatorAddresses.Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 50);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(1).Take(2))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 15);
                }
            }

            foreach (string creatorAddress in this.CreatorAddresses.Skip(3).Take(1))
            {
                Account account = await this.AlgodUtils.GetAccount(creatorAddress);
                var assets = account.CreatedAssets;
                foreach (var asset in assets)
                {
                    assetValues.Add(asset.Index, 15);
                }
            }

            var special = new ulong[] { 347651906, 357426823, 359481762, 367021386, 367022103, 367964905, 376240235, 377971517, 383882272, 383882965, 383883486, 394018370, 394023514, 394027046, 394045074, 426666398, 426668073, 426669153, 448940242, 448942013, 448942852, 448944565, 456927167, 458857903, 464922140, 465784756, 465786382, 479713669, 479714159, 479714690, 509313434, 539486996, 539493113, 539496085, 550479103, 565655418, 569892360, 569893015, 580770869, 580772330, 600455743, 600459038, 610351994, 610353102, 634931864, 634933194, 659544605, 659545781, 667692196, 681544984, 692368452, 896702570, 695179360, 700699974, 715906460, 358807656, 376284058, 602191303, 358807656, 471425028, 471432143, 603495392, 378737303, 378740470, 378749021, 378745878, 378741573, 378743513, 378747756, 378749900, 376467532, 376465336, 376458480, 376451340, 893904996, 647399151, 378739330, 378750823, 893907615, 378746838, 378742487, 892658507, 762045471, 887050348, 376461939, 893906446, 927222821, 933835994, 926582825, 962995770, 945618427 };

            foreach (var id in special)
            {
                assetValues[id] = 400;
            }

            assetValues.Remove(755007963);
            assetValues.Remove(721340445);

            return assetValues;
        }
    }
}
