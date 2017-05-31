using System;
using NT.Core;

namespace NT.PaymentService.Core
{
    public enum PaymentStatus
    {
        SubmitToPaymentGateway,
        Accepted
    }

    public class CustomerPayment : EntityBase
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public Guid EmployeeId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public double Money { get; set; }
    }
}