using NT.Core;

namespace NT.CheckoutProcess.Core
{
    public class SagaInfo : EntityBase
    {
        public string Data { get; set; }
        public int SagaStatus { get; set; } = 0; // Checkout
    }
}