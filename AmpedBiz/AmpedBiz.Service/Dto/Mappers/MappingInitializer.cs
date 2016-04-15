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
        }
    }
}
