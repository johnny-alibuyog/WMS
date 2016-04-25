﻿using System;
using ExpressMapper;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;


namespace AmpedBiz.Service.Dto.Mappers
{
    class Mapper : IMapper
    {
        public void Initialze()
        {
            ExpressMapper.Mapper.Register<Dto.Address, Entity.Address>();
            ExpressMapper.Mapper.Register<Dto.ProductCategory, Entity.ProductCategory>();
            ExpressMapper.Mapper.Register<Dto.PaymentType, Entity.PaymentType>();
            ExpressMapper.Mapper.Register<Dto.Branch, Entity.Branch>();
            ExpressMapper.Mapper.Register<Dto.Customer, Entity.Customer>();
            ExpressMapper.Mapper.Register<Dto.Product, Entity.Product>();
            ExpressMapper.Mapper.Register<Dto.Supplier, Entity.Supplier>();

            ExpressMapper.Mapper.Register<Entity.Product, Dto.Product>()
                 .Member(x => x.SupplierId, x => x.Supplier.Id)
                 .Member(x => x.CategoryId, x => x.Category.Id)
                 .Member(x => x.BasePriceAmount, x => x.BasePrice.Amount)
                 .Member(x => x.BasePriceCurrencyId, x => x.BasePrice.Currency.Id)
                 .Member(x => x.WholeSalePriceAmount, x => x.WholeSalePrice.Amount)
                 .Member(x => x.WholeSalePriceCurrencyId, x => x.WholeSalePrice.Currency.Id)
                 .Member(x => x.RetailPriceAmount, x => x.RetailPrice.Amount)
                 .Member(x => x.RetailPriceCurrencyId, x => x.RetailPrice.Currency.Id);

            ExpressMapper.Mapper.Register<Entity.Product, Dto.ProductPageItem>()
                 .Member(x => x.SupplierName, x => x.Supplier.Name)
                 .Member(x => x.CategoryName, x => x.Category.Name)
                 .Member(x => x.BasePrice, x => x.BasePrice.ToStringWithSymbol())
                 .Member(x => x.RetailPrice, x => x.RetailPrice.ToStringWithSymbol())
                 .Member(x => x.WholeSalePrice, x => x.WholeSalePrice.ToStringWithSymbol());
        }
    }
}