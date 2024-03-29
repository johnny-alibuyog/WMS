﻿using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Common
{
    public class Branch : Entity<Guid, Branch>, IHasTenant, IAuditable
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string TaxpayerIdentificationNumber { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Address Address { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual DateTime? ModifiedOn { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual User ModifiedBy { get; set; }

        public Branch() : base(default(Guid)) { }

        public Branch(Guid id) : base(id) { }

        public static Branch Default = new Branch(new Guid("406A71D8-4DFD-4D34-821E-A5E52E144E50"))
        {
            Name = "Nicon",
            Tenant = Tenant.Default,
            Description = "Nicon Services",
            TaxpayerIdentificationNumber = "102-7078388",
            Contact = new Contact()
            {
                Landline = "(052) 811-3678"
            },
            Address = new Address()
            {
                Street = string.Empty,
                Barangay = "Francia",
                City = "Virac",
                Province = "Catanduanes",
                Region = string.Empty,
                Country = "Philippines",
                ZipCode = "4800"
            }
        };
    }
}