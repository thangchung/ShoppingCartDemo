using System;
using NT.Core;
using NT.Core.SharedKernel;

namespace NT.CustomerService.Core
{
    public class Customer : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactTitle { get; set; }
        public Guid AddressInfoId { get; set; }
        public virtual AddressInfo AddressInfo { get; set; }
        public Guid ContactInfoId { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
    }
}