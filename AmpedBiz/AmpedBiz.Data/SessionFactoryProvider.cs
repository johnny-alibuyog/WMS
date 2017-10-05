using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Validator.Engine;
using System;

namespace AmpedBiz.Data
{
    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        private readonly ConfigurationContainer _configuration;

        public static Func<IContext> GetContext { get; private set; }

        public static ValidatorEngine Validator { get; private set; }

        public static IAuditProvider AuditProvider { get; private set; }

        public static ISessionFactory SessionFactory { get; private set; }

        public ISessionFactory GetSessionFactory()
        {
            lock (this._configuration)
            {
                if (SessionFactoryProvider.SessionFactory == null)
                {
                    SessionFactoryProvider.SessionFactory = Fluently.Configure()
                        .Database(this._configuration.Database)
                        .Mappings(this._configuration.Mappings)
                        .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                        .ExposeConfiguration(this._configuration.Batcher)
                        .ExposeConfiguration(this._configuration.EventListener)
                        .ExposeConfiguration(this._configuration.Cache)
                        .ExposeConfiguration(this._configuration.Validator)
                        .ExposeConfiguration(this._configuration.IndexForeignKey)
                        .ExposeConfiguration(this._configuration.Schema)
                        .ExposeConfiguration(this._configuration.SessionContext)
                        .ExposeConfiguration(this._configuration.Interceptor)
                        .BuildSessionFactory();
                }
            }
            return SessionFactoryProvider.SessionFactory;
        }

        public SessionFactoryProvider(ValidatorEngine validator, IAuditProvider auditProvider, Func<IContext> getContext = null)
        {
            SessionFactoryProvider.Validator = validator;
            SessionFactoryProvider.AuditProvider = auditProvider;
            SessionFactoryProvider.GetContext = getContext;
            _configuration = new ConfigurationContainer();
        }

        public SessionFactoryProvider WithDatabase(IPersistenceConfigurer configurer)
        {
            this._configuration.Database = configurer;
            return this;
        }

        public SessionFactoryProvider WithMappings(Action<MappingConfiguration> configurer)
        {
            this._configuration.Mappings = configurer;
            return this;
        }

        public SessionFactoryProvider WithBatcher(Action<Configuration> configurer)
        {
            this._configuration.Batcher = configurer;
            return this;
        }

        public SessionFactoryProvider WithEventListener(Action<Configuration> configurer)
        {
            this._configuration.EventListener = configurer;
            return this;
        }

        public SessionFactoryProvider WithCache(Action<Configuration> configurer)
        {
            this._configuration.Cache = configurer;
            return this;
        }

        public SessionFactoryProvider WithValidator(Action<Configuration> configurer)
        {
            this._configuration.Validator = configurer;
            return this;
        }

        public SessionFactoryProvider WithIndexForeignKey(Action<Configuration> configurer)
        {
            this._configuration.IndexForeignKey = configurer;
            return this;
        }

        public SessionFactoryProvider WithSchema(Action<Configuration> configurer)
        {
            this._configuration.Schema = configurer;
            return this;
        }

        public SessionFactoryProvider WithSessionContext(Action<Configuration> configurer)
        {
            this._configuration.SessionContext = configurer;
            return this;
        }

        public SessionFactoryProvider WithInterceptors(Action<Configuration> configurer)
        {
            this._configuration.Interceptor = configurer;
            return this;
        }

        private class ConfigurationContainer
        {
            public IPersistenceConfigurer Database { get; set; } = DatabaseConfiguration.Configure();

            public Action<MappingConfiguration> Mappings { get; set; } = MappingDefinitionConfiguration.Configure;

            public Action<Configuration> Batcher { get; set; } = BatcherConfiguration.Configure;

            public Action<Configuration> EventListener { get; set; } = EventListenerConfiguration.Configure;

            public Action<Configuration> Cache { get; set; } = CacheConfiguration.Configure;

            public Action<Configuration> Validator { get; set; } = ValidatorConfiguration.Configure;

            public Action<Configuration> IndexForeignKey { get; set; } = IndexForeignKeyConfiguration.Configure;

            public Action<Configuration> Schema { get; set; } = SchemaConfiguration.Configure;

            public Action<Configuration> SessionContext { get; set; } = SessionContextConfiguration.Configure;

            public Action<Configuration> Interceptor { get; set; } = InterceptorConfiguration.Configure;
        }
    }
}
