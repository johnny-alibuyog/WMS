using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class UserModel : LookupModel<Guid>
    {
        public UserModel(Guid id, string name) : base(id, name) { }
    }
}
