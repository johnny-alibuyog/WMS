using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    // Reference: https://docs.oracle.com/cd/A60725_05/html/comnls/us/inv/uomov.htm#c_uomov

    public class UnitOfMeasure : Entity<string, UnitOfMeasure>
    {
        public virtual string Name { get; set; }

        public virtual bool IsBaseUnit { get; set; }

        public virtual decimal? ConvertionFactor { get; set; }

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