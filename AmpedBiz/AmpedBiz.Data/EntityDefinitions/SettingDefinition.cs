using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class SettingDefinition
    {
        public class Mapping : ClassMap<Core.Entities.Setting>
        {
            public Mapping()
            {

                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Tenant);

                DiscriminateSubClassesOnColumn("Type");

                ApplyFilter<TenantDefinition.Filter>();
            }
        }

        public class Validation : ValidationDef<Core.Entities.Setting>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Tenant);
            }
        }
    }
}
