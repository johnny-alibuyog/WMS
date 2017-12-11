using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventoryLevelPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductInventoryLevelPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session
                        .Query<Product>()
                        .Select(x => new
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            SupplierName = x.Supplier.Name,
                        });

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
                        query = query.Where(x => x.SupplierName.ToLower().Contains(value.ToLower()));
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
                            ? query.OrderBy(x => x.SupplierName)
                            : query.OrderByDescending(x => x.SupplierName);
                    });

                    var idsFuture = query
                        .Select(x => x.Id)
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    var inventories = session.QueryOver<Inventory>()
                        .WhereRestrictionOn(x => x.Product.Id).IsIn(idsFuture.ToArray())
                        .Fetch(x => x.Product).Eager
                        .Fetch(x => x.Product.Supplier).Eager
                        .Fetch(x => x.Product.Inventory).Eager
                        .Fetch(x => x.Product.UnitOfMeasures).Eager
                        .Fetch(x => x.Product.UnitOfMeasures.First().UnitOfMeasure).Eager
                        .Future();

                    var Hydrate = new Func<Guid, Dto.ProductInventoryLevelPageItem>((productId) =>
                    {
                        var inventory = inventories.FirstOrDefault(o => o.Product.Id == productId);
                        var unitOfMeasure = inventory.Product.UnitOfMeasures.Default(o => o.UnitOfMeasure);
                        var item = new Dto.ProductInventoryLevelPageItem()
                        {
                            Id = inventory.Product.Id,
                            Code = inventory.Product.Code,
                            Name = inventory.Product.Name,
                            Supplier = inventory.Product.Supplier.Name,
                            OnHand = inventory.Product.Inventory
                                .BreakDown(i => i.OnHand)
                                .InterpretAsString(),
                            Allocated = inventory.Product.Inventory
                                .BreakDown(i => i.Allocated)
                                .InterpretAsString(),
                            Available = inventory.Product.Inventory
                                .BreakDown(i => i.Available)
                                .InterpretAsString(),
                            OnOrder = inventory.Product.Inventory
                                .BreakDown(i => i.OnOrder)
                                .InterpretAsString(),
                            CurrentLevel = inventory.Product.Inventory
                                .BreakDown(i => i.CurrentLevel)
                                .InterpretAsString(),
                            ReorderLevel = inventory.Product.Inventory
                                .BreakDown(i => i.ReorderLevel)
                                .InterpretAsString(),
                            TargetLevel = inventory.Product.Inventory
                                .BreakDown(i => i.TargetLevel)
                                .InterpretAsString(),
                            BelowTargetLevel = inventory.Product.Inventory
                                .BreakDown(i => i.BelowTargetLevel)
                                .InterpretAsString(),
                        };
                        return item;
                    });

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = idsFuture
                            .Select(x => Hydrate(x))
                            .ToList()
                    };
                }

                return response;
            }
        }

        //public class Handler : RequestHandlerBase<Request, Response>
        //{
        //    public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

        //    public override Response Handle(Request message)
        //    {
        //        var response = new Response();

        //        using (var session = _sessionFactory.RetrieveSharedSession(_context))
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            var query = session.Query<Product>()
        //                .Select(x => new Dto.ProductInventoryLevelPageItem()
        //                {
        //                    Id = x.Id,
        //                    Code = x.Code,
        //                    Name = x.Name,
        //                    Supplier = x.Supplier.Name,
        //                    UnitOfMeasure = session.Query<ProductUnitOfMeasure>()
        //                        .Where(o => o.IsStandard && o.Product.Id == x.Id)
        //                        .Select(o => o.UnitOfMeasure.Name)
        //                        .FirstOrDefault(),
        //                    OnHandValue = x.Inventory.OnHand.Value,
        //                    AllocatedValue = x.Inventory.Allocated.Value,
        //                    AvailableValue = x.Inventory.Available.Value,
        //                    OnOrderValue = x.Inventory.OnOrder.Value,
        //                    CurrentLevelValue = x.Inventory.CurrentLevel.Value,
        //                    ReorderLevelValue = x.Inventory.ReorderLevel.Value,
        //                    TargetLevelValue = x.Inventory.TargetLevel.Value,
        //                    BelowTargetLevelValue = x.Inventory.BelowTargetLevel.Value,
        //                });

        //            // compose filters
        //            message.Filter.Compose<string>("code", value =>
        //            {
        //                query = query.Where(x => x.Code.ToLower().Contains(value.ToLower()));
        //            });

        //            message.Filter.Compose<string>("name", value =>
        //            {
        //                query = query.Where(x => x.Name.ToLower().Contains(value.ToLower()));
        //            });

        //            message.Filter.Compose<string>("supplier", value =>
        //            {
        //                query = query.Where(x => x.Supplier.ToLower().Contains(value.ToLower()));
        //            });

        //            // compose sort
        //            message.Sorter.Compose("code", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.Code)
        //                    : query.OrderByDescending(x => x.Code);
        //            });

        //            message.Sorter.Compose("name", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.Name)
        //                    : query.OrderByDescending(x => x.Name);
        //            });

        //            message.Sorter.Compose("supplierName", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.Supplier)
        //                    : query.OrderByDescending(x => x.Supplier);
        //            });

        //            message.Sorter.Compose("unitOfMeasure", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.UnitOfMeasure)
        //                    : query.OrderByDescending(x => x.UnitOfMeasure);
        //            });

        //            message.Sorter.Compose("onHand", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.OnHandValue)
        //                    : query.OrderByDescending(x => x.OnHandValue);
        //            });

        //            message.Sorter.Compose("allocated", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.AllocatedValue)
        //                    : query.OrderByDescending(x => x.AllocatedValue);
        //            });

        //            message.Sorter.Compose("available", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.AvailableValue)
        //                    : query.OrderByDescending(x => x.AvailableValue);
        //            });

        //            message.Sorter.Compose("onOrder", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.OnOrderValue)
        //                    : query.OrderByDescending(x => x.OnOrderValue);
        //            });

        //            message.Sorter.Compose("reorderLevel", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.ReorderLevelValue)
        //                    : query.OrderByDescending(x => x.ReorderLevelValue);
        //            });

        //            message.Sorter.Compose("currentLevel", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.CurrentLevelValue)
        //                    : query.OrderByDescending(x => x.CurrentLevelValue);
        //            });

        //            message.Sorter.Compose("targetLevel", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.TargetLevelValue)
        //                    : query.OrderByDescending(x => x.TargetLevelValue);
        //            });

        //            message.Sorter.Compose("belowTargetLevel", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.BelowTargetLevelValue)
        //                    : query.OrderByDescending(x => x.BelowTargetLevelValue);
        //            });

        //            var itemsFuture = query

        //                .Skip(message.Pager.SkipCount)
        //                .Take(message.Pager.Size)
        //                .ToFuture();

        //            var countFuture = query
        //                .ToFutureValue(x => x.Count());

        //            response = new Response()
        //            {
        //                Count = countFuture.Value,
        //                Items = itemsFuture.ToList()
        //            };
        //        }

        //        return response;
        //    }
        //}

        //public class Handler : RequestHandlerBase<Request, Response>
        //{
        //    public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

        //    public override Response Handle(Request message)
        //    {
        //        var response = new Response();

        //        using (var session = _sessionFactory.RetrieveSharedSession(_context))
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            var query = session.Query<Product>()
        //                .Select(x => new
        //                {
        //                    Id = x.Id,
        //                    Code = x.Code,
        //                    Name = x.Name,
        //                    SupplierName = x.Supplier.Name,
        //                    UnitOfMeasureName = session.Query<ProductUnitOfMeasure>()
        //                        .Where(o => o.IsDefault && o.Product.Id == x.Id)
        //                        .Select(o => o.UnitOfMeasure.Name)
        //                        .FirstOrDefault()
        //                });

        //            // compose filters
        //            message.Filter.Compose<string>("code", value =>
        //            {
        //                query = query.Where(x => x.Code.ToLower().Contains(value.ToLower()));
        //            });

        //            message.Filter.Compose<string>("name", value =>
        //            {
        //                query = query.Where(x => x.Name.ToLower().Contains(value.ToLower()));
        //            });

        //            message.Filter.Compose<string>("supplier", value =>
        //            {
        //                query = query.Where(x => x.SupplierName.ToLower().Contains(value.ToLower()));
        //            });

        //            // compose sort
        //            message.Sorter.Compose("code", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.Code)
        //                    : query.OrderByDescending(x => x.Code);
        //            });

        //            message.Sorter.Compose("name", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.Name)
        //                    : query.OrderByDescending(x => x.Name);
        //            });

        //            message.Sorter.Compose("supplierName", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.SupplierName)
        //                    : query.OrderByDescending(x => x.SupplierName);
        //            });

        //            message.Sorter.Compose("unitOfMeasure", direction =>
        //            {
        //                query = direction == SortDirection.Ascending
        //                    ? query.OrderBy(x => x.UnitOfMeasureName)
        //                    : query.OrderByDescending(x => x.UnitOfMeasureName);
        //            });

        //            var idsFuture = query
        //                .Select(x => x.Id)
        //                .Skip(message.Pager.SkipCount)
        //                .Take(message.Pager.Size)
        //                .ToFuture();

        //            var countFuture = query
        //                .ToFutureValue(x => x.Count());

        //            var inventories = session.QueryOver<Inventory>()
        //                .WhereRestrictionOn(x => x.Product.Id).IsIn(idsFuture.ToArray())
        //                .Fetch(x => x.Product).Eager
        //                .Fetch(x => x.Product.Supplier).Eager
        //                .Fetch(x => x.Product.Inventory).Eager
        //                .Fetch(x => x.Product.UnitOfMeasures).Eager
        //                .Future();

        //            var Hydrate = new Func<Guid, Dto.ProductInventoryLevelPageItem>((productId) =>
        //            {
        //                var inventory = inventories.FirstOrDefault(o => o.Product.Id == productId);
        //                var unitOfMeasure = inventory.Product.UnitOfMeasures.Default(o => o.UnitOfMeasure);
        //                var item = new Dto.ProductInventoryLevelPageItem()
        //                {
        //                    Id = inventory.Product.Id,
        //                    Code = inventory.Product.Code,
        //                    Name = inventory.Product.Name,
        //                    Supplier = inventory.Product.Supplier.Name,
        //                    UnitOfMeasure = unitOfMeasure.Name,
        //                    OnHandValue = inventory.Product.Inventory
        //                        .Convert(i => i.OnHand)
        //                        .ToValue(unitOfMeasure),
        //                    AllocatedValue = inventory.Product.Inventory
        //                        .Convert(i => i.Allocated)
        //                        .ToValue(unitOfMeasure),
        //                    AvailableValue = inventory.Product.Inventory
        //                        .Convert(i => i.Available)
        //                        .ToValue(unitOfMeasure),
        //                    OnOrderValue = inventory.Product.Inventory
        //                        .Convert(i => i.OnOrder)
        //                        .ToValue(unitOfMeasure),
        //                    CurrentLevelValue = inventory.Product.Inventory
        //                        .Convert(i => i.CurrentLevel)
        //                        .ToValue(unitOfMeasure),
        //                    ReorderLevelValue = inventory.Product.Inventory
        //                        .Convert(i => i.ReorderLevel)
        //                        .ToValue(unitOfMeasure),
        //                    TargetLevelValue = inventory.Product.Inventory
        //                        .Convert(i => i.TargetLevel)
        //                        .ToValue(unitOfMeasure),
        //                    BelowTargetLevelValue = inventory.Product.Inventory
        //                        .Convert(i => i.BelowTargetLevel)
        //                        .ToValue(unitOfMeasure),
        //                };
        //                return item;
        //            });

        //            response = new Response()
        //            {
        //                Count = countFuture.Value,
        //                Items = idsFuture
        //                    .Select(x => Hydrate(x))
        //                    .ToList()
        //            };
        //        }

        //        return response;
        //    }
        //}
    }
}
