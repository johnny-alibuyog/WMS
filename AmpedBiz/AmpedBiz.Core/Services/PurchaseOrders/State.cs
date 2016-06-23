using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public abstract class State
    {
        protected PurchaseOrder Target { get; private set; }

        public virtual IDictionary<PurchaseOrderStatus, string> AllowedTransitions { get; protected set; }

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
                    return new PayedState(target);
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

        public virtual void New(Employee createdBy, DateTime createdOn, PaymentType paymentType = null, Shipper shipper = null, Money shippingFee = null, Money tax = null, Supplier supplier = null)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.New))
                throw new InvalidOperationException(string.Format("You cannot perform creation of new purchase order on {0} stage.", this.Target.Status));

            this.Target.New(
                createdBy: createdBy,
                createdOn: createdOn,
                paymentType: paymentType,
                tax: tax,
                shippingFee: shippingFee,
                supplier: supplier
            );
        }

        public virtual void Submit(Employee submittedBy, DateTime submittedOn)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Submitted))
                throw new InvalidOperationException(string.Format("You cannot perform submission of purchase order on {0} stage.", this.Target.Status));

            this.Target.Submit(submittedBy, submittedOn);
        }

        public virtual void Approve(Employee approvedBy, DateTime approvedOn)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Approved))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this.Target.Status));

            this.Target.Approve(approvedBy, approvedOn);
        }

        public virtual void Pay(Employee payedBy, DateTime payedOn, Money payment)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Paid))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this.Target.Status));

            this.Target.Pay(payedBy, payedOn, payment);
        }

        public virtual void Recieve(Employee recieveBy, DateTime recieveOn)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Received))
                throw new InvalidOperationException(string.Format("You cannot perform recieving of purchase order on {0} stage.", this.Target.Status));

            this.Target.Recieve(recieveBy, recieveOn);
        }

        public virtual void Complete(Employee completedBy, DateTime completedOn)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Completed))
                throw new InvalidOperationException(string.Format("You cannot perform completion of purchase order on {0} stage.", this.Target.Status));

            this.Target.Complete(completedBy, completedOn);
        }

        public virtual void Cancel(Employee cancelledBy, DateTime cancelledOn, string cancellationReason)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format("You cannot perform cancellation of purchase order on {0} stage.", this.Target.Status));

            this.Target.Cancel(cancelledBy, cancelledOn, cancellationReason);
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
            this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Recieve");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class PayedState : State
    {
        public PayedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.Paid, "Pay");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Recieve");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Completed, "Complete");
            this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class ReceivedState : State
    {
        public ReceivedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Recieve");
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
