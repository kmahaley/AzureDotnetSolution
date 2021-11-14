using CoreWebApplication.Models;
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

        private readonly IMongoCollection<Item> itemCollection;

        public MongoDbRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemCollection = database.GetCollection<Item>(collectionName);
        }


        public Item CreateItem(Item item)
        {
            itemCollection.InsertOne(item);
            return item;
        }

        public Guid DeleteItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemCollection.Find(new BsonDocument()).ToList();
        }

        public Item UpdateItem(Guid id, Item item)
        {
            throw new NotImplementedException();
        }
    }
}
