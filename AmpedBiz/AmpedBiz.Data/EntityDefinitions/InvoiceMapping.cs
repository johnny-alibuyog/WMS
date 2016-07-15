using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InvoiceMapping : ClassMap<Invoice>
    {
        public InvoiceMapping()
        {
            Id(x => x.Id)
                .GeneratedBy
                .GuidComb();

            References(x => x.Order);

            Map(x => x.DueDate);
            Map(x => x.InvoiceDate);
            Component(x => x.Shipping, MoneyMapping.Map("Shipping_", nameof(Invoice)));
            Component(x => x.SubTotal, MoneyMapping.Map("SubTotal_", nameof(Invoice)));
            Component(x => x.Tax, MoneyMapping.Map("Tax_", nameof(Invoice)));
            Component(x => x.Total, MoneyMapping.Map("Total_", nameof(Invoice)));
        }
    }
}