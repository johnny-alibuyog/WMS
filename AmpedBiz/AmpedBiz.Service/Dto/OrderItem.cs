using System;

namespace AmpedBiz.Service.Dto
{
    public enum OrderItemStatus
    {
        Allocated,
        Invoiced,
        Shipped,
        BackOrdered
    }

    public class OrderItem
    {
        public Guid Id { get; set; }

        public string ProductId { get; set; }

        public string OrderId { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public OrderItemStatus Status { get; set; }

        public bool InsufficientInventory { get; set; }
    }

    public class OrderItemPageItem
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public string Quantity { get; set; }

        public string Discount { get; set; }

        public string UnitPrice { get; set; }

        public string ExtendedPrice { get; set; }

        public string StatusName { get; set; }

        public bool InsufficientInventory { get; set; }
    }
}