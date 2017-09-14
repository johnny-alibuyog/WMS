using System;
using AmpedBiz.Common.CustomTypes;

namespace AmpedBiz.Service.Dto
{
    public class Inventory
    {
        public virtual Guid Id { get; set; }

        public virtual decimal? BadStockValue { get; set; }

        public virtual decimal? ReceivedValue { get; set; }

        public virtual decimal? OnOrderValue { get; set; }

        public virtual decimal? OnHandValue { get; set; }

        public virtual decimal? AllocatedValue { get; set; }

        public virtual decimal? ShippedValue { get; set; }

        public virtual decimal? BackOrderedValue { get; set; }

        public virtual decimal? ReturnedValue { get; set; }

        public virtual decimal? AvailableValue { get; set; }

        public virtual decimal? InitialLevelValue { get; set; }

        public virtual decimal? ShrinkageValue { get; set; }

        public virtual decimal? CurrentLevelValue { get; set; }

        public virtual decimal? TargetLevelValue { get; set; }

        public virtual decimal? BelowTargetLevelValue { get; set; }

        public virtual decimal? ReorderLevelValue { get; set; }

        public virtual decimal? ReorderQuantityValue { get; set; }

        public virtual decimal? MinimumReorderQuantityValue { get; set; }
    }
}
