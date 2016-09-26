using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public class Return
    {
        public virtual Guid Id { get; set; }

        public virtual Lookup<Guid> Branch { get; set; }

        public virtual Lookup<string> Customer { get; set; }

        public virtual Lookup<Guid> ReturnedBy { get; set; }

        public virtual DateTime? ReturnedOn { get; set; }

        public virtual Lookup<string> Reason { get; set; }

        public virtual string Remarks { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual IEnumerable<ReturnItem> Items { get; set; }
    }

    public class ReturnPageItem
    {
        public virtual Guid Id { get; set; }

        public virtual string BranchName { get; set; }

        public virtual string CustomerName { get; set; }

        public virtual string ReturnedByName { get; set; }

        public virtual DateTime? ReturnedOn { get; set; }

        public virtual string ReasonName { get; set; }

        public virtual string Remarks { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }
}
