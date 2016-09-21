using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class ShrinkageCause : Entity<string, ShrinkageCause>
    {
        public virtual string Name { get; set; }

        public ShrinkageCause() : base(default(string)) { }

        public ShrinkageCause(string id, string name = null) : base(id)
        {
            this.Name = name;
        }

        public static ShrinkageCause Damaged = new ShrinkageCause("D", "Damaged");

        public static ShrinkageCause Expired = new ShrinkageCause("E", "Expired");

        public static ShrinkageCause Shoplifting = new ShrinkageCause("SL", "Shoplifting");

        public static ShrinkageCause VendorFraud = new ShrinkageCause("VF", "Vendor Fraud");

        public static ShrinkageCause EmployeeTheft = new ShrinkageCause("ET", "Employee Theft");

        public static ShrinkageCause UnknownReason = new ShrinkageCause("UR", "Unknown Reason");

        public static ShrinkageCause AdministrativeError = new ShrinkageCause("AE", "Administrative Error");

        public static IEnumerable<ShrinkageCause> All = new []
        {
            ShrinkageCause.Damaged,
            ShrinkageCause.Expired,
            ShrinkageCause.Shoplifting,
            ShrinkageCause.VendorFraud,
            ShrinkageCause.EmployeeTheft,
            ShrinkageCause.UnknownReason,
            ShrinkageCause.AdministrativeError
        };
    }
}
