using System;
using System.Collections.Generic;
using System.IO;
using AmpedBiz.Service.PaymentTypes;
using Autofac;
using Autofac.Features.Variance;
using MediatR;

namespace AmpedBiz.Service.Host.Plugins.DependencInjection.Modules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(GetPaymentType).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterInstance(Console.Out).As<TextWriter>();

            builder.Register<SingleInstanceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return type => c.Resolve(type);
            });

            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return type => (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(type));
            });
        }
    }
}