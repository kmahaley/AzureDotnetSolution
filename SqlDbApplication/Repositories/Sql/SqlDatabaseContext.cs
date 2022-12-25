using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SqlDbApplication.Models.Sql;

#nullable disable

namespace SqlDbApplication.Repositories.Sql
{
    /// <summary>
    /// Database context to query the database entities.
    /// you can create separate context for each entity or single context for all entities
    /// </summary>
    public class SqlDatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        //public SqlDatabaseContext()
        //{
        //}

        public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> options) : base(options)
        {
        }

    }
}
