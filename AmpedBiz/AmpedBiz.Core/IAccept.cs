namespace AmpedBiz.Core
{
    public interface IAccept<T> where T : IVisitor
    {
        void Accept(T visitor);
    }

    //public interface IAccept<T, R> where T : IVisitor<T, R>
    //{
    //    R Accept(T visitor);
    //}
}
