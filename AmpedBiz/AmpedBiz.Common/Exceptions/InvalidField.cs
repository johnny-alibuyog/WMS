namespace AmpedBiz.Common.Exceptions
{
    public class InvalidField
    {
        public virtual string Entity { get; set; }

        public virtual string Property { get; set; }

        public virtual string Message { get; set; }

        public virtual object Value { get; set; }
    }
}
