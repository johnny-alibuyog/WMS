using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderNewlyCreatedVisitor : OrderVisitor
    {
        public virtual string OrderNumber { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual User OrderedBy { get; set; }

        public virtual DateTime? OrderedOn { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual PricingScheme PricingScheme { get; set; }

        public virtual Shipper Shipper { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        public virtual decimal? TaxRate { get; set; }

        public virtual Money Tax { get; set; }

        public virtual Money ShippingFee { get; set; }

        public virtual IEnumerable<OrderItem> Items { get; set; }

        public override void Visit(Order target)
        {
            this.SetItemsTo(target);

            target.OrderNumber = this.OrderNumber ?? target.OrderNumber;
            target.CreatedBy = this.CreatedBy ?? target.CreatedBy;
            target.CreatedOn = this.CreatedOn ?? target.CreatedOn;
            target.OrderedBy = this.OrderedBy ?? target.OrderedBy;
            target.OrderedOn = this.OrderedOn ?? target.OrderedOn;
            target.Branch = this.Branch ?? target.Branch;
            target.Customer = this.Customer ?? target.Customer;
            target.Shipper = this.Shipper ?? target.Shipper;
            target.PaymentType = this.PaymentType ?? target.PaymentType;
            target.TaxRate = this.TaxRate ?? target.TaxRate;
            target.TaxRate = this.TaxRate ?? target.TaxRate;
            target.Tax = this.Tax ?? target.Tax;
            target.ShippingFee = this.ShippingFee ?? target.ShippingFee;
            target.Accept(new OrderCalculateTotalVisitor());
            target.Status = OrderStatus.New;
        }

        private void SetItemsTo(Order target)
        {
            if (this.Items.IsNullOrEmpty())
                return;

            var itemsToInsert = this.Items.Except(target.Items).ToList();
            var itemsToUpdate = target.Items.Where(x => this.Items.Contains(x)).ToList();
            var itemsToRemove = target.Items.Except(this.Items).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Order = target;
                target.Items.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Items.Single(x => x == item);
                item.SerializeWith(value);
                item.Order = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.Order = null;
                target.Items.Remove(item);
            }
        }
    }
}
