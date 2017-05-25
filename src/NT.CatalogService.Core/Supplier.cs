using System.Collections.Generic;
using NT.Core;
using NT.Core.SharedKernel;

namespace NT.CatalogService.Core
{
    public class Supplier : EntityBase
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}