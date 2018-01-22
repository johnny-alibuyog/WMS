using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class GetUnitOfMeasureList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.UnitOfMeasure>
        {
            public Response() { }

            public Response(List<Dto.UnitOfMeasure> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var entities = session.Query<UnitOfMeasure>().Cacheable().ToList();
                    var dtos = entities.MapTo(default(List<Dto.UnitOfMeasure>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
