using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Stock : Entity<Guid, Stock>, IAuditable
    {
        public virtual DateTime? CreatedOn { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual User ModifiedBy { get; set; }

        public virtual Inventory Inventory { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual DateTime? ExpiresOn { get; protected set; }

        public virtual bool Bad { get; protected set; }

        public Stock() : base(default(Guid)) { }

        public Stock(Inventory inventory, Measure quantity, DateTime? expiresOn = null, bool bad = false, Guid? id = null)
            : base(id ?? default(Guid))
        {
            this.Inventory = inventory;
            this.Quantity = quantity;
            this.ExpiresOn = expiresOn;
            this.Bad = bad;
        }
    }

    public class ReceivedStock : Stock { }

    public class ReleasedStock : Stock { }

    public class ShrinkedStock : Stock { }
}
