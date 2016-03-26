using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.ProductTypes
{
    public class CreatePaymentType
    {
        public class Request : Dto.PaymentType, IRequest<Response> { }

        public class Response : Dto.PaymentType { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<PaymentType>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Payment Type with id {message.Id} already exists.");

                    session.Save(new PaymentType()
                    {
                        Id = message.Id,
                        Name = message.Name
                    });

                    transaction.Commit();
                }

                return new Response()
                {
                    Id = message.Id,
                    Name = message.Id
                };
            }
        }
    }
}
