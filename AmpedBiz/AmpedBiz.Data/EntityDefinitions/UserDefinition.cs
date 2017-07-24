using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserDefinition
    {
        public class Mapping : ClassMap<User>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                Map(x => x.Username)
                    .Unique();

                Map(x => x.Password);

                Component(x => x.Person);

                Component(x => x.Address);

                References(x => x.Branch);

                HasManyToMany(x => x.Roles)
                   .Table("UsersRoles")
                   .ForeignKeyConstraintNames(
                        parentForeignKeyName: "FK_Users_UsersRoles",
                        childForeignKeyName: "FK_Roles_UsersRoles"
                    )
                   .Cascade.All()
                   .AsSet();

                //UseUnionSubclassForInheritanceMapping();
            }
        }

        public class Validation : ValidationDef<User>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Username)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(50);

                Define(x => x.Password)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(50);

                Define(x => x.Person)
                    .IsValid();

                Define(x => x.Address)
                    .IsValid();

                Define(x => x.Branch)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Roles)
                    .NotNullableAndNotEmpty()
                    .And.HasValidElements();
            }
        }
    }
}
