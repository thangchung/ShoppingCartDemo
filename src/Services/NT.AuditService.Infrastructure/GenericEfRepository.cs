using NT.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.AuditService.Infrastructure
{
    public class GenericEfRepository<TEntity> : EfRepository<AuditDbContext, TEntity>
        where TEntity : EntityBase
    {
        public GenericEfRepository(AuditDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}