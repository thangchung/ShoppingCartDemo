using NT.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CustomerService.Infrastructure
{
    public class GenericEfRepository<TEntity> : EfRepository<CustomerDbContext, TEntity>
        where TEntity : EntityBase
    {
        public GenericEfRepository(CustomerDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}