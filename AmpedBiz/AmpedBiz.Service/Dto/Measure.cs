namespace AmpedBiz.Service.Dto
{
    public class Measure
    {
        public decimal Value { get; set; }

        public UnitOfMeasure Unit { get; set; }

        public Measure() { }

        public Measure(decimal value, UnitOfMeasure unit)
        {
            this.Value = value;
            this.Unit = unit;
        }
    }
}
