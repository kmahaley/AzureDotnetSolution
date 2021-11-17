using CoreWebApplication.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CoreWebApplication.Repositories
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


        public Item CreateItem(Item item)
        {
            itemCollection.InsertOne(item);
            return item;
        }

        public Guid DeleteItem(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            itemCollection.DeleteOne(filter);
            return id;
        }

        public Item GetItem(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            var existingItem = itemCollection.Find(filter).FirstOrDefault();
            if(existingItem == null)
            {
                logger.LogError($"Document not found in mongodb {id}");
            }
            return existingItem;
        }

        public IEnumerable<Item> GetItems()
        {
            return itemCollection.Find(filterBuilder.Empty).ToList();
        }

        public Item UpdateItem(Guid id, Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, id);
            itemCollection.ReplaceOne(filter, item);
            return item;
        }
    }
}
