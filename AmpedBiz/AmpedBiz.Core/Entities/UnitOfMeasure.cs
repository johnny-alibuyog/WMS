using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    // Reference: https://docs.oracle.com/cd/A60725_05/html/comnls/us/inv/uomov.htm#c_uomov

    public class UnitOfMeasure : Entity<string, UnitOfMeasure>
    {
        public virtual string Name { get; set; }

        public UnitOfMeasure() : base(default(string)) { }

        public UnitOfMeasure(string id = null, string name = null) : base(id)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Id;
        }

        public static readonly UnitOfMeasure Piece = new UnitOfMeasure("PC", "Piece");

        public static readonly UnitOfMeasure Strips = new UnitOfMeasure("ST", "Strips");

        public static readonly UnitOfMeasure Box = new UnitOfMeasure("BX", "Box");

        public static readonly UnitOfMeasure Case = new UnitOfMeasure("CS", "Case");

        public static readonly IEnumerable<UnitOfMeasure> All = new[]
        {
            UnitOfMeasure.Piece,
            UnitOfMeasure.Strips,
            UnitOfMeasure.Box,
            UnitOfMeasure.Case,
        };
    }
}