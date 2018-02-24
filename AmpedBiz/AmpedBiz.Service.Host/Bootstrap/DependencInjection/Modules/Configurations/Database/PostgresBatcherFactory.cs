using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules.Configurations.Database
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