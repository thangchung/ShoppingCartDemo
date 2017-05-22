using System;

namespace NT.Core.UserContext
{
    public class ShippingAddress : ValueObject
    {
        public ShippingAddress(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
