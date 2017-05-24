using System;
using System.Collections.Generic;
using NT.Core;

namespace NT.OrderService.Core
{
    public class Order : EntityBase
    {
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual ShipInfo ShipInfo { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}