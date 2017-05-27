using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.CatalogService.Core;
using NT.Core.SharedKernel;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CatalogService.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = new List<Type>
            {
                typeof(Product),
                typeof(Supplier)
            };

            var valueTypes = new List<Type>
            {
                typeof(AddressInfo),
                typeof(ContactInfo)
            };

            base.OnModelCreating(modelBuilder.RegisterTypes(entityTypes, valueTypes, "catalog"));
        }
    }
}