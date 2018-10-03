using AmpedBiz.Pos.Common.Models;
using DynamicData;
using DynamicData.Aggregation;
using DynamicData.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace AmpedBiz.Pos.Features
{
	public class PointOfSaleModel : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _disposer = new CompositeDisposable();

		private readonly SourceList<PointOfSaleItemModel> _itemSource = new SourceList<PointOfSaleItemModel>();

		[Reactive] public Branch Branch { get; set; }

        [Reactive] public Customer Customer { get; set; }

        [Reactive] public string InvoiceNumber { get; set; }

        [Reactive] public User SalesBy { get; set; }

        [Reactive] public DateTime SalesOn { get; set; }

		[Reactive] public ReactiveList<PointOfSaleItemModel> Items { get; private set; } = new ReactiveList<PointOfSaleItemModel>();

        public decimal GrandTotal { [ObservableAsProperty] get; }

		public void AddItem(PointOfSaleItemModel item) => this._itemSource.Add(item);

		public void RemoveItem(PointOfSaleItemModel item) => this._itemSource.Remove(item);

		public void ReplaceItems(IEnumerable<PointOfSaleItemModel> items)
		{
			this._itemSource.Edit(values =>
			{
				values.Clear();
				values.AddRange(items);
			});
		}

		public PointOfSaleModel()
        {
			var observable = this._itemSource.Connect();

			var d1 = observable.ObserveOn(RxApp.MainThreadScheduler)
				.Bind(this.Items)
				.DisposeMany()
				.Subscribe();

			var itemsObservable = new
			{
				UpdateChanges = observable.WhenAnyPropertyChanged()
					.Select(x => this._itemSource.Items),
				AddRemoveChanges = observable.QueryWhenChanged()
			};

			var d2 = Observable.Merge(
					itemsObservable.UpdateChanges,
					itemsObservable.AddRemoveChanges
				)
				.Select(x => x.Sum(o => o.Total))
				.ToPropertyEx(this, x => x.GrandTotal);

			this._disposer = new CompositeDisposable(d1, d2);
        }

        public void Clear()
        {
            this.Branch = null;
            this.Customer = null;
            this.InvoiceNumber = null;
            this.SalesBy = null;
            this.SalesOn = DateTime.Now;
            this._itemSource.Clear();
        }

        public void Dispose() => this._disposer.Dispose();
    }

    public class PointOfSaleItemModel : ReactiveObject
    {
        [Reactive] public Product Product { get; set; }

        [Reactive] public string Barcode { get; set; }

        [Reactive] public UnitOfMeasure Unit { get; set; }

        [Reactive] public decimal Quantity { get; set; }

        [Reactive] public decimal Price { get; set; }

        public extern decimal Total { [ObservableAsProperty]get; }

        public PointOfSaleItemModel()
        {
            this.WhenAnyValue(
                    (model) => model.Quantity,
                    (model) => model.Price,
                    (quantity, price) => quantity * price
                )
                .ToPropertyEx(this, x => x.Total);
        }
    }

    public class LookupsModel : ReactiveObject
    {
        [Reactive] public IReadOnlyCollection<User> Users { get; set; }

        [Reactive] public IReadOnlyCollection<Branch> Branches { get; set; }

        [Reactive] public IReadOnlyCollection<Product> Products { get; set; }

        [Reactive] public IReadOnlyCollection<Customer> Customers { get; set; }

        [Reactive] public IReadOnlyCollection<UnitOfMeasure> UnitOfMeasures { get; set; }

        public LookupsModel(
            IReadOnlyCollection<User> users,
            IReadOnlyCollection<Branch> branches,
            IReadOnlyCollection<Product> products,
            IReadOnlyCollection<Customer> customers,
            IReadOnlyCollection<UnitOfMeasure> unitOfMeasures)
        {
            Users = users;
            Branches = branches;
            Products = products;
            Customers = customers;
            UnitOfMeasures = unitOfMeasures;
        }
    }
}
