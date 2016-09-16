﻿using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderReturnMapping : ClassMap<OrderReturn>
    {
        public OrderReturnMapping()
        {
            Id(x => x.Id)
         .GeneratedBy.GuidComb();

            References(x => x.Product);

            References(x => x.Order);

            Component(x => x.Quantity,
                MeasureMapping.Map("Quantity_", nameof(OrderItem)));

            Component(x => x.UnitPrice,
                MoneyMapping.Map("UnitPrice_", nameof(OrderItem)));

            Component(x => x.TotalPrice,
                MoneyMapping.Map("TotalPrice_", nameof(OrderItem)));
        }
    }
}
