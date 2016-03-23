using System;

namespace AmpedBiz.Domain.Entities
{
    public class Currency : Entity<Currency, string>
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public Currency() { }

        public Currency(string id, string symbol, string name)
        {
            this.Id = id;
            this.Symbol = symbol;
            this.Name = name;
        }

        public static readonly Currency PHP = new Currency("PHP", "₱", "Philipine Peso");
    }
}
