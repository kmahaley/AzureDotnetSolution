using CoreWebApplication.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebApplication.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<Item> items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Name="laptop", Price=1000, CreatedDate = DateTimeOffset.UtcNow},
            new Item { Id = Guid.NewGuid(), Name="macbook", Price=2000, CreatedDate = DateTimeOffset.UtcNow},
            new Item { Id = Guid.NewGuid(), Name="keyboard", Price=124, CreatedDate = DateTimeOffset.UtcNow},
        };

        public string GetRepositoryName => nameof(InMemoryRepository);

        private readonly ILogger<InMemoryRepository> logger;

        public InMemoryRepository(ILogger<InMemoryRepository> logger)
        {
            this.logger = logger;
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            items.Add(item);
            return await Task.FromResult(item);
        }

        public async Task<Item> UpdateItemAsync(Guid id, Item item)
        {
            var existingItem = await GetItemAsync(id);
            var existingItemIndex = items.IndexOf(existingItem);
            items[existingItemIndex] = item;
            return item;
        }

        public async Task<Item> QuickUpdateItemAsync(Guid id, Item item)
        {
            var existingItem = await GetItemAsync(id);
            var existingItemIndex = items.IndexOf(existingItem);
            logger.LogInformation($"updating item {id}");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            items[existingItemIndex] = item;
            logger.LogInformation($"updated item {id}");
            return item;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var existingItem = items.FirstOrDefault(item => id == item.Id);
            if(existingItem == null)
            {
                throw new ArgumentException($"{nameof(GetItemAsync)}, item not present in repository: {id}");
            }
            return await Task.FromResult(existingItem);
        }

        public async Task<Guid> DeleteItemAsync(Guid id)
        {
            var existingItem = await GetItemAsync(id);
            items.Remove(existingItem);
            return id;
        }
    }
}
