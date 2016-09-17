using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AmpedBiz.Service.Dto
{
    public class OrderInvoiceDetail
    {
        public virtual string CustomerName { get; set; }

        public virtual string InvoiceNumber { get; set; }

        public virtual DateTime InvoicedOn { get; set; }

        public virtual string InvoicedByName { get; set; }

        public virtual string PricingSchemeName { get; set; }

        public virtual string PaymentTypeName { get; set; }

        public virtual string BranchName { get; set; }

        public virtual DateTime OrderedOn { get; set; }

        public virtual string OrderedFromName { get; set; }

        public virtual DateTime ShippedOn { get; set; }

        public virtual string ShippedByName { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual decimal TaxAmount { get; set; }

        public virtual decimal ShippingFeeAmount { get; set; }

        public virtual decimal DiscountAmount { get; set; }

        public virtual decimal ReturnedAmount { get; set; }

        public virtual decimal SubTotalAmount { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual IEnumerable<OrderInvoiceDetailItem> Items { get; set; } = new Collection<OrderInvoiceDetailItem>();

        internal static IEnumerable<OrderInvoiceDetailItem> EvaluateItems(Core.Entities.Order entity)
        {
            Func<Core.Entities.OrderItem, IEnumerable<Core.Entities.OrderReturn>, decimal> EvaluateDiscount = (item, returns) => item.Discount.Amount - returns.Sum(o => o.Discount.Amount);
            Func<Core.Entities.OrderItem, IEnumerable<Core.Entities.OrderReturn>, decimal> EvaluateQuantity = (item, returns) => item.Quantity.Value - returns.Sum(o => o.Quantity.Value);
            Func<Core.Entities.OrderItem, IEnumerable<Core.Entities.OrderReturn>, decimal> EvaluateExtendedPrice = (item, returns) => item.ExtendedPrice.Amount - returns.Sum(o => o.ExtendedPrice.Amount);
            Func<Core.Entities.OrderItem, IEnumerable<Core.Entities.OrderReturn>, decimal> EvaluateTotalPrice = (item, returns) => EvaluateExtendedPrice(item, returns) - EvaluateDiscount(item, returns);

            return entity.Items
                .GroupJoin(entity.Returns,
                    (x) => x.Product,
                    (x) => x.Product,
                    (item, returns) => new OrderInvoiceDetailItem()
                    {
                        Id = item.Id,
                        OrderId = item.Order.Id,
                        Product = new Lookup<string>(
                            item.Product.Id,
                            item.Product.Name
                        ),
                        UnitPriceAmount = item.UnitPrice.Amount,
                        DiscountRate = item.DiscountRate,
                        DiscountAmount = EvaluateDiscount(item, returns),
                        QuantityValue = EvaluateQuantity(item, returns),
                        ExtendedPriceAmount = EvaluateExtendedPrice(item, returns),
                        TotalPriceAmount = EvaluateTotalPrice(item, returns)
                    }
                )
                .ToList();
        }
    }

    public class OrderInvoiceDetailItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Lookup<string> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public decimal TotalPriceAmount { get; set; }
    }
}
