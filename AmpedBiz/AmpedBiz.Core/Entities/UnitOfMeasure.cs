using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class UnitOfMeasure : Entity<string, UnitOfMeasure>
    {
        public virtual string Name { get; set; }

        public virtual bool IsBaseUnit { get; set; }

        public virtual decimal? ConversionFactor { get; set; }

        public virtual UnitOfMeasureClass UnitOfMeasureClass { get; set; }

        public UnitOfMeasure(string id) : base(id) { }

        public UnitOfMeasure() : base(default(string)) { }

        public UnitOfMeasure(string id, string name = null, bool isBaseUnit = false, decimal? conversionFactor = null) : base(id)
        {
            this.Name = name;
            this.IsBaseUnit = isBaseUnit;
            this.ConversionFactor = conversionFactor;
        }

        public override string ToString()
        {
            return this.Id;
        }
    }
}