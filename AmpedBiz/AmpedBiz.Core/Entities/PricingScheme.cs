using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class PricingScheme : Entity<string, PricingScheme>
    {
        public virtual string Name { get; set; }

        public PricingScheme() : this(default(string))
        {
        }

        public PricingScheme(string id, string name = null) : base(id)
        {
            this.Name = name;
        }

        public static readonly PricingScheme WholeSale = new PricingScheme("WS", "WholeSale");

        public static readonly PricingScheme SRP = new PricingScheme("SRP", "SRP");

        public static readonly PricingScheme CostPrice = new PricingScheme("CP", "CostPrice");

        public static readonly IEnumerable<PricingScheme> All = new List<PricingScheme>() { WholeSale, SRP, CostPrice };
    }
}