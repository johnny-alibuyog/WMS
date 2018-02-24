using AmpedBiz.Core.Services.Products;
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
            ExpressMapper.Mapper.Register<Entity.Inventory, Dto.Inventory>()
                .Member(x => x.BadStockValue, x => x.Product.ConvertToDefaultValue(x.BadStock))
                .Member(x => x.ReceivedValue, x => x.Product.ConvertToDefaultValue(x.Received))
                .Member(x => x.OnOrderValue, x => x.Product.ConvertToDefaultValue(x.OnOrder))
                .Member(x => x.OnHandValue, x => x.Product.ConvertToDefaultValue(x.OnHand))
                .Member(x => x.AllocatedValue, x => x.Product.ConvertToDefaultValue(x.Allocated))
                .Member(x => x.ShippedValue, x => x.Product.ConvertToDefaultValue(x.Shipped))
                .Member(x => x.BackOrderedValue, x => x.Product.ConvertToDefaultValue(x.BackOrdered))
                .Member(x => x.ReturnedValue, x => x.Product.ConvertToDefaultValue(x.Returned))
                .Member(x => x.AvailableValue, x => x.Product.ConvertToDefaultValue(x.Available))
                .Member(x => x.InitialLevelValue, x => x.Product.ConvertToDefaultValue(x.InitialLevel))
                .Member(x => x.ShrinkageValue, x => x.Product.ConvertToDefaultValue(x.Shrinkage))
                .Member(x => x.CurrentLevelValue, x => x.Product.ConvertToDefaultValue(x.CurrentLevel))
                .Member(x => x.TargetLevelValue, x => x.Product.ConvertToDefaultValue(x.TargetLevel))
                .Member(x => x.BelowTargetLevelValue, x => x.Product.ConvertToDefaultValue(x.BelowTargetLevel))
                .Member(x => x.ReorderLevelValue, x => x.Product.ConvertToDefaultValue(x.ReorderLevel))
                .Member(x => x.ReorderQuantityValue, x => x.Product.ConvertToDefaultValue(x.ReorderQuantity))
                .Member(x => x.MinimumReorderQuantityValue, x => x.Product.ConvertToDefaultValue(x.MinimumReorderQuantity)) ;
                
            ExpressMapper.Mapper.Register<Entity.ProductUnitOfMeasure, Dto.ProductUnitOfMeasure>().Flatten();
            ExpressMapper.Mapper.Register<Entity.ProductUnitOfMeasurePrice, Dto.ProductUnitOfMeasurePrice>().Flatten();
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