using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NT.Infrastructure.EntityFrameworkCore;
using NT.PaymentService.Infrastructure;

namespace NT.PaymentService.Migrator
{
    public class PaymentDbContextFactory : IDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext Create(DbContextFactoryOptions options)
        {
            return new PaymentDbContext(
                options.BuildDbContext<PaymentDbContext>(
                    typeof(PaymentDbContextFactory).GetTypeInfo().Assembly).Options);
        }
    }
}