using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Conventions;
using AmpedBiz.Data.EntityDefinitions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Data
{
    public class SessionFactoryProvider1 : ISessionFactoryProvider
    {
        internal static ValidatorEngine Validator { get; private set; }

        internal static IAuditProvider AuditProvider { get; private set; }

        internal static ISessionFactory SessionFactory { get; private set; }

        public virtual ISessionFactory GetSessionFactory() => SessionFactoryProvider1.SessionFactory;

        public SessionFactoryProvider1(ValidatorEngine validator, IAuditProvider auditProvider)
        {
            SessionFactoryProvider1.Validator = validator;

            SessionFactoryProvider1.AuditProvider = auditProvider;

            SessionFactoryProvider1.SessionFactory = Fluently.Configure()
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
                    .Conventions.AddFromAssemblyOf<CustomJoinedSubclassConvention>()
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
                .BuildSessionFactory();
        }
    }

   
}
