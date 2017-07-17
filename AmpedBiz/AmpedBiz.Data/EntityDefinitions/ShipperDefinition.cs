using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ShipperDefinition
    {
        public class Mapping : ClassMap<Shipper>
        {
            public Mapping()
            {

                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                Map(x => x.Name);

                //References(x => x.Tenant);

                Component(x => x.Address);

                Component(x => x.Contact);

                HasMany(x => x.Orders)
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .AsBag();
            }
        }

        public class Validation : ValidationDef<Shipper>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(255);

                //Define(x => x.Tenant);
                //    .NotNullable()
                //    .And.IsValid();

                Define(x => x.Address)
                    .IsValid();

                Define(x => x.Contact)
                    .IsValid();

                Define(x => x.Orders);
            }
        }
    }
}
