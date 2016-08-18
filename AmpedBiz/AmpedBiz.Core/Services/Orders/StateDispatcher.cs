using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class StateDispatcher
    {
        protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

        private readonly Order _target;

        public virtual IDictionary<OrderStatus, string> AllowedTransitions { get; protected set; }

        public StateDispatcher(Order target)
        {
            this._target = target;
            this.AllowedTransitions = Stage.Transitions[target.Status];
        }

        public virtual void Process(OrderVisitor args)
        {
            this.Process((dynamic)args);
        }

        private void Process(OrderNewlyCreatedVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.New))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "creation/modification", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderInvoicedVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderPaidVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Paid))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "payment", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderStagedVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Staged))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "staging", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderRoutedVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Routed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "routing", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderShippedVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Shipped))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "shipping", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderCompletedVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Completed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "completing", this._target.Status));

            this._target.Accept(args);
        }

        private void Process(OrderCancelledVisitor args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "canceling", this._target.Status));

            this._target.Accept(args);
        }
    }

    public class Stage
    {
        public static readonly Dictionary<OrderStatus, Dictionary<OrderStatus, string>> Transitions = GetConfig();

        private static Dictionary<OrderStatus, Dictionary<OrderStatus, string>> GetConfig()
        {
            Func<string, OrderStatus> ParseStatus = (value) =>
            {
                var result = default(OrderStatus);

                if (!Enum.TryParse<OrderStatus>(value, out result))
                {
                    throw new Exception($"{value} is an invalid order status. Please check check your transition configuration.");
                }

                return result;
            };

            var stageTransitions = new Dictionary<OrderStatus, Dictionary<OrderStatus, string>>();

            foreach (var item in TransitionConfig.Instance.OrderTransitions)
            {
                var stage = ParseStatus(item.Key);
                var transitions = item.Value.ToDictionary(x => ParseStatus(x.Key), x => x.Value);

                stageTransitions.Add(stage, transitions);
            }

            return stageTransitions;
        }
    }
}