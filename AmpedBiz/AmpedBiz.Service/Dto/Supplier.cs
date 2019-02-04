using System;

namespace AmpedBiz.Service.Dto
{
    public class Supplier
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public bool Assigned { get; set; }
    }

    public class SupplierPageItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }
    }

    public class SupplierReportPageItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }
    }
}
