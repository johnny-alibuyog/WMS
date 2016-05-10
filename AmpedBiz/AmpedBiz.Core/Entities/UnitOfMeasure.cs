using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class UnitOfMeasure : Entity<string, UnitOfMeasure>
    {
        public virtual string Name { get; set; }

        public virtual bool IsBaseUnit { get; set; }

        public virtual decimal? ConvertionFactor { get; set; }

        public virtual UnitOfMeasureClass UnitOfMeasureClass { get; set; }

        public UnitOfMeasure() : this(default(string)) { }

        public UnitOfMeasure(string id, string name = null, bool isBaseUnit = false, decimal? convertionFactor = null) : base(id)
        {
            this.Name = name;
            this.IsBaseUnit = isBaseUnit;
            this.ConvertionFactor = convertionFactor;
        }

        public override string ToString()
        {
            return this.Id;
        }
    }
}