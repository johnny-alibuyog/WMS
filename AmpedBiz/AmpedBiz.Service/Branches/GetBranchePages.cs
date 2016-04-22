using System.Linq;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Branches
{
    public class GetBranchePages
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.Branch> { }

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
                    var query = session.Query<Entity.Branch>();

                    // compose filters
                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x => x.Name.StartsWith(value));
                    });

                    message.Filter.Compose<string>("description", value =>
                    {
                        query = query.Where(x => x.Description.StartsWith(value));
                    });

                    // select query
                    var selectQuery = query.Select(x => new Dto.Branch()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    });

                    // compose sort
                    message.Sorter.Compose("code", direction =>
                    {
                        selectQuery = direction == SortDirection.Ascending
                            ? selectQuery.OrderBy(x => x.Id)
                            : selectQuery.OrderByDescending(x => x.Id);
                    });

                    // compose sort
                    message.Sorter.Compose("name", direction =>
                    {
                        selectQuery = direction == SortDirection.Ascending
                            ? selectQuery.OrderBy(x => x.Name)
                            : selectQuery.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("description", direction =>
                    {
                        selectQuery = direction == SortDirection.Ascending
                            ? selectQuery.OrderBy(x => x.Description)
                            : selectQuery.OrderByDescending(x => x.Description);
                    });

                    var itemsFuture = selectQuery
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var countFuture = selectQuery
                        .ToFutureValue(x => x.Count());

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };
                }

                return response;
            }
        }
    }
}
