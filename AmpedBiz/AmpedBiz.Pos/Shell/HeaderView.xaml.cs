using ReactiveUI;
using System.Reactive.Disposables;

namespace AmpedBiz.Pos.Shell
{
    /// <summary>
    /// Interaction logic for HeaderView.xaml
    /// </summary>
    public partial class HeaderView : ReactiveUserControl<HeaderViewModel>
    {
        public HeaderView()
        {
            InitializeComponent();

            // setup bindings
            this.WhenActivated(block =>
            {
                this.Bind(this.ViewModel, vm => vm.Title, v => v.TitleTextBlock.Text).DisposeWith(block);
            });
        }
    }
}
