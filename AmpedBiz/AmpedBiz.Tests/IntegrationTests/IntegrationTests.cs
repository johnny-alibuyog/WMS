using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Branches;
using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.Host.Plugins.Providers;
using AmpedBiz.Service.Orders;
using AmpedBiz.Service.Products;
using AmpedBiz.Service.PurchaseOrders;
using AmpedBiz.Service.Suppliers;
using AmpedBiz.Service.Users;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Tests.IntegrationTests
{

    /*
    Common Scenarios

    Entity Creations

    - Create Users
    - Create Customers
    - Create Suppliers
    - Create Product Categories
    - Create Products

    (Asserts - verify created entities)

    Fill Inventory

    - Select Products for Purchase Order (New, add to cart functionality)
    - Create Purchase Order(s) - Assert Status, it should be Active orders
    - Transition PO - Assert Status, it should be awaiting approval
    - Approve Purchase Order - Assert Status, it should be awaiting completion
    - Complete Purchases - Assert Status, it should be completed

    (Asserts - product stocks should be updated)

    Customer Order

    - Create Customer Order(s) - Assert Status, it should be Active orders
    - Transition Order - Assert Status, it should be On-Staging
    - Transition Order - Assert Status, it should be On-Route
    - Set Customer Payments (salesman), etc then Transition Order, Assert Status, it should be For Invoicing
    - Transition Order - Assert Status, it should be Invoiced
    - Transition Order - Assert Status, it should be Incomplete Payments (if needed)
    - Complete Orders - Assert Status, it should be completed


    (Asserts - product stocks should be updated) 

    */

    [TestFixture]
    public class IntegrationTests
    {
        private readonly DummyData dummyData = new DummyData();
        private readonly Random random = new Random();

        private ISessionFactory sessionFactory;
        private IAuditProvider auditProvider;

        private List<Role> _roles = new List<Role>();
        private List<User> _users = new List<User>();
        private List<Branch> _branches = new List<Branch>();
        private List<Shipper> _shippers = new List<Shipper>();
        private List<Supplier> _suppliers = new List<Supplier>();
        private List<Customer> _customers = new List<Customer>();
        private List<Currency> _currencies = new List<Currency>();
        private List<PaymentType> _paymentTypes = new List<PaymentType>();
        private List<PricingScheme> _pricingSchemes = new List<PricingScheme>();
        private List<ProductCategory> _productCategories = new List<ProductCategory>();
        private List<UnitOfMeasureClass> _unitOfMeasures = new List<UnitOfMeasureClass>();

        private Service.Common.Filter filter = new Service.Common.Filter();
        private Service.Common.Pager pager = new Service.Common.Pager() { Offset = 1, Size = 1000 };
        private Service.Common.Sorter sorter = new Service.Common.Sorter() { };

        public IntegrationTests()
        {
        }

        [TestFixtureSetUp]
        public void SetupTest()
        {
            new Mapper().Initialze();

            this.auditProvider = new AuditProvider();
            this.sessionFactory = new SessionProvider(new ValidatorEngine(), this.auditProvider).SessionFactory;

            this.SetupDefaultSeeders();
        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            this.auditProvider = null;
            this.sessionFactory = null;
        }

        private void SetupDefaultSeeders()
        {
            var seeders = (from t in AppDomain.CurrentDomain.GetAssemblies()
                                       .FirstOrDefault(a => a.FullName.Contains("AmpedBiz.Data")).GetTypes()
                           where t.GetInterfaces().Contains(typeof(ISeeder))
                           select Activator.CreateInstance(t, this.sessionFactory) as ISeeder)
                                       .Where(h => !h.IsDummyData);

            foreach (var seeder in seeders)
            {
                seeder.Seed();
            }

            //load data
            using (var session = this.sessionFactory.OpenSession())
            {
                this._roles = session.Query<Role>().ToList();
                this._users = session.Query<User>().ToList();
                this._branches = session.Query<Branch>().ToList();
                this._shippers = session.Query<Shipper>().ToList();
                this._customers = session.Query<Customer>().ToList();
                this._currencies = session.Query<Currency>().ToList();
                this._paymentTypes = session.Query<PaymentType>().ToList();
                this._pricingSchemes = session.Query<PricingScheme>().ToList();
                this._productCategories = session.Query<ProductCategory>().ToList();
                this._unitOfMeasures = session.Query<UnitOfMeasureClass>().ToList();
                this._suppliers = session.Query<Supplier>().Where(x => x.Products.Count() > 0).ToList();
            }
        }

        #region Common Helpers

        private Lookup<string> RandomRoles()
        {
            var random = this._roles[this.random.Next(0, this._roles.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<Guid> RandomUser()
        {
            var random = this._users[this.random.Next(0, this._users.Count - 1)];
            return new Lookup<Guid>(random.Id, random.Name);
        }

        private Lookup<string> RandomBranch()
        {
            var random = this._branches[this.random.Next(0, this._branches.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomShipper()
        {
            var random = this._shippers[this.random.Next(0, this._shippers.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomCustomer()
        {
            var random = this._customers[this.random.Next(0, this._customers.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomSupplier()
        {
            var random = this._suppliers[this.random.Next(0, this._suppliers.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomCurrency()
        {
            var random = this._currencies[this.random.Next(0, this._currencies.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomPaymentType()
        {
            var random = this._paymentTypes[this.random.Next(0, this._paymentTypes.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomPricingScheme()
        {
            var random = this._pricingSchemes[this.random.Next(0, this._pricingSchemes.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomProductCategory()
        {
            var random = this._productCategories[this.random.Next(0, this._productCategories.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Lookup<string> RandomUnitOfMeasure()
        {
            var random = this._unitOfMeasures[this.random.Next(0, this._unitOfMeasures.Count - 1)];
            return new Lookup<string>(random.Id, random.Name);
        }

        private Service.Dto.Branch CreateBranch(Service.Dto.Branch branch)
        {
            var request = new CreateBranch.Request()
            {
                Address = branch.Address,
                Description = branch.Description,
                Id = branch.Id,
                Name = branch.Name
            };

            var handler = new CreateBranch.Handler(this.sessionFactory).Handle(request);

            return handler as Service.Dto.Branch;
        }

        private List<Service.Dto.User> CreateUsers(int count = 1)
        {
            var users = new List<Service.Dto.User>();
            var branch = this.CreateBranch(this.dummyData.GenerateBranch());

            var empTypeIndex = -1;
            for (var i = 0; i < count; i++)
            {
                empTypeIndex++;

                var request = new CreateUser.Request()
                {
                    //Id = dummyData.GenerateUniqueString("Id"),
                    //Contact = dummyData.GenerateContact(),
                    BranchId = branch.Id,
                    Username = dummyData.GenerateUniqueString("Username"),
                    Password = dummyData.GenerateUniqueString("Password"),
                    Person = new Service.Dto.Person()
                    {
                        FirstName = dummyData.GenerateUniqueString("FirstName"),
                        MiddleName = dummyData.GenerateUniqueString("MiddleName"),
                        LastName = dummyData.GenerateUniqueString("LastName"),
                        BirthDate = DateTime.Today
                    },
                    Address = new Service.Dto.Address()
                    {
                        Street = dummyData.GenerateUniqueString("Street"),
                        Barangay = dummyData.GenerateUniqueString("Barangay"),
                        City = dummyData.GenerateUniqueString("City"),
                        Province = dummyData.GenerateUniqueString("Province"),
                        Region = dummyData.GenerateUniqueString("Region"),
                        Country = dummyData.GenerateUniqueString("Country"),
                        ZipCode = dummyData.GenerateUniqueString("ZipCode")
                    },
                    Roles = Role.All.Select(x => new Service.Dto.Role()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Assigned = true
                    })
                    .ToList()
                };

                var handler = new CreateUser.Handler(this.sessionFactory).Handle(request);

                users.Add(handler as Service.Dto.User);
            }

            return users;
        }

        private List<Service.Dto.Customer> CreateCustomers(int count = 1)
        {
            var customers = new List<Service.Dto.Customer>();

            for (var i = 0; i < count; i++)
            {
                var custData = this.dummyData.GenerateCustomer();
                custData.PricingSchemeId = this._pricingSchemes[this.random.Next(0, this._pricingSchemes.Count - 1)].Id;

                var request = new CreateCustomer.Request()
                {
                    Id = custData.Id,
                    Contact = custData.Contact,
                    BillingAddress = custData.BillingAddress,
                    CreditLimitAmount = custData.CreditLimitAmount,
                    Name = custData.Name,
                    OfficeAddress = custData.OfficeAddress,
                    PricingSchemeId = custData.PricingSchemeId
                };

                var handler = new CreateCustomer.Handler(this.sessionFactory).Handle(request);

                customers.Add(handler as Service.Dto.Customer);
            }

            return customers;
        }

        private List<Service.Dto.Supplier> CreateSuppliers(int count = 1)
        {
            var suppliers = new List<Service.Dto.Supplier>();

            for (var i = 0; i < count; i++)
            {
                var supplierData = this.dummyData.GenerateSupplier();

                var request = new CreateSupplier.Request()
                {
                    Id = supplierData.Id,
                    Address = supplierData.Address,
                    Contact = supplierData.Contact,
                    Name = supplierData.Name
                };

                var handler = new CreateSupplier.Handler(this.sessionFactory).Handle(request);

                suppliers.Add(handler as Service.Dto.Supplier);
            }

            return suppliers;
        }

        private List<Service.Dto.Product> CreateProducts(int count = 1)
        {
            var products = new List<Service.Dto.Product>();
            var suppliers = new List<Supplier>();

            using (var session = this.sessionFactory.OpenSession())
            {
                suppliers = session.Query<Supplier>().ToList();
            }

            for (var i = 0; i < count; i++)
            {
                var productData = this.dummyData.GenerateProduct(this.RandomProductCategory(), this.RandomSupplier());

                var request = new CreateProduct.Request()
                {
                    Id = productData.Id,
                    Category = productData.Category,
                    Description = productData.Description,
                    Discontinued = productData.Discontinued,
                    Image = productData.Image,
                    Name = productData.Name,
                    Supplier = productData.Supplier,
                    Inventory = productData.Inventory,
                };

                var handler = new CreateProduct.Handler(this.sessionFactory).Handle(request);

                products.Add(handler as Service.Dto.Product);
            }

            return products;
        }

        private IEnumerable<Service.Dto.Product> SelectRandomProducts(string supplierId, int count = 12)
        {
            var request = new GetProductList.Request() { SupplierId = supplierId };
            var response = new GetProductList.Handler(this.sessionFactory).Handle(request);
            var totalProduct = response.Count();

            count = totalProduct < count ? totalProduct : count;

            var randomIndexs = this.dummyData.GenerateUniqueNumbers(0, totalProduct, count);

            var products = response
                .Select((product, index) => new
                {
                    Index = index,
                    Product = product
                })
                .Where(x => randomIndexs.Contains(x.Index))
                .Select(x => x.Product);

            return products;
        }

        #endregion

        #region PurchaseOrders Helper

        private List<Service.Dto.PurchaseOrder> CreatePurchaseOrders(int count = 1)
        {
            var pOrders = new List<Service.Dto.PurchaseOrder>();

            for (var i = 0; i < count; i++)
            {
                var request = new CreateNewPurchaseOder.Request()
                {
                    CreatedBy = this.RandomUser(),
                    CreatedOn = DateTime.Now,
                    ExpectedOn = DateTime.Now.AddDays(10),
                    Supplier = RandomSupplier(),
                    Shipper = null,
                    ShippingFeeAmount = 0M,
                    TaxAmount = 0M,
                    PaymentType = RandomPaymentType(),
                };
                request.Items = this.CreatePurchaseOrderItems(request, this.random.Next(20, 90));

                var handler = new CreateNewPurchaseOder.Handler(this.sessionFactory).Handle(request);

                pOrders.Add(handler as Service.Dto.PurchaseOrder);
            }

            return pOrders;
        }

        private List<Service.Dto.PurchaseOrderItem> CreatePurchaseOrderItems(CreateNewPurchaseOder.Request request, int count = 1)
        {
            var poItems = new List<Service.Dto.PurchaseOrderItem>();
            var selectedProducts = this.SelectRandomProducts(request.Supplier.Id, count);

            foreach (var product in selectedProducts)
            {
                poItems.Add(new Service.Dto.PurchaseOrderItem
                {
                    //TotalAmount = product.RetailPriceAmount + 1m,
                    PurchaseOrderId = request.PurchaseOrderId,
                    Product = new Lookup<string>(product.Id, product.Name),
                    QuantityValue = this.random.Next(1, 100),
                    UnitCostAmount = product.Inventory.RetailPriceAmount ?? 0M + 1m,
                });
            }


            return poItems;
        }

        private IEnumerable<Service.Dto.PurchaseOrder> GetPurchaseOrders(Service.Dto.PurchaseOrderStatus status, int count = 1)
        {
            var request = new GetPurchaseOrderList.Request() { };
            var purchaseOrders = new GetPurchaseOrderList.Handler(this.sessionFactory).Handle(request);

            return purchaseOrders.Where(po => po.Status == Service.Dto.PurchaseOrderStatus.New).Take(count);
        }

        private Service.Dto.PurchaseOrder SubmitPurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
        {
            var request = new SubmitPurchaseOrder.Request() { PurchaseOrderId = purchaseOrder.Id, SubmittedBy = this.RandomUser() };
            var order = new SubmitPurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder ApprovePurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
        {
            var request = new ApprovePurchaseOder.Request() { PurchaseOrderId = purchaseOrder.Id, ApprovedBy = this.RandomUser() };
            var order = new ApprovePurchaseOder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder PayPurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
        {

            var request = new PayPurchaseOrder.Request()
            {
                PurchaseOrderId = purchaseOrder.Id,
                Payments = new Service.Dto.PurchaseOrderPayment[]
                {
                    new Service.Dto.PurchaseOrderPayment()
                    {
                        PaidOn = DateTime.Today,
                        PaidBy = this.RandomUser(),
                        PaymentType = this.RandomPaymentType(),
                        PaymentAmount = purchaseOrder.TotalAmount
                    }
                }
            };
            var order = new PayPurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder ReceivePurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
        {
            var data = new
            {
                BatchNumber = "XL001",
                ReceivedBy = this.RandomUser(),
                ReceivedOn = DateTime.Now
            };

            var request = new ReceivePurchaseOrder.Request()
            {
                PurchaseOrderId = purchaseOrder.Id,
                Receipts = purchaseOrder.Items
                    .Select(x => new Service.Dto.PurchaseOrderReceipt()
                    {
                        PurchaseOrderId = purchaseOrder.Id,
                        ReceivedBy = data.ReceivedBy,
                        ReceivedOn = data.ReceivedOn,
                        ExpiresOn = data.ReceivedOn.AddYears(1),
                        Product = x.Product,
                        QuantityValue = x.QuantityValue
                    })
            };

            var order = new ReceivePurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder CompletePurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
        {
            var request = new CompletePurchaseOder.Request() { PurchaseOrderId = purchaseOrder.Id, CompletedBy = this.RandomUser() };
            var order = new CompletePurchaseOder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder CancelPurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
        {
            var request = new CancelPurchaseOder.Request() { PurchaseOrderId = purchaseOrder.Id, CancelledBy = this.RandomUser(), CancellationReason = "Products not needed" };
            var order = new CancelPurchaseOder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        #endregion

        #region Orders Helper

        private List<Service.Dto.Order> CreateOrders(int count = 1)
        {
            var orders = new List<Service.Dto.Order>();
            var users = new List<User>();
            var branches = new List<Branch>();
            var customers = this.CreateCustomers(10);

            using (var session = this.sessionFactory.OpenSession())
            {
                users = session.Query<User>().ToList();
                branches = session.Query<Branch>().ToList();
            }

            for (var i = 0; i < count; i++)
            {
                var request = new CreateNewOrder.Request()
                {
                    CreatedBy = RandomUser(),
                    Branch = RandomBranch(),
                    Customer = RandomCustomer(),
                    TaxRate = .12M,
                    PaymentType = RandomPaymentType(),
                };

                request.Items = this.CreateOrderItems(this.random.Next(20, 50));
                var handler = new CreateNewOrder.Handler(this.sessionFactory).Handle(request);

                orders.Add(handler as Service.Dto.Order);
            }

            return orders;
        }

        private List<Service.Dto.OrderItem> CreateOrderItems(int count = 1)
        {
            var orderItems = new List<Service.Dto.OrderItem>();

            var suppliersId = new List<string>();

            using (var session = this.sessionFactory.OpenSession())
            {
                suppliersId = session
                    .Query<Supplier>()
                    .Select(s => s.Id)
                    .ToList();

                if (suppliersId.Count < 1)
                {
                    suppliersId = this.CreateSuppliers(10)
                        .Select(s => s.Id)
                        .ToList();
                }
            }

            var randomProductIndexes = this.dummyData.GenerateUniqueNumbers(0, count, count).ToArray();

            comeAsYouAre:

            var selectedProducts = this.SelectRandomProducts(suppliersId[this.random.Next(0, suppliersId.Count - 1)], 10).ToList();

            if (selectedProducts.Count < 1)
                goto comeAsYouAre;

            for (var i = 0; i < selectedProducts.Count; i++)
            {
                var product = selectedProducts[i];

                orderItems.Add(new Service.Dto.OrderItem()
                {
                    ExtendedPriceAmount = 0M,
                    Product = new Lookup<string>(product.Id, product.Name),
                    //UnitOfMeasure = new Lookup<string>(product.Inv)
                    QuantityValue = this.random.Next(1, 100),
                    UnitPriceAmount = product.Inventory.RetailPriceAmount ?? 0M
                });
            }

            return orderItems;
        }

        private Service.Dto.Order StageOrder(Service.Dto.Order order)
        {
            var request = new StageOrder.Request() { Id = order.Id, StagedBy = RandomUser() };
            var response = new StageOrder.Handler(this.sessionFactory).Handle(request);

            return response;
        }

        private Service.Dto.Order RouteOrder(Service.Dto.Order order)
        {
            var request = new RouteOrder.Request() { Id = order.Id, RoutedBy = RandomUser() };
            var response = new RouteOrder.Handler(this.sessionFactory).Handle(request);

            return response;
        }

        private Service.Dto.Order PayOrder(Service.Dto.Order order)
        {
            var request = new PayOrder.Request()
            {
                Id = order.Id,
                Payments = new Service.Dto.OrderPayment[]
                {
                    new Service.Dto.OrderPayment()
                    {
                        PaidOn = DateTime.Now,
                        PaidBy = RandomUser(), 
                        PaymentType = RandomPaymentType(),
                        PaymentAmount = order.TotalAmount
                    }
                }
            };
            var response = new PayOrder.Handler(this.sessionFactory).Handle(request);

            return response;
        }

        private Service.Dto.Order InvoiceOrder(Service.Dto.Order order)
        {
            var request = new InvoiceOrder.Request() { Id = order.Id, InvoicedBy = RandomUser() };
            var handler = new InvoiceOrder.Handler(this.sessionFactory).Handle(request);

            return handler;
        }

        private Service.Dto.Order CompleteOrder(Service.Dto.Order order)
        {
            var request = new CompleteOrder.Request() { Id = order.Id, CompletedBy = RandomUser() };
            var handler = new CompleteOrder.Handler(this.sessionFactory).Handle(request);

            return handler;
        }

        #endregion

        [Test]
        public void CommonScenarioTests()
        {
            //-----Create Users -----
            var users = this.CreateUsers(5);

            CollectionAssert.IsNotEmpty(users);
            CollectionAssert.AllItemsAreNotNull(users);

            //-----Create Customers-----
            var customers = this.CreateCustomers(10);

            CollectionAssert.IsNotEmpty(customers);
            CollectionAssert.AllItemsAreNotNull(customers);

            //Create Suppliers
            var suppliers = this.CreateSuppliers(10);
            CollectionAssert.IsNotEmpty(suppliers);
            CollectionAssert.AllItemsAreNotNull(suppliers);


            //Create Product Categories
            //provided by default seeder

            //Create Products
            var products = this.CreateProducts(20);

            CollectionAssert.IsNotEmpty(products);
            CollectionAssert.AllItemsAreNotNull(products);

            /*
            Fill Inventory

            Purchase Order cycles

            New = 1,
            Submitted = 2,
            Approved = 3,
            Paid = 4,
            Received = 5,
            Completed = 6,
            Cancelled = 7

            */

            //Select Products for Purchase Order (New, add to cart functionality)
            var expected = 10;
            var purchaseOrders = this.CreatePurchaseOrders(expected);

            //Create Purchase Order(s) - Assert Status, it should be NEW orders
            var actual = purchaseOrders.Count(p => p.Status == Service.Dto.PurchaseOrderStatus.New);
            //Assert.AreEqual(expected, expected);

            //Get list of created purchaseorders
            var purchaseOrderList = this.GetPurchaseOrders(Service.Dto.PurchaseOrderStatus.New, 2);

            var purchaseOrder1 = purchaseOrderList.First();
            var purchaseOrder2 = purchaseOrderList.Last();

            //- Submit PO - Assert Status, it should be Submitted
            var submittedPurchaseOrder1 = this.SubmitPurchaseOrder(purchaseOrder1);
            Assert.IsTrue(submittedPurchaseOrder1.Status == Service.Dto.PurchaseOrderStatus.Submitted);

            var submittedPurchaseOrder2 = this.SubmitPurchaseOrder(purchaseOrder2);
            Assert.IsTrue(submittedPurchaseOrder2.Status == Service.Dto.PurchaseOrderStatus.Submitted);

            //- Approve PO - Assert Status, it should be APPROVED
            var approvedPurchaseOrder = this.ApprovePurchaseOrder(purchaseOrder1);
            Assert.IsTrue(approvedPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Approved);

            //- Reject Submit PO - Assert Status, it should be rejected
            var rejectedPurchaseOrder = this.CancelPurchaseOrder(purchaseOrder2);
            Assert.IsTrue(rejectedPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Cancelled);

            //- Pay PO - Assert Status, it should be PAID
            var paidPurchaseOrder = this.PayPurchaseOrder(purchaseOrder1);
            Assert.IsTrue(paidPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Paid);

            //- Receive PO - Assert Status, it should be RECEIVED
            var receivePurchaseOrder = this.ReceivePurchaseOrder(purchaseOrder1);
            Assert.IsTrue(receivePurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Received);

            //- Complete Purchases - Assert Status, it should be completed
            var completedPurchaseOrder = this.CompletePurchaseOrder(purchaseOrder1);
            Assert.IsTrue(completedPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Completed);

            //todo: implement inventory checking


            /*
            Deduct Inventory
            
            Order cycles

            New = 1,
            Staged = 2,
            Routed = 3,
            Invoiced = 4,
            PartiallyPaid = 5,
            Completed = 6,
            Cancelled = 7

            */

            //Assert: Orders = New, Items = Allocated
            var newOrders = this.CreateOrders(expected);
            Assert.GreaterOrEqual(expected, newOrders.Count(o => o.Status == Service.Dto.OrderStatus.New));
            //Assert.False(newOrders.SelectMany(o => o.Items.Where(x => x != null))
            //    .Any(i => i.Status != Service.Dto.OrderItemStatus.Allocated));

            Service.Dto.Order
                order1 = newOrders[0],
                order2 = newOrders[1],
                order3 = newOrders[2],
                order4 = newOrders[3];

            var stagedOrder = this.StageOrder(order1);
            Assert.IsTrue(stagedOrder.Status == Service.Dto.OrderStatus.Staged);

            //var routedOrder = this.RouteOrder(stagedOrder);
            //Assert.IsTrue(routedOrder.Status == Service.Dto.OrderStatus.Routed);

            var paidOrder = this.PayOrder(stagedOrder);
            Assert.IsTrue(paidOrder.Status == Service.Dto.OrderStatus.Paid);

            //invoice from paid
            var invoicedOrder = this.InvoiceOrder(paidOrder);
            Assert.IsTrue(invoicedOrder.Status == Service.Dto.OrderStatus.Invoiced);

            //invoice from staged
            var invoicedOrder2 = this.InvoiceOrder(this.StageOrder(order2));
            Assert.IsTrue(invoicedOrder2.Status == Service.Dto.OrderStatus.Invoiced);

            //invoice from routed
            var invoicedOrder3 = this.InvoiceOrder(this.RouteOrder(order3));
            Assert.IsTrue(invoicedOrder3.Status == Service.Dto.OrderStatus.Invoiced);

            //complete all
            var completeOrder = this.CompleteOrder(paidOrder);
            Assert.IsTrue(completeOrder.Status == Service.Dto.OrderStatus.Completed);

            //todo: implement inventory checking

        }

        [Test]
        public void Testing()
        {
            //var p = this.SelectProducts(1);

            //var hash = this.dummyData.GenerateUniqueNumbers(0, 20, 10);

            // var purchaseOrders = this.CreatePurchaseOrders(10);
        }
    }
}
