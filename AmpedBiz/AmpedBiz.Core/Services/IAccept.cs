namespace AmpedBiz.Core.Services
{
    public interface IAccept<T> where T : IVisitor
    {
        void Accept(T visitor);
    }
}
