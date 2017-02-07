using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class StateDispatcher
    {
        protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

        private readonly Order _target;

        public virtual StageDefenition<OrderStatus, OrderAggregate> Stage { get; protected set; }

        //public virtual OrderStatus[] AllowedTransitions { get; protected set; }

        //public virtual OrderAggregate[] AllowedModifications { get; protected set; }

        //public virtual Definition<OrderStatus, OrderAggregate> StateDefinition { get; protected set; }

        public StateDispatcher(Order target)
        {
            this._target = target;
            this.Stage = StageDefinitionConfigReader.Values[target.Status];
            //this.AllowedTransitions = Orders.Stage.Values[target.Status].AllowedTransitions;
            //this.AllowedModifications = Orders.Stage.Values[target.Status].AllowedModifications;
        }

        public virtual void Process(OrderVisitor visitor)
        {
            this.Process((dynamic)visitor);
        }

        private void Process(OrderSaveVisitor visitor)
        {
            if (!this.Stage.IsModificationAllowed())
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "creation/modification", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(OrderInvoicedVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(OrderStagedVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Staged))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "staging", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(OrderRoutedVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Routed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "routing", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(OrderShippedVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Shipped))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "shipping", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(OrderCompletedVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Completed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "completing", this._target.Status));

            this._target.Accept(visitor);
        }

        private void Process(OrderCancelledVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "canceling", this._target.Status));

            this._target.Accept(visitor);
        }


    }

    public class StageDefinitionConfigReader
    {
        public static readonly Dictionary<OrderStatus, StageDefenition<OrderStatus, OrderAggregate>> Values = GetConfig();

        private static Dictionary<OrderStatus, StageDefenition<OrderStatus, OrderAggregate>> GetConfig()
        {
            var stageDefinitions = new Dictionary<OrderStatus, StageDefenition<OrderStatus, OrderAggregate>>();

            foreach (var item in StateConfig.Instance.Value.OrderConfig)
            {
                stageDefinitions.Add(
                    key: item.Key.As<OrderStatus>(),
                    value: new StageDefenition<OrderStatus, OrderAggregate>(
                        allowedTransitions: item.Value.AllowedTransitions,
                        allowedModifications: item.Value.AllowedModifications
                    )
                );
            }

            return stageDefinitions;
        }
    }

    public class StageOld
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