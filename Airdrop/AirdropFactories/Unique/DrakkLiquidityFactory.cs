using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Indexer;

namespace Airdrop.AirdropFactories.Unique
{
    public class DrakkLiquidityFactory
    {
        public IAlgodUtils AlgodUtils { get; }
        public IIndexerUtils IndexerUtils { get; }
        public ulong DropAssetId { get; set; }
        public ulong StakeFlagId { get; set; }
        public ulong Decimals { get; set; }

        public string[] RevokedAddresses { get; set; }

    public DrakkLiquidityFactory(IIndexerUtils indexerUtils, IAlgodUtils algodUtils)
        {
            this.IndexerUtils = indexerUtils;
            this.AlgodUtils = algodUtils;
            this.DropAssetId = 560039769;
            this.Decimals = 6;
            this.RevokedAddresses = new string[] { "7VGVH2G7R7MM7HNRV6AFIC7HP3WXIK6CZ42GGSK6LRRMREGNOQXRATBMLA", "UXXYI4CPUIZ27UTWNL42VO7EG5LQGRQHLNKD2JIPVHEBZ7T7JXYOHAGW4A", "B6PH4HYVR6MJOOQMFFJ3VFNHJO3NTBGHQBCFOHT4KKI6CGWI3DW2AZBMNM", "5MJJG6FPU43HRHGOMHWZT5ZGNCUD7DHICWF45GJVIEZDRQKS3M7KWMJWAQ", "CSRHTGQILWXV4ZUQKRKTVQMPT6X6EASEPJWVFF6R4JQSMUMVJ6KAPBBTRQ" };
        }

        public async Task<IEnumerable<AirdropUnitCollection>> FetchAirdropUnitCollections()
        {
            HashSet<string> addresses = (await IndexerUtils.GetWalletAddresses(this.DropAssetId)).Except(this.RevokedAddresses).ToHashSet<string>();

            AirdropUnitCollectionManager m = new AirdropUnitCollectionManager();

            var ids = new ulong[] { 1002660598, 776390976, 776405295 };

            foreach (ulong id in ids)
            {
                var balances = await IndexerUtils.GetBalances(id);

                foreach (var balance in balances)
                {
                    if (addresses.Contains(balance.Address))
                    {
                        m.AddAirdropUnit(new AirdropUnit(balance.Address, DropAssetId, id, .5, balance.Amount, true));
                    }
                }
            }

            return m.GetAirdropUnitCollections();
        }
    }
}
