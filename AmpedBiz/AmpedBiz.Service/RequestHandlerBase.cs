﻿using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace AmpedBiz.Service
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        // dependency of properties will be injected via property injection (Autofac.PropertiesAutowired)

        public ISessionFactory SessionFactory { get; set; }

        public IContext Context { get; set; }

        //protected ISession session;

        //protected ITransaction transaction;

        //protected readonly Lazy<User> User = new Lazy<User>(() => );

        public abstract TResponse Execute(TRequest request);

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Execute(request));
        }

        //public TResponse Handle(TRequest message)
        //{
        //    return this.Execute(message);
        //}

        //public TResponse Handle(TRequest message)
        //{
        //    var response = default(TResponse);

        //    using (session = _sessionFactory.RetrieveSharedSession(_context))
        //    using (transaction = session.BeginTransaction())
        //    {
        //        response = this.HandleMessage(message);

        //        transaction.Commit();
        //    }

        //    return response;
        //}

        //public RequestHandlerBase() { }

        //public RequestHandlerBase(ISessionFactory sessionFactory, IContext context)
        //{
        //    this.context = context;
        //    this.sessionFactory = sessionFactory;
        //}
    }

    public static class RequestHandlerExtention
    {
        public static RequestHandlerBase<TRequest, TResponse> With<TRequest, TResponse>(
            this RequestHandlerBase<TRequest, TResponse> handler, 
            ISessionFactory sessionFactory, 
            IContext context) 
            where TRequest : IRequest<TResponse>
        {
            handler.SessionFactory = sessionFactory;
            handler.Context = context;
            return handler;
        }
    } 
}
