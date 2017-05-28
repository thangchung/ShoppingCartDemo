using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NT.CheckoutProcess.Infrastructure;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CheckoutProcess.Host
{
    public class CheckoutProcessDbContextFactory : IDbContextFactory<CheckoutProcessDbContext>
    {
        public CheckoutProcessDbContext Create(DbContextFactoryOptions options)
        {
            return new CheckoutProcessDbContext(
                options.BuildDbContext<CheckoutProcessDbContext>(
                    typeof(CheckoutProcessDbContextFactory).GetTypeInfo().Assembly).Options);
        }
    }
}