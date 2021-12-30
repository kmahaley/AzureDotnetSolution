using MongoDbApplication.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDbApplication.Repositories
{
    public interface IRepository
    {
        string GetRepositoryName { get; }

        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> CreateItemAsync(Item item);
        Task<Item> UpdateItemAsync(Guid id, Item item);
        Task<Guid> DeleteItemAsync(Guid id);
    }
}