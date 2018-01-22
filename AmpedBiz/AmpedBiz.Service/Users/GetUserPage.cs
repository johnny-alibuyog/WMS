using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUserPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.UserPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<User>();

                    // compose filters
                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x => 
                            x.Username.ToLower().Contains(value.ToLower()) ||
                            x.Person.FirstName.ToLower().Contains(value.ToLower()) ||
                            x.Person.MiddleName.ToLower().Contains(value.ToLower()) ||
                            x.Person.LastName.ToLower().Contains(value.ToLower())
                        );
                    });

                    message.Filter.Compose<string>("username", value =>
                    {
                        query = query.Where(x => x.Username.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("firstName", value =>
                    {
                        query = query.Where(x => x.Person.FirstName.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("middleName", value =>
                    {
                        query = query.Where(x => x.Person.MiddleName.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("lastName", value =>
                    {
                        query = query.Where(x => x.Person.LastName.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("branchId", value =>
                    {
                        query = query.Where(x => x.Branch.Id.ToString() == value);
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
                            Person = x.Person.MapTo(default(Dto.Person)),
                            Address = x.Address.MapTo(default(Dto.Address)),
                            Roles = x.Roles
                                .Select(o => new Dto.Role()
                                {
                                    Id = o.Id,
                                    Name = o.Name
                                })
                                .ToList()
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
