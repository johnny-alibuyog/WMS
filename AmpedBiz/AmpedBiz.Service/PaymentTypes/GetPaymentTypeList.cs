using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.PaymentTypes
{
    public class GetPaymentTypeList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.PaymentType>
        {
            public Response() { }

            public Response(List<Dto.PaymentType> items) : base(items) { }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<PaymentType>()
                        .Select(x => new Dto.PaymentType()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToList();

                    response = new Response(entites);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
