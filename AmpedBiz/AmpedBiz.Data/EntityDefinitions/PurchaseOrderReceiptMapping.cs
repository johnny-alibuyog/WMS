using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderReceiptMapping : ClassMap<PurchaseOrderReceipt>
    {
        public PurchaseOrderReceiptMapping()
        {
            Id(x => x.Id)
               .GeneratedBy.GuidComb();

            Map(x => x.BatchNumber);

            References(x => x.PurchaseOrder);

            References(x => x.ReceivedBy);

            Map(x => x.ReceivedOn);

            Map(x => x.ExpiresOn);

            References(x => x.Product);

            Component(x => x.Quantity, 
                MeasureMapping.Map("Quantity_", nameof(PurchaseOrderReceipt)));
        }
    }
}
