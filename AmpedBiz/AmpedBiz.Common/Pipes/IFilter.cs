namespace AmpedBiz.Common.Pipes
{
    public interface IFilter<T>
    {
        T Execute(T input);
        void Register(IFilter<T> filter);
    }
}