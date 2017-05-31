using System;
using System.Collections.Generic;

namespace NT.CheckoutProcess.Core
{
    public class CheckoutInfo
    {
        public Guid OrderId { get; set; }
        public int OrderStatus { get; set; }
        public List<ProductInfo> Products { get; set; } = new List<ProductInfo>();
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid? PaymentId { get; set; }
        public double Money { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class ProductInfo
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}