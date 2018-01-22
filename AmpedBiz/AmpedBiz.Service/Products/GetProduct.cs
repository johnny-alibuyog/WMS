using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Transform;
using System;
using System.Linq;

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
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<Product>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Supplier).Eager
                        .Fetch(x => x.Category).Eager
                        .Fetch(x => x.Inventory).Eager
                        .Fetch(x => x.UnitOfMeasures).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices).Eager
                        .Fetch(x => x.Inventory.Stocks).Eager
                        .Fetch(x => x.Inventory.Stocks.First().ModifiedBy).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .FutureValue();

                    var entity = query.Value;
                    entity.EnsureExistence($"Product with id {message.Id} does not exists.");
                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}