namespace AmpedBiz.Service.Dto
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SupplierId { get; set; }

        public string CategoryId { get; set; }

        public string Image { get; set; }

        public decimal BasePriceAmount { get; set; }

        public decimal RetailPriceAmount { get; set; }

        public decimal WholesalePriceAmount { get; set; }

        public bool Discontinued { get; set; }
    }

    public class ProductPageItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public string BasePrice { get; set; }

        public string RetailPrice { get; set; }

        public string WholesalePrice { get; set; }

        public bool Discontinued { get; set; }
    }

    public class ProductInventory
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal? TargetValue { get; set; }

        public decimal? AvailableValue { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public decimal? RetailPriceAmount { get; set; }

        public decimal? WholeSalePriceAmount { get; set; }

        public decimal? DiscountAmount { get; set; }
    }
}