using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace AmpedBiz.Data.Configurations
{
    /// <summary> Postgres batcher factory </summary>
    public class PostgresBatcherFactory : IBatcherFactory
    {
        public virtual IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
        {
            return new PostgresBatcher(connectionManager, interceptor);
        }
    }
}