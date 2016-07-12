using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.OrderItems
{
    public abstract class State
    {
        private const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order item on {1} stage.";

        protected OrderItem Target { get; private set; }

        public virtual IDictionary<OrderItemStatus, string> AllowedTransitions { get; protected set; }

        public State(OrderItem target)
        {
            this.Target = target;
            this.AllowedTransitions = new Dictionary<OrderItemStatus, string>();
        }

        public static State GetState(OrderItem target)
        {
            switch (target.Status)
            {
                case OrderItemStatus.Allocated:
                    return new AllocatedState(target);

                case OrderItemStatus.Invoiced:
                    return new InvoicedState(target);

                case OrderItemStatus.Shipped:
                    return new ShippedState(target);

                case OrderItemStatus.BackOrdered:
                    return new BackOrderedState(target);
                default:
                    return new AllocatedState(target);
            }
        }

        public virtual void Allocate()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderItemStatus.Allocated))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "allocating", this.Target.Status));

            this.Target.Allocate();
        }

        public virtual void Invoice()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderItemStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this.Target.Status));

            this.Target.Invoice();
        }

        public virtual void Ship()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderItemStatus.Shipped))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "shipping", this.Target.Status));

            this.Target.Ship();
        }

        public virtual void BackOrder()
        {
            if (!this.AllowedTransitions.ContainsKey(OrderItemStatus.BackOrdered))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "back ordering", this.Target.Status));

            this.Target.BackOrder();
        }
    }

    public class AllocatedState : State
    {
        public AllocatedState(OrderItem target) : base(target)
        {
            this.AllowedTransitions.Add(OrderItemStatus.Invoiced, "Invoice");
            this.AllowedTransitions.Add(OrderItemStatus.BackOrdered, "Backorder");
        }
    }

    public class InvoicedState : State
    {
        public InvoicedState(OrderItem target) : base(target)
        {
            this.AllowedTransitions.Add(OrderItemStatus.BackOrdered, "Backorder");
            this.AllowedTransitions.Add(OrderItemStatus.Shipped, "Ship");
        }
    }

    public class ShippedState : State
    {
        public ShippedState(OrderItem target) : base(target)
        {
            this.AllowedTransitions.Add(OrderItemStatus.BackOrdered, "Backorder");
        }
    }

    public class BackOrderedState : State
    {
        public BackOrderedState(OrderItem target) : base(target)
        {
            this.AllowedTransitions.Add(OrderItemStatus.Allocated, "Allocate");
            this.AllowedTransitions.Add(OrderItemStatus.Invoiced, "Invoice");
        }
    }
}