using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderSaveVisitor : PurchaseOrderVisitor
    {
        public virtual string PurchaseOrderNumber { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual DateTime? ExpectedOn { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual Shipper Shipper { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual IEnumerable<PurchaseOrderItem> Items { get; set; }

        public virtual IEnumerable<PurchaseOrderPayment> Payments { get; set; }

        public virtual IEnumerable<PurchaseOrderReceipt> Receipts { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            target.PurchaseOrderNumber = this.PurchaseOrderNumber ?? target.PurchaseOrderNumber;
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
                target.Accept(new PurchaseOrderUpdateReceiptsVisitor(this.Receipts));

            target.Accept(new PurchaseOrderCalculateVisitor());
        }
    }
}
