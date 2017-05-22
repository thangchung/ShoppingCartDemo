namespace NT.Core.PaymentContext
{
    public class PaymentMethod : EntityBase
    {
        /// <summary>
        /// AM = AMEX
        /// MC = MasterCard
        /// CSH = Cash
        /// </summary>
        public string Code { get; set; }
    }
}