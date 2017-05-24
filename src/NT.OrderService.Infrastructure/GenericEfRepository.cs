using NT.Core;
using NT.Infrastructure;

namespace NT.OrderService.Infrastructure
{
    public class GenericEfRepository<TEntity> : EfRepository<OrderDbContext, TEntity>
        where TEntity : EntityBase
    {
        public GenericEfRepository(OrderDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}