using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class PricingScheme : Entity<string, PricingScheme>
    {
        public virtual string Name { get; set; }

        public PricingScheme() : base(default(string)) { }

        public PricingScheme(string id, string name) : base(id) 
        {
            this.Name = name;
        }

        public static readonly PricingScheme BasePrice = new PricingScheme("BP", "Base Price");

        public static readonly PricingScheme RetailPrice = new PricingScheme("RP", "Retail Price");

        public static readonly PricingScheme WholesalePrice = new PricingScheme("WP", "Wholesale Price");

        public static readonly IEnumerable<PricingScheme> All = new List<PricingScheme>() { BasePrice, WholesalePrice, RetailPrice };
    }
}