using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core;
using NT.Core.Events;
using NT.Core.Results;
using NT.Infrastructure.AspNetCore;
using NT.OrderService.Core;
using NT.PaymentService.Core;
using RawRabbit;
using RawRabbit.vNext;

namespace NT.PaymentService.Api
{
    [Route("api/payments")]
    public class PaymentApiController : Controller
    {
        private readonly IRepository<CustomerPayment> _genericPaymentRepository;
        private readonly IRepository<PaymentMethod> _genericPaymentMethodRepository;
        private readonly IBusClient _messageBus;
        private readonly RestClient _restClient;

        public PaymentApiController(
            IRepository<CustomerPayment> genericPaymentRepository, 
            IRepository<PaymentMethod> genericPaymentMethodRepository, 
            RestClient restClient)
        {
            _genericPaymentRepository = genericPaymentRepository;
            _genericPaymentMethodRepository = genericPaymentMethodRepository;
            _messageBus = BusClientFactory.CreateDefault();
            _restClient = restClient;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerPayment>> Get()
        {
            var result = await _genericPaymentRepository.ListAsync();
            return result;
        }

        [HttpPost]
        public async Task<SagaPaymentResult> MakePayment([FromBody] CustomerPayment paymentInfo)
        {
            var paymentMethod = await _genericPaymentMethodRepository.GetByIdAsync(paymentInfo.PaymentMethodId);
            if (paymentMethod == null)
            {
                return await Task.FromResult(new SagaPaymentResult { Succeed = false });
            }

            paymentInfo.PaymentStatus = PaymentStatus.SubmitToPaymentGateway;
            paymentInfo.PaymentMethod = paymentMethod;
            var result = await _genericPaymentRepository.AddAsync(paymentInfo);
            if (result == null)
            {
                return await Task.FromResult(new SagaPaymentResult { Succeed = false });
            }

            // TODO: Submit request to payment gateway
            // TODO: ...
            
            return await Task.FromResult(new SagaPaymentResult
            {
                Succeed = true,
                PaymentId = result.Id
            });
        }

        /// <summary>
        /// This should be a public function (webhook) 
        /// so that any payment gateway can call it to dispatch the data to our payment service
        /// </summary>
        /// <returns></returns>
        [HttpPost("{paymentId}/payment-gateway-callback")]
        public async Task<object> PaymentGatewayCallback(Guid paymentId)
        {
            var paymentInfo = await _genericPaymentRepository.GetByIdAsync(paymentId);
            paymentInfo.PaymentStatus = PaymentStatus.Accepted;
            await _genericPaymentRepository.UpdateAsync(paymentInfo);

            var order = await _restClient.GetAsync<Order>("order_service", $"/api/orders/{paymentInfo.OrderId}");
            await _messageBus.PublishAsync(new PaymentAcceptedEvent(order.SagaId.Value));

            return null;
        }

        [HttpPost("{paymentId}/compensate-money/{money}")]
        public async Task<SagaResult> CompensateMoney(Guid paymentId, double money)
        {
            // TODO: Call to payment gateway to compensate the money for customer
            // TODO: ...

            return await Task.FromResult(new SagaResult { Succeed = true });
        }
    }
}