using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using NHibernate.Transform;
using System;

namespace AmpedBiz.Service.Products
{
    public class GetProduct
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : Dto.Product { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<Product>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Supplier).Eager
                        .Fetch(x => x.Category).Eager
                        .Fetch(x => x.Inventory).Eager
                        .Fetch(x => x.Inventory.UnitOfMeasure).Eager
                        .Fetch(x => x.Inventory.UnitOfMeasureBase).Eager
                        .Fetch(x => x.Inventory.Stocks).Eager
                        .Fetch(x => x.Inventory.Stocks.First().ModifiedBy).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .FutureValue();

                    var entity = query.Value;
                    if (entity == null)
                        throw new BusinessException($"Product with id {message.Id} does not exists.");

                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}