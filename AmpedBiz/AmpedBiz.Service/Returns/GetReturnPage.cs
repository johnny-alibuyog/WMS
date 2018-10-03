using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturnPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ReturnPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Return>();

                    // compose filters
                    message.Filter.Compose<string>("branch", value =>
                    {
                        query = query.Where(x => x.Branch.Id.ToString() == value);
                    });

                    message.Filter.Compose<Guid>("customer", value =>
                    {
                        query = query.Where(x => x.Customer.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("branch", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Branch.Name)
                            : query.OrderByDescending(x => x.Branch.Name);
                    });

                    message.Sorter.Compose("customer", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Customer.Name)
                            : query.OrderByDescending(x => x.Customer.Name);
                    });

                    message.Sorter.Compose("returnedBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.ReturnedBy.Person.FirstName)
                                .OrderBy(x => x.ReturnedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.ReturnedBy.Person.FirstName)
                                .OrderByDescending(x => x.ReturnedBy.Person.LastName);
                    });

                    message.Sorter.Compose("returnedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReturnedOn)
                            : query.OrderByDescending(x => x.ReturnedOn);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.TotalReturned.Amount)
                            : query.OrderByDescending(x => x.TotalReturned.Amount);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.ReturnPageItem()
                        {
                            Id = x.Id,
                            BranchName = x.Branch.Name,
                            CustomerName = x.Customer.Name,
                            ReturnedByName = 
                                x.ReturnedBy.Person.FirstName + " " + 
                                x.ReturnedBy.Person.LastName,
                            ReturnedOn = x.ReturnedOn,
                            Remarks = x.Remarks,
                            ReturnedAmount = x.TotalReturned.Amount
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

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
