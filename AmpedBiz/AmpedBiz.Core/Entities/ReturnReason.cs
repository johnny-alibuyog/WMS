using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class ReturnReason : Entity<string, ReturnReason>
    {
        public virtual string Name { get; set; }

        public ReturnReason() : base(default(string)) { }

        public ReturnReason(string id, string name) : base(id) 
        {
            this.Name = name;
        }

        public static ReturnReason Damaged = new ReturnReason("D", "Damaged");

        public static ReturnReason Expired = new ReturnReason("E", "Expired");

        public static readonly IEnumerable<ReturnReason> All = new [] 
        {
            ReturnReason.Damaged,
            ReturnReason.Expired,
        };
    }
}
