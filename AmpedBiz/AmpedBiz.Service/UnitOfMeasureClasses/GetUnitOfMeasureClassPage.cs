﻿using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class GetUnitOfMeasureClassPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.UnitOfMeasureClassPageItem> { }

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

                    // compose filters
                    message.Filter.Compose<string>("code", value =>
                    {
                        query = query.Where(x => x.Id.StartsWith(value));
                    });

                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x => x.Name.StartsWith(value));
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

                    var itemsFuture = query
                        .Select(x => new Dto.UnitOfMeasureClassPageItem()
                        {
                            Id = x.Id,
                            Name = x.Name,
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
