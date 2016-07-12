using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderItemMapping : ClassMap<PurchaseOrderItem>
    {
        public PurchaseOrderItemMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.PurchaseOrder);

            References(x => x.Product);

            Component(x => x.Quantity, MeasureMapping.Map("Quantity_", nameof(GoodStockInventory)));

            Component(x => x.UnitPrice, MoneyMapping.Map("UnitPrice_", nameof(PurchaseOrderItem)));

            Component(x => x.Total, MoneyMapping.Map("Total_", nameof(PurchaseOrderItem)));

            Map(x => x.DateReceived);

            Map(x => x.Status);
        }
    }
}