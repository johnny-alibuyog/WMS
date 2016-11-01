using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Host.Auth.Models
{
    public class User : IUser<Guid>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<Role> Roles { get; set; }


        #region Equality Comparer

        private int? _oldHashCode;

        private bool IsTransient
        {
            get { return this.Id == Guid.Empty; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as User;
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

        public static bool operator ==(User x, User y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(User x, User y)
        {
            return !(x == y);
        }

        #endregion
    }
}