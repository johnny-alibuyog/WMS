using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class Customer : Lookup<Guid>
    {
        public Customer(Guid id, string name) : base(id, name) { }
    }
}
