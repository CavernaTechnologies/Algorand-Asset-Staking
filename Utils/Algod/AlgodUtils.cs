﻿using Algorand;
using Algorand.V2.Algod;
using Algorand.V2.Algod.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Encoder = Algorand.Encoder;

namespace Utils.Algod
{
    public class AlgodUtils : IAlgodUtils
    {
        private readonly ILogger<AlgodUtils> log;
        private readonly IDefaultApi algod;

        public AlgodUtils(ILogger<AlgodUtils> log, IDefaultApi algod)
        {
            this.log = log;
            this.algod = algod;
        }

        public async Task<PostTransactionsResponse> SubmitTransaction(SignedTransaction signedTransaction)
        {
            byte[] encodedTxBytes = Encoder.EncodeToMsgPack(signedTransaction);
            using MemoryStream ms = new MemoryStream(encodedTxBytes);
            return await algod.TransactionsAsync(ms);
        }

        public async Task<NodeStatusResponse> GetStatusAfterRound(ulong round)
        {
            return await algod.WaitForBlockAfterAsync(round);
        }

        public async Task<NodeStatusResponse> GetStatus()
        {
            return await algod.StatusAsync();
        }

        public async Task<ulong> GetLastRound()
        {
            NodeStatusResponse status = await this.GetStatus();
            return status.LastRound;
        }

        public async Task<TransactionParametersResponse> GetTransactionParams()
        {
            return await this.algod.ParamsAsync();
        }

        public async Task<IEnumerable<string>> SubmitSignedTransactions(IEnumerable<SignedTransaction> signedTransactions)
        {
            List<string> txnIds = new List<string>();

            foreach (SignedTransaction stxn in signedTransactions)
            {
                try
                {
                    PostTransactionsResponse resp = await this.SubmitTransaction(stxn);

                    txnIds.Add(resp.TxId);
                }
                catch (ApiException<ErrorResponse> ex)
                {
                    log.LogError(ex.Result.Message);
                }
            }

            return txnIds;
        }

        public async Task<Algorand.V2.Algod.Model.Account> GetAccount(string address)
        {
            return await this.algod.AccountsAsync(address, Format.Json);
        }

        public async Task<ulong> GetAssetAmount(string address, ulong assetid)
        {
            var acc = await GetAccount(address);

            foreach (var asset in acc.Assets)
            {
                if (asset.AssetId == assetid)
                {
                    return asset.Amount;
                }
            }

            return 0;
        }
    }
}
