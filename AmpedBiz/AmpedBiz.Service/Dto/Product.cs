namespace AmpedBiz.Service.Dto
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Supplier Supplier { get; set; }

        public ProductCategory Category { get; set; }

        public string Image { get; set; }

        public Money BasePrice { get; set; }

        public Money RetailPrice { get; set; }

        public Money WholeSalePrice { get; set; }

        public bool Discontinued { get; set; }
    }
}