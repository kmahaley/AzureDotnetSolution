using MongoDbApplication.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDbApplication.Repositories
{
    public class MongoDbRepository : IRepository
    {
        private const string databaseName = "catalog";

        private const string collectionName = "items";

        public string GetRepositoryName => nameof(MongoDbRepository);

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        private readonly IMongoCollection<Item> itemCollection;

        private readonly ILogger<MongoDbRepository> logger;

        public MongoDbRepository(IMongoClient mongoClient, ILogger<MongoDbRepository> logger)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemCollection = database.GetCollection<Item>(collectionName);
            this.logger = logger;
        }


        public async Task<Item> CreateItemAsync(Item item)
        {
            await itemCollection.InsertOneAsync(item);
            return item;
        }

        public async Task<Guid> DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await itemCollection.DeleteOneAsync(filter);
            return id;
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            var existingItem = await itemCollection.Find(filter).FirstOrDefaultAsync();
            if(existingItem == null)
            {
                logger.LogError($"Document not found in mongodb {id}");
            }
            return existingItem;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> UpdateItemAsync(Guid id, Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, id);
            await itemCollection.ReplaceOneAsync(filter, item);
            return item;
        }
    }
}
