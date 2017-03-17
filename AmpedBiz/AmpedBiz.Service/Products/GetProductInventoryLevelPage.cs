using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventoryLevelPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductInventoryLevelPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Product>();

                    // compose filters
                    message.Filter.Compose<string>("code", value =>
                    {
                        query = query.Where(x => x.Code.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("name", value =>
                    {
                        query = query.Where(x => x.Name.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("supplier", value =>
                    {
                        query = query.Where(x => x.Supplier.Name.ToLower().Contains(value.ToLower()));
                    });

                    message.Filter.Compose<string>("category", value =>
                    {
                        query = query.Where(x => x.Category.Name.ToLower().Contains(value.ToLower()));
                    });

                    // compose sort
                    message.Sorter.Compose("code", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Code)
                            : query.OrderByDescending(x => x.Code);
                    });

                    message.Sorter.Compose("name", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                    });

                    message.Sorter.Compose("supplierName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Supplier.Name)
                            : query.OrderByDescending(x => x.Supplier.Name);
                    });

                    message.Sorter.Compose("unitOfMeasure", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.UnitOfMeasure.Name)
                            : query.OrderByDescending(x => x.Inventory.UnitOfMeasure.Name);
                    });

                    message.Sorter.Compose("onHand", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.OnHand.Value)
                            : query.OrderByDescending(x => x.Inventory.OnHand.Value);
                    });

                    message.Sorter.Compose("allocated", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.Allocated.Value)
                            : query.OrderByDescending(x => x.Inventory.Allocated.Value);
                    });

                    message.Sorter.Compose("available", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.Available.Value)
                            : query.OrderByDescending(x => x.Inventory.Available.Value);
                    });

                    message.Sorter.Compose("onOrder", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.OnOrder.Value)
                            : query.OrderByDescending(x => x.Inventory.OnOrder.Value);
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

                    message.Sorter.Compose("belowTargetLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Inventory.BelowTargetLevel.Value)
                            : query.OrderByDescending(x => x.Inventory.BelowTargetLevel.Value);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.ProductInventoryLevelPageItem()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Supplier = x.Supplier.Name,
                            UnitOfMeasure = x.Inventory.UnitOfMeasure.Name,
                            OnHandValue = x.Inventory.OnHand.Value,
                            AllocatedValue = x.Inventory.Allocated.Value,
                            AvailableValue = x.Inventory.Available.Value,
                            OnOrderValue = x.Inventory.OnOrder.Value,
                            CurrentLevelValue = x.Inventory.CurrentLevel.Value,
                            TargetLevelValue = x.Inventory.TargetLevel.Value,
                            BelowTargetLevelValue = x.Inventory.BelowTargetLevel.Value,
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
