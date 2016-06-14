using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderDetailMapping : ClassMap<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.PurchaseOrder);

            References(x => x.Product);

            Component(x => x.Quantity, MeasureMapping.Map("Quantity_", nameof(GoodStockInventory)));

            Component(x => x.UnitPrice, MoneyMapping.Map("UnitCost_", nameof(PurchaseOrderDetail)));

            Component(x => x.Total, MoneyMapping.Map("ExtendedPrice_", nameof(PurchaseOrderDetail)));

            Map(x => x.DateReceived);

            Map(x => x.Status);
        }
    }
}