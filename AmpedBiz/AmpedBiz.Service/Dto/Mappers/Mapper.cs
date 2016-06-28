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
            
            ExpressMapper.Mapper.Register<Entity.Product, Dto.Product>()
                .Member(x => x.SupplierId, x => x.Supplier.Id)
                .Member(x => x.CategoryId, x => x.Category.Id)
                .Member(x => x.BasePriceAmount, x => x.BasePrice.Amount)
                .Member(x => x.WholesalePriceAmount, x => x.WholeSalePrice.Amount)
                .Member(x => x.RetailPriceAmount, x => x.RetailPrice.Amount);

            ExpressMapper.Mapper.Register<Entity.Product, GetProduct.Response>()
                .Member(x => x.SupplierId, x => x.Supplier.Id)
                .Member(x => x.CategoryId, x => x.Category.Id)
                .Member(x => x.BasePriceAmount, x => x.BasePrice.Amount)
                .Member(x => x.WholesalePriceAmount, x => x.WholeSalePrice.Amount)
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
                .Member(x => x.PurchaseOrderId, x => x.PurchaseOrder.Id)
                .Member(x => x.ProductId, x => x.Product.Id)
                .Member(x => x.UnitPriceAmount, x => x.UnitPrice.Amount)
                .Member(x => x.TotalAmount, x => x.Total.Amount);

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderDetail, Dto.PurchaseOrderDetailPageItem>()
                .Member(x => x.PurchaseOrderId, x => x.PurchaseOrder.Id.ToString())
                .Member(x => x.ProductName, x => x.Product.Name)
                .Member(x => x.QuantityValue, x => x.Quantity.Value)
                .Member(x => x.UnitPriceAmount, x => x.UnitPrice.ToStringWithSymbol())
                .Member(x => x.TotalAmount, x => x.Total.ToStringWithSymbol());

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, GetPurchaseOder.Response>()
                //.Member(x => x.CreatedBy, x => x.CompletedBy.User.Person.FirstName + " " + x.CompletedBy.User.Person.LastName)
                //.Member(x => x.SubmittedBy, x => x.SubmittedBy.User.Person.FirstName + " " + x.SubmittedBy.User.Person.LastName)
                //.Member(x => x.ApprovedBy, x => x.ApprovedBy.User.Person.FirstName + " " + x.ApprovedBy.User.Person.LastName)
                //.Member(x => x.PaidBy, x => x.PaidBy.User.Person.FirstName + " " + x.PaidBy.User.Person.LastName)
                //.Member(x => x.CompletedBy, x => x.CompletedBy.User.Person.FirstName + " " + x.CompletedBy.User.Person.LastName)
                //.Member(x => x.CancelledBy, x => x.CancelledBy.User.Person.FirstName + " " + x.CancelledBy.User.Person.LastName)
                //.Member(x => x.CreatedOn, x => x.CreatedOn)
                //.Member(x => x.SubmittedOn, x => x.SubmittedOn)
                //.Member(x => x.ApprovedOn, x => x.ApprovedOn)
                //.Member(x => x.PaidOn, x => x.PaidOn)
                //.Member(x => x.CompletedOn, x => x.CompletedOn)
                //.Member(x => x.CancelledOn, x => x.CancelledOn)
                .Member(x => x.Status, x => x.Status.Parse<Dto.PurchaseOrderStatus>())
                .Member(x => x.SubTotalAmount, x => x.Total.Amount)
                .Member(x => x.SupplierId, x => x.Supplier.Id)
                .Member(x => x.TaxAmount, x => x.Tax.Amount)
                .Member(x => x.TotalAmount, x => x.Total.Amount)
                .Member(x => x.PurchaseOrderDetails, x => x.PurchaseOrderDetails);

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, CreateNewPurchaseOder.Response>()
                //.Ignore(x => x.CreatedBy)
                //.Ignore(x => x.SubmittedBy)
                //.Ignore(x => x.ApprovedBy)
                //.Ignore(x => x.PaidBy)
                //.Ignore(x => x.CompletedBy)
                //.Ignore(x => x.CancelledBy)
                //.Member(x => x.CreatedBy, x => x.CreatedBy.ToString())
                //.Member(x => x.SubmittedBy, x => x.SubmittedBy.User.Person.FirstName + " " + x.SubmittedBy.User.Person.LastName)
                //.Member(x => x.ApprovedBy, x => x.ApprovedBy.User.Person.FirstName + " " + x.ApprovedBy.User.Person.LastName)
                //.Member(x => x.PaidBy, x => x.PaidBy.User.Person.FirstName + " " + x.PaidBy.User.Person.LastName)
                //.Member(x => x.CompletedBy, x => x.CompletedBy.User.Person.FirstName + " " + x.CompletedBy.User.Person.LastName)
                //.Member(x => x.CancelledBy, x => x.CancelledBy.User.Person.FirstName + " " + x.CancelledBy.User.Person.LastName)
                //.Member(x => x.CreatedOn, x => x.CreatedOn)
                //.Member(x => x.SubmittedOn, x => x.SubmittedOn)
                //.Member(x => x.ApprovedOn, x => x.ApprovedOn)
                //.Member(x => x.PaidOn, x => x.PaidOn)
                //.Member(x => x.CompletedOn, x => x.CompletedOn)
                //.Member(x => x.CancelledOn, x => x.CancelledOn)
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