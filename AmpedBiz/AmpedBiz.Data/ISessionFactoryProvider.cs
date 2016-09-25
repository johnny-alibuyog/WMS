using NHibernate;

namespace AmpedBiz.Data
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
    }
}
