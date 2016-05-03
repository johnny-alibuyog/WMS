using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Role : Entity<string, Role>
    {
        public virtual string Name { get; set; }

        public Role() : this(default(string)) { }

        public Role(string id, string name = null) : base(id)
        {
            this.Name = name;
        }

        public static Role Admin = new Role("A", "Admin");

        public static Role Encoder = new Role("E", "Encoder");

        public static IEnumerable<Role> All = new Role[] { Admin, Encoder };
    }
}
