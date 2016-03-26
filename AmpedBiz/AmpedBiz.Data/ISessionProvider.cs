using NHibernate;

namespace AmpedBiz.Data
{
    public interface ISessionProvider
    {
        ISession GetSharedSession();
        ISession ReleaseSharedSession();
        ISessionFactory SessionFactory { get; }
    }
}
