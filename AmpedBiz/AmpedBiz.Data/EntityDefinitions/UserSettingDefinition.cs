using AmpedBiz.Core.Entities;
using AmpedBiz.Data.CustomTypes;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserSettingDefinition
    {
        public class Mapping : SubclassMap<Core.Entities.Setting<UserSetting>>
        {
            public Mapping()
            {
                Map(x => x.Value)
                    .CustomType<JsonType<UserSetting>>();

                DiscriminatorValue(nameof(Core.Entities.UserSetting));
            }
        }

        public class Validation : ValidationDef<Core.Entities.Setting<UserSetting>>
        {
            public Validation()
            {
                Define(x => x.Value);
            }
        }
    }
}
