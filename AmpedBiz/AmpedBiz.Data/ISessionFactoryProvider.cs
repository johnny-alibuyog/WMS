using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace AmpedBiz.Data
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory();
        SessionFactoryProvider WithBatcher(Action<Configuration> configurer);
        SessionFactoryProvider WithCache(Action<Configuration> configurer);
        SessionFactoryProvider WithDatabase(IPersistenceConfigurer configurer);
        SessionFactoryProvider WithEventListener(Action<Configuration> configurer);
        SessionFactoryProvider WithIndexForeignKey(Action<Configuration> configurer);
        SessionFactoryProvider WithMappings(Action<MappingConfiguration> configurer);
        SessionFactoryProvider WithSchema(Action<Configuration> configurer);
        SessionFactoryProvider WithSessionContext(Action<Configuration> configurer);
        SessionFactoryProvider WithValidator(Action<Configuration> configurer);
    }
}