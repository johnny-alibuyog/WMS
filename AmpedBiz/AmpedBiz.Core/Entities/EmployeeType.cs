using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class EmployeeType : Entity<string, EmployeeType>
    {
        public virtual string Name { get; set; }

        public EmployeeType() : this(default(string)) { }

        public EmployeeType(string id, string name = null) : base(id)
        {
            this.Name = name;
        }


        public static readonly EmployeeType Sales = new EmployeeType("S", "Sales");

        public static readonly EmployeeType Warehouse = new EmployeeType("W", "Warehouse");

        public static readonly EmployeeType Admin = new EmployeeType("A", "Admin");

        public static readonly IEnumerable<EmployeeType> All = new List<EmployeeType>() { Sales, Warehouse, Admin };
    }
}