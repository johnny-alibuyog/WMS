using System;

namespace AmpedBiz.Service.Dto
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal CreditLimitAmount { get; set; }

        public string PricingId { get; set; }

        public Contact Contact { get; set; }

        public Address OfficeAddress { get; set; }

        public Address BillingAddress { get; set; }
    }

    public class CustomerPageItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string CreditLimitAmount { get; set; }

        public string PricingName { get; set; }

        public Contact Contact { get; set; }

        public Address OfficeAddress { get; set; }

        public Address BillingAddress { get; set; }
    }

    public class CustomerReportPageItem
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal CreditLimitAmount { get; set; }

        public Contact Contact { get; set; }

        public Address OfficeAddress { get; set; }

        public Address BillingAddress { get; set; }
    }
}