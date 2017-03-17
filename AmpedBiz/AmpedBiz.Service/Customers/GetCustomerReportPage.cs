using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
    public class GetCustomerReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.CustomerReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Customer>();

                    // compose order
                    message.Sorter.Compose("customerName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("creditLimitAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.CreditLimit.Amount)
                            : query.OrderByDescending(x => x.CreditLimit.Amount);
                    });

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new Dto.CustomerReportPageItem()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Contact = new Dto.Contact()
                            {
                                Email = x.Contact.Email,
                                Landline = x.Contact.Landline,
                                Fax = x.Contact.Fax,
                                Mobile = x.Contact.Mobile,
                                Web = x.Contact.Web
                            },
                            OfficeAddress = new Dto.Address()
                            {
                                Street = x.OfficeAddress.Street,
                                Barangay = x.OfficeAddress.Barangay,
                                City = x.OfficeAddress.City,
                                Province = x.OfficeAddress.Province,
                                Region = x.OfficeAddress.Region,
                                Country = x.OfficeAddress.Country,
                                ZipCode = x.OfficeAddress.ZipCode,
                            },
                            BillingAddress = new Dto.Address()
                            {
                                Street = x.BillingAddress.Street,
                                Barangay = x.BillingAddress.Barangay,
                                City = x.BillingAddress.City,
                                Province = x.BillingAddress.Province,
                                Region = x.BillingAddress.Region,
                                Country = x.BillingAddress.Country,
                                ZipCode = x.BillingAddress.ZipCode,
                            }
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
