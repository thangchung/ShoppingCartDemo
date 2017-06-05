using System;
using System.Collections.Generic;

namespace NT.WebApi.ShoppingCart
{
    public class CartViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public ShipInfoViewModel ShipInfo { get; set; }
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
    }

    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class ShipInfoViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}