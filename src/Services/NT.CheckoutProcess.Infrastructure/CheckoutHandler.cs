using System;
using System.Reactive;
using System.Reactive.Linq;
using NT.Core.Events;
using NT.Infrastructure.MessageBus.Event;

namespace NT.CheckoutProcess.Infrastructure
{
    public class CheckoutHandler : 
        IEventBusHandler<CheckoutEvent>,
        IEventBusHandler<ProcessOrderStatusEvent>,
        IEventBusHandler<ProcessProductQuantityEvent>,
        IEventBusHandler<MakePaymentEvent>,
        IEventBusHandler<EmailToCustomerEvent>,
        IEventBusHandler<NotifyToEmployeeEvent>
    {
        private readonly CheckoutWorkflow _workflow;

        public CheckoutHandler(CheckoutWorkflow workflow)
        {
            _workflow = workflow;
        }

        public IObservable<Unit> Handle(CheckoutEvent message)
        {
            _workflow.Checkout(message.OrderId);
            return Observable.Return(new Unit());
        }

        public IObservable<Unit> Handle(ProcessOrderStatusEvent message)
        {
            _workflow.ProcessOrderStatus();
            return Observable.Return(new Unit());
        }

        public IObservable<Unit> Handle(ProcessProductQuantityEvent message)
        {
            _workflow.ProcessProductQuantity();
            return Observable.Return(new Unit());
        }

        public IObservable<Unit> Handle(MakePaymentEvent message)
        {
            _workflow.MakePayment();
            return Observable.Return(new Unit());
        }

        public IObservable<Unit> Handle(EmailToCustomerEvent message)
        {
            _workflow.SendEmailToCustomer();
            return Observable.Return(new Unit());
        }

        public IObservable<Unit> Handle(NotifyToEmployeeEvent message)
        {
            _workflow.NotifyToEmployee();
            return Observable.Return(new Unit());
        }
    }
}