﻿using Algorand;
using Algorand.V2.Model;
using System;
using System.Collections.Generic;
using Util.Cosmos;

namespace Util
{
    public interface IAlgoApi
    {
        Algorand.V2.Model.Account GetAccountByAddress(string walletAddress);
        Asset GetAssetById(long assetId);
        IEnumerable<Asset> GetAssetById(IEnumerable<long> assetIds);
        long GetAssetDecimals(int assetId);
        IEnumerable<AssetHolding> GetAssetsByAddress(string walletAddress);
        IEnumerable<string> GetWalletAddressesWithAsset(long assetId);
        IEnumerable<string> GetWalletAddressesWithAsset(long assetId, params long[] assetIds);
        TransactionParametersResponse GetTransactionParams();
        IEnumerable<AssetValue> GetAccountAssetValues(string walletAddress, string unitNameContainsString = "", string projectId = null, long? value = null);
        TransactionsResponse GetAssetTransactions(string senderAddress, string addressRole, long assetId, long? currencyGreaterThan = null, long? currencyLessThan = null, long? minRound = null, long limit = 100, string next = null);
        TransactionsResponse GetAssetTransactions(string senderAddress, long assetId, DateTime afterTime, long limit = 100, string next = null);
        IEnumerable<string> GetAddressesSent(string senderAddress, long assetId, long minRound, long limit = 100);
        PendingTransactionResponse SubmitTransactionWait(SignedTransaction signedTxn);
        PostTransactionsResponse SubmitTransaction(SignedTransaction signedTxn);
        NodeStatusResponse GetStatus();
        long? GetLastRound();
        NodeStatusResponse GetStatusAfterRound(long round);
        long GetAssetLowest(string address, long assetId, long assetAmount, DateTime afterTime, long limit = 100);
        IDictionary<string, long> GetAlgoReceived(string receiverAddress);
    }
}