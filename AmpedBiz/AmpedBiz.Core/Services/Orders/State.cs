﻿using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Arguments.Orders;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public abstract class State
    {
        protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of order on {1} stage.";

        protected Order Target { get; private set; }

        public virtual IDictionary<OrderStatus, string> AllowedTransitions { get; protected set; }

        public State(Order target)
        {
            this.Target = target;
            this.AllowedTransitions = new Dictionary<OrderStatus, string>();
        }

        public static State GetState(Order target)
        {
            switch (target.Status)
            {
                case OrderStatus.New:
                    return new NewState(target);

                case OrderStatus.Staged:
                    return new StagedState(target);

                case OrderStatus.Routed:
                    return new RoutedState(target);

                case OrderStatus.Invoiced:
                    return new InvoicedState(target);

                case OrderStatus.Paid:
                    return new PaidState(target);

                case OrderStatus.Shipped:
                    return new ShippedState(target);

                case OrderStatus.Completed:
                    return new CompletedState(target);

                case OrderStatus.Cancelled:
                    return new CancelledState(target);

                default:
                    return new NewState(target);
            }
        }

        public virtual void Process(OrderNewlyCreatedArguments args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.New))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "creation/modification", this.Target.Status));

            this.Target.Process(args);
        }

        public virtual void Process(OrderInvoicedArguments args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Invoiced))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "invoicing", this.Target.Status));

            this.Target.Process(args);
        }

        public virtual void Process(OrderPaidArguments args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Paid))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "payment", this.Target.Status));

            this.Target.Process(args);
        }

        public virtual void Process(OrderStagedArguments args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Staged))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "staging", this.Target.Status));

            this.Target.Process(args);
        }

        public virtual void Process(OrderRoutedArguments args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Routed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "routing", this.Target.Status));

            this.Target.Process(args);
        }

        public virtual void Process(OrderShippedArguments args)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Shipped))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "shipping", this.Target.Status));

            this.Target.Process(args);
        }

        public virtual void Process(OrderCompletedArguments arguments)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Completed))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "completing", this.Target.Status));

            this.Target.Process(arguments);
        }

        public virtual void Process(OrderCancelledArguments arguments)
        {
            if (!this.AllowedTransitions.ContainsKey(OrderStatus.Cancelled))
                throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "canceling", this.Target.Status));

            this.Target.Process(arguments);
        }
    }

    public class NewState : State
    {
        public NewState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.New];
        }
    }

    public class InvoicedState : State
    {
        public InvoicedState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Invoiced];
        }
    }

    public class PaidState : State
    {
        public PaidState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Paid];
        }
    }

    public class StagedState : State
    {
        public StagedState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Staged];
        }
    }

    public class RoutedState : State
    {
        public RoutedState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Routed];
        }
    }

    public class ShippedState : State
    {
        public ShippedState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Shipped];
        }
    }

    public class CompletedState : State
    {
        public CompletedState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Completed];
        }
    }

    public class CancelledState : State
    {
        public CancelledState(Order target) : base(target)
        {
            this.AllowedTransitions = Stage.Transitions[OrderStatus.Cancelled];
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