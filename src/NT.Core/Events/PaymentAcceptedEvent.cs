using System;

namespace NT.Core.Events
{
    public class PaymentAcceptedEvent : Event
    {
        public PaymentAcceptedEvent(Guid sagaId)
        {
            SagaId = sagaId;
        }

        public Guid SagaId { get; }
    }
}