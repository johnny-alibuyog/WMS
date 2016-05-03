using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class UOM : Entity<string, UOM>
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public UOM() : this(default(string)) { }

        public UOM(string id, string symbol = null, string name = null) : base(id)
        {
            this.Symbol = symbol;
            this.Name = name;
        }

        public static readonly UOM Kilogram = new UOM("K", "KG", "Kilogram");

        public static readonly UOM Piece = new UOM("P", "PIECE", "Piece");

        public static readonly UOM Case = new UOM("C", "CASE", "Case");

        public static readonly UOM Dozen = new UOM("D", "DOZEN", "Dozen");

        public static IEnumerable<UOM> All = new UOM[] { Kilogram, Piece, Case, Dozen };
    }
}