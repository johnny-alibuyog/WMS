using AmpedBiz.Common.Extentions;
using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Products;
using AmpedBiz.Service.PurchaseOrders;
using AmpedBiz.Service.UnitOfMeasureClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Dto.Mappers
{
    public class Mapper : IMapper
    {
        public void Initialze()
        {
            ExpressMapper.Mapper.Register<Entity.Customer, GetCustomer.Response>()
                .Member(x => x.CreditLimitAmount, x => x.CreditLimit.Amount)
                .Member(x => x.PricingSchemeId, x => x.PricingScheme.Id);

            ExpressMapper.Mapper.Register<Entity.Product, GetProduct.Response>()
                .Member(x => x.SupplierId, x => x.Supplier.Id)
                .Member(x => x.CategoryId, x => x.Category.Id)
                .Member(x => x.BasePriceAmount, x => x.BasePrice.Amount)
                .Member(x => x.WholesalePriceAmount, x => x.WholesalePrice.Amount)
                .Member(x => x.RetailPriceAmount, x => x.RetailPrice.Amount);

            ExpressMapper.Mapper.Register<Entity.UnitOfMeasure, Dto.UnitOfMeasure>()
                .Member(x => x.UnitOfMeasureClassId, x => x.UnitOfMeasureClass.Id);

            ExpressMapper.Mapper.Register<Dto.UnitOfMeasure, Entity.UnitOfMeasure>()
                .Instantiate(source => new Entity.UnitOfMeasure(source.Id));

            ExpressMapper.Mapper.Register<Entity.UnitOfMeasureClass, GetUnitOfMeasureClass.Response>()
                .Member(x => x.Units, x => x.Units);

            ExpressMapper.Mapper.Register<UpdateUnitOfMeasureClass.Request, Entity.UnitOfMeasureClass>()
                .After((source, desitnation) =>
                {
                    var units = source.Units.MapTo(default(Collection<Entity.UnitOfMeasure>));
                    desitnation.WithUnits(units);
                });

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderDetail, Dto.PurchaseOrderDetail>()
                .Member(x => x.ExtendedPriceAmount, x => x.ExtendedPrice.Amount)
                .Member(x => x.ProductId, x => x.Product.Id)
                .Member(x => x.UnitCostAmount, x => x.UnitCost.Amount)
                .Member(x => x.PurchaseOrderId, x => x.PurchaseOrder.Id);

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderDetail, Dto.PurchaseOrderDetailPageItem>()
                .Member(x => x.ExtendedPriceAmount, x => x.ExtendedPrice.ToStringWithSymbol())
                .Member(x => x.ProductName, x => x.Product.Name)
                .Member(x => x.UnitCostAmount, x => x.UnitCost.ToStringWithSymbol())
                .Member(x => x.PurchaseOrderId, x => x.PurchaseOrder.Id.ToString());

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, GetPurchaseOrder.Response>()
                .Member(x => x.CompletedByEmployeeId, x => x.CompletedBy.Id)
                .Member(x => x.CreatedByEmployeeId, x => x.CreatedBy.Id)
                .Member(x => x.PaymentAmount, x => x.Payment.Amount)
                .Member(x => x.PaymentTypeId, x => x.PaymentType.Id)    
                .Member(x => x.ShippingFeeAmount, x => x.ShippingFee.Amount)
                .Member(x => x.SubmittedByEmployeeId, x => x.SubmittedBy.Id)
                .Member(x => x.SubTotalAmount, x => x.Total.Amount)
                .Member(x => x.SupplierId, x => x.Supplier.Id)
                .Member(x => x.TaxAmount, x => x.Tax.Amount)
                .Member(x => x.TotalAmount, x => x.Total.Amount)
                .Member(x => x.PurchaseOrderDetails, x => x.PurchaseOrderDetails);
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