using System;

namespace NT.Core.CatalogContext
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Model { get; set; }
        public DateTime DateAdded { get; set; }
        public Status Status { get; set; }
    }
}