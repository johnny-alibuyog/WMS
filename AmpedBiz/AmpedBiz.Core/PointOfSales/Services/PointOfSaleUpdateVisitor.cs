using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Common.Services.Generators;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.PointOfSales.Services
{
	public class PointOfSaleUpdateVisitor : IVisitor<PointOfSale>
	{
		public virtual Prop<string> InvoiceNumber { get; set; } = new Prop<string>();

		public virtual Prop<Branch> Branch { get; set; } = new Prop<Branch>();

		public virtual Prop<Customer> Customer { get; set; } = new Prop<Customer>();

		public virtual Prop<Pricing> Pricing { get; set; } = new Prop<Pricing>();

		public virtual Prop<PaymentType> PaymentType { get; set; } = new Prop<PaymentType>();

		public virtual Prop<User> TendredBy { get; set; } = new Prop<User>();

		public virtual Prop<DateTime> TendredOn { get; set; } = new Prop<DateTime>();

		public virtual Prop<IEnumerable<PointOfSaleItem>> Items { get; set; } = new Prop<IEnumerable<PointOfSaleItem>>();

		public virtual Prop<IEnumerable<PointOfSalePayment>> Payments { get; set; } = new Prop<IEnumerable<PointOfSalePayment>>();

		public void Visit(PointOfSale target)
		{
			target.Accept(new CodeGenVisitor()); // Generate invoice if not yet generated
			target.Branch = this.Branch.Value(target.Branch);
			target.Customer = this.Customer.Value(target.Customer);
			target.Pricing = this.Pricing.Value(target.Pricing);
			target.PaymentType = this.PaymentType.Value(target.PaymentType);
			target.TendredBy = this.TendredBy.Value(target.TendredBy);
			target.TendredOn = this.TendredOn.Value(target.TendredOn);
			target.Accept(new PointOfSaleUpdateItemsVisitor(this.Items.Value(target.Items)));
			target.Accept(new PointOfSaleUpdatePaymentsVisitor(this.Payments.Value(target.Payments)));
			target.Accept(new PointOfSaleCalculateVisitor());
		}
	}
}
