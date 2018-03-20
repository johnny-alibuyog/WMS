using AmpedBiz.Data.Seeders;
using Autofac;
using System;
using System.Linq;

namespace AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules
{
    public class SeederModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(type => type.IsAssignableTo<ISeeder>())
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<Runner>().AsSelf();
        }
    }
}