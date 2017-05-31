using System.Threading.Tasks;
using NT.PaymentService.Core;
using NT.PaymentService.Infrastructure;

namespace NT.PaymentService.Migrator
{
    public static class PaymentContextSeeder
    {
        public static async Task Seed(PaymentDbContext dbContext)
        {
            await dbContext.Set<PaymentMethod>().AddAsync(new PaymentMethod
            {
                Id = new System.Guid("c50683db-d227-4856-b777-ea07e030d926"),
                Code = "AM" // AMEX
            });
            await dbContext.Set<PaymentMethod>().AddAsync(new PaymentMethod
            {
                Id = new System.Guid("aae5885e-682b-42e8-b230-7ad2dad55331"),
                Code = "MC" // MasterCard
            });
            await dbContext.Set<PaymentMethod>().AddAsync(new PaymentMethod
            {
                Id = new System.Guid("7fa09019-f0c4-4479-b9fd-fae9652c4901"),
                Code = "CSH" // Cash
            });
            await dbContext.SaveChangesAsync();
        }
    }
}