using System;

namespace NT.Core.Events
{
    public class ProcessOrderStatusEvent : Event
    {
        public ProcessOrderStatusEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}