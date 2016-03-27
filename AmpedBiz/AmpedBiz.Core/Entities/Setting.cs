namespace AmpedBiz.Core.Entities
{
    public class Setting
    {
        public Setting()
        {
        }

        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}