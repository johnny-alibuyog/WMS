using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Products
{
    // Reference: https://docs.oracle.com/cd/A60725_05/html/comnls/us/inv/uomov.htm#c_uomov

    public class UnitOfMeasure : Entity<string, UnitOfMeasure>
    {
        public virtual string Name { get; set; }

        public UnitOfMeasure() : base(default(string)) { }

        public UnitOfMeasure(string id = null, string name = null) : base(id) => this.Name = name;

        public override string ToString() => this.Id;
	}
}