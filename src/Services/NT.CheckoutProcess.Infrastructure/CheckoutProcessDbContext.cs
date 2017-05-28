using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.CheckoutProcess.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CheckoutProcess.Infrastructure
{
    public class CheckoutProcessDbContext : DbContext
    {
        public CheckoutProcessDbContext(DbContextOptions<CheckoutProcessDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = new List<Type>
            {
                typeof(SagaInfo)
            };

            var valueTypes = new List<Type>();

            base.OnModelCreating(modelBuilder.RegisterTypes(entityTypes, valueTypes, "checkout"));
        }
    }
}