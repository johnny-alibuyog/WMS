namespace AmpedBiz.Service.Dto
{
    public class UnitOfMeasure
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public UnitOfMeasure() { }

        public UnitOfMeasure(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    public class UnitOfMeasurePageItem
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
    }
}
