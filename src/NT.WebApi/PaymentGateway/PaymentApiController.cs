using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CustomerService.Core;
using NT.Infrastructure.AspNetCore;
using NT.PaymentService.Core;
using NT.WebApi.OrderContext;

namespace NT.WebApi.PaymentGateway
{
    [Route("api/payments")]
    public class PaymentApiController : BaseGatewayController
    {
        public PaymentApiController(RestClient restClient)
            : base(restClient)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<PaymentViewModel>> Get()
        {
            var payments = await RestClient.GetAsync<List<CustomerPayment>>("payment_service", "/api/payments");
            var viewModels = new List<PaymentViewModel>();
            foreach (var payment in payments)
            {
                var customer =
                    await RestClient.GetAsync<Customer>("customer_service", $"/api/customers/{payment.CustomerId}");
                var user = await RestClient.GetAsync<UserViewModel>(
                    "security_service",
                    $"/api/users/{payment.EmployeeId}");
                viewModels.Add(new PaymentViewModel
                {
                    Id = payment.Id,
                    CustomerId = payment.CustomerId,
                    CustomerName = customer == null ? "" : $"{customer.FirstName} {customer.LastName}",
                    EmployeeId = payment.EmployeeId,
                    EmployeeEmail = user == null ? "" : user.Email,
                    OrderId = payment.OrderId,
                    Money = payment.Money,
                    PaymentStatus = payment.PaymentStatus,
                    // PaymentMethod = payment.PaymentMethod.Code,
                    PaymentMethodId = payment.PaymentMethodId
                });
            }
            return viewModels;
        }

        [HttpPost("{paymentId}/payment-gateway-callback")]
        public async Task<string> Post(Guid paymentId)
        {
            var payment =
                await RestClient.PostAsync<object>("payment_service",
                    $"/api/payments/{paymentId}/payment-gateway-callback");
            return await Task.FromResult($"Payment #{paymentId} is processing...");
        }
    }

    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid OrderId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }
        public string PaymentMethod { get; set; }
        public Guid PaymentMethodId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public double Money { get; set; }
    }
}