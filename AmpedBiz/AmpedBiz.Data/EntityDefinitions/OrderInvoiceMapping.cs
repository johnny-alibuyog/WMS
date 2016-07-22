using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderInvoiceMapping : ClassMap<OrderInvoice>
    {
        public OrderInvoiceMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Order);

            Map(x => x.DueOn);

            Map(x => x.InvoicedOn);

            Component(x => x.Tax, 
                MoneyMapping.Map("Tax_", nameof(OrderInvoice)));

            Component(x => x.Shipping, 
                MoneyMapping.Map("Shipping_", nameof(OrderInvoice)));

            Component(x => x.Discount, 
                MoneyMapping.Map("Discount_", nameof(OrderInvoice)));

            Component(x => x.SubTotal, 
                MoneyMapping.Map("SubTotal_", nameof(OrderInvoice)));

            Component(x => x.Total, 
                MoneyMapping.Map("Total_", nameof(OrderInvoice)));
        }
    }
}