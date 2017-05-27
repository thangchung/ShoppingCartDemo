using NT.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.CheckoutProcess.Infrastructure
{
    public class GenericEfRepository<TEntity> : EfRepository<CheckoutProcessDbContext, TEntity>
        where TEntity : EntityBase
    {
        public GenericEfRepository(CheckoutProcessDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}