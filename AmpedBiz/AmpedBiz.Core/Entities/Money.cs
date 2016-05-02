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
            return this.Amount.ToString(this.Currency.Symbol + " #,##0.00");
        }

        public static Money operator +(Money value1, Money value2)
        {
            if (value1 == null && value2 == null)
                return null;

            var currency = value1?.Currency ?? value2?.Currency;
            if (value1 == null)
                value1 = new Money(0M, currency);

            if (value2 == null)
                value2 = new Money(0M, currency);

            return new Money(value1.Amount + value2.Amount, currency);
        }

        public static Money operator -(Money value1, Money value2)
        {
            if (value1 == null && value2 == null)
                return null;

            var currency = value1?.Currency ?? value2?.Currency;
            if (value1 == null)
                value1 = new Money(0M, currency);

            if (value2 == null)
                value2 = new Money(0M, currency);

            return new Money(value1.Amount - value2.Amount, currency);
        }
    }
}
