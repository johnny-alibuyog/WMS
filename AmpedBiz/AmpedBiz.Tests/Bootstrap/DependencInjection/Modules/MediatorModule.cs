using AmpedBiz.Service.PaymentTypes;
using Autofac;
using Autofac.Features.Variance;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;

namespace AmpedBiz.Tests.Bootstrap.DependencInjection.Modules
{
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(GetPaymentType).Assembly)
                .AsImplementedInterfaces()
                .PropertiesAutowired();

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

        //protected override void Load(ContainerBuilder builder)
        //{
        //    builder.RegisterSource(new ContravariantRegistrationSource());

        //    builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
        //        .AsImplementedInterfaces()
        //        .PropertiesAutowired();

        //    var mediatrOpenTypes = new[]
        //    {
        //        typeof(IRequestHandler<,>),
        //        typeof(IRequestHandler<>),
        //        typeof(INotificationHandler<>),
        //    };

        //    foreach (var mediatrOpenType in mediatrOpenTypes)
        //    {
        //        builder
        //            .RegisterAssemblyTypes(typeof(GetPaymentType).Assembly)
        //            .AsClosedTypesOf(mediatrOpenType)
        //            .AsImplementedInterfaces()
        //            .PropertiesAutowired();
        //    }

        //    // It appears Autofac returns the last registered types first
        //    builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>))
        //        .As(typeof(IPipelineBehavior<,>))
        //        .PropertiesAutowired();

        //    builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>))
        //        .As(typeof(IPipelineBehavior<,>))
        //        .PropertiesAutowired();

        //    //builder.RegisterGeneric(typeof(GenericRequestPreProcessor<>)).As(typeof(IRequestPreProcessor<>));

        //    builder.RegisterGeneric(typeof(RequestPostProcessorBase<,>))
        //        .As(typeof(IRequestPostProcessor<,>))
        //        .PropertiesAutowired();

        //    //builder.RegisterGeneric(typeof(GenericPipelineBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        //    //builder.RegisterGeneric(typeof(ConstrainedRequestPostProcessor<,>)).As(typeof(IRequestPostProcessor<,>));

        //    //builder.RegisterGeneric(typeof(ConstrainedPingedHandler<>)).As(typeof(INotificationHandler<>));

        //    builder.Register<SingleInstanceFactory>(context =>
        //    {
        //        var c = context.Resolve<IComponentContext>();
        //        return t => c.Resolve(t);
        //    });

        //    builder.Register<MultiInstanceFactory>(context =>
        //    {
        //        var c = context.Resolve<IComponentContext>();
        //        return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
        //    });

        //    //var container = builder.Build();

        //    // The below returns:
        //    //  - RequestPreProcessorBehavior
        //    //  - RequestPostProcessorBehavior
        //    //  - GenericPipelineBehavior

        //    //var behaviors = container
        //    //    .Resolve<IEnumerable<IPipelineBehavior<Ping, Pong>>>()
        //    //    .ToList();

        //    //var mediator = container.Resolve<IMediator>();

        //    //return mediator;
        //}
    }
}