using CoreWebApplication.Models;
using System;
using System.Collections.Generic;

namespace CoreWebApplication.Repositories
{
    public interface IRepository
    {
        string GetRepositoryName { get; }

        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
        Item CreateItem(Item item);
        Item UpdateItem(Guid id, Item item);
        Guid DeleteItem(Guid id);
    }
}