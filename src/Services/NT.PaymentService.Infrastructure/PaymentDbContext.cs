using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.Infrastructure.EntityFrameworkCore;
using NT.PaymentService.Core;

namespace NT.PaymentService.Infrastructure
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = new List<Type>
            {
                typeof(CustomerPayment),
                typeof(PaymentMethod)
            };

            var valueTypes = new List<Type>();

            base.OnModelCreating(modelBuilder.RegisterTypes(entityTypes, valueTypes, "payment"));
        }
    }
}