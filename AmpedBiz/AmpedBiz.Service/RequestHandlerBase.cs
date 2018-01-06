using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ISessionFactory _sessionFactory;

        protected readonly IContext _context;

        //protected ISession session;

        //protected ITransaction transaction;

        //protected readonly Lazy<User> User = new Lazy<User>(() => );


        public abstract TResponse Handle(TRequest message);

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

        public RequestHandlerBase(ISessionFactory sessionFactory, IContext context)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }
    }
}
