using System.Collections.Generic;

namespace AmpedBiz.Core.Common
{
    public class PaymentType : Entity<string, PaymentType>
    {
        public virtual string Name { get; set; }

        public PaymentType() : base(default(string)) { }

        public PaymentType(string id, string name = null) : base(id)
        {
            this.Name = name;
        }

        public static readonly PaymentType Cash = new PaymentType("CS", "Cash");

        public static readonly PaymentType Check = new PaymentType("CK", "Check");

        public static readonly PaymentType CreditCard = new PaymentType("CC", "Credit Card");

        public static readonly PaymentType Mixed = new PaymentType("MX", "Mixed");

        public static readonly IEnumerable<PaymentType> All = new[] 
        {
            PaymentType.Cash,
            PaymentType.Check,
            PaymentType.CreditCard,
            PaymentType.Mixed
        };
    }
}
