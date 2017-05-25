using System;
using System.Collections.Generic;
using NT.OrderService.Core;

namespace NT.WebApi.OrderContext
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipInfoName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}