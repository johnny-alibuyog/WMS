namespace AmpedBiz.Core.Entities
{
    public class Product : Entity<string, Product>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ProductCategory Category { get; set; }

        public virtual string Image { get; set; }

        public virtual bool Discontinued { get; set; }

        public virtual Inventory Inventory { get; set; }

        public Product() : base(default(string))
        {
            this.Inventory = new Inventory(this);
        }

        public Product(string id) : base(id)
        {
            this.Inventory = new Inventory(this);
        }
    }
}