using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Services;
using AmpedBiz.Core.Services.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmpedBiz.Core.Entities
{
    public class User : Entity<Guid, User>, IAccept<IVisitor<User>>
    {
        public virtual string Username { get; set; }

        public virtual string PasswordSalt { get; protected internal set; }

        public virtual string PasswordHash { get; protected internal set; }

        public virtual Person Person { get; set; }

        public virtual Address Address { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual IEnumerable<Role> Roles { get; protected set; } = new Collection<Role>();

        public virtual string Name => Regex.Replace($"{this.Person.FirstName} {this.Person.LastName}", @"\s+", " "); // this will be used by ExpressMapper

        public User() : base(default(Guid)) { }

        public User(Guid id) : base(id) { }

        public virtual bool IsManager()
        {
            if (this.Roles.Contains(Role.Admin))
                return true;

            if (this.Roles.Contains(Role.Manger))
                return true;

            return false; ;
        }

        public virtual bool HasPassword()
        {
            return !string.IsNullOrEmpty(this.PasswordHash) && !string.IsNullOrEmpty(this.PasswordSalt);
        }

        public virtual void Accept(IVisitor<User> visitor)
        {
            visitor.Visit(this);
        }

        static User()
        {
            User.Admin.Accept(new SetPasswordVisitor()
            {
                NewPassword = $"{User.Admin.Username}123",
                ConfirmPassword = $"{User.Admin.Username}123"
            });

            User.Manger.Accept(new SetPasswordVisitor()
            {
                NewPassword = $"{User.Manger.Username}123",
                ConfirmPassword = $"{User.Manger.Username}123"
            });

            User.Salesclerk.Accept(new SetPasswordVisitor()
            {
                NewPassword = $"{User.Salesclerk.Username}123",
                ConfirmPassword = $"{User.Salesclerk.Username}123"
            });

            User.Warehouseman.Accept(new SetPasswordVisitor()
            {
                NewPassword = $"{User.Warehouseman.Username}123",
                ConfirmPassword = $"{User.Warehouseman.Username}123"
            });
        }

        public static User Admin = new User(new Guid("{CA42947A-0BA3-4FC8-86E0-A635014B6B11}"))
        {
            Username = "admin",
            Person = new Person()
            {
                FirstName = "User",
                LastName = "Admin",
                BirthDate = new DateTime(1999, 1, 1)
            },
            Address = new Address()
            {
                Street = "Admin Street",
                Barangay = "Admin Ville",
                City = "Admin City",
                Province = "Admin Province",
                Region = "Admin Region",
                Country = "Admin Country",
                ZipCode = "1870"
            },
            Branch = Branch.Default,
            Roles = new Collection<Role>()
            {
                Role.Admin
            },
        };

        public static User Manger = new User(new Guid("{43AFE0B7-F313-457A-8682-B5B76A706DE0}"))
        {
            Username = "manager",
            Person = new Person()
            {
                FirstName = "User",
                LastName = "Manager",
                BirthDate = new DateTime(1999, 1, 1)
            },
            Address = new Address()
            {
                Street = "Manager Street",
                Barangay = "Manager Ville",
                City = "Manager City",
                Province = "Manager Province",
                Region = "Manager Region",
                Country = "Manager Country",
                ZipCode = "1870"
            },
            Branch = Branch.Default,
            Roles = new Collection<Role>()
            {
                Role.Manger
            },
        };

        public static User Salesclerk = new User(new Guid("{C9700CC9-8A0A-48B4-984F-D6987A17DB07}"))
        {
            Username = "salesclerk",
            Person = new Person()
            {
                FirstName = "User",
                LastName = "Salesclerk",
                BirthDate = new DateTime(1999, 1, 1)
            },
            Address = new Address()
            {
                Street = "Salesclerk Street",
                Barangay = "Salesclerk Ville",
                City = "Amin City",
                Province = "Salesclerk Province",
                Region = "Salesclerk Region",
                Country = "Salesclerk Country",
                ZipCode = "1870"
            },
            Branch = Branch.Default,
            Roles = new Collection<Role>()
            {
                Role.Salesclerk
            },
        };

        public static User Warehouseman = new User(new Guid("{32E805A4-9F57-435F-A669-6B016F08F7A0}"))
        {
            Username = "warehouseman",
            Person = new Person()
            {
                FirstName = "User",
                LastName = "Warehouseman",
                BirthDate = new DateTime(1999, 1, 1)
            },
            Address = new Address()
            {
                Street = "Warehouseman Street",
                Barangay = "Warehouseman Ville",
                City = "Warehouseman City",
                Province = "Warehouseman Province",
                Region = "Warehouseman Region",
                Country = "Warehouseman Country",
                ZipCode = "1870"
            },
            Branch = Branch.Default,
            Roles = new Collection<Role>()
            {
                Role.Warehouseman
            },
        };

        public static IEnumerable<User> All = new[]
        {
            User.Admin,
            User.Manger,
            User.Salesclerk,
            User.Warehouseman,
        };
    }
}
