using System;
using System.Collections.Generic;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Orders
{
    public abstract class State
    {
        private const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

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

                case OrderStatus.PartiallyPaid:
                    return new PartiallyPaidState(target);

                case OrderStatus.Completed:
                    return new CompletedState(target);

                case OrderStatus.Cancelled:
                    return new CancelledState(target);

                default:
                    return new NewState(target);
            }
        }

        public virtual void New(User createdBy, PaymentType paymentType = null, Shipper shipper = null,
            Money shippingFee = null, decimal? taxRate = null, Customer customer = null, Branch branch = null)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.New))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, this.Target.Status));

            this.Target.New(
                paymentType,
                shipper,
                taxRate,
                shippingFee,
                createdBy,
                customer,
                branch
                );
        }

        public virtual void Stage(User stagedBy)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Staged))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "staging", this.Target.Status));

            this.Target.Stage(stagedBy);
        }

        public virtual void Route(User routedBy)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Routed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "routing", this.Target.Status));

            this.Target.Route(routedBy);
        }

        public virtual void Invoice(User invoicedBy, Invoice invoice)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this.Target.Status));

            this.Target.Invoice(invoicedBy, invoice);
        }

        public virtual void PartiallyPay(User partiallyPaidBy)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.PartiallyPaid))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "partial payment", this.Target.Status));

            this.Target.PartiallyPay(partiallyPaidBy);
        }

        public virtual void Complete(User completedBy)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Completed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "completing", this.Target.Status));

            this.Target.Complete(completedBy);
        }

        public virtual void Cancel(User cancelledBy, string cancellationReason)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "cancelling", this.Target.Status));

            this.Target.Cancel(cancelledBy, cancellationReason);
        }
    }

    public class NewState : State
    {
        public NewState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.New, "Save");
            this.AllowedTransitions.Add(OrderStatus.Routed, "Stage");
            this.AllowedTransitions.Add(OrderStatus.Routed, "Route");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class StagedState : State
    {
        public StagedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.PartiallyPaid, "Partially Pay");
            this.AllowedTransitions.Add(OrderStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class RoutedState : State
    {
        public RoutedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.PartiallyPaid, "Partially Pay");
            this.AllowedTransitions.Add(OrderStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class InvoicedState : State
    {
        public InvoicedState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.PartiallyPaid, "Partially Pay");
            this.AllowedTransitions.Add(OrderStatus.Completed, "Complete");
        }
    }

    public class PartiallyPaidState : State
    {
        public PartiallyPaidState(Order target) : base(target)
        {
            this.AllowedTransitions.Add(OrderStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderStatus.Completed, "Complete");
            this.AllowedTransitions.Add(OrderStatus.Cancelled, "Cancel");
        }
    }

    public class CompletedState : State
    {
        public CompletedState(Order target) : base(target)
        {
        }
    }

    public class CancelledState : State
    {
        public CancelledState(Order target) : base(target)
        {
        }
    }
}