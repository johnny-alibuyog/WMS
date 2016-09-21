using System;
using System.IO;
using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Conventions;
using AmpedBiz.Data.EntityDefinitions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Context;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Data
{
    public class SessionProvider : ISessionProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private static ValidatorEngine _validator;
        private static IAuditProvider _auditProvider;

        internal static ValidatorEngine Validator
        {
            get { return _validator; }
            private set { _validator = value; }
        }

        internal static IAuditProvider AuditProvider
        {
            get { return _auditProvider; }
            private set { _auditProvider = value; }
        }

        public virtual ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public virtual ISession GetSharedSession()
        {
            if (CurrentSessionContext.HasBind(_sessionFactory) != true)
            {
                CurrentSessionContext.Bind(_sessionFactory.OpenSession());
            }

            if (_sessionFactory.GetCurrentSession().IsConnected == false ||
                _sessionFactory.GetCurrentSession().IsOpen == false)
            {
                CurrentSessionContext.Unbind(_sessionFactory);
                CurrentSessionContext.Bind(_sessionFactory.OpenSession());
            }

            return _sessionFactory.GetCurrentSession();
        }

        public virtual ISession ReleaseSharedSession()
        {
            return CurrentSessionContext.Unbind(_sessionFactory);
        }

        public SessionProvider(ValidatorEngine validator, IAuditProvider auditProvider)
        {
            _validator = validator;
            _auditProvider = auditProvider;

            _sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(x => x
                        .Host(DbConfig.Instance.Host)
                        .Port(DbConfig.Instance.Port)
                        .Database(DbConfig.Instance.Name)
                        .Username(DbConfig.Instance.Username)
                        .Password(DbConfig.Instance.Password)
                    )
                    //.ConnectionString(x => x.FromConnectionStringWithKey("AmpedbizConnectionString"))
                    .QuerySubstitutions("true 1, false 0, yes y, no n")
                    .DefaultSchema("public")
                    .AdoNetBatchSize(1)
                    .FormatSql()
                    .ShowSql()
                )
                .Mappings(x => x
                    .FluentMappings.AddFromAssemblyOf<UserMapping>()
                    .Conventions.AddFromAssemblyOf<_CustomJoinedSubclassConvention>()
                    .Conventions.Setup(o => o.Add(AutoImport.Never()))
                    .ExportTo(DbConfig.Instance.GetWorkingPath("Mappings"))
                )
                .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                .ExposeConfiguration(BatcherConfiguration.Configure)
                .ExposeConfiguration(EventListenerConfiguration.Configure)
                .ExposeConfiguration(CacheConfiguration.Configure)
                .ExposeConfiguration(ValidatorConfiguration.Configure)
                .ExposeConfiguration(IndexForeignKeyConfiguration.Configure)
                .ExposeConfiguration(SchemaConfiguration.Configure)
                .ExposeConfiguration(SessionContextConfiguration.Configure)
                //.ExposeConfiguration(x => x.SetProperty("adonet.batch_size", "15"))
                .BuildSessionFactory();
        }
    }
}
