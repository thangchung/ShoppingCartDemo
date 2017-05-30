using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NT.OrderService.Core;

namespace NT.OrderService.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetFullOrders()
        {
            return await _context.Set<Order>()
                .Include(x => x.OrderDetails)
                .Include(x => x.ShipInfo)
                .Include(x => x.ShipInfo.AddressInfo)
                .ToListAsync();
        }

        public async Task<Order> GetFullOrder(Guid id)
        {
            return await _context.Set<Order>()
                .Include(x => x.OrderDetails)
                .Include(x => x.ShipInfo)
                .Include(x => x.ShipInfo.AddressInfo)
                .SingleOrDefaultAsync(x=>x.Id == id);
        }
    }
}