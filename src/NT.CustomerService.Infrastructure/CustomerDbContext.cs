using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.Core.SharedKernel;
using NT.CustomerService.Core;

namespace NT.CustomerService.Infrastructure
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = new List<Type>();
            entityTypes.Add(typeof(Customer));

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in entityTypes)
                modelBuilder.Entity(type).ToTable($"{type.Name}s", "customer");

            var valueTypes = new List<Type>();
            valueTypes.AddRange(new List<Type> {typeof(AddressInfo), typeof(ContactInfo)});

            // temporary to concanate with s at the end, but need to have a way to translate it to a plural noun
            foreach (var type in valueTypes)
                modelBuilder.Entity(type).ToTable($"{type.Name}s", "shared");

            base.OnModelCreating(modelBuilder);
        }

        // public DbSet<Customer> Customers { get; set; }
        // public DbSet<AddressInfo> AddressInfos { get; set; }
        // public DbSet<ContactInfo> ContactInfos { get; set; }
    }
}