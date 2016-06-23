using System;
using System.Collections.Generic;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.OrderDetailDetails
{
    public abstract class State
    {
        private const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of Detail details on {1} stage.";

        protected OrderDetail Target { get; private set; }

        public virtual IDictionary<OrderDetailStatus, string> AllowedTransitions { get; protected set; }

        public State(OrderDetail target)
        {
            this.Target = target;
            this.AllowedTransitions = new Dictionary<OrderDetailStatus, string>();
        }

        public static State GetState(OrderDetail target)
        {
            switch (target.Status)
            {
                case OrderDetailStatus.Allocated:
                    return new AllocatedState(target);

                case OrderDetailStatus.Invoiced:
                    return new InvoicedState(target);

                case OrderDetailStatus.Shipped:
                    return new ShippedState(target);

                case OrderDetailStatus.BackOrdered:
                    return new BackOrderedState(target);
                default:
                    return new AllocatedState(target);
            }
        }

        public virtual void Allocate()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderDetailStatus.Allocated))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "allocating", this.Target.Status));

            this.Target.Allocate();
        }

        public virtual void Invoice()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderDetailStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this.Target.Status));

            this.Target.Invoice();
        }

        public virtual void Ship()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderDetailStatus.Shipped))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "shipping", this.Target.Status));

            this.Target.Ship();
        }

        public virtual void BackOrder()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderDetailStatus.BackOrdered))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "back ordering", this.Target.Status));

            this.Target.BackOrder();
        }
    }

    public class AllocatedState : State
    {
        public AllocatedState(OrderDetail target) : base(target)
        {
            this.AllowedTransitions.Add(OrderDetailStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderDetailStatus.BackOrdered, "Backorder");
        }
    }

    public class InvoicedState : State
    {
        public InvoicedState(OrderDetail target) : base(target)
        {
            this.AllowedTransitions.Add(OrderDetailStatus.BackOrdered, "Backorder");
            this.AllowedTransitions.Add(OrderDetailStatus.Shipped, "Ship");
        }
    }

    public class ShippedState : State
    {
        public ShippedState(OrderDetail target) : base(target)
        {
            this.AllowedTransitions.Add(OrderDetailStatus.BackOrdered, "Backorder");
        }
    }

    public class BackOrderedState : State
    {
        public BackOrderedState(OrderDetail target) : base(target)
        {
            this.AllowedTransitions.Add(OrderDetailStatus.Allocated, "Allocate");
            this.AllowedTransitions.Add(OrderDetailStatus.Invoiced, "Invoice");
        }
    }
}