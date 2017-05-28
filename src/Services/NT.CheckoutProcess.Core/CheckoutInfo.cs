using System;
using System.Collections.Generic;

namespace NT.CheckoutProcess.Core
{
    public class CheckoutInfo
    {
        public Guid OrderId { get; set; }
        
        public int OrderStatus { get; set; }

        public ICollection<ProductInfo> Products { get; set; } = new List<ProductInfo>(); 

        public Guid CustomerId { get; set; }

        public Guid EmployeeId { get; set; }
    }

    public class ProductInfo
    {
        public Guid ProductId { get; set; }
        
        public long Quantity { get; set; }
    }
}