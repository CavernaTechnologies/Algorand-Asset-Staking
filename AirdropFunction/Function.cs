using Airdrop;
using Airdrop.AirdropFactories.Holdings;
using Airdrop.AirdropFactories.Liquidity;
using Algorand;
using Algorand.V2.Algod.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
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

namespace AirdropFunction
{
    public class Function
    {
        public const string CryptoBunnyHoldingsAirdropSchedule = "0 0 15 * * Mon";
        public const string NanaHoldingsAirdropSchedule = "0 0 14 * * Mon,Fri";
        public const string ShrimpHoldingsAirdropSchedule = "0 0 16 * * Mon";
        public const string MantisHoldingsAirdropSchedule = "0 0 18 * * *";
        public const string AlvaHoldingsAirdropSchedule = "0 0 16 * * Sun,Tue,Thu,Sat";
        public const string GooseHoldingsAirdropSchedule = "0 0 16 * * Mon,Thu";
        public const string PyreneesHoldingsAirdropSchedule = "0 0 0 * * Mon";
        public const string GrubHoldingsAirdropSchedule = "0 0 19 * * Thu";
        public const string HootHoldingsAirdropSchedule = "0 0 19 * * Mon";
        public const string BuzzyBeeHoldingsAirdropSchedule = "0 0 19 * * Fri";
        public const string BrontoHoldingsAirdropSchedule = "0 0 21 * * Wed";
        public const string BiteHoldingsAirdropSchedule = "0 0 0 * * Thu";
        public const string YarnHoldingsAirdropSchedule = "0 30 23 * * Thu";
        public const string CreamHoldingsAirdropSchedule = "0 20 20 * * Sun";
        public const string BallstarHoldingsAirdropSchedule = "0 0 22 * * Mon";
        public const string GrowHoldingsAirdropSchedule = "0 0 1 * * Mon";
        public const string MagoHoldingsAirdropSchedule = "0 0 15 * * Mon";
        public const string AlgoleaguesHoldingsAirdropSchedule = "0 0 23 * * Sun";
        public const string BlopHoldingsAirdropSchedule = "0 0 13 * * *";
        public const string FrogHoldingsAirdropSchedule = "0 0 23 * * Fri";
        private readonly IAlgodUtils algodUtils;
        private readonly IIndexerUtils indexerUtils;
        private readonly ICosmos cosmos;
        private readonly IConfiguration config;
        private readonly IKeyManager keyManager;
        private readonly IHttpClientFactory httpClientFactory;

        public Function(IAlgodUtils algodUtils, IIndexerUtils indexerUtils, ICosmos cosmos, IConfiguration config, IKeyManager keyManager, IHttpClientFactory httpClientFactory)
        {
            this.algodUtils = algodUtils;
            this.indexerUtils = indexerUtils;
            this.cosmos = cosmos;
            this.config = config;
            this.keyManager = keyManager;
            this.httpClientFactory = httpClientFactory;
        }

