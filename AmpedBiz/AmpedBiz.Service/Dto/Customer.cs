using System;

namespace AmpedBiz.Service.Dto
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ContactPerson { get; set; }

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

        public string ContactPerson { get; set; }

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

        public string ContactPerson { get; set; }

        public decimal CreditLimitAmount { get; set; }

        public Contact Contact { get; set; }

        public Address OfficeAddress { get; set; }

        public Address BillingAddress { get; set; }
    }

    public class CustomerSalesReportPageItem
    {
        public DateTime? PaidOn { get; set; }

        public string BranchName { get; set; }

        public string CustomerName { get; set; }

        public string InvoiceNumber { get; set; }

        public string Status { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal? BalanceAmount => (this.TotalAmount ?? 0M) - (this.PaidAmount ?? 0M);
    }

    public class CustomerPaymentReportPageItem
    {
        public DateTime? PaidOn { get; set; }

        public string InvoiceNumber { get; set; }

        public string BranchName { get; set; }

        public string CustomerName { get; set; }

        public string PaymentTypeName { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? PaidAmount { get; set; }

        public decimal? BalanceAmount { get; set; }
    }

    public class CustomerOrderDeliveryReportPageItem
    {
        public Guid OrderId { get; set; }

        public DateTime? ShippedOn { get; set; }

        public string BranchName { get; set; }

        public string InvoiceNumber { get; set; }

        public string CustomerName { get; set; }

        public string PricingName { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? SubTotalAmount { get; set; }
    }
}