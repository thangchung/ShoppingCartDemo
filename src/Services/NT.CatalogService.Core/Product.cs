using System;
using NT.Core;

namespace NT.CatalogService.Core
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Model { get; set; }
        public DateTime DateAdded { get; set; }
        public Status Status { get; set; }
        public virtual Supplier Supplier { get; set; }
        public Guid SupplierId { get; set; }
    }
}