using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderMapping : ClassMap<Order>
    {
        public OrderMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Branch);

            Map(x => x.OrderDate);

            Map(x => x.ShippedDate);

            Map(x => x.PaymentDate);

            Map(x => x.CompletedDate);

            Map(x => x.CancelDate);

            Map(x => x.CancelReason);

            References(x => x.PaymentType);

            //References(x => x.Shipper);

            Map(x => x.TaxRate);

            Component(x => x.Tax, MoneyMapping.Map("Tax_", nameof(Order)));

            Component(x => x.ShippingFee, MoneyMapping.Map("ShippingFee_", nameof(Order)));

            Component(x => x.SubTotal, MoneyMapping.Map("SubTotal_", nameof(Order)));

            Component(x => x.Total, MoneyMapping.Map("Total_", nameof(Order)));

            Map(x => x.Status);

            Map(x => x.IsActive);

            //References(x => x.Employee);

            References(x => x.Customer);

            //HasMany(x => x.Invoices)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();

            //HasMany(x => x.OrderDetails)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();
        }
    }
}
