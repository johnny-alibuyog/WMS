namespace AmpedBiz.Service.Dto
{
    public class Lookup<T>
    {
        public virtual T Id { get; set; }

        public virtual string Name { get; set; }
    }
}
