namespace AmpedBiz.Common.Pipes
{
    public interface IStep<T>
    {
        T Execute(T input);
        void Register(IStep<T> step);
    }
}