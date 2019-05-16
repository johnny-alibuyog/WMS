using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.PurchaseOrders.Services
{
	public class StateDispatcher
	{
		protected const string STATE_EXCEPTION_MESSAGE = "You cannot perform {0} of purchase order on {1} stage.";

		private readonly PurchaseOrder _target;

		public virtual StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate> Stage { get; protected set; }

		public virtual IDictionary<PurchaseOrderStatus, string> AllowedTransitions { get; protected set; }

		public StateDispatcher(PurchaseOrder target)
		{
			this._target = target;
			this.Stage = StageDefinitionConfigReader.Values[target.Status];
			//this.AllowedTransitions = StageOld.Transitions[target.Status];
		}

		public virtual void Process(IVisitor<PurchaseOrder> visitor)
		{
			this.Process((dynamic)visitor);
		}

		private void Process(PurchaseOrderSubmittedVisitor visitor)
		{
			if (!this.Stage.IsTransitionAllowedTo(PurchaseOrderStatus.Submitted))
				throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "submission", this._target.Status));

			this._target.Accept(visitor);
		}

		private void Process(PurchaseOrderRecreatedVisitor visitor)
		{
			if (!this.Stage.IsTransitionAllowedTo(PurchaseOrderStatus.Created))
				throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "recreate", this._target.Status));

			this._target.Accept(visitor);
		}

		private void Process(PurchaseOrderApprovedVisitor visitor)
		{
			if (!this.Stage.IsTransitionAllowedTo(PurchaseOrderStatus.Approved))
				throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "approval", this._target.Status));

			this._target.Accept(visitor);
		}

		private void Process(PurchaseOrderCompletedVisitor visitor)
		{
			if (!this.Stage.IsTransitionAllowedTo(PurchaseOrderStatus.Completed))
				throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "completion", this._target.Status));

			this._target.Accept(visitor);
		}

		private void Process(PurchaseOrderCancelledVisitor visitor)
		{
			if (!this.Stage.IsTransitionAllowedTo(PurchaseOrderStatus.Cancelled))
				throw new InvalidOperationException(string.Format(STATE_EXCEPTION_MESSAGE, "cancellation", this._target.Status));

			this._target.Accept(visitor);
		}
	}


	public class StageDefinitionConfigReader
	{
		public static readonly Dictionary<PurchaseOrderStatus, StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>> Values = GetConfig();

		private static Dictionary<PurchaseOrderStatus, StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>> GetConfig()
		{
			var stageDefinitions = new Dictionary<PurchaseOrderStatus, StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>>();

			foreach (var item in StateConfig.Instance.Value.PurchaseOrderConfig)
			{
				stageDefinitions.Add(
					key: item.Key.As<PurchaseOrderStatus>(),
					value: new StageDefinition<PurchaseOrderStatus, PurchaseOrderAggregate>(
						allowedTransitions: item.Value.AllowedTransitions,
						allowedModifications: item.Value.AllowedModifications
					)
				);
			}

			return stageDefinitions;
		}
	}
}
