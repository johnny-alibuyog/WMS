namespace AmpedBiz.Service.Dto
{
    public class UnitOfMeasure
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsBaseUnit { get; set; }

        public virtual decimal? ConversionFactor { get; set; }

        public virtual string UnitOfMeasureClassId { get; set; }
    }

    public class UnitOfMeasurePageItem
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsBaseUnit { get; set; }

        public virtual decimal? ConversionFactor { get; set; }

        public virtual string UnitOfMeasureClassName { get; set; }
    }
}
