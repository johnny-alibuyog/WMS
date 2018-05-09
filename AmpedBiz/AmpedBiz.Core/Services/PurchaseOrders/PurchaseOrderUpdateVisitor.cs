using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderUpdateVisitor : IVisitor<PurchaseOrder>
    {
        public Branch Branch { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public User CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ExpectedOn { get; set; }

        public PaymentType PaymentType { get; set; }

        public Shipper Shipper { get; set; }

        public Money ShippingFee { get; set; }

        public Money Tax { get; set; }

        public Supplier Supplier { get; set; }

        public IEnumerable<PurchaseOrderItem> Items { get; set; }

        public IEnumerable<PurchaseOrderPayment> Payments { get; set; }

        public IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }

        public void Visit(PurchaseOrder target)
        {
            if (string.IsNullOrWhiteSpace(target.VoucherNumber))
                target.VoucherNumber = new VoucherGenerator().Generate();

            target.PurchaseOrderNumber = this.PurchaseOrderNumber ?? target.PurchaseOrderNumber;
            target.ReferenceNumber = this.ReferenceNumber ?? target.ReferenceNumber;
            target.CreatedBy = this.CreatedBy ?? target.CreatedBy;
            target.CreatedOn = this.CreatedOn ?? target.CreatedOn;
            target.ExpectedOn = this.ExpectedOn ?? target.ExpectedOn;
            target.PaymentType = this.PaymentType ?? target.PaymentType;
            target.Tax = this.Tax ?? target.Tax;
            target.ShippingFee = this.ShippingFee ?? target.ShippingFee;
            target.Shipper = this.Shipper ?? target.Shipper;
            target.Supplier = this.Supplier ?? target.Supplier;

            if (target.State.Stage.IsModificationAllowedTo(PurchaseOrderAggregate.Items))
                target.Accept(new PurchaseOrderUpdateItemsVisitor(this.Items));

            if (target.State.Stage.IsModificationAllowedTo(PurchaseOrderAggregate.Payments))
                target.Accept(new PurchaseOrderUpdatePaymentsVisitor(this.Payments));

            if (target.State.Stage.IsModificationAllowedTo(PurchaseOrderAggregate.Receipts))
                target.Accept(new PurchaseOrderUpdateReceiptsVisitor(this.Receipts, this.Branch));

            target.Accept(new PurchaseOrderCalculateVisitor());

            target.Accept(new PurchaseOrderLogTransactionVisitor(
                transactedBy: target.CreatedBy,
                transactedOn: target.CreatedOn ?? DateTime.Now
            ));
        }
    }
}
