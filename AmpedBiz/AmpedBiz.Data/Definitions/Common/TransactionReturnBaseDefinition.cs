using AmpedBiz.Core.Common;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
    public class TransactionReturnBaseDefinition
    {
        public class Mapping : ClassMap<TransactionReturnBase>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Product);

                References(x => x.Reason);

                Component(x => x.Quantity,
                    MeasureDefinition.Mapping.Map("Quantity_", nameof(TransactionReturnBase)));

                Component(x => x.Standard,
                    MeasureDefinition.Mapping.Map("Standard_", nameof(TransactionReturnBase)));

                Component(x => x.QuantityStandardEquivalent,
                    MeasureDefinition.Mapping.Map("QuantityStandardEql_", nameof(TransactionReturnBase)));

                Component(x => x.Returned,
                    MoneyDefinition.Mapping.Map("Returned_", nameof(TransactionReturnBase)));
            }
        }

        public class Validation : ValidationDef<TransactionReturnBase>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Product)
                    .NotNullable();

                Define(x => x.Reason)
                    .NotNullable();

                Define(x => x.Quantity)
                    .IsValid();

                Define(x => x.Standard)
                    .IsValid();

                Define(x => x.QuantityStandardEquivalent)
                    .IsValid();

                Define(x => x.Returned)
                    .IsValid();
            }
        }
    }
}
