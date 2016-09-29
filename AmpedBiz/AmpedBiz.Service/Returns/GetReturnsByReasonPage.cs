﻿using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturnsByReasonPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ReturnsByReasonPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<ReturnItem>()
                        .GroupBy(x => x.ReturnReason.Id)
                        .Select(x => new Dto.ReturnsByReasonPageItem()
                        {
                            Id = x.Key,
                            ReturnReasonName = x.Max(o => o.ReturnReason.Name),
                            TotalAmount = x.Sum(o => o.TotalPrice.Amount)
                        });

                    // compose filters
                    message.Filter.Compose<string>("returnReason", value =>
                    {
                        query = query.Where(x => x.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("product", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReturnReasonName)
                            : query.OrderByDescending(x => x.ReturnReasonName);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.TotalAmount)
                            : query.OrderByDescending(x => x.TotalAmount);
                    });

                    var itemsFuture = query
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