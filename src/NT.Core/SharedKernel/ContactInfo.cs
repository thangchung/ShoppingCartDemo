using System;

namespace NT.Core.SharedKernel
{
    public class ContactInfo : ValueObject
    {
        internal ContactInfo()
        {
        }

        public ContactInfo(Guid id, string phone, string fax, string homePage)
        {
            Id = id;
            Phone = phone;
            Fax = fax;
            HomePage = homePage;
        }

        public Guid Id { get; private set; }
        public string Phone { get; private set; }
        public string Fax { get; private set; }
        public string HomePage { get; private set; }
    }
}