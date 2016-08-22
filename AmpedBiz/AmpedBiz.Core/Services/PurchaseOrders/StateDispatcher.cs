using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class StateDispatcher
    {
        protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

        private readonly PurchaseOrder _target;

        public virtual IDictionary<PurchaseOrderStatus, string> AllowedTransitions { get; protected set; }

        public StateDispatcher(PurchaseOrder target)
        {
            this._target = target;
            this.AllowedTransitions = Stage.Transitions[target.Status];
        }

        public virtual void Process(PurchaseOrderVisitor visitor)
        {
            this.Process((dynamic)visitor);
        }

        private void Process(PurchaseOrderNewlyCreatedVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.New))
                throw new InvalidOperationException(string.Format("You cannot perform creation of new purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(PurchaseOrderSubmittedVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Submitted))
                throw new InvalidOperationException(string.Format("You cannot perform submission of purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(PurchaseOrderApprovedVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Approved))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(PurchaseOrderPaidVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Paid))
                throw new InvalidOperationException(string.Format("You cannot perform approval of purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(PurchaseOrderReceivedVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Received))
                throw new InvalidOperationException(string.Format("You cannot perform recieving of purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(PurchaseOrderCompletedVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Completed))
                throw new InvalidOperationException(string.Format("You cannot perform completion of purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(PurchaseOrderCancelledVisitor visitor)
        {
            if (!this.AllowedTransitions.ContainsKey(PurchaseOrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format("You cannot perform cancellation of purchase order on {0} stage.", this._target.Status));

            this._target.Accept(visitor);
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
