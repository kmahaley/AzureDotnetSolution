using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlDbApplication.Repositories.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using SqlDbApplication.Repositories.Sql.Interface;

namespace SqlDbApplication.DependencyExtensions
{
    /// <summary>
    /// Add denpendencies of the database context and repositories
    /// </summary>
    public static class RepositoryInstances
    {
        /// <summary>
        /// DbContext is a scope context, hence service consuming DbContext should have lifespan
        /// less than scoped[transient or scoped]
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseInstances(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddDbContext<SqlDatabaseContext>(option =>
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
