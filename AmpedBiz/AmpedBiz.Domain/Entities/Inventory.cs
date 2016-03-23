using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Domain.Entities
{
    public class Inventory : Entity<Inventory, Guid>
    {
        public virtual IEnumerable<InventoryItem> Items { get; protected set; }
    }
}
