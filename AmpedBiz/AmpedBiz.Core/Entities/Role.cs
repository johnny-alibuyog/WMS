using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
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

        public static readonly Role Encoder = new Role("E", "Encoder");

        public static readonly Role Manger = new Role("M", "Manager");

        public static readonly Role Sales = new Role("S", "Sales");

        public static readonly Role SuperAdmin = new Role("SA", "Super Admin");

        public static readonly Role Warehouse = new Role("W", "Warehouse");

        public static IEnumerable<Role> All = new [] 
        {
            Role.Admin,
            Role.Encoder,
            Role.Manger,
            Role.Sales,
            Role.SuperAdmin,
            Role.Warehouse
        };
    }
}
