using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Employees
{
    public class GetEmployeePage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.EmployeePageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Employee>();

                    // compose filters
                    message.Filter.Compose<Guid>("code", value =>
                    {
                        query = query.Where(x => x.Id == value);
                    });

                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x =>
                            x.Person.FirstName.ToLower().Contains(value.ToLower()) ||
                            x.Person.MiddleName.ToLower().Contains(value.ToLower()) ||
                            x.Person.LastName.ToLower().Contains(value.ToLower()));
                    });

                    // compose sort
                    message.Sorter.Compose("code", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Id)
                            : query.OrderByDescending(x => x.Id);
                    });

                    // compose sort
                    message.Sorter.Compose("firstname", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Person.FirstName)
                            : query.OrderByDescending(x => x.Person.FirstName);
                    });

                    message.Sorter.Compose("lastname", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Person.LastName)
                            : query.OrderByDescending(x => x.Person.LastName);
                    });

                    message.Sorter.Compose("middlename", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Person.MiddleName)
                            : query.OrderByDescending(x => x.Person.MiddleName);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.EmployeePageItem()
                        {
                            Id = x.Id,
                            EmployeeTypeName = x.EmployeeType.Name,
                            FirstName = x.Person.FirstName,
                            LastName = x.Person.LastName,
                            MiddleName = x.Person.MiddleName,
                            Contact = x.Contact.MapTo(default(Dto.Contact))
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
