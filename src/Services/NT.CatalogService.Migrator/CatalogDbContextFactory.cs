using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NT.CatalogService.Infrastructure;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CatalogService.Migrator
{
    public class CatalogDbContextFactory : IDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext Create(DbContextFactoryOptions options)
        {
            return new CatalogDbContext(
                options.BuildDbContext<CatalogDbContext>(
                    typeof(CatalogDbContextFactory).GetTypeInfo().Assembly).Options);
        }
    }
}