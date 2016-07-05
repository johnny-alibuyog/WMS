namespace AmpedBiz.Common.CustomTypes
{
    public class Lookup<TId>
    {
        public virtual TId Id { get; set; }

        public virtual string Name { get; set; }

        public Lookup() { }

        public Lookup(TId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
