using System;

namespace AmpedBiz.Service.Dto
{
    public enum OrderDetailStatus
    {
        Allocated,
        Invoiced,
        Shipped,
        BackOrdered
    }

    public class OrderDetail
    {
        public Guid Id { get; set; }

        public string ProductId { get; set; }

        public string OrderId { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public OrderDetailStatus Status { get; set; }

        public bool InsufficientInventory { get; set; }
    }

    public class OrderDetailPageItem
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