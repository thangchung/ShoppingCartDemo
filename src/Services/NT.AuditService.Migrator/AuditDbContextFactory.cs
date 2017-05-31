using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NT.AuditService.Infrastructure;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.AuditService.Migrator
{
    public class AuditDbContextFactory : IDbContextFactory<AuditDbContext>
    {
        public AuditDbContext Create(DbContextFactoryOptions options)
        {
            return new AuditDbContext(
                options.BuildDbContext<AuditDbContext>(
                    typeof(AuditDbContextFactory).GetTypeInfo().Assembly).Options);
        }
    }
}