using NT.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CatalogService.Infrastructure
{
    public class GenericEfRepository<TEntity> : EfRepository<CatalogDbContext, TEntity>
        where TEntity : EntityBase
    {
        public GenericEfRepository(CatalogDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}