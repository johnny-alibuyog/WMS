using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.OrderReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Order>();

                    // compose filter
                    message.Filter.Compose<string>("customerId", value =>
                    {
                        query = query.Where(x => x.Customer.Id == value);
                    });

                    message.Filter.Compose<Guid>("branchId", value =>
                    {
                        query = query.Where(x => x.Branch.Id == value);
                    });

                    message.Filter.Compose<string>("pricingSchemeId", value =>
                    {
                        query = query.Where(x => x.PricingScheme.Id == value);
                    });

                    message.Filter.Compose<OrderStatus>("status", value =>
                    {
                        query = query.Where(x => x.Status == value);
                    });

                    message.Filter.Compose<DateTime>("fromDate", value =>
                    {
                        query = query.Where(x => x.OrderedOn >= value);
                    });


                    message.Filter.Compose<DateTime>("toDate", value =>
                    {
                        query = query.Where(x => x.OrderedOn <= value);
                    });

                    // compose order
                    message.Sorter.Compose("branchName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Branch.Name)
                            : query.OrderByDescending(x => x.Branch.Name);
                    });

                    message.Sorter.Compose("customerName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Customer.Name)
                            : query.OrderByDescending(x => x.Customer.Name);
                    });

                    message.Sorter.Compose("pricingSchemeName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PricingScheme.Name)
                            : query.OrderByDescending(x => x.PricingScheme.Name);
                    });

                    message.Sorter.Compose("orderedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.OrderedOn)
                            : query.OrderByDescending(x => x.OrderedOn);
                    });

                    message.Sorter.Compose("orderedByName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x =>
                                x.OrderedBy.Person.FirstName + " " +
                                x.OrderedBy.Person.LastName
                            )
                            : query.OrderByDescending(x =>
                                x.OrderedBy.Person.FirstName + " " +
                                x.OrderedBy.Person.LastName
                            );
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Status)
                            : query.OrderByDescending(x => x.Status);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Total.Amount)
                            : query.OrderByDescending(x => x.Total.Amount);
                    });

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new Dto.OrderReportPageItem()
                        {
                            Id = x.Id,
                            BranchName = x.Branch.Name,
                            CustomerName = x.Customer.Name,
                            PricingSchemeName = x.PricingScheme.Name,
                            OrderedOn = x.OrderedOn,
                            OrderedByName = 
                                x.OrderedBy.Person.FirstName + " " +
                                x.OrderedBy.Person.LastName,
                            Status = x.Status.Parse<Dto.OrderStatus>(),
                            TotalAmount = x.Total.Amount
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
