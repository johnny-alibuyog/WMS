﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Data;
using AmpedBiz.Service.Host.Plugins.Providers;
using AmpedBiz.Service.ProductCategories;
using AmpedBiz.Service.Employees;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Users;
using NHibernate;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Service.Branches;
using AmpedBiz.Service.EmployeeTypes;
using System.Diagnostics;
using AmpedBiz.Service.Customers;
using AmpedBiz.Data.Seeders;
using System.Reflection;
using NHibernate.Linq;
using AmpedBiz.Service.Suppliers;
using AmpedBiz.Service.Products;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.PurchaseOrders;

namespace AmpedBiz.Tests.IntegrationTests
{

    /*
    Common Scenarios

    Entity Creations

    - Create Employees
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
        private List<EmployeeType> employeeTypes = new List<EmployeeType>();
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
                this.employeeTypes = session.Query<EmployeeType>().ToList();
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

        private Service.Dto.User CreateUser(Service.Dto.User user)
        {
            var request = new CreateUser.Request()
            {
                Address = user.Address,
                BranchId = user.BranchId,
                Id = user.Id,
                Password = user.Password,
                Person = user.Person,
                Roles = user.Roles,
                Username = user.Username
            };

            var handler = new CreateUser.Handler(this.sessionFactory).Handle(request);

            return handler as Service.Dto.User;
        }

        private List<Service.Dto.Employee> CreateEmployees(int count = 1)
        {
            var employees = new List<Service.Dto.Employee>();

            var branch = this.CreateBranch(this.dummyData.GenerateBranch());
            var user = this.dummyData.GenerateUser();
            user.BranchId = branch.Id;

            this.CreateUser(user);

            var empTypeIndex = -1;
            for (var i = 0; i< count; i++)
            {
                empTypeIndex++;

                var request = new CreateEmployee.Request()
                {
                    Id = dummyData.GenerateUniqueString("Id"),
                    Contact = dummyData.GenerateContact(),
                    EmployeeTypeId = this.employeeTypes[empTypeIndex].Id,
                    User = user
                };

                var handler = new CreateEmployee.Handler(this.sessionFactory).Handle(request);

                employees.Add(handler as Service.Dto.Employee);

                if (empTypeIndex == this.employeeTypes.Count - 1)
                    empTypeIndex = 0;
            }

            return employees;
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

        private IEnumerable<Service.Dto.Product> SelectProducts(int[] indexes = null)
        {
            var request = new GetProductPage.Request() { Filter = this.filter, Pager = this.pager, Sorter = this.sorter };
            var products = new GetProductPage.Handler(this.sessionFactory).Handle(request).Items.ToList();

            if (indexes == null)
                indexes = new int[] { 1,2,3,4,5 };

            var count = indexes.Count();

            for (var i = 0; i< count; i++)
            {
                var product = new GetProduct.Handler(this.sessionFactory).Handle(new GetProduct.Request()
                {
                    Id = products[indexes[i]].Id
                });

                yield return product;
            }
        }

        private List<Service.Dto.PurchaseOrder> CreatePurchaseOrders(int count = 1)
        {
            var pOrders = new List<Service.Dto.PurchaseOrder>();

            var suppliers = new List<Supplier>();
            var employees = new List<Employee>();

            using (var session = this.sessionFactory.OpenSession())
            {
                suppliers = session.Query<Supplier>().ToList();
                employees = session.Query<Employee>().ToList();
            }

            for (var i = 0; i < count; i++)
            {
                var request = new CreatePurchaseOrder.Request()
                {
                    Id = Guid.NewGuid(),
                    Status = Service.Dto.PurchaseOrderStatus.New,
                    CreatedByEmployeeId = employees[this.rnd.Next(0, employees.Count - 1)].Id,
                    SupplierId = suppliers[this.rnd.Next(0, suppliers.Count - 1)].Id,
                    PaymentTypeId = this.paymentTypes[this.rnd.Next(0, this.paymentTypes.Count - 1)].Id
                };
                request.PurchaseOrderDetails = this.CreatePurchaseOrderDetails(request, this.rnd.Next(10, 20));


                var handler = new CreatePurchaseOrder.Handler(this.sessionFactory).Handle(request);

                pOrders.Add(handler as Service.Dto.PurchaseOrder);
            }

            return pOrders;
        }

        private List<Service.Dto.PurchaseOrderDetail> CreatePurchaseOrderDetails(Service.Dto.PurchaseOrder po, int count = 1)
        {
            var poDetails = new List<Service.Dto.PurchaseOrderDetail>();

            var randomProductIndexes = this.dummyData.GenerateUniqueNumbers(0, 20, count).ToArray();
            var selectedProducts = this.SelectProducts(randomProductIndexes).ToList();

            for (var i = 0; i < count; i++)
            {
                var product = selectedProducts[i];

                poDetails.Add(new Service.Dto.PurchaseOrderDetail
                {
                   ExtendedPriceAmount = product.RetailPriceAmount + 1m,
                   ProductId = product.Id,
                   PurchaseOrderId = po.Id,
                   Quantity = this.rnd.Next(1, 100),
                   UnitCostAmount = product.BasePriceAmount + 1m
                });
            }


            return poDetails;
        }

        private Service.Dto.PurchaseOrder GetPurchaseOrder(Service.Dto.PurchaseOrderStatus status)
        {
            var request = new GetPurchaseOrderList.Request() { };
            var pOrders = new GetPurchaseOrderList.Handler(this.sessionFactory).Handle(request);

            return pOrders.Where(po => po.Status == Service.Dto.PurchaseOrderStatus.New).ToList()[this.rnd.Next(0, pOrders.Count - 1)];
        }

        private Service.Dto.PurchaseOrder SubmitPurchaseOrder(Service.Dto.PurchaseOrder pOrder)
        {
            var employees = new List<Employee>();

            using (var session = this.sessionFactory.OpenSession())
                employees = session.Query<Employee>().ToList();

            var employeeId = employees[this.rnd.Next(0, employees.Count - 1)].Id;

            var request = new SubmitPurchaseOrder.Request() { Id = pOrder.Id, SubmittedByEmployeeId = employeeId };
            var order = new SubmitPurchaseOrder.Handler(this.sessionFactory).Handle(request);

            return order;
        }

        [Test]
        public void CommonScenarioTests()
        {
            //-----Create Employees-----
            var employees = this.CreateEmployees(5);

            CollectionAssert.IsNotEmpty(employees);
            CollectionAssert.AllItemsAreNotNull(employees);

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

            -Select Products for Purchase Order (New, add to cart functionality)

            -Create Purchase Order(s) - Assert Status, it should be Active orders
            - Transition PO - Assert Status, it should be awaiting approval
            - Approve Purchase Order - Assert Status, it should be awaiting completion
            - Complete Purchases - Assert Status, it should be completed
                */

            //Select Products for Purchase Order (New, add to cart functionality)
            var purchaseOrders = this.CreatePurchaseOrders(10);

            //Create Purchase Order(s) - Assert Status, it should be Active orders
            Assert.IsTrue(purchaseOrders.Count(p => p.Status == Service.Dto.PurchaseOrderStatus.New) == 10);


            //- Submit PO - Assert Status, it should be awaiting approval
            var purchaseOrder = this.GetPurchaseOrder(Service.Dto.PurchaseOrderStatus.New);
            var submittedPurchaseOrder = this.SubmitPurchaseOrder(purchaseOrder);
            Assert.IsTrue(submittedPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.ForApproval);

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
