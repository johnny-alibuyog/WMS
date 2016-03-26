using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace AmpedBiz.Data.Conventions
{
    public class CustomForeignKeyConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            // many-to-one
            if (property != null)
                return property.Name + "Id";

            // many-to-many, one-to-many, join
            return type.Name + "Id";
        }
    }
}
