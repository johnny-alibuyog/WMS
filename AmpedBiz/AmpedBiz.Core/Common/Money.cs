using AmpedBiz.Common.Exceptions;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Common
{
    public class Money : ValueObject<Money>
    {
        public virtual decimal Amount { get; set; }

        public virtual Currency Currency { get; set; }

        public Money() : this(0M) { }

        public Money(decimal amount) : this(amount, Currency.PHP) { }

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

            var result = new Money(value1.Amount - value2.Amount, currency);

			if (result.Amount < 0)
				result.Amount = 0;

			return result;
        }

		public static bool operator <(Money value1, Money value2)
		{
			if (value1 == null && value2 == null)
				return true;

			var currency = value1?.Currency ?? value2?.Currency;
			if (value1 == null)
				value1 = new Money(0M, currency);

			if (value2 == null)
				value2 = new Money(0M, currency);

			if (value1.Currency != value2.Currency)
				throw new BusinessException($"You cannot compare money with currency {value1.Currency} to {value2.Currency}!");

			return Math.Round(value1.Amount, 4) < Math.Round(value2.Amount, 4);
		}

		public static bool operator >(Money value1, Money value2)
		{
			if (value1 == null && value2 == null)
				return true;

			var currency = value1?.Currency ?? value2?.Currency;
			if (value1 == null)
				value1 = new Money(0M, currency);

			if (value2 == null)
				value2 = new Money(0M, currency);

			if (value1.Currency != value2.Currency)
				throw new BusinessException($"You cannot compare money with currency {value1.Currency} to {value2.Currency}!");

			return Math.Round(value1.Amount, 4) > Math.Round(value2.Amount);

		}
	}

	public static class MoneyExtention
    {
        public static decimal Amount(this Money money, decimal? defaultValue = 0M)
        {
            if (money == null)
            {
                return defaultValue ?? 0M;
            }

            return money.Amount;
        }
        
        public static Money Sum<T>(this IEnumerable<T> items, Func<T, Money> selector)
        {
            var sum = default(Money);
            foreach (var item in items)
            {
                sum += selector(item);
            }
            return sum;
        }

		public static bool IsNullOrEmpty(this Money money)
		{
			if (money == null)
				return true;

			if (money.Amount == 0)
				return true;

			if (money.Currency == null)
				return true;

			return false;
		}
    }
}
