using System;

namespace NT.Core.PaymentContext
{
    public class CustomerPayment : EntityBase
    {
        public PaymentCustomerLink Customer { get; set; }
        public Guid CustomerId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string CardNumber { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}