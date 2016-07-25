using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderInvoiceMapping : ClassMap<OrderPayment>
    {
        public OrderInvoiceMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Order);

            Map(x => x.PaidOn);

            Component(x => x.Tax, 
                MoneyMapping.Map("Tax_", nameof(OrderPayment)));

            Component(x => x.ShippingFee, 
                MoneyMapping.Map("ShippingFee_", nameof(OrderPayment)));

            Component(x => x.Discount, 
                MoneyMapping.Map("Discount_", nameof(OrderPayment)));

            Component(x => x.SubTotal, 
                MoneyMapping.Map("SubTotal_", nameof(OrderPayment)));

            Component(x => x.Total, 
                MoneyMapping.Map("Total_", nameof(OrderPayment)));
        }
    }
}