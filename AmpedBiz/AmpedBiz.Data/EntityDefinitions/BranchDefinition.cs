using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class BranchDefinition
    {
        public class Mapping : ClassMap<Branch>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                Map(x => x.Name);

                Map(x => x.Description);

                Map(x => x.TaxpayerIdentificationNumber);

                Component(x => x.Contact);

                Component(x => x.Address);
            }
        }

        public class Validation : ValidationDef<Branch>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);

                Define(x => x.Description)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);

                Define(x => x.Address)
                    .IsValid();
            }
        }
    }
}
