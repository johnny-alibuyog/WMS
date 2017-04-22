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

            Component(x => x.Quantity, 
                MeasureMapping.Map("Quantity_", nameof(PurchaseOrderItem)));

            Component(x => x.UnitCost, 
                MoneyMapping.Map("UnitCost_", nameof(PurchaseOrderItem)));

            Component(x => x.TotalCost, 
                MoneyMapping.Map("TotalCost_", nameof(PurchaseOrderItem)));
        }
    }
}