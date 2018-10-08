using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Dto
{
    public class Return
    {
        public Guid Id { get; set; }

        public Lookup<Guid> Branch { get; set; }

        public Lookup<Guid> Customer { get; set; }

        public Lookup<Guid> ReturnedBy { get; set; }

        public DateTime? ReturnedOn { get; set; }

        public string Remarks { get; set; }

        public decimal TotalReturnedAmount { get; set; }

        public IEnumerable<ReturnItem> Items { get; set; }
    }

    public class ReturnPageItem
    {
        public Guid Id { get; set; }

        public string BranchName { get; set; }

        public string CustomerName { get; set; }

        public string ReturnedByName { get; set; }

        public DateTime? ReturnedOn { get; set; }

        public string Remarks { get; set; }

        public decimal ReturnedAmount { get; set; }
    }

	public class ReturnsDetailsReportPageItem
	{
		public Guid Id { get; set; }

		public string BranchName { get; set; }

		public string CustomerName { get; set; }

		public string ProductName { get; set; }

		public string ReasonName { get; set; }

		public string ReturnedByName { get; set; }

		public DateTime ReturnedOn { get; set; }

		public decimal ReturnedAmount { get; set; }
	}

	public class ReturnsByCustomerPageItem
    {
        public Guid Id { get; set; }

		public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string BranchName { get; set; }

        public decimal ReturnedAmount { get; set; }
    }

	public class ReturnsByCustomerDetailsPageItem
	{
		public Guid Id { get; set; }

		public string BranchName { get; set; }

		public string CustomerCode { get; set; }

		public string CustomerName { get; set; }

		public DateTime ReturnedOn { get; set; }

		public decimal ReturnedAmount { get; set; }
	}

	public class ReturnsByCustomerReportPageItem
	{
		public Guid Id { get; set; }

		public string BranchName { get; set; }

		public string CustomerCode { get; set; }

		public string CustomerName { get; set; }

		public string ReturnedByName { get; set; }

		public DateTime ReturnedOn { get; set; }

		public decimal ReturnedAmount { get; set; }
	}

	public class ReturnsByProductPageItem
    {
        public Guid Id { get; set; }

		public string BranchName { get; set; }

		public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string QuantityUnit { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal ReturnedAmount { get; set; }
    }

	public class ReturnsByProductDetailsPageItem
	{
		public Guid Id { get; set; }

		public string BranchName { get; set; }

		public string ProductCode { get; set; }

		public string ProductName { get; set; }

		public DateTime ReturnedOn { get; set; }

		public string QuantityUnit { get; set; }

		public decimal QuantityValue { get; set; }

		public decimal ReturnedAmount { get; set; }
	}

	public class ReturnsByReasonPageItem
    {
        public string Id { get; set; }

		public string BranchName { get; set; }

        public string ReasonName { get; set; }

        public decimal ReturnedAmount { get; set; }
    }

	public class ReturnsByReasonDetailsPageItem
	{
		public string Id { get; set; }

		public string BranchName { get; set; }

		public string ReasonName { get; set; }

		public DateTime ReturnedOn { get; set; }

		public decimal ReturnedAmount { get; set; }
	}
}
