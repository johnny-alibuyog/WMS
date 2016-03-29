using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Role : Entity<Role, string>
    {
        public virtual string Name { get; set; }

        public Role() { }

        public Role(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static Role Admin = new Role("A", "Admin");

        public static Role Encoder = new Role("E", "Encoder");

        public static IEnumerable<Role> All = new Role[] { Role.Admin, Role.Encoder };
    }
}
