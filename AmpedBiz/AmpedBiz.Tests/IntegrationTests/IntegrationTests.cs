using System;
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
        private ISessionFactory sessionFactory;
        private IAuditProvider auditProvider;

        public IntegrationTests()
        {
        }

        [TestFixtureSetUp]
        public void SetupTest()
        {
            this.auditProvider = new AuditProvider();
            this.sessionFactory = new SessionProvider(new ValidatorEngine(), this.auditProvider).SessionFactory;
        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            this.auditProvider = null;
            this.sessionFactory = null;
        }

        private Service.Dto.Branch PersistBranch(Service.Dto.Branch branch)
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
           
        private Service.Dto.User PersistUser(Service.Dto.User user)
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

        private Service.Dto.Employee PersistEmployee(Service.Dto.Employee employee)
        {
            var branch = this.PersistBranch(this.dummyData.GenerateBranch());
            var dummyUser = this.dummyData.GenerateUser();
            dummyUser.BranchId = branch.Id;

            var user = this.PersistUser(dummyUser);

            var request = new CreateEmployee.Request()
            {
                Id = employee.Id,
                Contact = employee.Contact,
                EmployeeTypeId = employee.EmployeeTypeId,
                User = user
            };

            var handler = new CreateEmployee.Handler(this.sessionFactory).Handle(request);

            return handler as Service.Dto.Employee;
        }



        [Test]
        public void CommonScenarioTests()
        {
            //Create Employees
            var employee = this.PersistEmployee(new Service.Dto.Employee()
            {
                Id = dummyData.GenerateUniqueString("Id"),
                Contact = dummyData.GenerateContact(),
                EmployeeTypeId = EmployeeType.Admin.Id
            });

            //Create Customers
            //Create Suppliers
            //Create Product Categories
            //Create Products

        }

        [Test]
        public void Test1()
        {
            //var pc = new CreateProductCategory.Request() { Id = Guid.NewGuid().ToString().Substring(0, 30), Name = "Name_" + Guid.NewGuid().ToString() };
            //var handler = new CreateProductCategory.Handler(new SessionProvider(new ValidatorEngine(), new AuditProvider()).SessionFactory);
            //var result = handler.Handle(pc);
        }
    }
}
