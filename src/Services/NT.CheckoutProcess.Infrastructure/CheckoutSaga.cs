using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NT.CheckoutProcess.Core;
using NT.Core;
using NT.Core.Events;
using NT.Core.Results;
using NT.Infrastructure;
using NT.Infrastructure.AspNetCore;
using NT.OrderService.Core;
using NT.PaymentService.Core;
using RawRabbit;
using RawRabbit.vNext;
using Stateless;

namespace NT.CheckoutProcess.Infrastructure
{
    public class CheckoutSaga
    {
        public enum State
        {
            Checkout,
            OrderStatus,
            ProductQuantity,
            Payment,
            PaymentReceived,
            Completed
        }

        public enum Trigger
        {
            Checkout,
            UpdateOrderStatus,
            UpdateOrderStatusSucceed,
            UpdateOrderStatusFailed,
            UpdateProductQuantity,
            UpdateProductQuantitySucceed,
            UpdateProductQuantityFailed,
            MakePayment,
            PaymentReceived,
            MakePaymentSucceed,
            MakePaymentFailed,
            ChangeCompletedStatus
        }

        private readonly StateMachine<State, Trigger> _machine;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _checkoutTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _completedTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _orderStatusTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _paymentTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _receivedPaymentTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _productQuantityTrigger;

        private readonly IRepository<SagaInfo> _repository;
        private readonly RestClient _restClient;
        private Guid _correlationId;
        private CheckoutInfo _internalData;
        private State _state = State.Checkout;
        private readonly ILogger _logger;
        private readonly IBusClient _eventBus;

        public CheckoutSaga(
            IRepository<SagaInfo> repository,
            RestClient restClient)
        {
            _repository = repository;
            _restClient = restClient;
            _eventBus = BusClientFactory.CreateDefault();
            _logger = LogFactory.GetLogInstance<CheckoutSaga>();

            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);
            _checkoutTrigger = _machine.SetTriggerParameters<Guid>(Trigger.Checkout);
            _orderStatusTrigger = _machine.SetTriggerParameters<Guid>(Trigger.UpdateOrderStatus);
            _productQuantityTrigger = _machine.SetTriggerParameters<Guid>(Trigger.UpdateProductQuantity);
            _paymentTrigger = _machine.SetTriggerParameters<Guid>(Trigger.MakePayment);
            _receivedPaymentTrigger = _machine.SetTriggerParameters<Guid>(Trigger.PaymentReceived);
            _completedTrigger = _machine.SetTriggerParameters<Guid>(Trigger.ChangeCompletedStatus);

            _machine.Configure(State.Checkout)
                .OnEntryFromAsync(_checkoutTrigger, OnCheckout)
                .PermitReentry(Trigger.Checkout)
                .Permit(Trigger.UpdateOrderStatus, State.OrderStatus);

            _machine.Configure(State.OrderStatus)
                .OnEntryFromAsync(Trigger.UpdateOrderStatus, OnUpdateOrderStatus)
                .InternalTransitionAsync(Trigger.UpdateOrderStatusSucceed, t => OnOrderStatusSucceed(_correlationId))
                .InternalTransitionAsync(Trigger.UpdateOrderStatusFailed, t => OnOrderStatusFailed(_correlationId))
                .Permit(Trigger.UpdateProductQuantity, State.ProductQuantity)
                .Permit(Trigger.ChangeCompletedStatus, State.Completed);

            _machine.Configure(State.ProductQuantity)
                .OnEntryFromAsync(Trigger.UpdateProductQuantity, OnUpdateProductQuantity)
                .InternalTransitionAsync(Trigger.UpdateProductQuantitySucceed,
                    t => OnProductQuantitySucceed(_correlationId))
                .InternalTransitionAsync(Trigger.UpdateProductQuantityFailed,
                    t => OnProductQuantityFailed(_correlationId))
                .Permit(Trigger.MakePayment, State.Payment)
                .Permit(Trigger.ChangeCompletedStatus, State.Completed);

