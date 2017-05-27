using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NT.CustomerService.Infrastructure;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CustomerService.Migrator
{
    public class CustomerDbContextFactory : IDbContextFactory<CustomerDbContext>
    {
        public CustomerDbContext Create(DbContextFactoryOptions options)
        {
            return new CustomerDbContext(
                options.BuildDbContext<CustomerDbContext>(
                    typeof(CustomerDbContextFactory).GetTypeInfo().Assembly).Options);
        }
    }
}