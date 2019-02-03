using AmpedBiz.Service.Middlewares;
using AmpedBiz.Service.PaymentTypes;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using System.Collections.Generic;

namespace AmpedBiz.Data.Initializer.Bootstrap.DependencInjection.Modules
{
    public class MediatorModule : Module
    {
        //protected override void Load(ContainerBuilder builder)
        //{
        //    builder.RegisterSource(new ContravariantRegistrationSource());

        //    builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
        //        .AsImplementedInterfaces()
        //        .PropertiesAutowired();

        //    builder.RegisterAssemblyTypes(typeof(GetPaymentType).Assembly)
        //        .AsImplementedInterfaces()
        //        .PropertiesAutowired();

        //    builder.RegisterInstance(Console.Out).As<TextWriter>();

        //    builder.Register<SingleInstanceFactory>(context =>
        //    {
        //        var c = context.Resolve<IComponentContext>();
        //        return type => c.Resolve(type);
        //    });

        //    builder.Register<MultiInstanceFactory>(context =>
        //    {
        //        var componentContext = context.Resolve<IComponentContext>();
        //        return type => (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(type));
        //    });
        //}

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestHandler<>),
                typeof(INotificationHandler<>),
                typeof(IPipelineBehavior<,>),
                typeof(IRequestPreProcessor<>),
                typeof(IRequestPostProcessor<,>),
                typeof(IPipelineBehavior<,>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(GetPaymentType).Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces()
                    .PropertiesAutowired();
            }

            // It appears Autofac returns the last registered types first
            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>))
                .PropertiesAutowired();

            builder.RegisterGeneric(typeof(LogginPipeline<,>))
                .As(typeof(IPipelineBehavior<,>))
                .PropertiesAutowired();

            builder.RegisterGeneric(typeof(TransactionPipeline<,>))
                .As(typeof(IPipelineBehavior<,>))
                .PropertiesAutowired();

            builder
              .RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            //builder.Register<SingleInstanceFactory>(context =>
            //{
            //    var c = context.Resolve<IComponentContext>();
            //    return t => c.Resolve(t);
            //});

            //builder.Register<MultiInstanceFactory>(context =>
            //{
            //    var c = context.Resolve<IComponentContext>();
            //    return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            //});
        }
    }
}