using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Infrastructure.AspNetCore;

namespace NT.WebApi.PaymentGateway
{
    [Route("api/payments")]
    public class PaymentApiController : BaseGatewayController
    {
        public PaymentApiController(RestClient restClient)
            : base(restClient)
        {
        }

        [HttpPost("{paymentId}/payment-gateway-callback")]
        public async Task<string> Post(Guid paymentId)
        {
            var payment = await RestClient.PostAsync<object>("payment_service", $"/api/payments/{paymentId}/payment-gateway-callback");
            return await Task.FromResult($"Payment #{paymentId} is processing...");
        }
    }
}