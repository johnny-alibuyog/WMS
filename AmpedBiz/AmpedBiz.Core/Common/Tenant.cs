using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Common
{
    public class Tenant : Entity<string, Tenant>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public Tenant() : base(default(string)) { }

        public Tenant(string id, string name, string description) : base(id)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        public static readonly Tenant Default = new Tenant(id: "nicon", name: "Nicon", description: "Nicon Services");
    }
}