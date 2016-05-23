using System.Collections.ObjectModel;
using Entity = AmpedBiz.Core.Entities;
using System.Linq;
using System.Collections.Generic;
using ExpressMapper.Extensions;

namespace AmpedBiz.Service.Dto.Mappers
{
    public class Mapper : IMapper
    {
        public void Initialze()
        {
            ExpressMapper.Mapper.Register<Dto.Address, Entity.Address>();
            ExpressMapper.Mapper.Register<Dto.ProductCategory, Entity.ProductCategory>();
            ExpressMapper.Mapper.Register<Dto.PaymentType, Entity.PaymentType>();
            ExpressMapper.Mapper.Register<Dto.Branch, Entity.Branch>();
            ExpressMapper.Mapper.Register<Dto.Supplier, Entity.Supplier>();
            ExpressMapper.Mapper.Register<Dto.Person, Entity.Person>();
            ExpressMapper.Mapper.Register<Dto.EmployeeType, Entity.EmployeeType>();
            ExpressMapper.Mapper.Register<Dto.Employee, Entity.Employee>();

            ExpressMapper.Mapper.Register<Entity.Customer, Dto.Customer>()
                .Member(x => x.CreditLimitAmount, x => x.CreditLimit.Amount)
                .Member(x => x.PricingSchemeId, x => x.PricingScheme.Id);

            ExpressMapper.Mapper.Register<Entity.Customer, Dto.CustomerPageItem>()
                .Member(x => x.CreditLimitAmount, x => x.CreditLimit.ToStringWithSymbol())
                .Member(x => x.PricingSchemeName, x => x.PricingScheme.Name);

            ExpressMapper.Mapper.Register<Entity.Product, Dto.Product>()
                .Member(x => x.SupplierId, x => x.Supplier.Id)
                .Member(x => x.CategoryId, x => x.Category.Id)
                .Member(x => x.BasePriceAmount, x => x.BasePrice.Amount)
                .Member(x => x.WholesalePriceAmount, x => x.WholesalePrice.Amount)
                .Member(x => x.RetailPriceAmount, x => x.RetailPrice.Amount);

            ExpressMapper.Mapper.Register<Entity.Product, Dto.ProductPageItem>()
                .Member(x => x.SupplierName, x => x.Supplier.Name)
                .Member(x => x.CategoryName, x => x.Category.Name)
                .Member(x => x.BasePrice, x => x.BasePrice.ToStringWithSymbol())
                .Member(x => x.RetailPrice, x => x.RetailPrice.ToStringWithSymbol())
                .Member(x => x.WholesalePrice, x => x.WholesalePrice.ToStringWithSymbol());

            ExpressMapper.Mapper.Register<Entity.UnitOfMeasure, Dto.UnitOfMeasure>()
                .Member(x => x.UnitOfMeasureClassId, x => x.UnitOfMeasureClass.Id);

            ExpressMapper.Mapper.Register<Dto.UnitOfMeasure, Entity.UnitOfMeasure>()
                .Member(x => x.UnitOfMeasureClass.Id, x => x.UnitOfMeasureClassId);

            ExpressMapper.Mapper.Register<Entity.UnitOfMeasureClass, Dto.UnitOfMeasureClass>()
                .Member(x => x.Units, x => x.Units);

            ExpressMapper.Mapper.Register<Dto.UnitOfMeasureClass, Entity.UnitOfMeasureClass>()
                .After((source, desitnation) =>
                {
                    var units = new Collection<Entity.UnitOfMeasure>();
                    source.Units.ForEach(sourceUnit =>
                    {
                        units.Add(ExpressMapper.Mapper.Map<Dto.UnitOfMeasure, Entity.UnitOfMeasure>(
                            src: sourceUnit,
                            dest: new Entity.UnitOfMeasure(sourceUnit.Id)
                        ));
                    });

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

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, Dto.PurchaseOrder>()
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
                .After((source, destination) =>
                {
                    var poDetails = new Collection<Dto.PurchaseOrderDetail>();
                    destination.PurchaseOrderDetails = new List<Dto.PurchaseOrderDetail>();

                    source.PurchaseOrderDetails.ToList().ForEach(details =>
                    {
                        poDetails.Add(ExpressMapper.Mapper.Map(src: details, dest: new PurchaseOrderDetail()));
                    });
                });

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, Dto.PurchaseOrderPageItem>()
                .Member(x => x.ClosedDate, x => x.ClosedDate.Value == null ? string.Empty : x.ClosedDate.Value.ToShortDateString())
                .Member(x => x.CompletedByEmployeeName, x => x.CompletedBy.User.FullName())
                .Member(x => x.CreatedByEmployeeName, x => x.CreatedBy.User.FullName())
                .Member(x => x.CreationDate, x => x.CreationDate.Value == null ? string.Empty : x.CreationDate.Value.ToShortDateString())
                .Member(x => x.ExpectedDate, x => x.ExpectedDate.Value == null ? string.Empty : x.ExpectedDate.Value.ToShortDateString())
                .Member(x => x.OrderDate, x => x.OrderDate.Value == null ? string.Empty : x.OrderDate.Value.ToShortDateString())
                .Member(x => x.Payment, x => x.Payment.ToStringWithSymbol())
                .Member(x => x.PaymentDate, x => x.PaymentDate.Value == null ? string.Empty : x.PaymentDate.Value.ToShortDateString())
                .Member(x => x.PaymentTypeName, x => x.PaymentType.Name)
                .Member(x => x.StatusName, x => x.Status.ToString())
                .Member(x => x.SubmittedByEmployeeName, x => x.SubmittedBy.User.FullName())
                .Member(x => x.SubmittedDate, x => x.SubmittedDate.Value == null ? string.Empty : x.SubmittedDate.Value.ToShortDateString())
                .Member(x => x.SubTotal, x => x.SubTotal.ToStringWithSymbol())
                .Member(x => x.SupplierName, x => x.Supplier.Name)
                .Member(x => x.Tax, x => x.Tax.ToStringWithSymbol())
                .Member(x => x.Total, x => x.Total.ToStringWithSymbol());
        }
    }
}