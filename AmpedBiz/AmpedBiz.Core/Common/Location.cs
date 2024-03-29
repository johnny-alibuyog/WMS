﻿using AmpedBiz.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Common
{
    public class Location : Entity<Guid, Location>
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public Location() : base(default(Guid)) { }

        public Location(Guid id) : base(id) { }
    }
}
