using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class User : Lookup<Guid>
    {
        public User(Guid id, string name) : base(id, name) { }
    }
}
