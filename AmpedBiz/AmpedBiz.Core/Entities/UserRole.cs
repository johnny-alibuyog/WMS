using System;

namespace AmpedBiz.Core.Entities
{
    public class UserRole : Entity<UserRole, Guid>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
