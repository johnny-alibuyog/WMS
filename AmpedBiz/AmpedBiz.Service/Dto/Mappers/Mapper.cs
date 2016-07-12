using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Products;
using AmpedBiz.Service.PurchaseOrders;
using AmpedBiz.Service.UnitOfMeasureClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Dto.Mappers
{
    public class Mapper : IMapper
    {
        public void Initialze()
        {
            ExpressMapper.Mapper.Register<Entity.Customer, GetCustomer.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.Product, Dto.Product>().Flatten();

            ExpressMapper.Mapper.Register<Entity.Product, GetProduct.Response>().Flatten();

            //ExpressMapper.Mapper.Register<Entity.Product, Lookup<string>>()
            //    .Member(x => x.Id, x => x.Id)
            //    .Member(x => x.Name, x => x.Name);

            ExpressMapper.Mapper.Register<Entity.UnitOfMeasure, Dto.UnitOfMeasure>().Flatten();

            ExpressMapper.Mapper.Register<Dto.UnitOfMeasure, Entity.UnitOfMeasure>()
                .Instantiate(x => new Entity.UnitOfMeasure(x.Id));

            ExpressMapper.Mapper.Register<Entity.UnitOfMeasureClass, GetUnitOfMeasureClass.Response>()
                .Member(x => x.Units, x => x.Units);

            ExpressMapper.Mapper.Register<UpdateUnitOfMeasureClass.Request, Entity.UnitOfMeasureClass>()
                .After((source, desitnation) =>
                {
                    var units = source.Units.MapTo(default(Collection<Entity.UnitOfMeasure>));
                    desitnation.WithUnits(units);
                });

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, GetPurchaseOder.Response>()
                .Member(x => x.AllowedTransitions, x => x.State.AllowedTransitions
                    .ToDictionary(o => ExpressMapper.Mapper.Map<Entity.PurchaseOrderStatus, Dto.PurchaseOrderStatus>(o.Key), y => y.Value)
                )
                .Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, CreateNewPurchaseOder.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderItem, Dto.PurchaseOrderItem>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderItem, Dto.PurchaseOrderItemPageItem>().Flatten();

            ExpressMapper.Mapper.Register<Entity.User, Lookup<Guid>>()
                .Member(x => x.Id, x => x.Id)
                .Member(x => x.Name, x => x.FullName());
        }

        private void RegisterMapping<TEntity, TDto>()
        {
            var entityType = typeof(TEntity);
            var dtoBaseType = typeof(TDto);
            var dtoTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(TDto).IsAssignableFrom(x))
                .ToList();

            dtoTypes.Add(dtoBaseType);

            foreach(var dtoType in dtoTypes)
            {
                //ExpressMapper.Mapper.Instance.Register<TEntity, TDto>().Flatten()

                //ExpressMapper.Mapper.Instance.Register(dtoType, entityType);
            }
        }
    }
}