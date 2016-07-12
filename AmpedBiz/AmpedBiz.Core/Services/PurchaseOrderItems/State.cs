using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.PurchaseOrderItems
{
    public class State
    {
        protected PurchaseOrderItem Target { get; private set; }

        public virtual IDictionary<PurchaseOrderItemStatus, string> AllowedTransitions { get; protected set; }

        public State() { }

        public State(PurchaseOrderItem purchaseOrderItem)
        {
            this.Target = purchaseOrderItem;
            this.AllowedTransitions = new Dictionary<PurchaseOrderItemStatus, string>();
        }

        public static State GetState(PurchaseOrderItem target)
        {
            switch (target.Status)
            {
                case PurchaseOrderItemStatus.New:
                    return new NewState(target);
                case PurchaseOrderItemStatus.Submitted:
                    return new SubmittedState(target);
                case PurchaseOrderItemStatus.Cancelled:
                    return new CancelledState(target);
                case PurchaseOrderItemStatus.Posted:
                    return new PostedState(target);
                default:
                    return new NewState(target);
            }
        }

        public virtual PurchaseOrderItem New(Product product, Money unitPrice, decimal quantity)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderItemStatus.New))
                throw new InvalidOperationException(string.Format("You cannot perform creation of new purchase order item on {0} stage.", this.Target.Status));

            return this.Target.New(product: product, unitPrice: unitPrice, quantity: quantity);
        }

        public virtual PurchaseOrderItem Submit()
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderItemStatus.Submitted))
                throw new InvalidOperationException(string.Format("You cannot perform submission of purchase order item on {0} stage.", this.Target.Status));

            return this.Target.Submit();
        }

        public virtual PurchaseOrderItem Post()
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderItemStatus.Posted))
                throw new InvalidOperationException(string.Format("You cannot perform post of purchase order item on {0} stage.", this.Target.Status));

            return this.Target.Post();
        }

        public virtual PurchaseOrderItem Cancel()
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderItemStatus.Cancelled))
                throw new InvalidOperationException(string.Format("You cannot perform cancellation of purchase order item on {0} stage.", this.Target.Status));

            return this.Target.Cancel();
        }
    }

    public class NewState : State
    {
        public NewState(PurchaseOrderItem target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderItemStatus.New, "Save");
            this.AllowedTransitions.Add(PurchaseOrderItemStatus.Submitted, "Submit");
            this.AllowedTransitions.Add(PurchaseOrderItemStatus.Cancelled, "Cancel");
        }
    }

    public class SubmittedState : State
    {
        public SubmittedState(PurchaseOrderItem target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderItemStatus.New, "Save");
            this.AllowedTransitions.Add(PurchaseOrderItemStatus.Posted, "Post");
            this.AllowedTransitions.Add(PurchaseOrderItemStatus.Cancelled, "Cancel");
        }
    }

    public class CancelledState : State
    {
        public CancelledState(PurchaseOrderItem target) : base(target) { }
    }

    public class PostedState : State
    {
        public PostedState(PurchaseOrderItem target) : base(target) { }
    }
}
