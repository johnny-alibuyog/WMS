using System;

namespace AmpedBiz.Core.Entities
{
    public class EmployeeType : Entity<EmployeeType, Guid>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}