using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturnList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }
        }

        public class Response : List<Dto.Return>
        {
            public Response() { }

            public Response(List<Dto.Return> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.QueryOver<Return>()
                        .Fetch(x => x.Branch).Eager
                        .Fetch(x => x.Customer).Eager
                        .Fetch(x => x.ReturnedBy).Eager
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Inventory).Eager
                        .Fetch(x => x.Items.First().ReturnReason).Eager
                        .List();

                    var dtos = entites.MapTo(default(List<Dto.Return>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
