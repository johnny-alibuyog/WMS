using System;
using System.Collections.Generic;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;

namespace AmpedBiz.Core.Services.Orders
{
    public abstract class State
    {
        protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

        protected Order Target { get; private set; }

        public virtual IDictionary<OrderStatus, string> AllowedTransitions { get; protected set; }

        public State(Order target)
        {
            this.Target = target;
            this.AllowedTransitions = new Dictionary<OrderStatus, string>();
        }

        public static State GetState(Order target)
        {
            switch (target.Status)
            {
                case OrderStatus.New:
                    return new NewState(target);

                case OrderStatus.Staged:
                    return new StagedState(target);

                case OrderStatus.Routed:
                    return new RoutedState(target);

                case OrderStatus.Invoiced:
                    return new InvoicedState(target);

                case OrderStatus.Paid:
                    return new PaidState(target);

                case OrderStatus.Completed:
                    return new CompletedState(target);

                case OrderStatus.Cancelled:
                    return new CancelledState(target);

                default:
                    return new NewState(target);
            }
        }

        public virtual void Process(OrderNewlyCreatedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.New))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "creation/modification", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderStagedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Staged))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "staging", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderRoutedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Routed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "routing", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderInvoicedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderPaidEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Paid))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "payment", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderShippedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Shipped))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "shipping", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderCompletedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Completed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "completing", this.Target.Status));

            this.Target.Process(@event);
        }

        public virtual void Process(OrderCancelledEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "canceling", this.Target.Status));

            this.Target.Process(@event);
        }
    }

    public class NewState : State
    {
        public NewState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.New, "Save");
            this.AllowedTransitions.Add(OrderStatus.Staged, "Stage");
            this.AllowedTransitions.Add(OrderStatus.Routed, "Route");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class StagedState : State
    {
        public StagedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.Routed, "Route");
            this.AllowedTransitions.Add(OrderStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class RoutedState : State
    {
        public RoutedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.Paid, "Pay");
            this.AllowedTransitions.Add(OrderStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class InvoicedState : State
    {
        public InvoicedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.Paid, "Pay");
            this.AllowedTransitions.Add(OrderStatus.Shipped, "Ship");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class PaidState : State
    {
        public PaidState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.Shipped, "Ship");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class ShippedState : State
    {
        public ShippedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.Completed, "Complete");
        }
    }

    public class CompletedState : State
    {
        public CompletedState(Order target) : base(target) { }
    }

    public class CancelledState : State
    {
        public CancelledState(Order target) : base(target) { }
    }
}