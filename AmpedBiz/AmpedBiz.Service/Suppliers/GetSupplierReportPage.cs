using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Suppliers
{
    public class GetSupplierReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.SupplierReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Supplier>();

                    // compose order
                    message.Sorter.Compose("supplierName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new Dto.SupplierReportPageItem()
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
                            Address = new Dto.Address()
                            {
                                Street = x.Address.Street,
                                Barangay = x.Address.Barangay,
                                City = x.Address.City,
                                Province = x.Address.Province,
                                Region = x.Address.Region,
                                Country = x.Address.Country,
                                ZipCode = x.Address.ZipCode,
                            },
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
