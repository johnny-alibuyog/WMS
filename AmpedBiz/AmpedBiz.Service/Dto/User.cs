using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public class User
    {
        public virtual Guid Id { get; set; }

        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        public virtual Person Person { get; set; }

        public virtual Address Address { get; set; }

        public virtual string BranchId { get; set; }

        public virtual List<Role> Roles { get; set; }
    }

    public class UserPageItem
    {
        public virtual Guid Id { get; set; }

        public virtual string Username { get; set; }

        public virtual string BranchName { get; set; }

        public virtual Person Person { get; set; }

        public virtual Address Address { get; set; }
    }
}
