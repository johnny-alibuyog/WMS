using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderItemDefinition
    {
        public class Mapping : ClassMap<PurchaseOrderItem>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.PurchaseOrder);

                References(x => x.Product);

                Component(x => x.Quantity,
                    MeasureDefinition.Mapping.Map("Quantity_", nameof(PurchaseOrderItem)));

                Component(x => x.UnitCost,
                    MoneyDefinition.Mapping.Map("UnitCost_", nameof(PurchaseOrderItem)));

                Component(x => x.TotalCost,
                    MoneyDefinition.Mapping.Map("TotalCost_", nameof(PurchaseOrderItem)));
            }
        }

        public class Validation : ValidationDef<PurchaseOrderItem>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.PurchaseOrder)
                    .NotNullable();

                Define(x => x.Product)
                    .NotNullable();

                Define(x => x.Quantity);

                Define(x => x.UnitCost)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.TotalCost)
                    .NotNullable()
                    .And.IsValid();
            }
        }
    }
}