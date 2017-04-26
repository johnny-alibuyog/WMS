using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetNeedsReorderingPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.NeedsReorderingPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Product>()
                        .Where(x => x.Inventory.CurrentLevel.Value <= x.Inventory.ReorderLevel.Value);

                    // compose filters
                    message.Filter.Compose<string>("supplierId", value =>
                    {
                        query = query.Where(x => x.Supplier.Id.ToString() == value);
                    });

                    // compose sort
                    message.Sorter.Compose("code", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Code)
                            : query.OrderByDescending(x => x.Code);
                    });

                    message.Sorter.Compose("productName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("categoryName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Category.Name)
                            : query.OrderByDescending(x => x.Category.Name);
                    });

                    message.Sorter.Compose("reorderLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.ReorderLevel.Value)
                            : query.OrderByDescending(x => x.Inventory.ReorderLevel.Value);
                    });

                    message.Sorter.Compose("available", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.Available.Value)
                            : query.OrderByDescending(x => x.Inventory.Available.Value);
                    });

                    message.Sorter.Compose("currentLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.CurrentLevel.Value)
                            : query.OrderByDescending(x => x.Inventory.CurrentLevel.Value);
                    });

                    message.Sorter.Compose("targetLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.TargetLevel.Value)
                            : query.OrderByDescending(x => x.Inventory.TargetLevel.Value);
                    });

                    message.Sorter.Compose("belowTarget", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.BelowTargetLevel.Value)
                            : query.OrderByDescending(x => x.Inventory.BelowTargetLevel.Value);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.NeedsReorderingPageItem()
                        {
                            Id = x.Id.ToString(),
                            ProductCode = x.Code,
                            ProductName = x.Name,
                            SupplierName = x.Supplier.Name,
                            CategoryName = x.Category.Name,
                            ReorderLevelValue = x.Inventory.ReorderLevel.Value,
                            AvailableValue = x.Inventory.Available.Value,
                            CurrentLevelValue = x.Inventory.CurrentLevel.Value,
                            TargetLevelValue = x.Inventory.TargetLevel.Value,
                            BelowTargetValue = x.Inventory.BelowTargetLevel.Value
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
