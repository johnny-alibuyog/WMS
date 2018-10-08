using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class CustomerModel : LookupModel<Guid>
    {
        public CustomerModel(Guid id, string name) : base(id, name) { }
    }
}
