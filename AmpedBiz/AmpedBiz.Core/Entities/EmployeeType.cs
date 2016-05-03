using System;

namespace AmpedBiz.Core.Entities
{
    public class EmployeeType : Entity<Guid, EmployeeType>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public EmployeeType() : this(default(Guid)) { }

        public EmployeeType(Guid id) : base(id) { }
    }
}