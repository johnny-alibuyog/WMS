using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Users.Services
{
	public class SetRoleVisitor : IVisitor<User>
    {
        public virtual IEnumerable<Role> Items { get; set; }

        public SetRoleVisitor(IEnumerable<Role> items)
        {
            this.Items = items;
        }

        public void Visit(User target)
        {
            var rolesToAdd = this.Items.Except(target.Roles).ToList();

            var rolesToRemove = target.Roles.Except(this.Items).ToList();

            foreach (var role in rolesToRemove)
            {
                var item = target.Roles.FirstOrDefault(x => x == role);
                target.Roles.Remove(item);
            }

            foreach (var role in rolesToAdd)
            {
                target.Roles.Add(role);
            }
        }
    }
}
