using NHibernate;
using NHibernate.Context;

namespace AmpedBiz.Data
{
    public static class SessionFactoryExtention
    {
        public static ISession RetrieveSharedSession(this ISessionFactory sessionFactory)
        {
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                CurrentSessionContext.Bind(sessionFactory.OpenSession());
            }

            if (!sessionFactory.GetCurrentSession().IsConnected ||
                !sessionFactory.GetCurrentSession().IsOpen)
            {
                CurrentSessionContext.Unbind(sessionFactory);
                CurrentSessionContext.Bind(sessionFactory.OpenSession());
            }

            return sessionFactory.GetCurrentSession();
        }

        public static ISession ReleaseSharedSession(this ISessionFactory sessionFactory)
        {
            return CurrentSessionContext.Unbind(sessionFactory);
        }
    }
}
