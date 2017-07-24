using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Service.Dto
{
    public class Voucher
    {
        public Guid Id { get; set; }

        public string SupplierName { get; set; }

        public string ReferenceNumber { get; set; }

        public string VoucherNumber { get; set; }

        public string ApprovedByName { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public string PaymentTypeName { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        //public decimal PaidAmount { get; set; }

        public virtual IEnumerable<VoucherItem> Items { get; set; } = new Collection<VoucherItem>();

    }

    public class VoucherItem
    {
        public Guid Id { get; set; }

        public Lookup<Guid> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitCostAmount { get; set; }

        public decimal TotalCostAmount { get; set; }
    }
}
