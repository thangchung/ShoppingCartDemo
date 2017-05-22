using System;

namespace NT.Core.OrderContext
{
    public class OrderDetails : EntityBase
    {
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public ProductLink Product { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}