using System;

namespace NT.Core.Events
{
    public class ProcessProductQuantityEvent : Event
    {
        public ProcessProductQuantityEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}