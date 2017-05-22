using System;

namespace NT.Core.OrderContext
{
    public class Order : EntityBase
    {
        public CustomerLink Customer { get; set; }
        public Guid CustomerId { get; set; }
        public UserLink Employee { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public ShipInfo ShipInfo { get; set; }
    }
}