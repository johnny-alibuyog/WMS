using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Orders;
using AmpedBiz.Service.Products;
using AmpedBiz.Service.PurchaseOrders;
using AmpedBiz.Service.Returns;
using AmpedBiz.Service.Users;
using System;
using System.Linq;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Dto.Mappers
{
    public class Mapper : IMapper
    {
        public void Initialze()
        {
            RegisterUserMap();
            RegisterCustomersMap();
            RegisterProductsMap();
            RegisterUnitOfMeasuresMap();
            RegisterPurchaseOrderMap();
            RegisterOrderMap();
            RegisterReturnMap();
        }

        private void RegisterBranchMap()
        {
            //ExpressMapper.Mapper.Register<Entity.Branch, GetBranch.Response>().Flatten();
        }

        private void RegisterUserMap()
        {
            ExpressMapper.Mapper.Register<Entity.User, Login.Response>().Flatten();
        }

        private void RegisterCustomersMap()
        {
            ExpressMapper.Mapper.Register<Entity.Customer, GetCustomer.Response>().Flatten();
        }

        private void RegisterProductsMap()
        {
            ExpressMapper.Mapper.Register<Entity.Product, Dto.Product>().Flatten();
            ExpressMapper.Mapper.Register<Entity.Inventory, Dto.Inventory>().Flatten();
            ExpressMapper.Mapper.Register<Entity.Product, GetProduct.Response>().Flatten();
        }

        private void RegisterUnitOfMeasuresMap()
        {
            ExpressMapper.Mapper.Register<Entity.UnitOfMeasure, Dto.UnitOfMeasure>().Flatten();

            ExpressMapper.Mapper.Register<Dto.UnitOfMeasure, Entity.UnitOfMeasure>()
                .Instantiate(x => new Entity.UnitOfMeasure(x.Id, null));
        }

        private void RegisterPurchaseOrderMap()
        {
            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, GetPurchaseOrder.Response>()
                .Member(x => x.Stage, x => x.State.Stage).Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, SavePurchaseOrder.Response>().Flatten()
                .Member(x => x.Stage, x => x.State.Stage).Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, GetPurchaseOrderPayable.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrder, GetVoucher.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderItem, Dto.PurchaseOrderItem>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderItem, Dto.VoucherItem>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderPayment, Dto.PurchaseOrderPayment>().Flatten();

            ExpressMapper.Mapper.Register<Entity.PurchaseOrderReceipt, Dto.PurchaseOrderReceipt>().Flatten();
        }

        private void RegisterOrderMap()
        {
            ExpressMapper.Mapper.Register<Entity.Order, GetOrder.Response>()
                .Member(x => x.Stage, x => x.State.Stage).Flatten();

            ExpressMapper.Mapper.Register<Entity.Order, SaveOrder.Response>()
                .Member(x => x.Stage, x => x.State.Stage).Flatten();

            ExpressMapper.Mapper.Register<Entity.Order, GetOrderPayable.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.Order, InvoiceOrder.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.Order, GetOrderInvoiceDetail.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.OrderItem, Dto.OrderReturnable>().Flatten();

            ExpressMapper.Mapper.Register<Entity.OrderItem, Dto.OrderInvoiceDetailItem>().Flatten();

            ExpressMapper.Mapper.Register<Entity.OrderItem, Dto.OrderItem>().Flatten();

            ExpressMapper.Mapper.Register<Entity.OrderReturn, Dto.OrderReturn>().Flatten();

            ExpressMapper.Mapper.Register<Entity.OrderPayment, Dto.OrderPayment>().Flatten();

            //ExpressMapper.Mapper.Register<Entity.OrderItem, Dto.OrderItemPageItem>().Flatten();
        }

        private void RegisterReturnMap()
        {
            ExpressMapper.Mapper.Register<Entity.Return, GetReturn.Response>().Flatten();

            ExpressMapper.Mapper.Register<Entity.Return, Dto.Return>().Flatten();

            ExpressMapper.Mapper.Register<Entity.ReturnItem, Dto.ReturnItem>().Flatten();
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