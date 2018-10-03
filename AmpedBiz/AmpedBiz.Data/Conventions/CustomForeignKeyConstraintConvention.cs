using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace AmpedBiz.Data.Conventions
{
	public class CustomForeignKeyConstraintConvention : IHasManyConvention, IReferenceConvention, IHasOneConvention, IJoinedSubclassConvention
	{
        private readonly PluralizationService _pluralizationService;

        public CustomForeignKeyConstraintConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey(string.Format("FK_{0}_{1}", instance.Member.Name, instance.EntityType.Name));
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.ForeignKey(string.Format("FK_{0}_{1}", _pluralizationService.Pluralize(instance.EntityType.Name), instance.Name));
        }

        public void Apply(IOneToOneInstance instance)
        {
            instance.ForeignKey(string.Format("FK_{0}_{1}", _pluralizationService.Pluralize(instance.EntityType.Name), instance.Name));
        }

		public void Apply(IJoinedSubclassInstance instance)
		{
			instance.Key.ForeignKey(string.Format("FK_{0}_{1}", _pluralizationService.Pluralize(instance.EntityType.Name), instance.EntityType.BaseType.Name));
		}
	}

	//public class ForeignKeyConstraintNameConvention : IClassConvention, ISubclassConvention, IJoinedSubclassConvention
	//{
	//	private static readonly FieldInfo classField;
	//	private static readonly FieldInfo subclassField;
	//	private static readonly FieldInfo joinedSubclassField;

	//	static ForeignKeyConstraintNameConvention()
	//	{
	//		classField = typeof(ClassInstance).GetField("mapping", (BindingFlags.Instance | BindingFlags.NonPublic));
	//		subclassField = typeof(SubclassInstance).GetField("mapping", (BindingFlags.Instance | BindingFlags.NonPublic));
	//		joinedSubclassField = typeof(JoinedSubclassInstance).GetField("mapping", (BindingFlags.Instance | BindingFlags.NonPublic));

	//		if (classField == null || classField.FieldType != typeof(ClassMapping) ||
	//			subclassField == null || subclassField.FieldType != typeof(SubclassMapping) ||
	//			joinedSubclassField == null || joinedSubclassField.FieldType != typeof(SubclassMapping))
	//			throw new Exception("The internals of Fluent NHibernate have changed");

	//	}

	//	private static void MapClass(IHasMappedMembers instance, string tableName)
	//	{
	//		foreach (var manyToOne in instance.References.OfType<ManyToOneMapping>())
	//		{
	//			var fk = string.Format("FK_{0}_{1}", tableName, manyToOne.Member.Name).Replace("`", "");
	//			manyToOne.ForeignKey = fk;
	//		}

	//		foreach (var oneToMany in instance.Collections)
	//		{
	//			var fk = string.Format("FK_{0}_{1}", tableName, oneToMany.Member.Name).Replace("`", "");
	//			oneToMany.Key.ForeignKey = fk;
	//		}
	//	}

	//	public void Apply(IClassInstance instance)
	//	{
	//		if (instance is ClassInstance)
	//		{
	//			var mapping = classField.GetValue(instance) as ClassMapping;
	//			if (mapping != null)
	//				MapClass(mapping, mapping.TableName);
	//		}
	//	}

	//	public void Apply(ISubclassInstance instance)
	//	{
	//		if (instance is SubclassInstance)
	//		{
	//			var mapping = subclassField.GetValue(instance) as SubclassMapping;
	//			if (mapping != null)
	//				MapClass(mapping, mapping.TableName);
	//		}
	//	}

	//	public void Apply(IJoinedSubclassInstance instance)
	//	{
	//		if (instance is JoinedSubclassInstance)
	//		{
	//			var mapping = joinedSubclassField.GetValue(instance) as SubclassMapping;
	//			if (mapping != null)
	//				MapClass(mapping, mapping.TableName);
	//		}
	//	}
	//}
}
