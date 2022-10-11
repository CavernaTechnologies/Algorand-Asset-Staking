using Airdrop;
using Airdrop.AirdropFactories.Holdings;
using Airdrop.AirdropFactories.Liquidity;
using Airdrop.AirdropFactories.Random;
using Airdrop.AirdropFactories.Unique;
using Airdrop.AirdropFactories.AcornPartners;
using Algorand;
using Algorand.V2.Algod.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils.Algod;
using Utils.Cosmos;
using Utils.Indexer;
using Utils.KeyManagers;
using Transaction = Algorand.Transaction;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace AirdropRunner
{
    public class App
    {
        private readonly ILogger<App> logger;
        private readonly IAlgodUtils algodUtils;
        private readonly IIndexerUtils indexerUtils;
        private readonly ICosmos cosmos;
        private readonly IConfiguration config;
        private readonly IKeyManager keyManager;
        private readonly IHttpClientFactory httpClientFactory;

        public App(ILogger<App> logger, IAlgodUtils algodUtils, IIndexerUtils indexerUtils, ICosmos cosmos, IConfiguration config, IKeyManager keyManager, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.algodUtils = algodUtils;
            this.indexerUtils = indexerUtils;
            this.cosmos = cosmos;
            this.config = config;
            this.keyManager = keyManager;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Run()
        {
            Key key = keyManager.CavernaWallet;

            /*ulong acorn = 20000;

            var fact = new GoannaPartnerFactory(indexerUtils, algodUtils, acorn);

            var accounts = await fact.FetchAccounts();

            AirdropUnitCollectionManager manager = new AirdropUnitCollectionManager();

            await fact.FetchAirdropUnitCollections(manager, accounts);
            await new AlchemonPartnerFactory(indexerUtils, algodUtils, cosmos, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new StarfacePartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new MngoPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new BananaMintPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new TrinleyPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new ParlimentAowlPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoGangPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new SwappyPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoOwlPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoBotsPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoPlanetsPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoWhalesPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new FlemishPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new KnitHeadsPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new LingLingPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new MonstiPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new MushiesPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new StupidHorsePartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new TinyWhalesPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new YieldlingPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoSaiyansPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgoBullsPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new DopeCatsPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new PyreneesPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new CorvusPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new AlgorillaPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);
            await new PuffinPartnerFactory(indexerUtils, algodUtils, acorn).FetchAirdropUnitCollections(manager, accounts);

            IEnumerable<AirdropUnitCollection> collections = manager.GetAirdropUnitCollections();*/

            var fact = new LundisHoldingsFactory(indexerUtils, algodUtils);
            var collections = await fact.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderByDescending(a => a.Total))
            {
                Console.WriteLine(collection.ToString());
            }

            Console.WriteLine(collections.Sum(c => (double)c.Total));
            Console.WriteLine(collections.Count());

            Console.ReadKey();

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: key.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(collection.ToString().Length < 1024 ? collection.ToString() : "Note too long..."),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = key.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            Console.WriteLine("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            await this.algodUtils.GetStatusAfterRound(await this.algodUtils.GetLastRound() + 5);

            var transactions = await indexerUtils.GetTransactions(key.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    Console.WriteLine("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }
    }
}
