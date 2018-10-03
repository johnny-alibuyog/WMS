using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class Product : Lookup<Guid>
    {
        public Product(Guid id, string name) : base(id, name) { }
    }
}
