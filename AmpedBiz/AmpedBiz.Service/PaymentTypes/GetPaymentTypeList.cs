using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

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

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<PaymentType>().Cacheable().ToList();
                    var dtos = entites.MapTo(default(List<Dto.PaymentType>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
