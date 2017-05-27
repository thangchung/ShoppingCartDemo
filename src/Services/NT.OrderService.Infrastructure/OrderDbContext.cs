using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NT.Core.SharedKernel;
using NT.Infrastructure.EntityFrameworkCore;
using NT.OrderService.Core;

namespace NT.OrderService.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = new List<Type>
            {
                typeof(Order),
                typeof(OrderDetail)
            };

            var valueTypes = new List<Type>
            {
                typeof(ShipInfo),
                typeof(AddressInfo)
            };

            base.OnModelCreating(modelBuilder.RegisterTypes(entityTypes, valueTypes, "order"));
        }
    }
}