            _machine.Configure(State.Payment)
                .OnEntryFromAsync(_paymentTrigger, (correlationId, t) => OnMakePayment(correlationId))
                .Permit(Trigger.PaymentReceived, State.PaymentReceived);

            _machine.Configure(State.PaymentReceived)
                .SubstateOf(State.Payment)
                .OnEntryFromAsync(_receivedPaymentTrigger, (correlationId, t) => OnPaymentReceived(correlationId))
                .InternalTransitionAsync(Trigger.MakePaymentSucceed, t => OnPaymentSucceed(_correlationId))
                .InternalTransitionAsync(Trigger.MakePaymentFailed, t => OnPaymentFailed(_correlationId))
                .Permit(Trigger.ChangeCompletedStatus, State.Completed);

            _machine.Configure(State.Completed)
                .OnEntryFromAsync(_completedTrigger, (correlationId, t) => OnCompletedProcess(correlationId));
        }

        public async Task Checkout(Guid correlationId, Guid orderId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | Checkout({correlationId}, {orderId}):Action", "The order is checking out...");

            var result = await LoadFromStorage(correlationId, orderId);
            _state = (State) result.Item1.SagaStatus;
            _internalData = result.Item2;
            _correlationId = correlationId;
            await _machine.FireAsync(_checkoutTrigger, correlationId);
        }

        public async Task PaymentAccepted(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | PaymentAccepted({correlationId}):Action", "The payment is accepted by the payment gateway.");
                                                                                                                           
            var result = await LoadFromStorage(correlationId);
            _state = (State) result.Item1.SagaStatus;
            _internalData = result.Item2;
            _correlationId = correlationId;
            await _machine.FireAsync(_receivedPaymentTrigger, correlationId);
        }

        private async Task OnCheckout(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnCheckout({correlationId}):Trigger", "Checkout is processing...");

            await _machine.FireAsync(_orderStatusTrigger, correlationId);
        }

        private async Task OnUpdateOrderStatus()
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnUpdateOrderStatus({_correlationId}):Trigger", "Order status is updating...");

