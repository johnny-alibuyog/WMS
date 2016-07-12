using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Branches;
using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.Host.Plugins.Providers;
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
        private readonly Random rnd = new Random();

        private ISessionFactory sessionFactory;
        private IAuditProvider auditProvider;

        private List<Currency> currencies = new List<Currency>();
        private List<PaymentType> paymentTypes = new List<PaymentType>();
        private List<PricingScheme> pricingSchemes = new List<PricingScheme>();
        private List<ProductCategory> productCategories = new List<ProductCategory>();
        private List<Role> roles = new List<Role>();
        private List<UnitOfMeasureClass> unitOfMeasures = new List<UnitOfMeasureClass>();

        private Service.Common.Filter filter = new Service.Common.Filter();
        private Service.Common.Pager pager = new Service.Common.Pager() { Offset = 1, Size = 1000 };
        private Service.Common.Sorter sorter = new Service.Common.Sorter() {};
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
                this.currencies = session.Query<Currency>().ToList();
                this.paymentTypes = session.Query<PaymentType>().ToList();
                this.pricingSchemes = session.Query<PricingScheme>().ToList();
                this.productCategories = session.Query<ProductCategory>().ToList();
                this.roles = session.Query<Role>().ToList();
                this.unitOfMeasures = session.Query<UnitOfMeasureClass>().ToList();
            }
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

            for(var i = 0; i < count; i++)
            {
                var custData = this.dummyData.GenerateCustomer();
                custData.PricingSchemeId = this.pricingSchemes[this.rnd.Next(0, this.pricingSchemes.Count - 1)].Id;

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
                var productData = this.dummyData.GenerateProduct();
                productData.SupplierId = suppliers[rnd.Next(0, suppliers.Count - 1)].Id;

                var request = new CreateProduct.Request()
                {
                    Id = productData.Id,
                    BasePriceAmount = productData.BasePriceAmount,
                    CategoryId = this.productCategories[this.rnd.Next(0, this.productCategories.Count - 1)].Id,
                    Description = productData.Description,
                    Discontinued = productData.Discontinued,
                    Image = productData.Image,
                    Name = productData.Name,
                    RetailPriceAmount = productData.RetailPriceAmount,
                    SupplierId = productData.SupplierId,
                    WholesalePriceAmount = productData.WholesalePriceAmount
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

        private List<Service.Dto.PurchaseOrder> CreatePurchaseOrders(int count = 1)
        {
            var pOrders = new List<Service.Dto.PurchaseOrder>();

            var suppliers = new List<Supplier>();
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
            {
                suppliers = session.Query<Supplier>()
                    .Where(x => x.Products.Count() > 0)
                    .ToList();

                users = session.Query<User>().ToList();
            }

            for (var i = 0; i < count; i++)
            {
                var request = new CreateNewPurchaseOder.Request()
                {
                    Id = Guid.NewGuid(),
                    Status = Service.Dto.PurchaseOrderStatus.New,
                    UserId = users[this.rnd.Next(0, users.Count - 1)].Id,
                    SupplierId = suppliers[this.rnd.Next(0, suppliers.Count - 1)].Id,
                    PaymentTypeId = this.paymentTypes[this.rnd.Next(0, this.paymentTypes.Count - 1)].Id,
                    ExpectedOn = DateTime.Now.AddDays(10),
                    PaidOn = DateTime.Now.AddDays(5)
                };
                request.Items = this.CreatePurchaseOrderItems(request, this.rnd.Next(20, 90));

                var handler = new CreateNewPurchaseOder.Handler(this.sessionFactory).Handle(request);

                pOrders.Add(handler as Service.Dto.PurchaseOrder);
            }

            return pOrders;
        }

        private List<Service.Dto.PurchaseOrderItem> CreatePurchaseOrderItems(Service.Dto.PurchaseOrder po, int count = 1)
        {
            var poItems = new List<Service.Dto.PurchaseOrderItem>();
            var selectedProducts = this.SelectRandomProducts(po.SupplierId, count);
            
            foreach (var product in selectedProducts)
            {
                poItems.Add(new Service.Dto.PurchaseOrderItem
                {
                   //TotalAmount = product.RetailPriceAmount + 1m,
                   Product = new Lookup<string>(product.Id, product.Name),
                   PurchaseOrderId = po.Id,
                   QuantityValue = this.rnd.Next(1, 100),
                   UnitPriceAmount = product.RetailPriceAmount + 1m,
                });
            }


            return poItems;
        }

        private IEnumerable<Service.Dto.PurchaseOrder> GetPurchaseOrders(Service.Dto.PurchaseOrderStatus status, int count = 1)
        {
            var request = new GetPurchaseOderList.Request() { };
            var pOrders = new GetPurchaseOderList.Handler(this.sessionFactory).Handle(request);

            return pOrders.Where(po => po.Status == Service.Dto.PurchaseOrderStatus.New).Take(count);
        }

        private Service.Dto.PurchaseOrder SubmitPurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new SubmitPurchaseOrder.Request() { Id = pOrder.Id, UserId = userId };
            var order = new SubmitPurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder ApprovePurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new ApprovePurchaseOder.Request() { Id = pOrder.Id, UserId = userId };
            var order = new ApprovePurchaseOder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder PaymentPurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new PayPurchaseOrder.Request() { Id = pOrder.Id, UserId = userId };
            var order = new PayPurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder CancelPurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new CancelPurchaseOder.Request() { Id = pOrder.Id, UserId = userId, CancelReason = "Products not needed" };
            var order = new CancelPurchaseOder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder PayPurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new PayPurchaseOrder.Request() { Id = pOrder.Id, UserId = userId, TotalAmount = pOrder.TotalAmount };
            var order = new PayPurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder ReceivePurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new ReceivePurchaseOrder.Request() { Id = pOrder.Id, UserId = userId };
            var order = new ReceivePurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        private Service.Dto.PurchaseOrder CompletePurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var users = new List<User>();

            using (var session = this.sessionFactory.OpenSession())
                users = session.Query<User>().ToList();

            var userId = users[this.rnd.Next(0, users.Count - 1)].Id;

            var request = new CompletePurchaseOder.Request() { Id = pOrder.Id, UserId = userId };
            var order = new CompletePurchaseOder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

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
            Assert.AreEqual(expected, expected);

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
            var getCustomer = new GetCustomer.Handler(this.sessionFactory).Handle(new GetCustomer.Request() { Id = customer.Id });
            var getCustomerPage = new GetCustomerPage
                .Handler(this.sessionFactory)
                .Handle(new GetCustomerPage.Request()
                {
                    Filter = new Service.Common.Filter(),
                    Pager = new Service.Common.Pager() { Size =10, Offset = 1 },
                    Sorter = new Service.Common.Sorter()
                });
            */

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
