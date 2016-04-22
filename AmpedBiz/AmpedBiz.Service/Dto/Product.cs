namespace AmpedBiz.Service.Dto
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public Supplier Supplier { get; set; }
        public string SupplierId { get; set; }

        //public ProductCategory Category { get; set; }
        public string CategoryId { get; set; }

        public string Image { get; set; }

        //public Money BasePrice { get; set; }

        public decimal BasePriceAmount { get; set; }

        public string BasePriceCurrencyId { get; set; }

        //public Money RetailPrice { get; set; }

        public decimal RetailPriceAmount { get; set; }

        public string RetailPriceCurrencyId { get; set; }

        //public Money WholeSalePrice { get; set; }

        public decimal WholeSalePriceAmount { get; set; }

        public string WholeSalePriceCurrencyId { get; set; }

        public bool Discontinued { get; set; }
    }
}