using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


#nullable disable

namespace CoreConsoleApplication
{
    public partial class SqlDatabaseContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public SqlDatabaseContext()
        {
        }

        //public SqlDatabaseContext(DbContextOptions<SqlDatabaseContext> options) : base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=SqlDbApplication;User ID=SqlDbApplication;Password=SqlDbApplication;Trusted_Connection=True;");
        }

    }
}
