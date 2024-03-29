﻿using Algorand.V2.Indexer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utils.Indexer
{
    public interface IIndexerUtils
    {
        Task<Account> GetAccount(string address, ExcludeType[] exclude = null);
        Task<IEnumerable<Account>> GetAccounts(ulong assetId, ExcludeType[] exclude = null);
        Task<IEnumerable<Account>> GetAccounts(IEnumerable<ulong> assetId, ExcludeType[] exclude = null);
        Task<IEnumerable<Asset>> GetAsset(IEnumerable<ulong> assetIds);
        Task<Asset> GetAsset(ulong assetId);
        Task<IEnumerable<MiniAssetHolding>> GetBalances(ulong assetId);
        Task<IEnumerable<Asset>> GetCreatedAssets(string address, string prefix = null);
        Task<IEnumerable<Asset>> GetCreatedAssets(IEnumerable<string> addresses, string prefix = null);
        Task<IEnumerable<Transaction>> GetTransactions(string address, ulong? assetId = null, AddressRole? addressRole = null, TxType? txType = null, ulong? currencyGreaterThan = null, ulong? currencyLessThan = null, ulong? minRound = null, ulong? maxRound = null, DateTimeOffset? afterTime = null, DateTimeOffset? beforeTime = null);
        Task<IEnumerable<string>> GetWalletAddresses(string address, ulong? assetId = null, AddressRole? addressRole = null, TxType? txType = null, ulong? currencyGreaterThan = null, ulong? currencyLessThan = null, ulong? minRound = null, ulong? maxRound = null, DateTimeOffset? afterTime = null);
        Task<IEnumerable<string>> GetWalletAddresses(ulong assetId);
        Task<IEnumerable<string>> GetWalletAddresses(IEnumerable<ulong> assetIds);
        Task<IEnumerable<string>> GetWalletAddressesIntersect(IEnumerable<ulong> assetIds);
        Task<dynamic> GetArc69Data(ulong assetId);
    }
}