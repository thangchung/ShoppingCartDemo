using NT.Core.SharedKernel;

namespace NT.Core.CatalogContext
{
    public class Supplier : EntityBase
    {
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }
}