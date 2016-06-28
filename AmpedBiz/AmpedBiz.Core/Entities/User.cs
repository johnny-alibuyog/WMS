using AmpedBiz.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.Entities
{
    public class User : Entity<Guid, User>
    {
        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        public virtual Person Person { get; set; }

        public virtual Address Address { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual IEnumerable<UserRole> UserRoles { get; set; }

        public User() : this(default(Guid)) { }

        public User(Guid id) : base(id)
        {
            this.UserRoles = new Collection<UserRole>();
        }

        public virtual void SetRoles(IEnumerable<Role> items)
        {
            var existingRoles = this.UserRoles
                .Select(x => x.Role)
                .ToList();

            var rolesToAdd = items.Except(existingRoles).ToList();

            var rolesToRemove = existingRoles.Except(items).ToList();

            foreach (var role in rolesToRemove)
            {
                var item = this.UserRoles.FirstOrDefault(x => x.Role == role);
                item.User = null;
                item.Role = null;
                this.UserRoles.Remove(item);
            }

            foreach (var role in rolesToAdd)
            {
                var item = new UserRole();
                item.User = this;
                item.Role = role;
                this.UserRoles.Add(item);
            }
        }

        public virtual string FullName()
        {
            return Regex.Replace(string.Format("{0} {1} {2}",
                this.Person.FirstName,
                this.Person.MiddleName,
                this.Person.LastName),
                @"\s+", " ");
        }
    }
}
