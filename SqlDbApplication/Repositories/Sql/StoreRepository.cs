using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlDbApplication.Models.Sql;
using SqlDbApplication.Repositories.Sql.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDbApplication.Repositories.Sql
{
    public class StoreRepository : IStoreRepository
    {
        private readonly SqlDatabaseContext databaseContext;

        private readonly ILogger<StoreRepository> logger;


        public StoreRepository(SqlDatabaseContext databaseContext, ILogger<StoreRepository> logger)
        {
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Store> AddStoreAsync(Store store)
        {
            var savedSore = await databaseContext.AddAsync(store);
            await databaseContext.SaveChangesAsync();
            return savedSore.Entity;
        }

        public async Task<Store> DeleteStoreByIdAsync(int id)
        {
            var existingStore = await GetStoreByIdAsync(id);
            databaseContext.Stores.Remove(existingStore);
            await databaseContext.SaveChangesAsync();
            return existingStore;
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            var list = await databaseContext.Stores.ToListAsync();
            return list;
        }

        public async Task<Store> GetStoreByIdAsync(int id)
        {
            var existingEntity = await databaseContext.Stores.FindAsync(id);
            if (existingEntity == null)
            {
                throw new ArgumentException($"Store with Id:{id} does not exists.");
            }
            return existingEntity;
        }

        public async Task<Store> UpdateStoreAsync(int id, Store store)
        {
            var existingstore = await GetStoreByIdAsync(id);
            existingstore.StoreName = store.StoreName;
            existingstore.City = store.City;
            existingstore.State = store.State;
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(4));
                await databaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) //when (ex is DbUpdateConcurrencyException)
            {
                logger.LogError($"Error in database save.{ex.GetType().Name} {ex.Message}");
                foreach (var entry in ex.Entries)
                {
                    logger.LogError($"{entry}");
                }
            }
            return store;
        }
    }
}
