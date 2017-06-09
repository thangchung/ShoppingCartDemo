using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NT.Core.SharedKernel;
using NT.Infrastructure;
using NT.OrderService.Core;
using NT.OrderService.Infrastructure;

namespace NT.OrderService.Migrator
{
    public static class OrderContextSeeder
    {
        public static async Task Seed(OrderDbContext dbContext)
        {
            var order1 = new Order
            {
                Id = new Guid("4c9a0398-016f-4a80-be17-e756dba1e5df"),
                CustomerId = new Guid("37EB08EF-E4C2-4211-B808-F64A81AE02FC"),
                EmployeeId = new Guid("d3b13f7e-8978-4364-96dd-978878de9fce"),
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.New,
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

            var order2 = new Order
            {
                Id = new Guid("bc87c7fe-c853-44b1-8c02-26b8790b96bb"),
                CustomerId = new Guid("77DE2ED8-C423-4D98-8833-D6E4CCCE5B0D"),
                EmployeeId = new Guid("d3b13f7e-8978-4364-96dd-978878de9fce"),
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.New,
                ShipInfo = new ShipInfo(Guid.NewGuid(), "Ship Info 2", new AddressInfo(Guid.NewGuid(), "123 Address", "Ho Chi Minh", "Tan Binh district", "7000", "Vietnam")),
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        ProductId = new Guid("85DB18F3-9C27-47D8-BD45-86DD67068960"),
                        Quantity = 10
                    }
                }
            };

            await dbContext.Set<Order>().AddAsync(order1);
            await dbContext.Set<Order>().AddAsync(order2);
            await dbContext.SaveChangesAsync();
        }
    }
}