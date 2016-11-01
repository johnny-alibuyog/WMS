using NHibernate;

namespace AmpedBiz.Data
{
    public interface ISessionFactoryProvider1
    {
        ISessionFactory GetSessionFactory();
    }
}
