using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NT.CheckoutProcess.Core;
using NT.Core;
using NT.Core.Results;
using NT.Infrastructure;
using NT.Infrastructure.AspNetCore;
using NT.OrderService.Core;
using Stateless;

namespace NT.CheckoutProcess.Infrastructure
{
    /// <summary>
    /// TODO: Need to have an ActivityAudit Service so that we can track the activity in UI
    /// </summary>
    public class CheckoutSaga
    {
        public enum State
        {
            Checkout,
            OrderStatus,
            ProductQuantity,
            Payment,
            WaitingPayment,
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
            WaitingPayment,
            MakePaymentSucceed,
            MakePaymentFailed,
            ChangeCompletedStatus
        }

        private readonly StateMachine<State, Trigger> _machine;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _checkoutTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _completedTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _orderStatusTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _paymentTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _waitingPaymentTrigger;
        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _productQuantityTrigger;

        private readonly IRepository<SagaInfo> _repository;
        private readonly RestClient _restClient;
        private Guid _correlationId;
        private CheckoutInfo _internalData;
        private State _state = State.Checkout;
        private readonly ILogger _logger;

        public CheckoutSaga(
            IRepository<SagaInfo> repository,
            RestClient restClient)
        {
            _repository = repository;
            _restClient = restClient;
            _logger = LogFactory.GetLogInstance<CheckoutSaga>();

            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);
            _checkoutTrigger = _machine.SetTriggerParameters<Guid>(Trigger.Checkout);
            _orderStatusTrigger = _machine.SetTriggerParameters<Guid>(Trigger.UpdateOrderStatus);
            _productQuantityTrigger = _machine.SetTriggerParameters<Guid>(Trigger.UpdateProductQuantity);
            _paymentTrigger = _machine.SetTriggerParameters<Guid>(Trigger.MakePayment);
            _waitingPaymentTrigger = _machine.SetTriggerParameters<Guid>(Trigger.WaitingPayment);
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
                .OnEntryFromAsync(_paymentTrigger, (correlationId, t) => OnMakePayment(correlationId));

            _machine.Configure(State.WaitingPayment)
                .OnEntryFromAsync(_waitingPaymentTrigger, (correlationId, t) => OnPaymentReceived(correlationId))
                .InternalTransitionAsync(Trigger.MakePaymentSucceed, t => OnPaymentSucceed(_correlationId))
                .InternalTransitionAsync(Trigger.MakePaymentFailed, t => OnPaymentFailed(_correlationId))
                .Permit(Trigger.ChangeCompletedStatus, State.Completed)
                .OnExitAsync(async () =>
                {
                    await OnSendEmail(_internalData.CustomerId);
                    await OnNotifyEmployee(_internalData.EmployeeId);
                });


            _machine.Configure(State.Completed)
                .OnEntryFromAsync(_completedTrigger, (correlationId, t) => OnCompletedProcess(correlationId));
        }

        public async Task Checkout(Guid correlationId, Guid orderId)
        {
            var result = await LoadFromStorage(correlationId, orderId);
            _state = (State)result.Item1.SagaStatus;
            _internalData = result.Item2;
            _correlationId = correlationId;
            await _machine.FireAsync(_checkoutTrigger, correlationId);
        }

        public async Task PaymentAccepted(Guid correlationId)
        {
            var result =  await LoadFromStorage(correlationId);
            _state = (State)result.Item1.SagaStatus;
            _internalData = result.Item2;
            _correlationId = correlationId;
            await _machine.FireAsync(_waitingPaymentTrigger, correlationId);
        }

        private async Task OnCheckout(Guid correlationId)
        {
            await _machine.FireAsync(_orderStatusTrigger, correlationId);
        }

        private async Task OnUpdateOrderStatus()
        {
            var isSucceed = await UpdateOrderStatus(_internalData.OrderId, OrderStatus.Processing);
            if (isSucceed)
                await _machine.FireAsync(Trigger.UpdateOrderStatusSucceed);
            else
                await _machine.FireAsync(Trigger.UpdateOrderStatusFailed);
        }

        private async Task OnOrderStatusSucceed(Guid correlationId)
        {
            await _machine.FireAsync(_productQuantityTrigger, correlationId);
        }

        private async Task OnOrderStatusFailed(Guid correlationId)
        {
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.New);
            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnUpdateProductQuantity()
        {
            var isSucceed = true;
            foreach (var product in _internalData.Products)
                isSucceed = isSucceed && await DescreaseQuantityOfProductInCatalog(product.ProductId, product.Quantity);

            if (isSucceed)
                await _machine.FireAsync(Trigger.UpdateProductQuantitySucceed);
            else
                await _machine.FireAsync(Trigger.UpdateProductQuantityFailed);
        }

        private async Task OnProductQuantitySucceed(Guid correlationId)
        {
            await _machine.FireAsync(_paymentTrigger, correlationId);
        }

        private async Task OnProductQuantityFailed(Guid correlationId)
        {
            foreach (var product in _internalData.Products)
                await CompensateQuantityOfProductInCatalog(product.ProductId, product.Quantity);
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.New);
            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnMakePayment(Guid correlationId)
        {
            var money = 0.0D;
            foreach (var productInOrder in _internalData.Products)
            {
                money += await GetProductPrice(productInOrder.ProductId) * productInOrder.Quantity;
            }

            // Submit request to order service to update WaitingPayment status, and correlationId to SagaID column
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.WaitingPayment);
            await UpdateSagaInfo(_internalData.OrderId, correlationId);

            // TODO: 2. Submit request to payment service
            // TODO: ...

            // update storage for next time processing
            await UpdateStorage(correlationId, (int)State.WaitingPayment, _internalData);
        }

        private async Task OnPaymentReceived(Guid correlationId)
        {
            await _machine.FireAsync(Trigger.MakePaymentSucceed);
        }

        private async Task OnPaymentSucceed(Guid correlationId)
        {
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.Paid);
            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnPaymentFailed(Guid correlationId)
        {
            // TODO: roll back money if has

            foreach (var product in _internalData.Products)
                await CompensateQuantityOfProductInCatalog(product.ProductId, product.Quantity);
            await UpdateOrderStatus(_internalData.OrderId, OrderStatus.New);
            await _machine.FireAsync(_completedTrigger, correlationId);
        }

        private async Task OnSendEmail(Guid customerId)
        {
            // TODO: Send email to customer based on information in the internal data
            _logger.LogInformation($"Sending an email to customer [{customerId}]");
        }

        private async Task OnNotifyEmployee(Guid employeeId)
        {
            // TODO: Put one record for notification into the notification service
            _logger.LogInformation($"Notifying information to employee [{employeeId}]");
        }

        private async Task OnCompletedProcess(Guid correlationId)
        {
            await UpdateStorage(correlationId, (int)State.Completed, _internalData);
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

        private async Task<bool> DescreaseQuantityOfProductInCatalog(Guid productId, int quantityInOrder)
        {
            var result = await _restClient.PutAsync<SagaResult>(
                "catalog_service",
                $"/api/products/{productId}/descrease-quantity/{quantityInOrder}");
            return result.Succeed;
        }

        private async Task<bool> CompensateQuantityOfProductInCatalog(Guid productId, int quantityInOrder)
        {
            var result = await _restClient.PutAsync<SagaResult>(
                "catalog_service",
                $"/api/products/{productId}/increase-quantity/{quantityInOrder}");
            return result.Succeed;
        }
    }
}