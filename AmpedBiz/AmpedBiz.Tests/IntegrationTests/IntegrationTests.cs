using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Branches;
using AmpedBiz.Service.Customers;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.Orders;
using AmpedBiz.Service.PointOfSales;
using AmpedBiz.Service.Products;
using AmpedBiz.Service.PurchaseOrders;
using AmpedBiz.Service.Suppliers;
using AmpedBiz.Service.Users;
using AmpedBiz.Tests.Bootstrap;
using Autofac;
using Common.Logging;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

		private ISessionFactory sessionFactory = Ioc.Container.Resolve<ISessionFactory>();
		private IMediator mediator = Ioc.Container.Resolve<IMediator>();

		private List<Role> _roles = new List<Role>();
		private List<User> _users = new List<User>();
		private List<Branch> _branches = new List<Branch>();
		private List<Shipper> _shippers = new List<Shipper>();
		private List<Supplier> _suppliers = new List<Supplier>();
		private List<Customer> _customers = new List<Customer>();
		private List<Currency> _currencies = new List<Currency>();
		private List<PaymentType> _paymentTypes = new List<PaymentType>();
		private List<Pricing> _pricings = new List<Pricing>();
		private List<ProductCategory> _productCategories = new List<ProductCategory>();
		private List<UnitOfMeasure> _unitOfMeasures = new List<UnitOfMeasure>();

		private Service.Common.Filter filter = new Service.Common.Filter();
		private Service.Common.Pager pager = new Service.Common.Pager() { Offset = 1, Size = 1000 };
		private Service.Common.Sorter sorter = new Service.Common.Sorter() { };

		public IntegrationTests()
		{
			Console.WriteLine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath);
		}

		[OneTimeSetUp]
		public void SetupTest()
		{
			var log = LogManager.GetLogger<IntegrationTests>();
			log.Error("log me like you do");

			var config = DatabaseConfig.Instance.Seeder;

			Ioc.Container.Resolve<IMapper>().Initialze();
			Ioc.Container.Resolve<Runner>().Run(config);

			this.LoadLookups();
		}

		[OneTimeTearDown]
		public void TeardownTest()
		{
			this.sessionFactory = null;
		}

		private void LoadLookups()
		{
			using (var session = this.sessionFactory.OpenSession())
			{
				this._roles = session.Query<Role>().Cacheable().ToList();
				this._users = session.Query<User>().Cacheable().ToList();
				this._branches = session.Query<Branch>().Cacheable().ToList();
				this._shippers = session.Query<Shipper>().Cacheable().ToList();
				this._customers = session.Query<Customer>().Cacheable().ToList();
				this._currencies = session.Query<Currency>().Cacheable().ToList();
				this._paymentTypes = session.Query<PaymentType>().Cacheable().ToList();
				this._pricings = session.Query<Pricing>().Cacheable().ToList();
				this._productCategories = session.Query<ProductCategory>().Cacheable().ToList();
				this._unitOfMeasures = session.Query<UnitOfMeasure>().Cacheable().ToList();
				this._suppliers = session.Query<Supplier>().Where(x => x.Products.Count() > 0).ToList();
			}
		}

		#region Common Helpers

		private Service.Dto.ProductInventoryUnitOfMeasure RandomProductUnitOfMeasure(Service.Dto.ProductInventory productInventory)
		{
			var random = productInventory.UnitOfMeasures[this.random.Next(0, productInventory.UnitOfMeasures.Count - 1)];
			return random;
		}

		private Service.Dto.ProductInventoryUnitOfMeasurePrice RandomProductUnitOfMeasurePrice(Service.Dto.ProductInventoryUnitOfMeasure productUnitOfMeasure)
		{
			var random = productUnitOfMeasure.Prices[this.random.Next(0, productUnitOfMeasure.Prices.Count - 1)];
			return random;
		}

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

		private Lookup<Guid> RandomBranch()
		{
			var random = this._branches[this.random.Next(0, this._branches.Count - 1)];
			return new Lookup<Guid>(random.Id, random.Name);
		}

		private Lookup<string> RandomShipper()
		{
			var random = this._shippers[this.random.Next(0, this._shippers.Count - 1)];
			return new Lookup<string>(random.Id, random.Name);
		}

		private Lookup<Guid> RandomCustomer()
		{
			var random = this._customers[this.random.Next(0, this._customers.Count - 1)];
			return new Lookup<Guid>(random.Id, random.Name);
		}

		private Lookup<Guid> RandomSupplier()
		{
			var random = this._suppliers[this.random.Next(0, this._suppliers.Count - 1)];
			return new Lookup<Guid>(random.Id, random.Name);
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

		private Lookup<string> RandomPricing()
		{
			var random = this._pricings[this.random.Next(0, this._pricings.Count - 1)];
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

		private async Task<Service.Dto.Branch> CreateBranch(Service.Dto.Branch branch)
		{
			var response = await this.mediator.Send(new GetBranch.Request() { Id = Branch.Default.Id });

			return response as Service.Dto.Branch;
		}

		private async Task<List<Service.Dto.User>> CreateUsers(int count = 1)
		{
			var users = new List<Service.Dto.User>();
			var branch = await this.CreateBranch(this.dummyData.GenerateBranch());

			var tasks = Enumerable.Range(0, count)
				.Select(index => this.mediator.Send(new CreateUser.Request()
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
				}));

			var response = await Task.WhenAll(tasks);

			users = response.Cast<Service.Dto.User>().ToList();

			using (var session = sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				this._users = session.Query<User>().ToList();
				transaction.Commit();
			}

			return users;
		}

		private async Task<List<Service.Dto.Customer>> CreateCustomers(int count = 1)
		{
			var customers = new List<Service.Dto.Customer>();

			//var tasks = new List<Task<CreateCustomer.Response>>();

			//for (var i = 0; i < count; i++)
			//{
			//    var custData = this.dummyData.GenerateCustomer();
			//    custData.PricingId = this._pricings[this.random.Next(0, this._pricings.Count - 1)].Id;

			//    tasks.Add(this.mediator.Send(new CreateCustomer.Request()
			//    {
			//        Id = custData.Id,
			//        Contact = custData.Contact,
			//        BillingAddress = custData.BillingAddress,
			//        CreditLimitAmount = custData.CreditLimitAmount,
			//        Name = custData.Name,
			//        Code = custData.Code,
			//        OfficeAddress = custData.OfficeAddress,
			//        PricingId = custData.PricingId
			//    }));

			//    //var customer = await mediator.Send(request);

			//    //customers.Add(customer as Service.Dto.Customer);
			//}

			var tasks = Enumerable.Range(0, count)
				.Select(x => this.dummyData.GenerateCustomer())
				.Select(x => this.mediator.Send(new CreateCustomer.Request()
				{
					Id = x.Id,
					Contact = x.Contact,
					BillingAddress = x.BillingAddress,
					CreditLimitAmount = x.CreditLimitAmount,
					Name = x.Name,
					Code = x.Code,
					OfficeAddress = x.OfficeAddress,
					PricingId = this._pricings[this.random.Next(0, this._pricings.Count - 1)].Id
				}));

			var responses = await Task.WhenAll(tasks);

			customers = responses.Cast<Service.Dto.Customer>().ToList();

			using (var session = sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				this._customers = session.Query<Customer>().ToList();
				transaction.Commit();
			}

			return customers;
		}

		private async Task<List<Service.Dto.Supplier>> CreateSuppliers(int count = 1)
		{
			var suppliers = new List<Service.Dto.Supplier>();

			var tasks = Enumerable.Range(0, count)
				.Select(x => this.dummyData.GenerateSupplier())
				.Select(x => this.mediator.Send(new CreateSupplier.Request()
				{
					Id = x.Id,
					Address = x.Address,
					Contact = x.Contact,
					Name = x.Name
				}));


			//for (var i = 0; i < count; i++)
			//{
			//    var supplierData = this.dummyData.GenerateSupplier();

			//    tasks.Add(this.mediator.Send(new CreateSupplier.Request()
			//    {
			//        Id = supplierData.Id,
			//        Address = supplierData.Address,
			//        Contact = supplierData.Contact,
			//        Name = supplierData.Name
			//    }));
			//}

			var responses = await Task.WhenAll(tasks);

			suppliers = responses.Cast<Service.Dto.Supplier>().ToList();

			using (var session = sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				this._suppliers = session.Query<Supplier>().ToList();
				transaction.Commit();
			}

			return suppliers;
		}

		private async Task<List<Service.Dto.Product>> CreateProducts(int count = 1)
		{
			var products = new List<Service.Dto.Product>();
			var suppliers = new List<Supplier>();

			using (var session = this.sessionFactory.OpenSession())
			{
				suppliers = session.Query<Supplier>().Cacheable().ToList();
			}

			var tasks = Enumerable.Range(0, count)
				.Select(x => this.dummyData.GenerateProduct(
					category: this.RandomProductCategory(),
					supplier: this.RandomSupplier()
				))
				.Select(productData => this.mediator.Send(new CreateProduct.Request()
				{
					Id = productData.Id,
					Category = productData.Category,
					Description = productData.Description,
					Discontinued = productData.Discontinued,
					Image = productData.Image,
					Name = productData.Name,
					Supplier = productData.Supplier,
					Inventory = new Service.Dto.Inventory()
					{
						InitialLevelValue = productData.Inventory.InitialLevelValue,
						BackOrderedValue = productData.Inventory.BackOrderedValue,
					},
					UnitOfMeasures = new List<Service.Dto.ProductUnitOfMeasure>()
					{
						new Service.Dto.ProductUnitOfMeasure()
						{
							Size = "Size",
							IsDefault = true,
							IsStandard = true,
							StandardEquivalentValue = 1,
							UnitOfMeasure = new Func<Service.Dto.UnitOfMeasure>(() =>
							{
								var uom = this.RandomUnitOfMeasure();
								return new Service.Dto.UnitOfMeasure()
								{
									Id = uom.Id,
									Name = uom.Name
								};
							})(),
							Prices = this._pricings
								.Select(x => new Service.Dto.ProductUnitOfMeasurePrice()
								{
									Pricing = new Lookup<string>(x.Id, x.Name),
									PriceAmount = this.random.Next(20, 40),
								})
								.ToList()
						},
					}
				}));

			var response = await Task.WhenAll(tasks);

			products = response.Cast<Service.Dto.Product>().ToList();

			return products;
		}

		private async Task<IEnumerable<Service.Dto.Product>> SelectRandomAvailableProducts(Guid supplierId, int count = 12)
		{
			var response = await this.mediator.Send(new GetProductList.Request() { SupplierId = supplierId });

			var availableProducts = response
				.Where(x => x.Inventory.AvailableValue > 0);

			var totalProduct = availableProducts.Count();

			count = totalProduct < count ? totalProduct : count;

			var randomIndexs = this.dummyData.GenerateUniqueNumbers(0, totalProduct, count);

			var products = availableProducts
				.Select((product, index) => new
				{
					Index = index,
					Product = product
				})
				.Where(x => randomIndexs.Contains(x.Index))
				.Select(x => x.Product);

			return products;
		}

		private async Task<IEnumerable<Service.Dto.ProductInventory>> SelectRandomProductInventories(Guid supplierId, int count = 12)
		{
			var response = await this.mediator.Send(new GetProductInventoryList.Request() { SupplierId = supplierId });
			var productInventories = response.Cast<Service.Dto.ProductInventory>();

			var totalProductInventory = productInventories.Count();

			count = totalProductInventory < count ? totalProductInventory : count;

			var randomIndexs = this.dummyData.GenerateUniqueNumbers(0, totalProductInventory, count);

			var result = productInventories
				.Select((productInventory, index) => new
				{
					Index = index,
					ProductInventory = productInventory
				})
				.Where(x => randomIndexs.Contains(x.Index))
				.Select(x => x.ProductInventory);

			return result;
		}

		private async Task<IEnumerable<Service.Dto.ProductInventory>> SelectRandomAvailableProductInventories(Guid supplierId, int count = 12)
		{
			var response = await this.mediator.Send(new GetProductInventoryList.Request() { SupplierId = supplierId });
			var availableProductInventories = response.Where(x =>
				x.UnitOfMeasures != null &&
				x.UnitOfMeasures.Any() &&
				x.UnitOfMeasures.All(o => o.Available?.Value >= 1)
			);

			var totalProductInventory = availableProductInventories.Count();

			count = totalProductInventory < count ? totalProductInventory : count;

			var randomIndexs = this.dummyData.GenerateUniqueNumbers(0, totalProductInventory, count);

			var productInventories = availableProductInventories
				.Select((productInventory, index) => new
				{
					Index = index,
					ProductInventory = productInventory
				})
				.Where(x => randomIndexs.Contains(x.Index))
				.Select(x => x.ProductInventory)
				.ToList();

			return productInventories;
		}

		private async Task<IEnumerable<Service.Dto.Product>> SelectRandomProducts(Guid supplierId, int count = 12)
		{
			var response = await this.mediator.Send(new GetProductList.Request() { SupplierId = supplierId });

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

		private async Task<List<Service.Dto.PurchaseOrder>> CreatePurchaseOrders(int count = 1)
		{
			var orders = new List<Service.Dto.PurchaseOrder>();

			var tasks = Enumerable.Range(0, count)
				.Select(x => new SavePurchaseOrder.Request()
				{
					CreatedBy = this.RandomUser(),
					CreatedOn = DateTime.Now,
					ExpectedOn = DateTime.Now.AddDays(10),
					Supplier = RandomSupplier(),
					Shipper = null,
					ShippingFeeAmount = 0M,
					TaxAmount = 0M,
					PaymentType = RandomPaymentType(),
				})
				.Select(async x =>
				{
					x.Items = await this.CreatePurchaseOrderItems(
						request: x,
						count: this.random.Next(20, 90)
					);

					return x;
				})
				.Select(async x => await this.mediator.Send(await x));

			//for (var i = 0; i < count; i++)
			//{
			//    tasks.Add(this.mediator.Send(new SavePurchaseOrder.Request()
			//    {
			//        CreatedBy = this.RandomUser(),
			//        CreatedOn = DateTime.Now,
			//        ExpectedOn = DateTime.Now.AddDays(10),
			//        Supplier = RandomSupplier(),
			//        Shipper = null,
			//        ShippingFeeAmount = 0M,
			//        TaxAmount = 0M,
			//        PaymentType = RandomPaymentType(),
			//    }))
			//    ;
			//    request.Items = this.CreatePurchaseOrderItems(request, this.random.Next(20, 90));

			//    var handler = new SavePurchaseOrder.Handler().With(this.sessionFactory, DefaultContext.Instance).Execute(request);

			//    orders.Add(handler as Service.Dto.PurchaseOrder);
			//}

			var response = await Task.WhenAll(tasks);

			orders = response.Cast<Service.Dto.PurchaseOrder>().ToList();

			return orders;
		}

		private async Task<List<Service.Dto.PurchaseOrderItem>> CreatePurchaseOrderItems(SavePurchaseOrder.Request request, int count = 1)
		{
			var poItems = new List<Service.Dto.PurchaseOrderItem>();
			var productInventories = await this.SelectRandomProductInventories(request.Supplier.Id, count);

			foreach (var productInventory in productInventories)
			{
				var productInventoryUnitOfMeasure = this.RandomProductUnitOfMeasure(productInventory);
				var productInventoryUnitOfMeasurePrice = this.RandomProductUnitOfMeasurePrice(productInventoryUnitOfMeasure);

				poItems.Add(new Service.Dto.PurchaseOrderItem()
				{
					PurchaseOrderId = request.Id,
					Product = new Lookup<Guid>(productInventory.Id, productInventory.Name),
					Quantity = new Service.Dto.Measure()
					{
						Value = this.random.Next(1, 100),
						Unit = productInventoryUnitOfMeasure.UnitOfMeasure
					},
					Standard = new Service.Dto.Measure()
					{
						Unit = productInventoryUnitOfMeasure.Standard?.Unit,
						Value = productInventoryUnitOfMeasure.Standard?.Value ?? 0M
					},
					UnitCostAmount = productInventoryUnitOfMeasurePrice.PriceAmount.Value,
				});
			}


			return poItems;
		}

		private async Task<IEnumerable<Service.Dto.PurchaseOrder>> GetPurchaseOrders(Service.Dto.PurchaseOrderStatus status, int count = 1)
		{
			var purchaseOrders = await this.mediator.Send(new GetPurchaseOrderList.Request() { });

			return purchaseOrders.Where(po => po.Status == status).Take(count).ToList();
		}

		private async Task<Service.Dto.PurchaseOrder> SubmitPurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
		{
			var order = await this.mediator.Send(new SubmitPurchaseOrder.Request()
			{
				Id = purchaseOrder.Id,
				SubmittedBy = this.RandomUser()
			});

			return order;
		}

		private async Task<Service.Dto.PurchaseOrder> ApprovePurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
		{
			var order = await this.mediator.Send(new ApprovePurchaseOder.Request()
			{
				Id = purchaseOrder.Id,
				ApprovedBy = this.RandomUser()
			});

			return order;
		}

		private async Task<Service.Dto.PurchaseOrder> PayPurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
		{
			var order = await this.mediator.Send(new SavePurchaseOrder.Request()
			{
				Id = purchaseOrder.Id,
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
			});

			return order;
		}

		private async Task<Service.Dto.PurchaseOrder> ReceivePurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
		{
			var order = await this.mediator.Send(new SavePurchaseOrder.Request()
			{
				Id = purchaseOrder.Id,
				Receipts = purchaseOrder.Items
					.Select(x => new Service.Dto.PurchaseOrderReceipt()
					{
						PurchaseOrderId = purchaseOrder.Id,
						ReceivedBy = this.RandomUser(),
						ReceivedOn = DateTime.Now,
						ExpiresOn = DateTime.Now.AddYears(1),
						Product = x.Product,
						Quantity = x.Quantity,
						Standard = x.Standard
					})
			});

			return order;
		}

		private async Task<Service.Dto.PurchaseOrder> CompletePurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
		{
			var order = await this.mediator.Send(new CompletePurchaseOder.Request()
			{
				Id = purchaseOrder.Id,
				CompletedBy = this.RandomUser()
			});

			return order;
		}

		private async Task<Service.Dto.PurchaseOrder> CancelPurchaseOrder(Service.Dto.PurchaseOrder purchaseOrder)
		{
			var order = await this.mediator.Send(new CancelPurchaseOder.Request()
			{
				Id = purchaseOrder.Id,
				CancelledBy = this.RandomUser(),
				CancellationReason = "Products not needed"
			});

			return order;
		}

		#endregion

		#region Orders Helper

		private async Task<List<Service.Dto.Order>> CreateOrders(int count = 1)
		{
			var orders = new List<Service.Dto.Order>();
			var users = new List<User>();
			var branches = new List<Branch>();
			var customers = await this.CreateCustomers(10);
			var tasks = new List<Task<SaveOrder.Response>>();

			using (var session = this.sessionFactory.OpenSession())
			{
				users = session.Query<User>().Cacheable().ToList();
				branches = session.Query<Branch>().Cacheable().ToList();
			}

			for (var i = 0; i < count; i++)
			{
				tasks.Add(this.mediator.Send(new SaveOrder.Request()
				{
					CreatedBy = RandomUser(),
					Branch = RandomBranch(),
					Customer = RandomCustomer(),
					TaxRate = .12M,
					PaymentType = RandomPaymentType(),
					Items = await this.CreateOrderItems(this.random.Next(3, 6))
				}));

				//if (request.Items.Count() == 0)
				//    Console.WriteLine("Hey");

				//tasks.Add(this.mediator.Send(request));

				//var handler = new SaveOrder.Handler().With(this.sessionFactory, DefaultContext.Instance).Execute(request);

				//orders.Add(handler as Service.Dto.Order);
			}

			//var tasks = Enumerable.Range(0, count)
			//    .Select(x => this.mediator.Send(new SaveOrder.Request()
			//    {
			//        CreatedBy = RandomUser(),
			//        Branch = RandomBranch(),
			//        Customer = RandomCustomer(),
			//        TaxRate = .12M,
			//        PaymentType = RandomPaymentType(),
			//        Items = await this.CreateOrderItems(this.random.Next(3, 6))
			//    }));

			var responses = await Task.WhenAll(tasks);

			orders = responses.Cast<Service.Dto.Order>().ToList();

			return orders;
		}

		private async Task<List<Service.Dto.OrderItem>> CreateOrderItems(int count = 1)
		{
			var orderItems = new List<Service.Dto.OrderItem>();

			var suppliersId = new List<Guid>();

			using (var session = this.sessionFactory.OpenSession())
			{
				suppliersId = session
					.Query<Supplier>()
					.Where(x => x.Products.Any())
					.Select(x => x.Id)
					.ToList();

				if (suppliersId.Count < 1)
				{
					var suppliers = await this.CreateSuppliers(10);

					suppliersId = suppliers
						.Select(x => x.Id)
						.ToList();
				}
			}

			var randomProductIndexes = this.dummyData.GenerateUniqueNumbers(0, count, count).ToArray();

			comeAsYouAre:
			var selectedProductInventories = await this.SelectRandomAvailableProductInventories(suppliersId[this.random.Next(0, suppliersId.Count - 1)], 10);

			if (selectedProductInventories.Count() == 0)
				goto comeAsYouAre;

			foreach (var productInventory in selectedProductInventories)
			{
				var productInventoryUnitOfMeasure = this.RandomProductUnitOfMeasure(productInventory);
				var productInventoryUnitOfMeasurePrice = this.RandomProductUnitOfMeasurePrice(productInventoryUnitOfMeasure);

				orderItems.Add(new Service.Dto.OrderItem()
				{
					ExtendedPriceAmount = 0M,
					Product = new Lookup<Guid>(productInventory.Id, productInventory.Name),
					Quantity = new Service.Dto.Measure()
					{
						Unit = productInventoryUnitOfMeasure.UnitOfMeasure,
						Value = this.random.Next(1, (int)(productInventoryUnitOfMeasure?.Available?.Value ?? 0M))
					},
					Standard = new Service.Dto.Measure()
					{
						Unit = productInventoryUnitOfMeasure.Standard?.Unit,
						Value = productInventoryUnitOfMeasure.Standard?.Value ?? 0M
					},
					UnitPriceAmount = productInventoryUnitOfMeasurePrice.PriceAmount ?? 0M
				});
			}

			return orderItems;
		}

		private async Task<Service.Dto.Order> InvoiceOrder(Service.Dto.Order order)
		{
			await this.AdjustOrderByProductAvailability(order);

			var response = await this.mediator.Send(new InvoiceOrder.Request()
			{
				Id = order.Id,
				InvoicedBy = RandomUser()
			});

			return response;
		}

		private async Task AdjustOrderByProductAvailability(Service.Dto.Order order)
		{
			using (var session = sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var productIds = order.Items.Select(x => x.Product.Id).ToArray();
				var productInventories = await this.mediator.Send(new GetProductInventoryList.Request() { ProductIds = productIds });

				productInventories.ForEach(productInventory =>
				{
					var item = order.Items.FirstOrDefault(x => x.Product.Id == productInventory.Id);
					var productInventorUnitOfMeasure = productInventory.UnitOfMeasures.FirstOrDefault(x => x.UnitOfMeasure.Id == item.Quantity.Unit.Id);
					if (productInventorUnitOfMeasure.Available.Value == 0)
					{
						order.Items.Remove(item);
						return;
					}

					if (productInventorUnitOfMeasure.Available.Value < item.Quantity.Value)
					{
						item.Quantity.Value = productInventorUnitOfMeasure.Available.Value;
						return;
					}
				});
			}

			if (order.Items.Count() == 0)
				Console.WriteLine("Hey");

			var response = await this.mediator.Send(new SaveOrder.Request()
			{
				Id = order.Id,
				CreatedBy = order.CreatedBy,
				Branch = order.Branch,
				Customer = order.Customer,
				TaxRate = order.TaxRate,
				PaymentType = order.PaymentType,
				Items = order.Items
			});
		}

		private async Task<Service.Dto.Order> StageOrder(Service.Dto.Order order)
		{
			var response = await this.mediator.Send(new StageOrder.Request()
			{
				Id = order.Id,
				StagedBy = RandomUser()
			});

			return response;
		}

		private async Task<Service.Dto.Order> PayOrder(Service.Dto.Order order)
		{
			var response = await this.mediator.Send(new SaveOrder.Request()
			{
				Id = order.Id,
				Payments = new Service.Dto.OrderPayment[]
				{
					new Service.Dto.OrderPayment()
					{
						PaidOn = DateTime.Now,
						PaidTo = RandomUser(),
						PaymentType = RandomPaymentType(),
						PaymentAmount = order.TotalAmount
					}
				}
			});

			return response;
		}

		private async Task<Service.Dto.Order> ShipOrder(Service.Dto.Order order)
		{
			var response = await this.mediator.Send(new ShipOrder.Request()
			{
				Id = order.Id,
				ShippedBy = RandomUser()
			});

			return response;
		}

		private async Task<Service.Dto.Order> RouteOrder(Service.Dto.Order order)
		{
			var response = await this.mediator.Send(new RouteOrder.Request()
			{
				Id = order.Id,
				RoutedBy = RandomUser()
			});

			return response;
		}

		private async Task<Service.Dto.Order> CompleteOrder(Service.Dto.Order order)
		{
			var response = await this.mediator.Send(new CompleteOrder.Request()
			{
				Id = order.Id,
				CompletedBy = RandomUser()
			});

			return response;
		}

		#endregion

		#region Point Of Sale

		private async Task<List<Service.Dto.PointOfSale>> CreatePointOfSales(int count = 1)
		{
			var requests = Enumerable.Range(0, count)
				.Select(x => new SavePointOfSale.Request()
				{
					Branch = this.RandomBranch(),
					Customer = this.RandomCustomer(),
					Pricing = this.RandomPricing(),
					PaymentType = this.RandomPaymentType(),
					TenderedBy = this.RandomUser(),
					TenderedOn = DateTime.Now,
					CreatedBy = this.RandomUser(),
					CreatedOn = DateTime.Now,
				});

			requests.ForEach(async request => request.Items = await this.CreatePointOfSaleItems(6));

			requests.ForEach(request => request.Items = this.CreatePointOfSalePayments());

			var response = await Task.WhenAll(requests.Select(x => this.mediator.Send(x)));

			return response.Cast<Service.Dto.PointOfSale>().ToList();
		}

		private async Task<List<Service.Dto.PointOfSaleItem>> CreatePointOfSaleItems(int count = 1)
		{
			var inventories = default(IEnumerable<Service.Dto.ProductInventory>);

			while (inventories == null || !inventories.Any())
			{
				var supplier = this.RandomSupplier();
				inventories = await this.SelectRandomAvailableProductInventories(supplier.Id, 8);
			}

			var items = new List<Service.Dto.PointOfSaleItem>();

			foreach (var inventory in inventories)
			{
				var productUnit = this.RandomProductUnitOfMeasure(inventory);
				var productUnitPrice = this.RandomProductUnitOfMeasurePrice(productUnit);

				items.Add(new Service.Dto.PointOfSaleItem()
				{
					Product = new Lookup<Guid>(inventory.Id, inventory.Name),
					Quantity = new Service.Dto.Measure()
					{
						Unit = productUnit.UnitOfMeasure,
						Value = this.random.Next(1, (int)(productUnit?.Available?.Value ?? 0M))
					},
					Standard = new Service.Dto.Measure()
					{
						Unit = productUnit.Standard?.Unit,
						Value = productUnit.Standard?.Value ?? 0M
					},
					UnitPriceAmount = productUnitPrice.PriceAmount ?? 0M
				});
			}

			return items;
		}

		private List<Service.Dto.PointOfSaleItem> CreatePointOfSalePayments(int count = 5)
		{
			return null;
		}

		#endregion

		[Test]
		public async Task CommonScenarioTests()
		{
			//-----Create Users -----
			var users = await this.CreateUsers(5);
			CollectionAssert.IsNotEmpty(users);
			CollectionAssert.AllItemsAreNotNull(users);

			//-----Create Customers-----
			var customers = await this.CreateCustomers(10);
			CollectionAssert.IsNotEmpty(customers);
			CollectionAssert.AllItemsAreNotNull(customers);

			//Create Suppliers
			var suppliers = await this.CreateSuppliers(10);
			CollectionAssert.IsNotEmpty(suppliers);
			CollectionAssert.AllItemsAreNotNull(suppliers);

			//Create Product Categories
			//provided by default seeder

			//Create Products
			var products = await this.CreateProducts(20);
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
			var purchaseOrders = await this.CreatePurchaseOrders(expected);

			//Create Purchase Order(s) - Assert Status, it should be NEW orders
			var actual = purchaseOrders.Count(p => p.Status == Service.Dto.PurchaseOrderStatus.Created);
			Assert.AreEqual(expected, expected);

			//Get list of created purchaseorders
			var purchaseOrderList = await this.GetPurchaseOrders(Service.Dto.PurchaseOrderStatus.Created, 2);

			var purchaseOrder1 = purchaseOrderList.First();
			var purchaseOrder2 = purchaseOrderList.Last();

			//- Submit PO - Assert Status, it should be Submitted
			var submittedPurchaseOrder1 = await this.SubmitPurchaseOrder(purchaseOrder1);
			Assert.IsTrue(submittedPurchaseOrder1.Status == Service.Dto.PurchaseOrderStatus.Submitted);

			var submittedPurchaseOrder2 = await this.SubmitPurchaseOrder(purchaseOrder2);
			Assert.IsTrue(submittedPurchaseOrder2.Status == Service.Dto.PurchaseOrderStatus.Submitted);

			//- Approve PO - Assert Status, it should be APPROVED
			var approvedPurchaseOrder = await this.ApprovePurchaseOrder(purchaseOrder1);
			Assert.IsTrue(approvedPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Approved);

			//- Reject Submit PO - Assert Status, it should be rejected
			var rejectedPurchaseOrder = await this.CancelPurchaseOrder(purchaseOrder2);
			Assert.IsTrue(rejectedPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Cancelled);

			//- Pay PO - Assert Status, it should be PAID
			var paidPurchaseOrder = await this.PayPurchaseOrder(purchaseOrder1);
			//Assert.IsTrue(paidPurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Paid);

			//- Receive PO - Assert Status, it should be RECEIVED
			var receivePurchaseOrder = await this.ReceivePurchaseOrder(purchaseOrder1);
			//Assert.IsTrue(receivePurchaseOrder.Status == Service.Dto.PurchaseOrderStatus.Received);

			//- Complete Purchases - Assert Status, it should be completed
			var completedPurchaseOrder = await this.CompletePurchaseOrder(purchaseOrder1);
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
			var newOrders = await this.CreateOrders(expected);
			Assert.GreaterOrEqual(expected, newOrders.Count(o => o.Status == Service.Dto.OrderStatus.Created));
			//Assert.False(newOrders.SelectMany(o => o.Items.Where(x => x != null))
			//    .Any(i => i.Status != Service.Dto.OrderItemStatus.Allocated));

			Service.Dto.Order
				order1 = newOrders[0],
				order2 = newOrders[1],
				order3 = newOrders[2],
				order4 = newOrders[3];

			// invoice
			var invoicedOrder = await this.InvoiceOrder(order1);
			Assert.IsTrue(invoicedOrder.Status == Service.Dto.OrderStatus.Invoiced);

			// pay
			var paidOrder = await this.PayOrder(invoicedOrder);
			Assert.IsTrue(paidOrder.Payments.Any());

			// staged
			var stagedOrder = await this.StageOrder(paidOrder);
			Assert.IsTrue(stagedOrder.Status == Service.Dto.OrderStatus.Staged);

			//var routedOrder = this.RouteOrder(stagedOrder);
			//Assert.IsTrue(routedOrder.Status == Service.Dto.OrderStatus.Routed);

			//invoice from payment
			var invoicedOrder2 = await this.PayOrder(await this.InvoiceOrder(order2));
			Assert.IsTrue(invoicedOrder2.Payments.Any());

			//invoice from payment
			var invoicedOrder3 = await this.PayOrder(await this.InvoiceOrder(order3));
			Assert.IsTrue(invoicedOrder3.Payments.Any());

			//complete all
			var completeOrder = await this.CompleteOrder(await this.ShipOrder(paidOrder));
			Assert.IsTrue(completeOrder.Status == Service.Dto.OrderStatus.Completed);

			//todo: implement inventory checking

		}
	}
}