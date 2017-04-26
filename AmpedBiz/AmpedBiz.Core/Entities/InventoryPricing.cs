using System;

namespace AmpedBiz.Core.Entities
{
    // https://docs.oracle.com/cd/E39583_01/fscm92pbr0/eng/fscm/fsit/task_UsingItemQuantityUOM-9f1965.html
    /*
        InventoryUOM[]
	        Pricings[]
		        Pricing
		        PricingItems[]
			        UOM
			        Amount
			        IsStandard
			        StandardConvertion

        InventoryUOMS[]
	        UOM
            IsStandard
	        IsDefault
	        StandardConvertion
	        PricingItems[]
		        Pricing
		        Amount
     */

    public class InventoryPricing : Entity<Guid, InventoryPricing>
    {
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual UnitOfMeasure PackagingUnitOfMeasure { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual decimal PackagingSize { get; set; }

        public InventoryPricing() : base(default(Guid)) { }

        public InventoryPricing(Inventory inventory, Guid id = default(Guid)) : base(id)
        {
            this.Inventory = inventory;
        }
    }
}
