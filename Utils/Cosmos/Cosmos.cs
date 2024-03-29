﻿using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils.Cosmos
{
    public class Cosmos : ICosmos
    {
        private readonly CosmosClient client;
        private readonly Container assetsContainer;

        public Cosmos(IConfiguration config)
        {
            CosmosClientOptions options = new CosmosClientOptions { AllowBulkExecution = true };
            client = new CosmosClient(config["Endpoints:Cosmos"], config["CosmosPrimaryKey"], options);
            assetsContainer = client.GetContainer("caverna", "Assets");
        }

        public async Task<AssetValue> GetAssetValueById(ulong assetId, string key)
        {
            AssetValue assetValue = await assetsContainer.ReadItemAsync<AssetValue>(assetId.ToString(), new PartitionKey(key));

            return assetValue;
        }

        public async Task<AssetValue> CreateAsset(AssetValue assetValue)
        {
            AssetValue av = await assetsContainer.CreateItemAsync<AssetValue>(assetValue, new PartitionKey(assetValue.ProjectId));
            return av;
        }

        public async Task CreateAsset(dynamic assetValue, string projectId)
        {
            await assetsContainer.CreateItemAsync(assetValue, new PartitionKey(projectId));
        }

        public async Task<AssetValue> ReplaceAsset(AssetValue assetValue)
        {
            AssetValue av = await assetsContainer.ReplaceItemAsync<AssetValue>(assetValue, assetValue.Id, new PartitionKey(assetValue.ProjectId));
            return av;
        }

        public async Task ReplaceAsset(dynamic assetValue, string id, string projectId)
        {
            await assetsContainer.ReplaceItemAsync(assetValue, id, new PartitionKey(projectId));
        }

        public async Task<IEnumerable<AssetValue>> GetAssetValues(string projectId)
        {
            string sql = "SELECT * FROM c";
            QueryRequestOptions options = new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey(projectId),
            };

            List<AssetValue> assetValues = new List<AssetValue>();

            FeedIterator<AssetValue> iterator = this.assetsContainer.GetItemQueryIterator<AssetValue>(sql, requestOptions: options);

            while (iterator.HasMoreResults)
            {
                FeedResponse<AssetValue> response = await iterator.ReadNextAsync();
                assetValues.AddRange(response);
            }

            return assetValues;
        }

        public async Task<IEnumerable<dynamic>> GetAssetValuesDynamic(string projectId)
        {
            string sql = "SELECT * FROM c";
            QueryRequestOptions options = new QueryRequestOptions()
            {
                PartitionKey = new PartitionKey(projectId),
            };

            List<dynamic> assetValues = new List<dynamic>();

            FeedIterator<ExpandoObject> iterator = this.assetsContainer.GetItemQueryIterator<ExpandoObject>(sql, requestOptions: options);

            while (iterator.HasMoreResults)
            {
                FeedResponse<ExpandoObject> response = await iterator.ReadNextAsync();
                assetValues.AddRange(response);
            }

            return assetValues;
        }

        public async Task<IEnumerable<AssetValue>> GetAssetValues(string projectId, params string[] projectIds)
        {
            List<string> ids = new List<string>
            {
                projectId
            };
            ids.AddRange(projectIds);

            List<AssetValue> assetValues = new List<AssetValue>();

            FeedIterator<AssetValue> iterator = this.assetsContainer.GetItemLinqQueryable<AssetValue>()
                .Where(av => ids.Contains(av.ProjectId))
                .ToFeedIterator<AssetValue>();

            while (iterator.HasMoreResults)
            {
                FeedResponse<AssetValue> response = await iterator.ReadNextAsync();
                assetValues.AddRange(response);
            }

            return assetValues;
        }

        public async Task<IEnumerable<AssetValue>> GetAssetValuesSql(string sql, string projectId = null)
        {

            QueryRequestOptions options = new QueryRequestOptions();

            if (projectId != null)
            {
                options.PartitionKey = new PartitionKey(projectId);
            }

            List<AssetValue> assetValues = new List<AssetValue>();

            FeedIterator<AssetValue> iterator = this.assetsContainer.GetItemQueryIterator<AssetValue>(sql, requestOptions: options);

            while (iterator.HasMoreResults)
            {
                FeedResponse<AssetValue> response = await iterator.ReadNextAsync();
                assetValues.AddRange(response);
            }

            return assetValues;
        }
    }
}
