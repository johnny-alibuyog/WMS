using System;
using ExpressMapper;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;


namespace AmpedBiz.Service.Dto.Mappers
{
    class MappingInitializer : IMappingInitializer
    {
        public void Initialze()
        {
            Mapper.Register<Dto.Address, Entity.Address>();
            Mapper.Register<Dto.ProductCategory, Entity.ProductCategory>();
            Mapper.Register<Dto.PaymentType, Entity.PaymentType>();
            Mapper.Register<Dto.Branch, Entity.Branch>();
            Mapper.Register<Dto.Customer, Entity.Customer>();
            Mapper.Register<Dto.Product, Entity.Product>();

            Mapper.Register<Entity.Product, Dto.Product>()
                 .Member(x => x.BasePriceAmount, x => x.BasePrice.Amount)
                 .Member(x => x.BasePriceCurrencyId, x => x.BasePrice.Currency.Id)
                 .Member(x => x.WholeSalePriceAmount, x => x.WholeSalePrice.Amount)
                 .Member(x => x.WholeSalePriceCurrencyId, x => x.WholeSalePrice.Currency.Id)
                 .Member(x => x.RetailPriceAmount, x => x.RetailPrice.Amount)
                 .Member(x => x.RetailPriceCurrencyId, x => x.RetailPrice.Currency.Id)
                 .Member(x => x.SupplierId, x => x.Supplier.Id)
                 .Member(x => x.CategoryId, x => x.Category.Id);
        }
    }
}
