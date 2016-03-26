using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Domain.Entities
{
    public class Audit2 : Entity<Audit2, Guid>
    {
        public virtual string Operation { get; set; }

        public virtual Nullable<DateTimeOffset> Date { get; set; }

        public virtual string UserName { get; set; }

        public virtual string EntityId { get; set; }

        public virtual string EntityName { get; set; }

        public virtual string FieldName { get; set; }

        public virtual string OldValue { get; set; }

        public virtual string NewValue { get; set; }

        public Audit2()
        {
            this.Date = DateTimeOffset.Now;
        }
    }
}
