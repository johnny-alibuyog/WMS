using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class BranchModel : LookupModel<Guid>
    {
        public BranchModel(Guid id, string name) : base(id, name) { }
    }
}
