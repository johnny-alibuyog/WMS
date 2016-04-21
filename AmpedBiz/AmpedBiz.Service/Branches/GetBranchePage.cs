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
        public class Request : PageRequest, IRequest<Response>
        {

        }

        public class Response : PageResponse<Dto.Branch>
        {

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

                //using (var session = _sessionFactory.OpenSession())
                //using (var transaction = session.BeginTransaction())
                //{
                //    var query = session.Query<Entity.Branch>();

                //    if (message.Ids != null && message.Ids.Count() > 0)
                //        query = query.Where(x => message.Ids.Contains(x.Id));

                //    if (message.Name != null)
                //        query = query.Where(x => x.Name.StartsWith(message.Name));

                //    if (message.Description != null)
                //        query = query.Where(x => x.Name.StartsWith(message.Description));

                //    var selectQuery = query
                //        .Select(x => new Dto.Branch()
                //        {
                //            Id = x.Id,
                //            Name = x.Name,
                //            Description = x.Description
                //        });

                //    switch (message.Sort.Field)
                //    {
                //        case "Name":
                //            selectQuery = message.Sort.Direction == SortDirection.Ascending
                //                ? selectQuery.OrderBy(x => x.Name)
                //                : selectQuery.OrderByDescending(x => x.Name);
                //            break;
                //        case "Description":
                //            selectQuery = message.Sort.Direction == SortDirection.Ascending
                //                ? selectQuery.OrderBy(x => x.Description)
                //                : selectQuery.OrderByDescending(x => x.Description);
                //            break;
                //        default:
                //            selectQuery = message.Sort.Direction == SortDirection.Ascending
                //                ? selectQuery.OrderBy(x => x.Name)
                //                : selectQuery.OrderByDescending(x => x.Name);
                //            break;
                //    }

                //    var itemsFuture = selectQuery
                //        .Skip(message.Pager.SkipCount)
                //        .Take(message.Pager.Size)
                //        .ToFuture();

                //    var countFuture = selectQuery
                //        .ToFutureValue(x => x.Count());

                //    response = new Response()
                //    {
                //        Count = countFuture.Value,
                //        Items = itemsFuture.ToList()
                //    };
                //}

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Entity.Branch>();

                    // apply filters
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

                    // apply sort
                    message.Sorter.Compose("name", direction =>
                    {
                        selectQuery = direction == SortDirection.Ascending
                            ? selectQuery.OrderBy(x => x.Name)
                            : selectQuery.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("description", direction =>
                    {
                        selectQuery = direction == SortDirection.Ascending
                            ? selectQuery.OrderBy(x => x.Name)
                            : selectQuery.OrderByDescending(x => x.Name);
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
