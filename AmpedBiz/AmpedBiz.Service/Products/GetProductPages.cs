using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Service.Common;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Products
{
    public class GetProductPages
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductPageItem> { }

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
                    var query = session.Query<Entity.Product>();

                    // compose filters
                    message.Filter.Compose<string>("code", value =>
                    {
                        query = query.Where(x => x.Id.StartsWith(value));
                    });

                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x => x.Name.StartsWith(value));
                    });

                    message.Filter.Compose<string>("description", value =>
                    {
                        query = query.Where(x => x.Description.StartsWith(value));
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

                    message.Sorter.Compose("description", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Description)
                            : query.OrderByDescending(x => x.Description);
                    });

                    message.Sorter.Compose("supplier", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Supplier.Name)
                            : query.OrderByDescending(x => x.Supplier.Name);
                    });

                    message.Sorter.Compose("category", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Category.Name)
                            : query.OrderByDescending(x => x.Category.Name);
                    });

                    message.Sorter.Compose("basePrice", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.BasePrice.Amount)
                            : query.OrderByDescending(x => x.BasePrice.Amount);
                    });

                    message.Sorter.Compose("retailPrice", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.RetailPrice.Amount)
                            : query.OrderByDescending(x => x.RetailPrice.Amount);
                    });

                    message.Sorter.Compose("wholeSalePrice", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.WholesalePrice.Amount)
                            : query.OrderByDescending(x => x.WholesalePrice.Amount);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.ProductPageItem()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            SupplierName = x.Supplier.Name,
                            CategoryName = x.Category.Name,
                            Image = x.Image,
                            BasePrice = x.BasePrice.ToStringWithSymbol(),
                            RetailPrice = x.RetailPrice.ToStringWithSymbol(),
                            WholesalePrice = x.WholesalePrice.ToStringWithSymbol(),
                            Discontinued = x.Discontinued
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
