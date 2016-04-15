using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Money : ValueObject<Money>
    {
        public virtual decimal Amount { get; set; }

        public virtual Currency Currency { get; set; }

        public Money() { }

        public Money(decimal amount) :this(amount, Currency.PHP)  { }

        public Money(decimal amount, Currency currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        public override string ToString()
        {
            return this.Amount.ToString("0.00");
        }

        public virtual string ToStringWithSymbol()
        {
            return this.Amount.ToString(this.Currency.Symbol + "0.00");
        }
    }
}
