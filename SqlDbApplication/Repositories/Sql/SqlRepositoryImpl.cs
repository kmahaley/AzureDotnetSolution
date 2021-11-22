using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SqlDbApplication.Models.Sql;

#nullable disable

namespace SqlDbApplication.Repositories.Sql
{
    public partial class SqlRepositoryImpl : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public SqlRepositoryImpl()
        {
        }

        public SqlRepositoryImpl(DbContextOptions<SqlRepositoryImpl> options) : base(options)
        {
        }

    }
}
