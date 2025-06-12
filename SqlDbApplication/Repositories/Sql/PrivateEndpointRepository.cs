using Microsoft.Extensions.Logging;
using System;

using SqlDbApplication.Repositories.Sql.Interface;
using System.Threading.Tasks;
using SqlDbApplication.Models.Dtos;
using SqlDbApplication.Models.Sql;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SqlDbApplication.Repositories.Sql
{
    public class PrivateEndpointRepository : IPrivateEndpointRepository
    {
        private readonly SqlDatabaseContext dbContext;

        private readonly ILogger<PrivateEndpointRepository> logger;


        public PrivateEndpointRepository(
            SqlDatabaseContext databaseContext, 
            ILogger<PrivateEndpointRepository> logger)
        {
            this.dbContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddPrivateEndpointAsync(PrivateEndpointRequest request, CancellationToken token)
        {
            var savedCity = await dbContext
                .PrivateEndpointMetadatas
                .AddAsync(request.PrivateEndpointMetadata, token);

            var t1 = await dbContext
                .PrivateEndpointReferenceMetadatas
                .AddAsync(request.ReferenceMetadata, token);

            var t2 = await dbContext
                .PrivateEndpointTargetResourceMetadatas
                .AddAsync(request.TargetResourceMetadata, token);

            try
            {
                await dbContext.SaveChangesAsync(token);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, $"failed to save PE. name:{request.PrivateEndpointMetadata.Name}");
                throw;
            }
            
        }

        public async Task DeletePrivateEndpointAsync(Guid peId, CancellationToken token)
        {
            var isSaved = false;
            try
            {
                //var pe1 = await GetPrivateEndpointMetadataAsync(peId, token);
                var peReference1 = await GetPeReferenceMetadataAsync(peId, token);
                var target1 = await GetPeTargetResourceMetadataAsync(peId, token);

                var peMetadata2 = await GetPrivateEndpointMetadataAsync(peId, token);
                peMetadata2.Status = "Deleted";
                peMetadata2.LastUpdatedOn = DateTime.UtcNow;

                // Change the person's name in the database to simulate a concurrency conflict
                await dbContext.Database.ExecuteSqlRawAsync(
                    "update PrivateEndpointMetadatas set Status = 'Deleting', LastUpdatedOn = '2025-05-18 11:11:10' where Name='testPeName'");


                dbContext.Remove(peReference1);
                dbContext.Remove(target1);
                dbContext.Update(peMetadata2);


                do
                {
                    try
                    {
                        await dbContext.SaveChangesAsync(token);
                        isSaved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        logger.LogError($"--- failed to delete PE. id:{peId}, entityCount:{ex.Entries.Count}, msg:{ex.Message}");
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is VnetPrivateEndpointReferenceMetadata
                                || entry.Entity is VnetPrivateEndpointTargetResourceMetadata)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = await entry.GetDatabaseValuesAsync();
                                if (databaseValues == null)
                                {
                                    entry.State = EntityState.Detached;
                                }
                                else
                                {
                                    entry.OriginalValues.SetValues(databaseValues);
                                }
                            }
                            else if (entry.Entity is VnetPrivateEndpointMetadata)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = await entry.GetDatabaseValuesAsync();
                                // database wins
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                            else
                            {
                                throw new Exception(
                                    "*** apple banana. Don't know how to handle concurrency conflicts for *** "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"new errrorrrr .failed to delete PE. id:{peId}");
                        throw;
                    }
                } while (!isSaved);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"failed to delete PE. id:{peId}");
                throw;
            }
        }

        public async Task<PrivateEndpointRequest> GetPrivateEndpointAsync(Guid peId, CancellationToken token)
        {
            var pe = await GetPrivateEndpointMetadataAsync(peId, token);
            var peReference = await GetPeReferenceMetadataAsync(peId, token);
            var target = await GetPeTargetResourceMetadataAsync(peId, token);

            var request = new PrivateEndpointRequest()
            {
                PrivateEndpointMetadata = pe,
                ReferenceMetadata = peReference,
                TargetResourceMetadata = target,
            };
            return request;
        }

        private async Task<VnetPrivateEndpointTargetResourceMetadata> GetPeTargetResourceMetadataAsync(Guid peId, CancellationToken token)
        {
            return await dbContext
                           .PrivateEndpointTargetResourceMetadatas
                           .FirstOrDefaultAsync(pe => pe.PrivateEndpointId == peId, token);
        }

        private async Task<VnetPrivateEndpointReferenceMetadata> GetPeReferenceMetadataAsync(Guid peId, CancellationToken token)
        {
            return await dbContext
                           .PrivateEndpointReferenceMetadatas
                           .FirstOrDefaultAsync(pe => pe.PrivateEndpointId == peId, token);
        }

        private async Task<VnetPrivateEndpointMetadata> GetPrivateEndpointMetadataAsync(Guid peId, CancellationToken token)
        {
            return await dbContext
                            .PrivateEndpointMetadatas
                            .FirstOrDefaultAsync(pe => pe.Id == peId, token);
        }
    }
}
