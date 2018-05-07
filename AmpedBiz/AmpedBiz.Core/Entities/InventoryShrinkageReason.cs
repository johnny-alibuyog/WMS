using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class InventoryShrinkageReason : Entity<string, InventoryShrinkageReason>, IHasTenant
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; internal protected set; }

        public InventoryShrinkageReason() : base(default(string)) { }

        public InventoryShrinkageReason(string id, string name = null) : base(id)
        {
            this.Name = name;
        }

        public static InventoryShrinkageReason Damaged = new InventoryShrinkageReason("D", "Damaged");

        public static InventoryShrinkageReason Expired = new InventoryShrinkageReason("E", "Expired");

        public static InventoryShrinkageReason Shoplifted = new InventoryShrinkageReason("SL", "Shoplifted");

        public static InventoryShrinkageReason VendorFraud = new InventoryShrinkageReason("VF", "Vendor Fraud");

        public static InventoryShrinkageReason EmployeeTheft = new InventoryShrinkageReason("ET", "Employee Theft");

        public static InventoryShrinkageReason UnknownReason = new InventoryShrinkageReason("UR", "Unknown Reason");

        public static InventoryShrinkageReason ClericalError = new InventoryShrinkageReason("CE", "Clerical Error");

        public static IEnumerable<InventoryShrinkageReason> All = new []
        {
            InventoryShrinkageReason.Damaged,
            InventoryShrinkageReason.Expired,
            InventoryShrinkageReason.Shoplifted,
            InventoryShrinkageReason.VendorFraud,
            InventoryShrinkageReason.EmployeeTheft,
            InventoryShrinkageReason.UnknownReason,
            InventoryShrinkageReason.ClericalError
        };
    }
}
