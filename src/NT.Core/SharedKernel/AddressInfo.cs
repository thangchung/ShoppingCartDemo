using System;

namespace NT.Core.SharedKernel
{
    public class AddressInfo : ValueObject
    {
        internal AddressInfo()
        {
        }

        public AddressInfo(Guid id, string address, string city, string region, string postalCode, string country)
        {
            Id = id;
            Address = address;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
        }

        public Guid Id { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
    }
}