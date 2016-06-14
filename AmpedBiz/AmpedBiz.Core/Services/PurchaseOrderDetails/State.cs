using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services.PurchaseOrderDetails
{
    public class State
    {
        protected PurchaseOrderDetail Target { get; private set; }

        public virtual IDictionary<PurchaseOrderDetailStatus, string> AllowedTransitions { get; protected set; }

        public State(PurchaseOrderDetail purchaseOrderDetail)
        {
            this.Target = purchaseOrderDetail;
            this.AllowedTransitions = new Dictionary<PurchaseOrderDetailStatus, string>();
        }

        public static State GetState(PurchaseOrderDetail target)
        {
            switch (target.Status)
            {
                case PurchaseOrderDetailStatus.New:
                    return new NewState(target);
                case PurchaseOrderDetailStatus.Submitted:
                    return new SubmittedState(target);
                case PurchaseOrderDetailStatus.Cancelled:
                    return new CancelledState(target);
                case PurchaseOrderDetailStatus.Posted:
                    return new PostedState(target);
                default:
                    return new NewState(target);
            }
        }

        public virtual void New(Product product, Money unitPrice, decimal quantity)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderDetailStatus.New))
                throw new InvalidOperationException(string.Format("You cannot perform creation of new purchase order detail on {0} stage.", this.Target.Status));

            this.Target.New(product: product, unitPrice: unitPrice, quantity: quantity);
        }

        protected internal virtual void Submit()
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderDetailStatus.Submitted))
                throw new InvalidOperationException(string.Format("You cannot perform submission of new purchase order detail on {0} stage.", this.Target.Status));

            this.Target.Submit();
        }

        protected internal virtual void Post()
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderDetailStatus.Posted))
                throw new InvalidOperationException(string.Format("You cannot perform post of new purchase order detail on {0} stage.", this.Target.Status));

            this.Target.Post();
        }

        protected internal virtual void Cancel()
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderDetailStatus.Cancelled))
                throw new InvalidOperationException(string.Format("You cannot perform cancellation of new purchase order detail on {0} stage.", this.Target.Status));

            this.Target.Cancel();
        }
    }

    public class NewState : State
    {
        public NewState(PurchaseOrderDetail target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderDetailStatus.New, "Save");
            this.AllowedTransitions.Add(PurchaseOrderDetailStatus.Submitted, "Submit");
            this.AllowedTransitions.Add(PurchaseOrderDetailStatus.Cancelled, "Cancel");
        }
    }

    public class SubmittedState : State
    {
        public SubmittedState(PurchaseOrderDetail target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderDetailStatus.New, "Save");
            this.AllowedTransitions.Add(PurchaseOrderDetailStatus.Posted, "Post");
            this.AllowedTransitions.Add(PurchaseOrderDetailStatus.Cancelled, "Cancel");
        }
    }

    public class CancelledState : State
    {
        public CancelledState(PurchaseOrderDetail target) : base(target) { }
    }

    public class PostedState : State
    {
        public PostedState(PurchaseOrderDetail target) : base(target) { }
    }
}
