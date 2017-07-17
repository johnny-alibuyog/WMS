using System;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class AddressDefenition
    {
        public class Mapping : ComponentMap<Address>
        {
            public Mapping()
            {
                Map(x => x.Street);
                //.Index("IDX_Street");

                Map(x => x.Barangay);
                //.Index("IDX_Barangay");

                Map(x => x.City);
                //.Index("IDX_City");

                Map(x => x.Province);
                //.Index("IDX_Province");

                Map(x => x.Region);
                //.Index("IDX_Region");

                Map(x => x.Country);
                //.Index("IDX_Country");

                Map(x => x.ZipCode);
                //.Index("IDX_ZipCode");
            }

            internal static Action<ComponentPart<Address>> Map(string prefix = "")
            {
                return mapping =>
                {
                    mapping.Map(x => x.Street, $"{prefix}Street");
                    //.Index($"IDX_{prefix}Street");

                    mapping.Map(x => x.Barangay, $"{prefix}Barangay");
                    //.Index($"IDX_{prefix}Barangay");

                    mapping.Map(x => x.City, $"{prefix}City");
                    //.Index($"IDX_{prefix}City");

                    mapping.Map(x => x.Province, $"{prefix}Province");
                    //.Index($"IDX_{prefix}Province");

                    mapping.Map(x => x.Region, $"{prefix}Region");
                    //.Index($"IDX_{prefix}Region");

                    mapping.Map(x => x.Country, $"{prefix}Country");
                    //.Index($"IDX_{prefix}Country");

                    mapping.Map(x => x.ZipCode, $"{prefix}ZipCode");
                    //.Index($"IDX_{prefix}ZipCode");
                };
            }
        }

        public class Validation : ValidationDef<Address>
        {
            public Validation()
            {
                Define(x => x.Street)
                    .MaxLength(150);

                Define(x => x.Barangay)
                    .MaxLength(150);

                Define(x => x.City)
                    .MaxLength(150);

                Define(x => x.Province)
                    .MaxLength(150);

                Define(x => x.Region)
                    .MaxLength(150);

                Define(x => x.Country)
                    .MaxLength(150);

                Define(x => x.ZipCode)
                    .MaxLength(150);
            }
        }
    }

}
