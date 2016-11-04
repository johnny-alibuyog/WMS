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

        public virtual IEnumerable<Role> Roles { get; set; } = new Collection<Role>();
        
        public virtual string Name
        {
            get { return Regex.Replace($"{this.Person.FirstName} {this.Person.LastName}", @"\s+", " "); } // this will be used by ExpressMapper
        }

        public User() : base(default(Guid)) { }

        public User(Guid id) : base(id) { }

        public virtual void SetRoles(IEnumerable<Role> items)
        {

            var rolesToAdd = items.Except(this.Roles).ToList();

            var rolesToRemove = this.Roles.Except(items).ToList();

            foreach (var role in rolesToRemove)
            {
                var item = this.Roles.FirstOrDefault(x => x == role);
                this.Roles.Remove(item);
            }

            foreach (var role in rolesToAdd)
            {
                this.Roles.Add(role);
            }
        }

        public static User SupperUser = new User(new Guid("{CA42947A-0BA3-4FC8-86E0-A635014B6B11}"))
        {
            Username = "supper_user",
            Password = "123!@#qweASD",
            Person = new Person()
            {
                FirstName = "Supper",
                MiddleName = "Power",
                LastName = "User",
                BirthDate = new DateTime(1999, 1, 1)
            },
            Address = new Address()
            {
                Street = "Ocean Street",
                Barangay = "Virginia Summer Ville, Mayamot",
                City = "Antipolo City",
                Province = "Rizal",
                Region = "NCR",
                Country = "Philippines",
                ZipCode = "1870"
            },
        };
    }
}
