namespace AmpedBiz.Service.Dto
{
    public class Supplier
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }
    }

    public class SupplierPageItem
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