        [FunctionName("NanaHoldingsAirdrop")]
        public async Task NanaHoldingsAirdrop([TimerTrigger(NanaHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            NanaHoldingsFactory factory = new NanaHoldingsFactory(this.indexerUtils, this.algodUtils, this.cosmos, this.config, this.httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            log.LogInformation("Total drop: " + collections.Sum(a => (double)a.Total));
            log.LogInformation("Number of drops: " + collections.Count());

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("CryptoBunnyHoldingsAirdrop")]
        public async Task CryptoBunnyHoldingsAirdrop([TimerTrigger(CryptoBunnyHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            CryptoBunnyHoldingsFactory factory = new CryptoBunnyHoldingsFactory(this.indexerUtils, this.algodUtils, this.cosmos);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            log.LogInformation("Total drop: " + collections.Sum(a => (double)a.Total));
            log.LogInformation("Number of drops: " + collections.Count());

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("ShrimpHoldingsAirdrop")]
        public async Task ShrimpHoldingsAirdrop([TimerTrigger(ShrimpHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key shrimpKey = this.keyManager.ShrimpWallet;
            ShrimpHoldingsFactory factory = new ShrimpHoldingsFactory(this.indexerUtils, this.algodUtils, this.cosmos, this.config, this.httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            log.LogInformation("Total drop: " + collections.Sum(a => (double)a.Total));
            log.LogInformation("Number of drops: " + collections.Count());

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: shrimpKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = shrimpKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(shrimpKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("MantisHoldingsAirdrop")]
        public async Task MantisHoldingsAirdrop([TimerTrigger(MantisHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            MantisHoldingsFactory factory = new MantisHoldingsFactory(this.indexerUtils, this.algodUtils);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            log.LogInformation("Total drop: " + collections.Sum(a => (double)a.Total));
            log.LogInformation("Number of drops: " + collections.Count());

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("AlvaHoldingsAirdrop")]
        public async Task AlvaHoldingsAirdrop([TimerTrigger(AlvaHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            AlvaHoldingsFactory factory = new AlvaHoldingsFactory(this.indexerUtils, this.algodUtils);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("GooseHoldingsAirdrop")]
        public async Task GooseHoldingsAirdrop([TimerTrigger(GooseHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            GooseHoldingsFactory factory = new GooseHoldingsFactory(this.indexerUtils, this.algodUtils);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("PyreneesHoldingsAirdrop")]
        public async Task PyreneesHoldingsAirdrop([TimerTrigger(PyreneesHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            PyreneesHoldingsFactory factory = new PyreneesHoldingsFactory(this.indexerUtils, this.algodUtils, this.config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("GrubHoldingsAirdrop")]
        public async Task GrubHoldingsAirdrop([TimerTrigger(GrubHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            GrubHoldingsFactory factory = new GrubHoldingsFactory(this.indexerUtils, this.algodUtils, this.cosmos, this.config, this.httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("HootHoldingsAirdrop")]
        public async Task HootHoldingsAirdrop([TimerTrigger(HootHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            HootAirdropFactory factory = new HootAirdropFactory(this.indexerUtils, this.algodUtils, this.cosmos);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("BuzzyBeeHoldingsAirdrop")]
        public async Task BuzzyBeeHoldingsAirdrop([TimerTrigger(BuzzyBeeHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            BuzzyBeesHoldingsFactory factory = new BuzzyBeesHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("BrontoHoldingsAirdrop")]
        public async Task BrontoHoldingsAirdrop([TimerTrigger(BrontoHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            BrontoHoldingsFactory factory = new BrontoHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("BiteHoldingsAirdrop")]
        public async Task BiteHoldingsAirdrop([TimerTrigger(BiteHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            BiteHoldingsFactory factory = new BiteHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("YarnHoldingsAirdrop")]
        public async Task YarnHoldingsAirdrop([TimerTrigger(YarnHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            YarnHoldingsFactory factory = new YarnHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("CreamHoldingsAirdrop")]
        public async Task CreamHoldingsAirdrop([TimerTrigger(CreamHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            CreamHoldingsFactory factory = new CreamHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("BallstarHoldingsAirdrop")]
        public async Task BallstarHoldingsAirdrop([TimerTrigger(BallstarHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            BallstarHoldingsFactory factory = new BallstarHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("GrowHoldingsAirdrop")]
        public async Task GrowHoldingsAirdrop([TimerTrigger(GrowHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            HighHogHoldingsFactory factory = new HighHogHoldingsFactory(this.indexerUtils, this.algodUtils, config, httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("MagoHoldingsAirdrop")]
        public async Task MagoHoldingsAirdrop([TimerTrigger(MagoHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            MagoHoldingsFactory factory = new MagoHoldingsFactory(this.indexerUtils, this.algodUtils);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
                            assetReceiver: address,
                            assetCloseTo: null,
                            assetAmount: collection.Total,
                            flatFee: transactionParameters.Fee,
                            firstRound: transactionParameters.LastRound,
                            lastRound: transactionParameters.LastRound + 1000,
                            note: Encoding.UTF8.GetBytes(""),
                            genesisID: transactionParameters.GenesisId,
                            genesisHash: new Algorand.Digest(transactionParameters.GenesisHash),
                            assetIndex: collection.DropAssetId
                        );

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("AlgoleaguesHoldingsAirdrop")]
        public async Task AlgoleaguesHoldingsAirdrop([TimerTrigger(AlgoleaguesHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            AlgoleaguesHoldingsFactory factory = new AlgoleaguesHoldingsFactory(this.indexerUtils, this.algodUtils);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
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

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("BlopHoldingsAirdrop")]
        public async Task BlopHoldingsAirdrop([TimerTrigger(BlopHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            BlopHoldingsFactory factory = new BlopHoldingsFactory(this.indexerUtils, this.algodUtils, this.config, this.httpClientFactory);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
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

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }

        [FunctionName("FrogHoldingsAirdrop")]
        public async Task FrogHoldingsAirdrop([TimerTrigger(FrogHoldingsAirdropSchedule)] TimerInfo myTimer, ILogger log)
        {
            if (myTimer.IsPastDue)
            {
                log.LogError("Airdrop is past due. Erroring out...");
                throw new Exception("Airdrop past due");
            }

            Key CavernaKey = this.keyManager.CavernaWallet;
            FrogHoldingsFactory factory = new FrogHoldingsFactory(this.indexerUtils, this.algodUtils);
            IEnumerable<AirdropUnitCollection> collections = await factory.FetchAirdropUnitCollections();

            foreach (AirdropUnitCollection collection in collections.OrderBy(c => c.Wallet))
            {
                log.LogInformation($"{collection.Wallet} : {collection.Total}");
            }

            ulong total = (ulong)collections.Sum(a => (double)a.Total);
            log.LogInformation("Total drop: " + total);
            log.LogInformation("Number of drops: " + collections.Count());

            ulong curr = await algodUtils.GetAssetAmount(CavernaKey.ToString(), factory.DropAssetId);

            if (curr < total)
            {
                log.LogError("Insufficient funds. Erroring out...");
                throw new Exception("Insufficient Funds");
            }

            List<SignedTransaction> signedTransactions = new List<SignedTransaction>();
            TransactionParametersResponse transactionParameters = await algodUtils.GetTransactionParams();

            foreach (AirdropUnitCollection collection in collections)
            {
                try
                {
                    Address address = new Address(collection.Wallet);

                    Transaction txn = Transaction.CreateAssetTransferTransaction(
                            assetSender: CavernaKey.GetAddress(),
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

                    SignedTransaction stxn = CavernaKey.SignTransaction(txn);

                    signedTransactions.Add(stxn);
                }
                catch (ArgumentException)
                {
                    log.LogWarning(collection.Wallet + " is an invalid address");
                }
            }

            ulong startingRound = await this.algodUtils.GetLastRound();
            log.LogInformation("Starting round: " + startingRound);

            await this.algodUtils.SubmitSignedTransactions(signedTransactions);

            ulong lastRound = await this.algodUtils.GetLastRound();
            await this.algodUtils.GetStatusAfterRound(lastRound + 10);

            var transactions = await indexerUtils.GetTransactions(CavernaKey.ToString(), addressRole: Algorand.V2.Indexer.Model.AddressRole.Sender, txType: Algorand.V2.Indexer.Model.TxType.Axfer, minRound: startingRound);
            HashSet<string> txIds = transactions.Select(t => t.Id).ToHashSet();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                if (!txIds.Contains(stxn.transactionID))
                {
                    log.LogWarning("Failed to drop: " + stxn.tx.assetReceiver);
                }
            }
        }
    }
}
