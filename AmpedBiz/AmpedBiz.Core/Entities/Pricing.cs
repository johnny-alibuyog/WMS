using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Pricing : Entity<string, Pricing>
    {
        public virtual string Name { get; set; }

        public Pricing() : base(default(string)) { }

        public Pricing(string id, string name) : base(id) 
        {
            this.Name = name;
        }

        public static readonly Pricing BasePrice = new Pricing("BP", "Base Price");

        public static readonly Pricing RetailPrice = new Pricing("RP", "Retail Price");

        public static readonly Pricing WholesalePrice = new Pricing("WP", "Wholesale Price");

        public static readonly Pricing BadStockPrice = new Pricing("BSP", "Bad Stock Price");

        public static readonly IEnumerable<Pricing> All = new [] 
        {
            Pricing.BasePrice,
            Pricing.WholesalePrice,
            Pricing.RetailPrice,
            Pricing.BadStockPrice
        };
    }
}