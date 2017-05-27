using System;
using NT.Core;

namespace NT.OrderService.Core
{
    public class OrderDetail : EntityBase
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}