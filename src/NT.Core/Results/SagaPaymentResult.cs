using System;

namespace NT.Core.Results
{
    public class SagaPaymentResult
    {
        public bool Succeed { get; set; }
        public Guid PaymentId { get; set; }
    }
}