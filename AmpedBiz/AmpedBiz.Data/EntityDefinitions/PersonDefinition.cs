using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PersonDefinition
    {
        public class Mapping : ComponentMap<Person>
        {
            public Mapping()
            {
                Map(x => x.FirstName)
                    .Index("IDX_FirstName");

                Map(x => x.MiddleName)
                    .Index("IDX_MiddleName");

                Map(x => x.LastName)
                    .Index("IDX_LastName");

                Map(x => x.BirthDate);
            }

            internal static Action<ComponentPart<Person>> Map(string prefix = "")
            {
                return mapping =>
                {
                    mapping.Map(x => x.FirstName, $"{prefix}FirstName")
                        .Index($"IDX_{prefix}FirstName");

                    mapping.Map(x => x.MiddleName, $"{prefix}MiddleName")
                        .Index($"IDX_{prefix}MiddleName");

                    mapping.Map(x => x.LastName, $"{prefix}LastName")
                        .Index($"IDX_{prefix}LastName");

                    mapping.Map(x => x.BirthDate, $"{prefix}BirthDate");
                };
            }
        }

        public class Validation : ValidationDef<Person>
        {
            public Validation()
            {
                Define(x => x.FirstName)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(75);

                Define(x => x.MiddleName)
                    .MaxLength(75);

                Define(x => x.LastName)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(75);

                Define(x => x.BirthDate);
            }
        }
    }
}
