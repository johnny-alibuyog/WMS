using System;
using AmpedBiz.Common.Configurations;
using AmpedBiz.Data;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Host.Plugins.Providers;
using AmpedBiz.Service.Host.Properties;
using Autofac;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Service.Host.Plugins.DependencInjection.Modules
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuditProvider>()
                .As<IAuditProvider>();

            builder.RegisterType<ValidatorEngine>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<SessionProvider>()
                .As<ISessionProvider>()
                .SingleInstance();

            builder.Register(context => context.Resolve<ISessionProvider>().SessionFactory)
                .SingleInstance();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(type => type.IsAssignableTo<ISeeder>())
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}