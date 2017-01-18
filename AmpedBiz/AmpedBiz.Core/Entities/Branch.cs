using System;

namespace AmpedBiz.Core.Entities
{
    public class Branch : Entity<Guid, Branch>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Address Address { get; set; }

        public Branch() : base(default(Guid)) { }

        public Branch(Guid id) : base(id) { }

        public static Branch SuperBranch = new Branch(new Guid("406A71D8-4DFD-4D34-821E-A5E52E144E50"))
        {
            Name = "Super Branch",
            Description = "Super Description",
            Address = new Address()
            {
                Street = "Super Street",
                Barangay = "Super Barangay",
                City = "Super City",
                Province = "Super Province",
                Region = "Super Region",
                Country = "Super Country",
                ZipCode = "Super Zip Code"
            }
        };
    }
}