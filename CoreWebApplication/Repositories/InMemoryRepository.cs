using CoreWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public Item CreateItem(Item item)
        {
            items.Add(item);
            return item;
        }

        public Item UpdateItem(Guid id, Item item)
        {
            var existingItem = GetItem(id);
            var existingItemIndex = items.IndexOf(existingItem);
            items[existingItemIndex] = item;
            return item;
        }

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid id)
        {
            var existingItem = items.FirstOrDefault(item => id == item.Id);
            if(existingItem == null)
            {
                throw new ArgumentException($"{nameof(GetItem)}, item not present in repository: {id}");
            }
            return existingItem;
        }

        public Guid DeleteItem(Guid id)
        {
            var existingItem = GetItem(id);
            items.Remove(existingItem);
            return id;
        }
    }
}
