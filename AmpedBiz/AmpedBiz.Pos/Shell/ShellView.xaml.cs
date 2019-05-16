using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Windows;

namespace AmpedBiz.Pos.Shell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window, IViewFor<ShellViewModel>
    {
        public ShellViewModel ViewModel
        {
            get => this.DataContext as ShellViewModel;
            set => this.DataContext = value;
        }

        object IViewFor.ViewModel
        {
            get => this.DataContext;
            set => this.DataContext = value;
        }

        public ShellView(ShellViewModel viewModel = null)
        {
            InitializeComponent();

            this.ViewModel = viewModel ?? Locator.Current.GetService<ShellViewModel>();

            this.WhenActivated(block =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Header, v => v.HeaderView.ViewModel).DisposeWith(block);

                this.Bind(this.ViewModel, vm => vm.HostScreen.Router, v => v.ContentView.Router).DisposeWith(block);
            });
        }
    }
}
