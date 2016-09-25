namespace AmpedBiz.Common.Pipes
{
    public interface IFilterChain<T>
    {
        void Execute(T input);
        IFilterChain<T> Register(IFilter<T> filter);
    }
}
