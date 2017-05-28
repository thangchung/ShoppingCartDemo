using System;
using NT.CheckoutProcess.Core;
using NT.Core;
using NT.Core.Events;
using NT.Infrastructure;
using NT.Infrastructure.AspNetCore;
using NT.Infrastructure.MessageBus.Event;
using NT.OrderService.Core;
using Stateless;

namespace NT.CheckoutProcess.Infrastructure
{
    public class CheckoutWorkflow
    {
        public enum State
        {
            Checkout,
            OrderStatus,
            ProductQuantity,
            Payment,
            EmailToCustomer,
            EmployeeNotification,
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
            MakePaymentSucceed,
            MakePaymentFailed,
            SendEmailToCustomer,
            NotifyMessageToEmployee
        }

        private readonly StateMachine<State, Trigger>.TriggerWithParameters<Guid> _checkoutTrigger;
        private readonly StateMachine<State, Trigger> _machine;

        private readonly IRepository<SagaInfo> _repository;
        private readonly IEventBus _messageBus;
        private readonly RestClient _restClient;
        private CheckoutInfo _internalData;
        private State _state = State.Checkout;

        public CheckoutWorkflow(
            IRepository<SagaInfo> repository, 
            IEventBus messageBus, 
            RestClient restClient)
        {
            _repository = repository;
            _messageBus = messageBus;
            _restClient = restClient;

            _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);
            _checkoutTrigger = _machine.SetTriggerParameters<Guid>(Trigger.Checkout);

            _machine.Configure(State.Checkout)
                .OnEntryFrom(_checkoutTrigger, OnCheckout)
                .PermitReentry(Trigger.Checkout)
                .Permit(Trigger.UpdateOrderStatus, State.OrderStatus);

            _machine.Configure(State.OrderStatus)
                .InternalTransition(Trigger.UpdateOrderStatus, t => OnUpdateOrderStatus())
                .InternalTransition(Trigger.UpdateOrderStatusSucceed, t => OnOrderStatusSucceed())
                .InternalTransition(Trigger.UpdateOrderStatusFailed, t => OnOrderStatusFailed())
                .Permit(Trigger.UpdateProductQuantity, State.ProductQuantity);

            _machine.Configure(State.ProductQuantity)
                .InternalTransition(Trigger.UpdateProductQuantity, t => OnUpdateProductQuantity())
                .InternalTransition(Trigger.UpdateProductQuantitySucceed, t => OnProductQuantitySucceed())
                .InternalTransition(Trigger.UpdateProductQuantityFailed, t => OnProductQuantityFailed())
                .Permit(Trigger.MakePayment, State.Payment);

            _machine.Configure(State.Payment)
                .InternalTransition(Trigger.MakePayment, t => OnMakePayment())
                .InternalTransition(Trigger.MakePaymentSucceed, t => OnPaymentSucceed())
                .InternalTransition(Trigger.MakePaymentFailed, t => OnPaymentFailed())
                .Permit(Trigger.SendEmailToCustomer, State.EmailToCustomer);

            _machine.Configure(State.EmailToCustomer)
                .InternalTransition(Trigger.SendEmailToCustomer, t => OnSendEmail())
                .Permit(Trigger.NotifyMessageToEmployee, State.EmployeeNotification);

            _machine.Configure(State.EmployeeNotification)
                .InternalTransition(Trigger.NotifyMessageToEmployee, t => OnNotifyEmployee());
        }

        public void Checkout(Guid orderId)
        {
            _machine.Fire(_checkoutTrigger, orderId);
            _messageBus.Publish(new ProcessOrderStatusEvent());
        }

        public void ProcessOrderStatus()
        {
            _machine.Fire(Trigger.UpdateOrderStatus);
        }

        public void ProcessProductQuantity()
        {
            _machine.Fire(Trigger.UpdateProductQuantity);
        }

        public void MakePayment()
        {
            _machine.Fire(Trigger.MakePayment);
        }

        public void SendEmailToCustomer()
        {
            _machine.Fire(Trigger.SendEmailToCustomer);
        }

        public void NotifyToEmployee()
        {
            _machine.Fire(Trigger.NotifyMessageToEmployee);
        }

        private void OnCheckout(Guid orderId)
        {
            var order = _restClient.GetAsync<Order>("order_service", $"/api/orders/{orderId}").Result;
            // TODO: need to put Status into Order entity
            var sagaInfo = _repository.GetByIdAsync(orderId).Result;
            if (sagaInfo == null)
            {
                _internalData = new CheckoutInfo
                {
                    OrderId = orderId,
                    OrderStatus = 1
                };
                var saga = new SagaInfo
                {
                    Id = orderId, // correlation id
                    Data = _internalData.ToString<CheckoutInfo>()
                };
                _repository.AddAsync(saga).Wait();
            }
            else
            {
                _internalData = sagaInfo.Data.ToObject<CheckoutInfo>();
            }
        }

        private void OnUpdateOrderStatus()
        {
            // TODO: call to OrderService for updating the Status of Order from 1 to 2
            // TODO: if succeed
            _machine.Fire(Trigger.UpdateOrderStatusSucceed);
            // TODO: else 
            // _machine.Fire(Trigger.OrderStatusFailProcessing);
        }

        private void OnOrderStatusSucceed()
        {
            _messageBus.Publish(new ProcessProductQuantityEvent());
        }

        private void OnOrderStatusFailed()
        {
            // TODO: compensate the status back to 1 
        }

        private void OnUpdateProductQuantity()
        {
        }

        private void OnProductQuantitySucceed()
        {

        }

        private void OnProductQuantityFailed()
        {

        }

        private void OnMakePayment()
        {
        }

        private void OnPaymentSucceed()
        {

        }

        private void OnPaymentFailed()
        {

        }

        private void OnSendEmail()
        {

        }

        private void OnNotifyEmployee()
        {

        }
    }
}