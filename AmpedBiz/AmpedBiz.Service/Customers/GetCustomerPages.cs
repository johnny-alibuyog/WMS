using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Service.Common;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Customers
{
    public class GetCustomerPages
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.CustomerPageItem> { }

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
                    var query = session.Query<Entity.Customer>();

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

                    // compose sort
                    message.Sorter.Compose("name", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.CustomerPageItem()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CreditLimit = x.CreditLimit.ToStringWithSymbol(),
                            PricingSchemeName = x.PricingScheme.Name,
                            Contact = Mapper.Map<Entity.Contact, Dto.Contact>(x.Contact),
                            OfficeAddress = Mapper.Map<Entity.Address, Dto.Address>(x.OfficeAddress),
                            BillingAddress = Mapper.Map<Entity.Address, Dto.Address>(x.BillingAddress),
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
