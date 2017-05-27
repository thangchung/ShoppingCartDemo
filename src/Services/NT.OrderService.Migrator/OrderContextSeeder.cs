using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.Core.SharedKernel;
using NT.OrderService.Core;
using NT.OrderService.Infrastructure;

namespace NT.OrderService.Migrator
{
    public static class OrderContextSeeder
    {
        public static async Task Seed(OrderDbContext dbContext)
        {
            var order = new Order
            {
                Id = new Guid("4c9a0398-016f-4a80-be17-e756dba1e5df"),
                CustomerId = new Guid("37EB08EF-E4C2-4211-B808-F64A81AE02FC"),
                EmployeeId = new Guid("d3b13f7e-8978-4364-96dd-978878de9fce"),
                OrderDate = DateTimeOffset.Now.UtcDateTime,
                ShipInfo = new ShipInfo(Guid.NewGuid(), "Ship Info 1", new AddressInfo(Guid.NewGuid(), "123 Address", "Hanoi", "Hoan Kiem district", "7000", "Vietnam")),
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        ProductId = new Guid("85db18f3-9c27-47d8-bd45-86dd67068960"),
                        Quantity = 2
                    }
                }
            };

            await dbContext.Set<Order>().AddAsync(order);
            await dbContext.SaveChangesAsync();
        }
    }
}