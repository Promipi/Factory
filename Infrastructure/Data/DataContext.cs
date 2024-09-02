using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Infrastructure.Data.Configurations;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; } 

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<ProductSale> ProductSales { get; set; } //Intermediate table

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Configurations with converters
            builder.ApplyConfigurationsFromAssembly(typeof(CustomerConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(ProductSaleConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(SaleConfiguration).Assembly);

        }
    }
}
