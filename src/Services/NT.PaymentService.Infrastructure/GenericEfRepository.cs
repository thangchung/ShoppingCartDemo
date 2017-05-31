using NT.Core;
using NT.Infrastructure.EntityFrameworkCore;

namespace NT.PaymentService.Infrastructure
{
    public class GenericEfRepository<TEntity> : EfRepository<PaymentDbContext, TEntity>
        where TEntity : EntityBase
    {
        public GenericEfRepository(PaymentDbContext dbContext) 
            : base(dbContext)
        {
        }
    }
}