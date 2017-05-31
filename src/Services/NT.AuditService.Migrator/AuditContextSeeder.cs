using System.Threading.Tasks;
using NT.AuditService.Infrastructure;

namespace NT.AuditService.Migrator
{
    public static class AuditContextSeeder
    {
        public static async Task Seed(AuditDbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }
    }
}