            var isSucceed = await UpdateOrderStatus(_internalData.OrderId, OrderStatus.Processing);
            if (isSucceed)
                await _machine.FireAsync(Trigger.UpdateOrderStatusSucceed);
            else
                await _machine.FireAsync(Trigger.UpdateOrderStatusFailed);
        }

        private async Task OnOrderStatusSucceed(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnOrderStatusSucceed({_correlationId}):Trigger", "Order status is updated successfully.");

            await _machine.FireAsync(_productQuantityTrigger, correlationId);
        }

        private async Task OnOrderStatusFailed(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnOrderStatusFailed({_correlationId}):Trigger", "Order status is not updated successfully.");

            await Audit("CheckoutService", $"CheckoutSaga | OnOrderStatusFailed({_correlationId}):Trigger", "Compensate the order status to NEW...");
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.New);
            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnUpdateProductQuantity()
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnUpdateProductQuantity({_correlationId}):Trigger", "Product quantity is updating...");

            var isSucceed = true;
            foreach (var product in _internalData.Products)
                isSucceed = isSucceed && await DecreaseQuantityOfProductInCatalog(product.ProductId, product.Quantity);

            if (isSucceed)
                await _machine.FireAsync(Trigger.UpdateProductQuantitySucceed);
            else
                await _machine.FireAsync(Trigger.UpdateProductQuantityFailed);
        }

        private async Task OnProductQuantitySucceed(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnProductQuantitySucceed({_correlationId}):Trigger", "Product quantity is updated successfully.");

            await _machine.FireAsync(_paymentTrigger, correlationId);
        }

        private async Task OnProductQuantityFailed(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnProductQuantityFailed({_correlationId}):Trigger", "Product quantity is not updated successfully.");
            
            foreach (var product in _internalData.Products)
            {
                await Audit("CheckoutService", $"CheckoutSaga | OnProductQuantityFailed({_correlationId}):Trigger", $"Compensate the product quantity for [{product.ProductId}]...");
                await CompensateQuantityOfProductInCatalog(product.ProductId, product.Quantity);
            }
            await Audit("CheckoutService", $"CheckoutSaga | OnProductQuantityFailed({_correlationId}):Trigger", $"Compensate the order status to NEW...");
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.New);

            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnMakePayment(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnMakePayment({_correlationId}):Trigger", "A payment is making...");

            var money = 0.0D;
            foreach (var productInOrder in _internalData.Products)
            {
                money += await GetProductPrice(productInOrder.ProductId) * productInOrder.Quantity;
            }
            await Audit("CheckoutService", $"CheckoutSaga | OnMakePayment({_correlationId}):Trigger", $"Calculate the payment. The money is $[{money}].");

            // Submit request to order service to update WaitingPayment status, and correlationId to SagaID column
            await Audit("CheckoutService", $"CheckoutSaga | OnMakePayment({_correlationId}):Trigger", "Update Order status to WAITINGPAYMENT.");
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.WaitingPayment);
            await UpdateSagaInfo(_internalData.OrderId, correlationId);

            // Submit request to payment service
            await Audit("CheckoutService", $"CheckoutSaga | OnMakePayment({_correlationId}):Trigger", "Make a request to the payment service.");
            var result = await MakePayment(
                _internalData.OrderId, 
                _internalData.CustomerId, 
                _internalData.EmployeeId, 
                new Guid("AAE5885E-682B-42E8-B230-7AD2DAD55331"), // Master Card 
                money);

            if (result.Succeed)
            {
                _internalData.Money = money;
                _internalData.PaymentId = result.PaymentId;
            }
            else
            {
                await _machine.FireAsync(_completedTrigger, correlationId);
            }

            // update storage for next time processing
            await UpdateStorage(correlationId, (int) State.PaymentReceived, _internalData);
        }

        private async Task OnPaymentReceived(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnPaymentReceived({_correlationId}):Trigger", "The payment is received");

            await _machine.FireAsync(Trigger.MakePaymentSucceed);
        }

        private async Task OnPaymentSucceed(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnPaymentSucceed({_correlationId}):Trigger", "The payment is processed successfully.");

            await Audit("CheckoutService", $"CheckoutSaga | OnPaymentSucceed({_correlationId}):Trigger", "Update the order status to PAID.");
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.Paid);

            await _machine.FireAsync(_completedTrigger, correlationId);

            await OnSendEmail(_internalData.CustomerId);
            await OnNotifyEmployee(_internalData.EmployeeId);
        }

        private async Task OnPaymentFailed(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnPaymentFailed({_correlationId}):Trigger", "The payment is not processed successfully.");

            // roll back money if has
            await Audit("CheckoutService", $"CheckoutSaga | OnPaymentFailed({_correlationId}):Trigger", "Compensate money for customer.");
            await CompensateMoney(_internalData.PaymentId.Value, _internalData.Money);

            foreach (var product in _internalData.Products)
            {
                await Audit("CheckoutService", $"CheckoutSaga | OnPaymentFailed({_correlationId}):Trigger", $"Compensate the product quantity for [{product.ProductId}]...");
                await CompensateQuantityOfProductInCatalog(product.ProductId, product.Quantity);
            }

            await Audit("CheckoutService", $"CheckoutSaga | OnProductQuantityFailed({_correlationId}):Trigger", $"Compensate the order status to NEW...");
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.New);

            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnSendEmail(Guid customerId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnSendEmail({_correlationId})", $"An email is sending to customer #[{customerId}]...");

            // TODO: Send email to customer based on information in the internal data
            // TODO: ...
        }

        private async Task OnNotifyEmployee(Guid employeeId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnNotifyEmployee({_correlationId})", $"Notification message is sending to employee #[{employeeId}]...");

            // TODO: Put one record for notification into the notification service
            // TODO: ...
        }

        private async Task OnCompletedProcess(Guid correlationId)
        {
            await Audit("CheckoutService", $"CheckoutSaga | OnCompletedProcess({_correlationId}):Trigger", "Checkout process is completed.");

            await UpdateStorage(correlationId, (int) State.Completed, _internalData);
        }

        private async Task<Tuple<SagaInfo, CheckoutInfo>> LoadFromStorage(Guid correlationId, Guid? orderId = null)
        {
            var sagaInfo = await _repository.GetByIdAsync(correlationId);
            CheckoutInfo internalData;
            if (sagaInfo != null)
            {
                internalData = sagaInfo.Data.ToObject<CheckoutInfo>();
            }
            else
            {
                if (!orderId.HasValue)
                    throw new Exception("Need to have an order id for the first time.");
                var order = await _restClient.GetAsync<Order>("order_service", $"/api/orders/{orderId.Value}");
                internalData = new CheckoutInfo
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    EmployeeId = order.EmployeeId,
                    OrderDate = order.OrderDate,
                    OrderStatus = (int) order.OrderStatus,
                    Products = order.OrderDetails.Select(x => new ProductInfo
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                };
                var saga = new SagaInfo
                {
                    Id = correlationId, // correlation id
                    Data = internalData.ToString<CheckoutInfo>()
                };
                sagaInfo = await _repository.AddAsync(saga);
            }
            return new Tuple<SagaInfo, CheckoutInfo>(sagaInfo, internalData);
        }

        private async Task UpdateStorage(Guid correlationId, int sagaStatus, CheckoutInfo checkoutInfo)
        {
            var updateSagaInfo = await _repository.GetByIdAsync(correlationId);
            updateSagaInfo.Data = checkoutInfo.ToString<CheckoutInfo>();
            updateSagaInfo.SagaStatus = sagaStatus;
            await _repository.UpdateAsync(updateSagaInfo);
        }

        private async Task<bool> UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            // TODO: Store OrderStatus into SagaInfo
            // TODO: ...

            var result = await _restClient.PutAsync<SagaResult>(
                "order_service",
                $"/api/orders/{orderId}/status/{status}");
            return result.Succeed;
        }

        private async Task<bool> UpdateSagaInfo(Guid orderId, Guid correlationId)
        {
            var result = await _restClient.PutAsync<SagaResult>(
                "order_service",
                $"/api/orders/{orderId}/update-saga/{correlationId}");
            return result.Succeed;
        }

        private async Task<double> GetProductPrice(Guid productId)
        {
            // TODO: need to return SagaResult (wrap the price in)
            var result = await _restClient.GetAsync<SagaDoubleResult>(
                "catalog_service",
                $"/api/products/{productId}/price");
            return result.Price;
        }

        private async Task<bool> DecreaseQuantityOfProductInCatalog(Guid productId, int quantityInOrder)
        {
            var result = await _restClient.PutAsync<SagaResult>(
                "catalog_service",
                $"/api/products/{productId}/decrease-quantity/{quantityInOrder}");
            return result.Succeed;
        }

        private async Task<bool> CompensateQuantityOfProductInCatalog(Guid productId, int quantityInOrder)
        {
            var result = await _restClient.PutAsync<SagaResult>(
                "catalog_service",
                $"/api/products/{productId}/increase-quantity/{quantityInOrder}");
            return result.Succeed;
        }

        private async Task<SagaPaymentResult> MakePayment(
            Guid orderId, Guid customerId, Guid employeeId, 
            Guid paymentMethodId, double money)
        {
            var paymentInfo = new CustomerPayment
            {
                OrderId = orderId,
                CustomerId = customerId,
                EmployeeId = employeeId,
                Money = money,
                PaymentMethodId = paymentMethodId
            };
            var result = await _restClient.PostAsync<SagaPaymentResult>(
                "payment_service",
                "/api/payments",
                paymentInfo
            );
            return result;
        }

        private async Task<bool> CompensateMoney(Guid paymentId, double money)
        {
            var result = await _restClient.PostAsync<SagaResult>(
                "payment_service",
                $"/api/payments/{paymentId}/compensate-money/{money}"
            );
            return result.Succeed;
        }

        private async Task<bool> Audit(string serviceName, string methodName, string actionMessage)
        {
            await _eventBus.PublishAsync(new AddAuditEvent(serviceName, methodName, actionMessage));
            return true;
        }
    }
}