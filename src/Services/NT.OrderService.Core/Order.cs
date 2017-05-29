using System;
using System.Collections.Generic;
using NT.Core;

namespace NT.OrderService.Core
{
    public enum OrderStatus
    {
        New,
        Processing,
        Paid
    }

    public class Order : EntityBase
    {
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public virtual ShipInfo ShipInfo { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}