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

        public virtual string Remarks { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }

    public class ReturnsByCustomerPageItem
    {
        public virtual string Id { get; set; }

        public virtual string CustomerName { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }

    public class ReturnsByProductPageItem
    {
        public virtual Guid Id { get; set; }

        public virtual string ProductName { get; set; }

        public virtual decimal QuantityValue { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }

    public class ReturnsByReasonPageItem
    {
        public virtual string Id { get; set; }

        public virtual string ReturnReasonName { get; set; }

        public virtual decimal TotalAmount { get; set; }
    }
}
