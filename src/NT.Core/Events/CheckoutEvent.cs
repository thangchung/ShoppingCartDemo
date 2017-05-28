using System;

namespace NT.Core.Events
{
    public class CheckoutEvent : Event
    {
        public CheckoutEvent(Guid orderId)
        {
            OrderId = orderId;
        } 
        
        public Guid OrderId { get; }
    }
}