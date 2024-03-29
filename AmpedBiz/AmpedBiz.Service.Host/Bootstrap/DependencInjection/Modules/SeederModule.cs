﻿using AmpedBiz.Data.Seeders;
using Autofac;
using System;
using System.Linq;

namespace AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules
{
    public class SeederModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ISeeder).Assembly)
                .Where(type => type.IsAssignableTo<ISeeder>())
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<Runner>().AsSelf();
        }
    }
}