using System;

namespace AmpedBiz.Service.Dto
{
    public class Branch
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TaxpayerIdentificationNumber { get; set; }

        public Contact Contact { get; set; }

        public Address Address { get; set; }
    }

    public class BranchPageItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
