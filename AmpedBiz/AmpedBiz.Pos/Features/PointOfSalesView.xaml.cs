using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace AmpedBiz.Pos.Features
{
	/// <summary>
	/// Interaction logic for PointOfSaleView.xaml
	/// </summary>
	public partial class PointOfSaleView : ReactiveUserControl<PointOfSalesViewModel>
	{
		public PointOfSaleView()
		{
			InitializeComponent();

			this.WhenActivated(block =>
			{
				this.ViewModel.InitializeCommand.Execute();

				this.Bind(this.ViewModel,
						model => model.PointOfSale.Customer,
						view => view.CustomerComboBox.SelectedItem
					)
					.DisposeWith(block);

				this.OneWayBind(this.ViewModel,
						model => model.Lookups.Customers,
						view => view.CustomerComboBox.ItemsSource
					)
					.DisposeWith(block);

				this.OneWayBind(this.ViewModel,
						model => model.PointOfSale.SalesBy,
						view => view.SalesByTextBox.Text
					)
					.DisposeWith(block);

				this.OneWayBind(this.ViewModel,
						model => model.PointOfSale.InvoiceNumber,
						view => view.InvoiceNumberTextBox.Text
					)
					.DisposeWith(block);

				this.OneWayBind(this.ViewModel,
						model => model.PointOfSale.SalesOn,
						view => view.SalesOnDatePicker.Text
					)
					.DisposeWith(block);

				this.OneWayBind(this.ViewModel,
						model => model.PointOfSale.GrandTotal,
						view => view.GrandTotalTextBlock.Text
					)
					.DisposeWith(block);

				this.BindCommand(this.ViewModel,
						model => model.CreateItemCommand,
						view => view.CreateItemButton
					)
					.DisposeWith(block);

				this.OneWayBind(this.ViewModel,
						model => model.PointOfSale.Items,
						view => view.ItemsDataGridView.ItemsSource
					)
					.DisposeWith(block);

				this.Bind(this.ViewModel,
						model => model.SelectedItem,
						view => view.ItemsDataGridView.SelectedItem
					)
					.DisposeWith(block);

				this.WhenAnyValue(x => x.ItemsDataGridView.SelectedItem)
					.Where(x => x != null)
					.Subscribe(x =>
					{
						//Console.WriteLine(x);
						this.ItemsDataGridView.UpdateLayout();
						this.ItemsDataGridView.ScrollIntoView(x, null);
						//this.ItemsDataGridView.UpdateLayout();
						//this.ItemsDataGridView.ScrollIntoView(x.ItemsDataGridView.SelectedItem, null);
					})
					.DisposeWith(block);
				
			});
		}

		private void ActionColumn_ImageFailed(object sender, System.Windows.ExceptionRoutedEventArgs e)
		{

		}
	}
}
