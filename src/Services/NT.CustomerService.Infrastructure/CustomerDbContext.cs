using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.Core.SharedKernel;
using NT.CustomerService.Core;
using NT.Infrastructure.EntityFrameworkCore;

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
            var entityTypes = new List<Type>
            {
                typeof(Customer)
            };

            var valueTypes = new List<Type>
            {
                typeof(AddressInfo),
                typeof(ContactInfo)
            };

            base.OnModelCreating(modelBuilder.RegisterTypes(entityTypes, valueTypes, "customer"));
        }
    }
}