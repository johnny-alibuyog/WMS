using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;

namespace AmpedBiz.Core.Returns
{
    public class ReturnReason : Entity<string, ReturnReason>, IHasTenant
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; set; }

        public ReturnReason() : base(default(string)) { }

        public ReturnReason(string id, string name = null) : base(id) 
        {
            this.Name = name;
        }

        public static ReturnReason Damaged = new ReturnReason("D", "Damaged");

        public static ReturnReason Expired = new ReturnReason("E", "Expired");

        public static ReturnReason SlowMoving = new ReturnReason("SM", "Slow Moving");

        public static readonly IEnumerable<ReturnReason> All = new [] 
        {
            ReturnReason.Damaged,
            ReturnReason.Expired,
            ReturnReason.SlowMoving
        };
    }
}
