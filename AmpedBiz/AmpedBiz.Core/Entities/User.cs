using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class User : Entity<User, string>
    {
        public virtual string Username { get; set; }

        public virtual string Password { get; set; }
    }
}
