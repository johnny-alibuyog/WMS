namespace AmpedBiz.Common.Pipes
{
    public interface IPipteline<T>
    {
        T Execute(T input);
        IPipteline<T> Register(IStep<T> filter);
    }
}
