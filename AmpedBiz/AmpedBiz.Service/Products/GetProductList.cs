﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }

            public Guid SupplierId { get; set; }
        }

        public class Response : List<Dto.Product>
        {
            public Response() { }

            public Response(List<Dto.Product> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<Product>()
                        .Fetch(x => x.Supplier).Eager
                        .Fetch(x => x.Category).Eager
                        .Fetch(x => x.Inventory).Eager
                        //.Fetch(x => x.Inventory.UnitOfMeasure).Eager
                        //.Fetch(x => x.Inventory.PackagingUnitOfMeasure).Eager
                        .Fetch(x => x.Inventory.Stocks).Eager
                        .Fetch(x => x.Inventory.Stocks.First().ModifiedBy).Eager
                        .Fetch(x => x.UnitOfMeasures).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices).Eager
                        .TransformUsing(Transformers.DistinctRootEntity);

                    if (message.Id.IsNullOrEmpty() != true)
                        query = query.WhereRestrictionOn(x => x.Id).IsIn(message.Id);
                    
                    if (message.SupplierId != Guid.Empty)
                        query = query.Where(x => x.Supplier.Id == message.SupplierId);

                    var entities = query.List();

                    var dtos = entities.MapTo(default(List<Dto.Product>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}