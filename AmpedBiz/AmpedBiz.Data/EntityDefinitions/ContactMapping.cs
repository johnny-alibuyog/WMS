using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ContactMapping : ComponentMap<Contact>
    {
        public ContactMapping()
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
}
