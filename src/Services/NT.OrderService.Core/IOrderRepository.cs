using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NT.OrderService.Core
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetFullOrders();
        Task<Order> GetFullOrder(Guid id);
    }
}