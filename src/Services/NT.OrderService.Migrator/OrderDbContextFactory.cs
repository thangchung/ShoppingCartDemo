using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NT.Infrastructure.EntityFrameworkCore;
using NT.OrderService.Infrastructure;

namespace NT.OrderService.Migrator
{
    public class OrderDbContextFactory : IDbContextFactory<OrderDbContext>
    {
        public OrderDbContext Create(DbContextFactoryOptions options)
        {
            return new OrderDbContext(
                options.BuildDbContext<OrderDbContext>(
                    typeof(OrderDbContextFactory).GetTypeInfo().Assembly).Options);
        }
    }
}