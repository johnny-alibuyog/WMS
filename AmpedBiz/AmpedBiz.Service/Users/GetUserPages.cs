using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Service.Common;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;


namespace AmpedBiz.Service.Users
{
    public class GetUserPages
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.UserPageItem> { }

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
                    var query = session.Query<Entity.User>();

                    // compose filters
                    message.Filter.Compose<string>("username", value =>
                    {
                        query = query.Where(x => x.Username.StartsWith(value));
                    });

                    message.Filter.Compose<string>("firstName", value =>
                    {
                        query = query.Where(x => x.Person.FirstName.StartsWith(value));
                    });

                    message.Filter.Compose<string>("middleName", value =>
                    {
                        query = query.Where(x => x.Person.MiddleName.StartsWith(value));
                    });

                    message.Filter.Compose<string>("lastName", value =>
                    {
                        query = query.Where(x => x.Person.LastName.StartsWith(value));
                    });

                    message.Filter.Compose<string>("branchId", value =>
                    {
                        query = query.Where(x => x.Branch.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("username", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Username)
                            : query.OrderByDescending(x => x.Username);
                    });

                    message.Sorter.Compose("name", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Person.FirstName)
                                .ThenBy(x => x.Person.MiddleName)
                                .ThenBy(x => x.Person.LastName)

                            : query.OrderByDescending(x => x.Person.FirstName)
                                .ThenByDescending(x => x.Person.MiddleName)
                                .ThenByDescending(x => x.Person.LastName);
                    });

                    message.Sorter.Compose("middleName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Person.MiddleName)
                            : query.OrderByDescending(x => x.Person.MiddleName);
                    });

                    message.Sorter.Compose("lastName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Person.LastName)
                            : query.OrderByDescending(x => x.Person.LastName);
                    });

                    message.Sorter.Compose("branchName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Branch.Name)
                            : query.OrderByDescending(x => x.Branch.Name);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.UserPageItem()
                        {
                            Id = x.Id,
                            Username = x.Username,
                            BranchName = x.Branch.Name,
                            Person = Mapper.Map<Entity.Person, Dto.Person>(x.Person),
                            Address = Mapper.Map<Entity.Address, Dto.Address>(x.Address),
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
