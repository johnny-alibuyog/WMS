using AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Dto
{
    public class Customer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal CreditLimit { get; set; }

        public string PricingSchemeId { get; set; }

        public Contact Contact { get; set; }

        public Address OfficeAddress { get; set; }

        public Address BillingAddress { get; set; }
    }

    public class CustomerPageItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreditLimit { get; set; }

        public string PricingSchemeName { get; set; }

        public Contact Contact { get; set; }

        public Address OfficeAddress { get; set; }

        public Address BillingAddress { get; set; }
    }
}