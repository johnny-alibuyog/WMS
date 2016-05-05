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

            References(x => x.Product);
            References(x => x.PurchaseOrder);

            Map(x => x.Quantity);
            Map(x => x.Status);
            Map(x => x.DateReceived);

            Component(x => x.ExtendedPrice, MoneyMapping.Map("ExtendedPrice_", nameof(PurchaseOrderDetail)));

            Component(x => x.UnitCost, MoneyMapping.Map("UnitCost_", nameof(PurchaseOrderDetail)));
        }
    }
}