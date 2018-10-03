using System;
using System.Windows;
using System.Windows.Controls;

namespace AmpedBiz.Pos.Common.AttachedProperties
{
	public static class ScrollToViewAttachedProperty
	{
		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
			"SelectedItem",
			typeof(object),
			typeof(ScrollToViewAttachedProperty),
			new PropertyMetadata(default(object), OnSelectedItemChanged));

		public static object GetSelectedItem(DependencyObject target)
		{
			return target.GetValue(SelectedItemProperty);
		}

		public static void SetSelectedItem(DependencyObject target, object value)
		{
			target.SetValue(SelectedItemProperty, value);
		}

		static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var grid = sender as DataGrid;
			if (grid == null || grid.SelectedItem == null)
				return;

			// Works with .Net 4.5
			grid.Dispatcher.InvokeAsync(() =>
			{
				grid.UpdateLayout();
				grid.ScrollIntoView(grid.SelectedItem, null);
			});

			// Works with .Net 4.0
			grid.Dispatcher.BeginInvoke((Action)(() =>
			{
				grid.UpdateLayout();
				grid.ScrollIntoView(grid.SelectedItem, null);
			}));
		}
	}
}
