using System;

namespace AmpedBiz.Pos.Common.Models
{
    public class ProductModel : LookupModel<Guid>
    {
        public ProductModel(Guid id, string name) : base(id, name) { }
    }
}
