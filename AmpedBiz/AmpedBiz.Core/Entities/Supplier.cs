using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Supplier : Entity<Supplier, string>
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }
    }
}