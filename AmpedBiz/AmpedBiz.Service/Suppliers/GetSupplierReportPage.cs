using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
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
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
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

                    message.Sorter.Compose("contactPerson", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ContactPerson)
                            : query.OrderByDescending(x => x.ContactPerson);
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
                            ContactPerson = x.ContactPerson,
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
