using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class PaymentType : Entity<PaymentType, string>
    {
        public virtual string Name { get; set; }

        public PaymentType() { }

        public PaymentType(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static readonly PaymentType Cash = new PaymentType("CS", "Cash");

        public static readonly PaymentType Check = new PaymentType("CK", "Check");

        public static readonly PaymentType CreditCard = new PaymentType("CC", "Credit Card");

        public static readonly IEnumerable<PaymentType> All = new List<PaymentType>()
        {
            PaymentType.Cash,
            PaymentType.Check,
            PaymentType.CreditCard
        };
    }
}
