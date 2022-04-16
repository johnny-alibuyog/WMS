using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUserSalesReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.UserSalesReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var userAlias = default(User);
                    var resultAlias = default(Dto.UserSalesReportPageItem);

                    var salesAlias = default(PointOfSale);
                    var salesSub = QueryOver.Of(() => salesAlias);

                    var returnAlias = default(Return);
                    var returnsSub = QueryOver.Of(() => returnAlias);

                    message.Filter.Compose<DateTime>("fromDate", value =>
                    {
                        salesSub = salesSub.Where(() => salesAlias.TenderedOn >= value.StartOfDay());
                        returnsSub = returnsSub.Where(() => returnAlias.ReturnedOn >= value.StartOfDay());
                    });

                    message.Filter.Compose<DateTime>("toDate", value =>
                    {
                        salesSub = salesSub.Where(() => salesAlias.TenderedOn <= value.EndOfDay());
                        returnsSub = returnsSub.Where(() => returnAlias.ReturnedOn <= value.EndOfDay());
                    });

                    /* http://www.andrewwhitaker.com/blog/2014/10/24/queryover-series-part-8-working-with-subqueries/ */
                    /* https://stackoverflow.com/questions/26893475/how-to-partially-project-a-child-object-with-many-fields-in-nhibernate */

                    var query = session.QueryOver(() => userAlias)
                        .SelectList(list => list
                            /* FirstName */
                            .Select(() => userAlias.Person.FirstName)
                                .WithAlias(() => resultAlias.FirstName)

                            /* LastName */
                            .Select(() => userAlias.Person.LastName)
                                .WithAlias(() => resultAlias.LastName)

                            /* Sales */
                            .SelectSubQuery(salesSub
                                .Where(() => salesAlias.TenderedBy.Id == userAlias.Id)
                                .Select(Projections.Sum(() => salesAlias.Paid.Amount))
                            )
                                .WithAlias(() => resultAlias.SalesAmount)

                            /* Returns */
                            .SelectSubQuery(returnsSub
                                .Where(() => returnAlias.ReturnedBy.Id == userAlias.Id)
                                .Select(Projections.Sum(() => returnAlias.TotalReturned.Amount))
                            )
                                .WithAlias(() => resultAlias.SalesAmount)
                        )
                        .TransformUsing(Transformers.AliasToBean<Dto.UserSalesReportPageItem>());

                    var itemsQuery = query;

                    message.Sorter.Compose("userFullname", direction =>
                    {
                        itemsQuery = direction == SortDirection.Ascending
                            ? itemsQuery
                                .OrderBy(() => resultAlias.FirstName).Asc
                                .OrderBy(() => resultAlias.LastName).Asc
                            : itemsQuery
                                .OrderBy(() => resultAlias.FirstName).Desc
                                .OrderBy(() => resultAlias.LastName).Desc;
                    });

                    message.Sorter.Compose("salesAmount", direction =>
                    {
                        itemsQuery = direction == SortDirection.Ascending
                            ? itemsQuery.OrderBy(() => resultAlias.SalesAmount).Asc
                            : itemsQuery.OrderBy(() => resultAlias.SalesAmount).Desc;
                    });

                    message.Sorter.Compose("returnsAmount", direction =>
                    {
                        itemsQuery = direction == SortDirection.Ascending
                            ? itemsQuery.OrderBy(() => resultAlias.ReturnsAmount).Asc
                            : itemsQuery.OrderBy(() => resultAlias.ReturnsAmount).Desc;
                    });

                    var countFuture = query
                        .ToRowCountQuery()
                        .FutureValue<int>();

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = itemsQuery
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .Future<Dto.UserSalesReportPageItem>();

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
