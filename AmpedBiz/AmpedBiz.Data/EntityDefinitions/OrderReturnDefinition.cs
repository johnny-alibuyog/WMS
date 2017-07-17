using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderReturnDefinition
    {
        public class Mapping : ClassMap<OrderReturn>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Product);

                References(x => x.Order);

                References(x => x.Reason);

                References(x => x.ReturnedBy);

                Map(x => x.ReturnedOn);

                Component(x => x.Quantity,
                    MeasureDefinition.Mapping.Map("Quantity_", nameof(OrderReturn)));

                Component(x => x.Returned,
                    MoneyDefinition.Mapping.Map("Returned_", nameof(OrderReturn)));
            }
        }

        public class Validation : ValidationDef<OrderReturn>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Product)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Order)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Reason)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ReturnedBy)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ReturnedOn);

                Define(x => x.Quantity);

                Define(x => x.Returned)
                    .IsValid();
            }
        }
    }
}
