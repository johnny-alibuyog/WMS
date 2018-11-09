using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
	public class GetCustomerList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }
        }

        public class Response : List<Dto.Customer>
        {
            public Response() { }

            public Response(List<Dto.Customer> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<Customer>().Cacheable().ToList();

                    var dtos = entites.MapTo(default(List<Dto.Customer>));

                    response = new Response(dtos);

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}