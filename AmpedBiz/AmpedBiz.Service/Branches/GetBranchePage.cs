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
    public class GetBranchePage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.BranchPageItem> { }

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

                    // compose sort
                    message.Sorter.Compose("code", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Id)
                            : query.OrderByDescending(x => x.Id);
                    });

                    message.Sorter.Compose("name", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("description", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Description)
                            : query.OrderByDescending(x => x.Description);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.BranchPageItem()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var countFuture = query
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
