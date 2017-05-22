using System;
using NT.Core.SharedKernel;

namespace NT.Core.CustomerContext
{
    public class Customer : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactTitle { get; set; }
        public Guid AddressInfoId { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public Guid ContactInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }
}