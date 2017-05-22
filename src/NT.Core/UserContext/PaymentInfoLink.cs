using System;

namespace NT.Core.UserContext
{
    public class PaymentInfoLink : ValueObject
    {
        public PaymentInfoLink(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}