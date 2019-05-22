using System;
using System.Data.Entity.Design.PluralizationServices;
using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace AmpedBiz.Data.Conventions
{
    //public class CustomForeignKeyConvention : ForeignKeyConvention
    //{
    //    protected override string GetKeyName(Member property, Type type)
    //    {
    //        // many-to-one
    //        if (property != null)
    //            return property.Name + "Id";

    //        // many-to-many, one-to-many, join
    //        return type.Name + "Id";
    //    }
    //}

    public class CustomForeignKeyConvention2 : IReferenceConvention, IHasManyToManyConvention, IJoinedSubclassConvention, IJoinConvention, ICollectionConvention, IHasManyConvention
    {
        private string GetKeyName(Member property, Type type)
        {
            // many-to-one
            if (property != null)
                return property.Name + "Id";

            // many-to-many, one-to-many, join
            return type.Name + "Id";
        }
        public void Apply(IManyToOneInstance instance)
        {
            var columnName = GetKeyName(instance.Property, instance.Class.GetUnderlyingSystemType());

            instance.Column(columnName);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            var keyColumn = GetKeyName(null, instance.EntityType);
            var childColumn = GetKeyName(null, instance.ChildType);

            instance.Key.Column(keyColumn);
            instance.Relationship.Column(childColumn);
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            var columnName = GetKeyName(null, instance.Type);

            instance.Key.Column(columnName);
        }

        public void Apply(IJoinInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.Column(columnName);
        }

        public void Apply(ICollectionInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.Column(columnName);
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            var columnName = GetKeyName(null, instance.EntityType);

            instance.Key.Column(columnName);
        }
    }
}
