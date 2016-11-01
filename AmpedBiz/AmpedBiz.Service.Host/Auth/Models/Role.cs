using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace AmpedBiz.Service.Host.Auth.Models
{
    public class Role : IRole<string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Role(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static readonly Role Admin = new Role("A", "Admin");

        public static readonly Role Encoder = new Role("E", "Encoder");

        public static readonly Role Sales = new Role("S", "Sales");

        public static readonly Role Warehouse = new Role("W", "Warehouse");

        public static IEnumerable<Role> All = new[]
        {
            Role.Admin,
            Role.Encoder,
            Role.Sales,
            Role.Warehouse
        };

        #region Equality Comparer

        private int? _oldHashCode;

        private bool IsTransient
        {
            get { return string.IsNullOrWhiteSpace(this.Id); }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Role;
            if (other == null)
                return false;

            //to handle the case of comparing two new objects
            if (this.IsTransient && other.IsTransient)
                return ReferenceEquals(other, this);

            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            //This is done se we won't change the hash code
            if (_oldHashCode.HasValue)
            {
                return _oldHashCode.Value;
            }

            //When we are transient, we use the base GetHashCode() and remember it, so an instance can't change its hash code.
            if (this.IsTransient)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }

            return Id.GetHashCode();
        }

        public static bool operator ==(Role x, Role y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Role x, Role y)
        {
            return !(x == y);
        }

        #endregion
    }
}