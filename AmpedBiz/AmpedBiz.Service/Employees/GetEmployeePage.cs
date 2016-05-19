﻿using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Service.Common;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Employees
{
    public class GetEmployeePage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.EmployeePageItem> { }

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
                    var query = session.Query<Entity.Employee>();

                    // compose filters
                    message.Filter.Compose<string>("code", value =>
                    {
                        query = query.Where(x => x.Id.StartsWith(value));
                    });

                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x =>
                            x.User.Person.FirstName.StartsWith(value) ||
                            x.User.Person.MiddleName.StartsWith(value) ||
                            x.User.Person.LastName.StartsWith(value));
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
                            ? query.OrderBy(x => x.User.Person.FirstName)
                            : query.OrderByDescending(x => x.User.Person.FirstName);
                    });

                    message.Sorter.Compose("lastname", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.User.Person.LastName)
                            : query.OrderByDescending(x => x.User.Person.LastName);
                    });

                    message.Sorter.Compose("middlename", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.User.Person.MiddleName)
                            : query.OrderByDescending(x => x.User.Person.MiddleName);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.EmployeePageItem()
                        {
                            Id = x.Id,
                            EmployeeTypeName = x.EmployeeType.Name,
                            FirstName = x.User.Person.FirstName,
                            LastName = x.User.Person.LastName,
                            MiddleName = x.User.Person.MiddleName,
                            Contact = Mapper.Map<Entity.Contact, Dto.Contact>(x.Contact)
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