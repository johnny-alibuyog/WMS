using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Envents.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class State
    {
        protected PurchaseOrder Target { get; private set; }

        public virtual IDictionary<PurchaseOrderStatus, string> AllowedTransitions { get; protected set; }

        public State() { }

        public State(PurchaseOrder target)
        {
            this.Target = target;
            this.AllowedTransitions = new Dictionary<PurchaseOrderStatus, string>();
        }

        public static State GetState(PurchaseOrder target)
        {
            switch (target.Status)
            {
                case PurchaseOrderStatus.New:
                    return new NewState(target);
                case PurchaseOrderStatus.Submitted:
                    return new SubmittedState(target);
                case PurchaseOrderStatus.Approved:
                    return new ApprovedState(target);
                case PurchaseOrderStatus.Paid:
                    return new PaidState(target);
                case PurchaseOrderStatus.Received:
                    return new ReceivedState(target);
                case PurchaseOrderStatus.Completed:
                    return new CompletedState(target);
                case PurchaseOrderStatus.Cancelled:
                    return new CancelledState(target);
                default:
                    return new NewState(target);
            }
        }

        public virtual PurchaseOrder New(PurchaseOrderNewlyCreatedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.New))
                throw new InvalidOperationException(string.Format("You cannot perform creation of new purchase order on {0} stage.", this.Target.Status));

            return this.Target.New(@event);
        }

        public virtual PurchaseOrder Submit(PurchaseOrderSubmittedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Submitted))
                throw new InvalidOperationException(string.Format("You cannot perform submission of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Submit(@event);
        }

        public virtual PurchaseOrder Approve(PurchaseOrderApprovedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Approved))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Approve(@event);
        }

        public virtual PurchaseOrder Pay(PurchaseOrderPaidEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Paid))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Pay(@event);
        }

        public virtual PurchaseOrder Receive(PurchaseOrderReceivedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Received))
                throw new InvalidOperationException(string.Format("You cannot perform recieving of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Receive(@event);
        }

        public virtual PurchaseOrder Complete(PurchaseOrderCompletedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Completed))
                throw new InvalidOperationException(string.Format("You cannot perform completion of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Complete(@event);
        }

        public virtual PurchaseOrder Cancel(PurchaseOrderCancelledEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format("You cannot perform cancellation of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Cancel(@event);
        }
    }

    public class NewState : State
    {
        public NewState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.New, "Save");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Submitted, "Submit");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class SubmittedState : State
    {
        public SubmittedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.New, "Reject");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Approved, "Approve");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class ApprovedState : State
    {
        public ApprovedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.New, "Reject");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Paid, "Pay");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Receive");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class PaidState : State
    {
        public PaidState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.Paid, "Pay");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Receive");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Completed, "Complete");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class ReceivedState : State
    {
        public ReceivedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Receive");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Completed, "Complete");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class CompletedState : State
    {
        public CompletedState(PurchaseOrder target) : base(target) { }
    }

    public class CancelledState : State
    {
        public CancelledState(PurchaseOrder target) : base(target) { }
    }
}
