using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class InventoryAdjustmentReason : Entity<Guid, InventoryAdjustmentReason>
    {

        public InventoryAdjustmentReason() : this(default(Guid)) { }

        public InventoryAdjustmentReason(Guid id) : base(id) { }
    }
}
