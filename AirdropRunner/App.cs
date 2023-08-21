using Airdrop;
using Airdrop.AirdropFactories.Unique;
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
using Utils.Indexer;
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
        private readonly IConfiguration config;
        private readonly IHttpClientFactory httpClientFactory;

        public App(ILogger<App> logger, IAlgodUtils algodUtils, IIndexerUtils indexerUtils, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.algodUtils = algodUtils;
            this.indexerUtils = indexerUtils;
            this.config = config;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Run()
        {
            var account = new Algorand.Account(config.GetValue<string>("mnemonic"));

            var fact = new AlcheFactory(indexerUtils, algodUtils);
            //var fact = new AlcheYldyFactory(indexerUtils, algodUtils);
            var collections = await fact.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderByDescending(a => a.Total))
            {
                Console.WriteLine(collection.ToString());
            }

            Console.WriteLine(collections.Sum(c => (double)c.Total));
            Console.WriteLine(collections.Count());

            Console.ReadKey();

            /*List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: account.Address,
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(collection.ToString().Length < 1000 ? collection.ToString() : "Note too long..."),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = account.SignTransaction(txn);

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
            }*/
        }
    }
}
