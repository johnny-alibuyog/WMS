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

        public static Branch SupperBranch = new Branch(new Guid("406A71D8-4DFD-4D34-821E-A5E52E144E50"))
        {
            Name = "Supper Branch",
            Description = "Supper Description",
            Address = new Address()
            {
                Street = "Supper Street",
                Barangay = "Supper Barangay",
                City = "Supper City",
                Province = "Supper Province",
                Region = "Supper Region",
                Country = "Supper Country",
                ZipCode = "Supper Zip Code"
            }
        };
    }
}