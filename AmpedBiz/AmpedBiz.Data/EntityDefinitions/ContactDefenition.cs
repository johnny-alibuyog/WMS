using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ContactDefenition
    {
        public class Mapping : ComponentMap<Contact>
        {
            public Mapping()
            {
                Map(x => x.Email);

                Map(x => x.Landline);

                Map(x => x.Fax);

                Map(x => x.Mobile);

                Map(x => x.Web);
            }

            internal static Action<ComponentPart<Contact>> Map(string prefix = "")
            {
                return mapping =>
                {
                    mapping.Map(x => x.Email, $"{prefix}Email");

                    mapping.Map(x => x.Landline, $"{prefix}Landline");

                    mapping.Map(x => x.Fax, $"{prefix}Fax");

                    mapping.Map(x => x.Mobile, $"{prefix}Mobile");

                    mapping.Map(x => x.Web, $"{prefix}Web");
                };
            }
        }

        public class Validation : ValidationDef<Contact>
        {
            public Validation()
            {
                Define(x => x.Email)
                    .MaxLength(50);
                //.And.IsEmail(); TODO: implement soon if ui validation is already setup :)

                Define(x => x.Landline)
                    .MaxLength(50);

                Define(x => x.Fax)
                    .MaxLength(50);

                Define(x => x.Mobile)
                    .MaxLength(50);

                Define(x => x.Web)
                    .MaxLength(100);
            }
        }
    }
}
