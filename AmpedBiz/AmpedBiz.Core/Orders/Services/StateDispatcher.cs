﻿using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Orders.Services
{
	public class StateDispatcher
    {
        protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

        private readonly Order _target;

        public virtual StageDefinition<OrderStatus, OrderAggregate> Stage { get; protected set; }

        public StateDispatcher(Order target)
        {
            this._target = target;
            this.Stage = StageDefinitionConfigReader.Values[target.Status];
        }

        public virtual void Process(IVisitor<Order> visitor)
        {
            this.Process((dynamic)visitor);
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

        private void Process(OrderModifiedBackVisitor visitor)
        {
            if (!this.Stage.IsTransitionAllowedTo(OrderStatus.Created))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "recreate", this._target.Status));

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
        public static readonly Dictionary<OrderStatus, StageDefinition<OrderStatus, OrderAggregate>> Values = GetConfig();

        private static Dictionary<OrderStatus, StageDefinition<OrderStatus, OrderAggregate>> GetConfig()
        {
            var stageDefinitions = new Dictionary<OrderStatus, StageDefinition<OrderStatus, OrderAggregate>>();

            foreach (var item in StateConfig.Instance.Value.OrderConfig)
            {
                stageDefinitions.Add(
                    key: item.Key.As<OrderStatus>(),
                    value: new StageDefinition<OrderStatus, OrderAggregate>(
                        allowedTransitions: item.Value.AllowedTransitions,
                        allowedModifications: item.Value.AllowedModifications
                    )
                );
            }

            return stageDefinitions;
        }
    }
}