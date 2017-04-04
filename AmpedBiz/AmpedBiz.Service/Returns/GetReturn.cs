using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturn
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }

            public Request() { }

            public Request(Guid id)
            {
                this.Id = id;
            }
        }

        public class Response : Dto.Return { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                if (message.Id == Guid.Empty)
                {
                    var entity = new Return();
                    entity.MapTo(response);

                    return response;
                }

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.QueryOver<Return>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Branch).Eager
                        .Fetch(x => x.Customer).Eager
                        .Fetch(x => x.ReturnedBy).Eager
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Inventory).Eager
                        .Fetch(x => x.Items.First().ReturnReason).Eager
                        .SingleOrDefault();

                    entity.EnsureExistence($"Returns with id {message.Id} does not exists.");
                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
