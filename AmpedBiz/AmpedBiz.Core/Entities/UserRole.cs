using System;

namespace AmpedBiz.Core.Entities
{
    public class UserRole : Entity<Guid, UserRole>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }

        public UserRole() : base(default(Guid)) { }

        public UserRole(Guid id) : base(id){ }
    }
}
