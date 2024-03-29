﻿using AmpedBiz.Core.Orders;
using AmpedBiz.Data.Definitions.Common;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Orders
{
	public class OrderItemDefinition
	{
		public class Mapping : SubclassMap<OrderItem>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(OrderItem)}_{nameof(OrderItem.Sequence)}");

                References(x => x.Order);

				References(x => x.Product);

				Component(x => x.Quantity,
					MeasureDefinition.Mapping.Map("Quantity_", nameof(OrderItem)));

				Component(x => x.Standard,
					MeasureDefinition.Mapping.Map("Standard_", nameof(OrderItem)));

				Component(x => x.QuantityStandardEquivalent,
					MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(OrderItem)));

				Map(x => x.DiscountRate);

				Component(x => x.Discount,
					MoneyDefinition.Mapping.Map("Discount_", nameof(OrderItem)));

				Component(x => x.UnitPrice,
					MoneyDefinition.Mapping.Map("UnitPrice_", nameof(OrderItem)));

				Component(x => x.ExtendedPrice,
					MoneyDefinition.Mapping.Map("ExtendedPrice_", nameof(OrderItem)));

				Component(x => x.TotalPrice,
					MoneyDefinition.Mapping.Map("TotalPrice_", nameof(OrderItem)));
			}
		}

		public class Validation : ValidationDef<OrderItem>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.Order)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Product)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Quantity)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Standard)
					.NotNullable()
					.And.IsValid();

				Define(x => x.QuantityStandardEquivalent)
					.NotNullable()
					.And.IsValid();

				Define(x => x.DiscountRate);

				Define(x => x.Discount)
					.IsValid();

				Define(x => x.UnitPrice)
					.IsValid();

				Define(x => x.ExtendedPrice)
					.IsValid();

				Define(x => x.TotalPrice)
					.IsValid();
			}
		}
	}
}
