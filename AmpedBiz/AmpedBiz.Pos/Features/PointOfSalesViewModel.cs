using AmpedBiz.Pos.Common;
using AmpedBiz.Pos.Common.Services;
using AmpedBiz.Common.Extentions;
using DynamicData;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Bogus;
using ReactiveUI.Fody.Helpers;
using AmpedBiz.Pos.Common.Models;
using System.Collections.Generic;

namespace AmpedBiz.Pos.Features
{
    public class PointOfSalesViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private readonly IApiService _api;

		[Reactive] public string UrlPathSegment { get; private set; } = "Sales";

        [Reactive] public IScreen HostScreen { get; private set; }

		[Reactive] public LookupsModel Lookups { get; private set; }

		[Reactive] public PointOfSaleModel PointOfSale { get; private set; }

		[Reactive] public PointOfSaleItemModel SelectedItem { get; private set; }

        public ReactiveCommand<Unit, Unit> InitializeCommand { get; private set; }

        public ReactiveCommand<Unit, Unit> CreateItemCommand { get; private set; }

        public ReactiveCommand<PointOfSaleItemModel, Unit> EditItemCommand { get; private set; }

        public ReactiveCommand<PointOfSaleItemModel, Unit> DeleteItemCommand { get; private set; }

        public PointOfSalesViewModel(IScreen screen, IApiService api)
        {
            this._api = api;
            this.HostScreen = screen;

            this.InitializeCommand = ReactiveCommand.CreateFromTask(this.Initialize);
            this.InitializeCommand.ThrownExceptions.Subscribe(exception => this.Log().WarnException("Error!", exception));

            this.CreateItemCommand = ReactiveCommand.CreateFromTask(this.CreateItem);
            this.CreateItemCommand.ThrownExceptions.Subscribe(exception => this.Log().WarnException("Error!", exception));

            this.EditItemCommand = ReactiveCommand.CreateFromTask<PointOfSaleItemModel>(this.EditItem);
            this.EditItemCommand.ThrownExceptions.Subscribe(exception => this.Log().WarnException("Error!", exception));

            this.DeleteItemCommand = ReactiveCommand.CreateFromTask<PointOfSaleItemModel>(this.DeleteItem);
            this.DeleteItemCommand.ThrownExceptions.Subscribe(exception => this.Log().WarnException("Error!", exception));

			this.WhenAnyValue(x => x.SelectedItem)
				.Subscribe(x =>
				{
					Console.WriteLine("Chorva");

				});
				
        }

        private async Task Initialize()
        {
            if (this.PointOfSale == null)
                this.PointOfSale = new PointOfSaleModel();

            this.PointOfSale.Clear();

            this.Lookups = await TaskExtention.WhenAll(
                task1: this._api.Users(),
                task2: this._api.Branches(),
                task3: this._api.Products(),
                task4: this._api.Customers(),
                task5: this._api.UnitOfMeasures(),
                result: (users, branches, products, customers, unitOfMeasures) =>
                {
                    return new LookupsModel(
                        users: users,
                        branches: branches,
                        products: products,
                        customers: customers,
                        unitOfMeasures: unitOfMeasures
                    );
                }
            );

            this.Fake(this.PointOfSale);
        }

        private void Fake(PointOfSaleModel value)
        {
            var generator = new FakeGenerator(this.Lookups);
            value.Branch = generator.RandomBranch();
            value.Customer = generator.RandomCustomer();
            value.InvoiceNumber = generator.RandomInvoiceNumber();
            value.SalesBy = generator.RandomUser();
            value.SalesOn = generator.RandomDate();
			value.ReplaceItems(generator.FakeSaleItems());
        }

        private Task CreateItem()
        {
			var generator = new FakeGenerator(this.Lookups);
			var newItem = generator.FakeSaleItem();
			this.PointOfSale.AddItem(newItem);
			this.SelectedItem = newItem;
            return Task.CompletedTask;
        }

        private Task EditItem(PointOfSaleItemModel item)
        {
            this.SelectedItem = item;
            return Task.CompletedTask;
        }

        private Task DeleteItem(PointOfSaleItemModel item)
        {
            this.PointOfSale.RemoveItem(item);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (this.PointOfSale != null)
                this.PointOfSale.Dispose();
        }
    }

	public class FakeGenerator
	{
		private readonly Faker fake;
		private readonly LookupsModel _lookups;

		public BranchModel RandomBranch() => fake.PickRandom(this._lookups.Branches.AsEnumerable());

		public CustomerModel RandomCustomer() => fake.PickRandom(this._lookups.Customers.AsEnumerable());

		public string RandomInvoiceNumber() => fake.Random.Replace("??-#####");

		public UserModel RandomUser() => fake.PickRandom(this._lookups.Users.AsEnumerable());

		public DateTime RandomDate() => DateTime.Now;

		public FakeGenerator(LookupsModel lookups)
		{
			this.fake = new Faker();
			this._lookups = lookups;
		}

		public PointOfSaleModel FakeSale()
		{
			var fake = new Faker();
			var sale = new PointOfSaleModel();
			sale.Branch = fake.PickRandom(this._lookups.Branches.AsEnumerable());
			sale.Customer = fake.PickRandom(this._lookups.Customers.AsEnumerable());
			sale.InvoiceNumber = fake.Random.Replace("??-#####");
			sale.SalesBy = fake.PickRandom(this._lookups.Users.AsEnumerable());
			sale.SalesOn = DateTime.Now;
			sale.ReplaceItems(this.FakeSaleItems());
			return sale;
		}

		public IEnumerable<PointOfSaleItemModel> FakeSaleItems()
		{
			return fake.Make(fake.Random.Int(1, 2), FakeSaleItem);
		}

		public PointOfSaleItemModel FakeSaleItem()
		{
			var item = new PointOfSaleItemModel();
			item.Product = fake.PickRandom(this._lookups.Products.AsEnumerable());
			item.Barcode = fake.Random.Replace("###############");
			item.Unit = fake.PickRandom(this._lookups.UnitOfMeasures.AsEnumerable());
			item.Quantity = decimal.Round(fake.Random.Int(1, 10), 0, MidpointRounding.AwayFromZero);
			item.Price = decimal.Round(fake.Random.Decimal(1.0M, 1000.00M), 2, MidpointRounding.AwayFromZero);
			return item;
		}
	}
}
