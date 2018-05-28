using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Service.Dto
{
    public enum InventoryAdjustmentType
    {
        Increase = 1,
        Decrease = 2
    }

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

        public virtual decimal? IncreaseAdjustmentValue { get; set; }

        public virtual decimal? DecreaseAdjustmentValue { get; set; }

        public void Load(Core.Entities.Inventory inventory, Core.Entities.UnitOfMeasure uom)
        {
            this.Id = inventory.Id;
            this.BadStockValue = inventory.Product.ConvertValue(inventory.BadStock, uom);
            this.ReceivedValue = inventory.Product.ConvertValue(inventory.Received, uom);
            this.OnOrderValue = inventory.Product.ConvertValue(inventory.OnOrder, uom);
            this.OnHandValue = inventory.Product.ConvertValue(inventory.OnHand, uom);
            this.AllocatedValue = inventory.Product.ConvertValue(inventory.Allocated, uom);
            this.ShippedValue = inventory.Product.ConvertValue(inventory.Shipped, uom);
            this.BackOrderedValue = inventory.Product.ConvertValue(inventory.BackOrdered, uom);
            this.ReturnedValue = inventory.Product.ConvertValue(inventory.Returned, uom);
            this.AvailableValue = inventory.Product.ConvertValue(inventory.Available, uom);
            this.InitialLevelValue = inventory.Product.ConvertValue(inventory.InitialLevel, uom);
            this.ShrinkageValue = inventory.Product.ConvertValue(inventory.Shrinkage, uom);
            this.CurrentLevelValue = inventory.Product.ConvertValue(inventory.CurrentLevel, uom);
            this.TargetLevelValue = inventory.Product.ConvertValue(inventory.TargetLevel, uom);
            this.BelowTargetLevelValue = inventory.Product.ConvertValue(inventory.BelowTargetLevel, uom);
            this.ReorderLevelValue = inventory.Product.ConvertValue(inventory.ReorderLevel, uom);
            this.ReorderQuantityValue = inventory.Product.ConvertValue(inventory.ReorderQuantity, uom);
            this.MinimumReorderQuantityValue = inventory.Product.ConvertValue(inventory.MinimumReorderQuantity, uom);
            this.IncreaseAdjustmentValue = inventory.Product.ConvertValue(inventory.IncreaseAdjustment, uom);
            this.DecreaseAdjustmentValue = inventory.Product.ConvertValue(inventory.DecreaseAdjustment, uom);
        }

        public void LoadAsDefault(Core.Entities.Inventory inventory)
        {
            var uom = inventory.Product.UnitOfMeasures.Default(x => x.UnitOfMeasure);
            this.Load(inventory, uom);
        }

        public void LoadAsStandard(Core.Entities.Inventory inventory)
        {
            var uom = inventory.Product.UnitOfMeasures.Standard(x => x.UnitOfMeasure);
            this.Load(inventory, uom);
        }
    }

    public class InventoryAdjustment
    {
        public Guid Id { get; set; }

        public Guid InventoryId { get; set; }

        public User AdjustedBy { get; set; }

        public DateTime AdjustedOn { get; set; }

        public InventoryAdjustmentReason Reason { get; set; }

        public string Remarks { get; set; }

        public InventoryAdjustmentType Type { get; set; }

        public Measure Quantity { get; set; }

        public Measure Standard { get; set; }
    }

    public class InventoryAdjustmentPageItem
    {
        public Guid Id { get; set; }

        public string AdjustedBy { get; set; }

        public DateTime AdjustedOn { get; set; }

        public string Reason { get; set; }

        public string Remarks { get; set; }

        public string Type { get; set; }

        public string Quantity { get; set; }
    }

    public class InventoryAdjustmentReason
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public InventoryAdjustmentType Type { get; set; }

    }
}
