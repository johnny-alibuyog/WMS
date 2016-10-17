using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class GetUnitOfMeasureClassReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.UnitOfMeasureClass> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<UnitOfMeasureClass>();

                    // compose filter
                    message.Filter.Compose<string>("unitOfMeasureClassId", value =>
                    {
                        query = query.Where(x => x.Id == value);
                    });

                    // compose order
                    message.Sorter.Compose("unitOfMeasureClassName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    var items = query
                        .FetchMany(x => x.Units)
                        .ToList();

                    var dtos = items.MapTo(default(List<Dto.UnitOfMeasureClass>));

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(dtos.Count);

                    var dtoPageItems = dtos
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToList();
                    
                    response = new Response()
                    {
                        Count = dtoPageItems.Count,
                        Items = dtoPageItems
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
