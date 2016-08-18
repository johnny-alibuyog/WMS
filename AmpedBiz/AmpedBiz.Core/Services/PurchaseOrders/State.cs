using AmpedBiz.Common.Configurations;
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

        public virtual PurchaseOrder Process(PurchaseOrderNewlyCreatedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.New))
                throw new InvalidOperationException(string.Format("You cannot perform creation of new purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }

        public virtual PurchaseOrder Process(PurchaseOrderSubmittedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Submitted))
                throw new InvalidOperationException(string.Format("You cannot perform submission of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }

        public virtual PurchaseOrder Process(PurchaseOrderApprovedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Approved))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }

        public virtual PurchaseOrder Process(PurchaseOrderPaidEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Paid))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }

        public virtual PurchaseOrder Process(PurchaseOrderReceivedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Received))
                throw new InvalidOperationException(string.Format("You cannot perform recieving of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }

        public virtual PurchaseOrder Process(PurchaseOrderCompletedEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Completed))
                throw new InvalidOperationException(string.Format("You cannot perform completion of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }

        public virtual PurchaseOrder Process(PurchaseOrderCancelledEvent @event)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format("You cannot perform cancellation of purchase order on {0} stage.", this.Target.Status));

            return this.Target.Process(@event);
        }
    }

    public class NewState : State
    {
        public NewState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.New];

            //this.AllowedTransitions.Add(PurchaseOrderStatus.New, "Save");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Submitted, "Submit");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class SubmittedState : State
    {
        public SubmittedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.Submitted];

            //this.AllowedTransitions.Add(PurchaseOrderStatus.New, "Reject");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Approved, "Approve");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class ApprovedState : State
    {
        public ApprovedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.Approved];

            //this.AllowedTransitions.Add(PurchaseOrderStatus.Paid, "Pay");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class PaidState : State
    {
        public PaidState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.Paid];

            //this.AllowedTransitions.Add(PurchaseOrderStatus.Paid, "Pay");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Receive");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Completed, "Complete");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class ReceivedState : State
    {
        public ReceivedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.Received];

            //this.AllowedTransitions.Add(PurchaseOrderStatus.Paid, "Pay");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Received, "Receive");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Completed, "Complete");
            //this.AllowedTransitions.Add(PurchaseOrderStatus.Cancelled, "Cancel");
        }
    }

    public class CompletedState : State
    {
        public CompletedState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.Completed];
        }
    }

    public class CancelledState : State
    {
        public CancelledState(PurchaseOrder target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[PurchaseOrderStatus.Cancelled];
        }
    }


    public class Stage
    {
        public static readonly Dictionary<PurchaseOrderStatus, Dictionary<PurchaseOrderStatus, string>> Transitions = GetConfig();

        private static Dictionary<PurchaseOrderStatus, Dictionary<PurchaseOrderStatus, string>> GetConfig()
        {
            Func<string, PurchaseOrderStatus> ParseStatus = (value) =>
            {
                var result = default(PurchaseOrderStatus);

                if (!Enum.TryParse<PurchaseOrderStatus>(value, out result))
                {
                    throw new Exception($"{value} is an invalid order status. Please check check your transition configuration.");
                }

                return result;
            };

            var stageTransitions = new Dictionary<PurchaseOrderStatus, Dictionary<PurchaseOrderStatus, string>>();

            foreach (var item in TransitionConfig.Instance.PurchaseOrderTransitions)
            {
                var stage = ParseStatus(item.Key);
                var transitions = item.Value.ToDictionary(x => ParseStatus(x.Key), x => x.Value);

                stageTransitions.Add(stage, transitions);
            }

            return stageTransitions;
        }
    }

}
