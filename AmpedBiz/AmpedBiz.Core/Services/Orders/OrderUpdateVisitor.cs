using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderUpdateVisitor : IVisitor<Order>
    {
        public virtual string OrderNumber { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual User OrderedBy { get; set; }

        public virtual DateTime? OrderedOn { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Shipper Shipper { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual Pricing Pricing { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual IEnumerable<OrderItem> Items { get; set; }

        public virtual IEnumerable<OrderPayment> Payments { get; set; }

        public virtual IEnumerable<OrderReturn> Returns { get; set; }

        public virtual void Visit(Order target)
        {
            if (string.IsNullOrEmpty(target.InvoiceNumber))
                target.InvoiceNumber = new InvoiceGenerator().Generate();

            target.OrderNumber = this.OrderNumber ?? target.OrderNumber;
            target.CreatedBy = this.CreatedBy ?? target.CreatedBy;
            target.CreatedOn = this.CreatedOn ?? target.CreatedOn;
            target.OrderedBy = this.OrderedBy ?? target.OrderedBy;
            target.OrderedOn = this.OrderedOn ?? target.OrderedOn;
            target.Branch = this.Branch ?? target.Branch;
            target.Customer = this.Customer ?? target.Customer;
            target.Shipper = this.Shipper ?? target.Shipper;
            target.ShippingAddress = this.ShippingAddress ?? target.ShippingAddress;
            target.Pricing = this.Pricing ?? target.Pricing;
            target.PaymentType = this.PaymentType ?? target.PaymentType;
            target.TaxRate = this.TaxRate ?? target.TaxRate;
            target.TaxRate = this.TaxRate ?? target.TaxRate;
            target.Tax = this.Tax ?? target.Tax;
            target.ShippingFee = this.ShippingFee ?? target.ShippingFee;

            if (target.State.Stage.IsModificationAllowedTo(OrderAggregate.Items))
                target.Accept(new OrderUpdateItemsVisitor(this.Items));

            if (target.State.Stage.IsModificationAllowedTo(OrderAggregate.Payments))
                target.Accept(new OrderUpdatePaymentsVisitor(this.Payments));

            if (target.State.Stage.IsModificationAllowedTo(OrderAggregate.Returns))
                target.Accept(new OrderUpdateReturnsVisitor(this.Returns, this.Branch));

            target.Accept(new OrderCalculateVisitor());
        }
    }
}
