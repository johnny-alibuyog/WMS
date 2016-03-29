using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PersonMapping : ComponentMap<Person>
    {
        public PersonMapping()
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
}
