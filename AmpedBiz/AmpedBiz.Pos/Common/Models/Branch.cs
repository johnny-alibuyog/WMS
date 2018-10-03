using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class Branch : Lookup<Guid>
    {
        public Branch(Guid id, string name) : base(id, name) { }
    }
}
