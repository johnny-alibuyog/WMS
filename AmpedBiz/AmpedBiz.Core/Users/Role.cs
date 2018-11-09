using System.Collections.Generic;

namespace AmpedBiz.Core.Users
{
    public class Role : Entity<string, Role>
    {
        public virtual string Name { get; set; }

        public Role() : base(default(string)) { }

        public Role(string id, string name = null) : base(id)
        {
            this.Name = name;
        }

        public static readonly Role Admin = new Role("A", "Admin");

        public static readonly Role Manger = new Role("M", "Manager");

        public static readonly Role Salesclerk = new Role("S", "Salesclerk");

        public static readonly Role Warehouseman = new Role("W", "Warehouseman");

        public static IEnumerable<Role> All = new [] 
        {
            Role.Admin,
            Role.Manger,
            Role.Salesclerk,
            Role.Warehouseman
        };
    }
}
