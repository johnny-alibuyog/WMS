namespace AmpedBiz.Core.Entities
{
    public class UOM : Entity<UOM, string>
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public UOM()
        {
        }

        public UOM(string id, string symbol, string name)
        {
            this.Id = id;
            this.Symbol = symbol;
            this.Name = name;
        }

        public static readonly UOM KILOGRAM = new UOM("K", "KG", "Kilogram");

        public static readonly UOM PIECE = new UOM("P", "PIECE", "Piece");

        public static readonly UOM CASE = new UOM("C", "CASE", "Case");

        public static readonly UOM DOZEN = new UOM("D", "DOZEN", "Dozen");
    }
}