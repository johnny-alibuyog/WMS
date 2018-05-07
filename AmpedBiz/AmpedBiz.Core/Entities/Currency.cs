using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Currency : Entity<string, Currency>
    {
        public virtual string Symbol { get; protected internal set; }

        public virtual string Name { get; protected internal set; }

        public Currency() : base(default(string)) { }

        public Currency(string id, string symbol = null, string name = null) : base(id)
        {
            this.Symbol = symbol;
            this.Name = name;
        }

        public static readonly Currency PHP = new Currency("PHP", "₱", "Philipine Peso");

        public static readonly IEnumerable<Currency> All = new Currency[] { PHP };
    }
}
