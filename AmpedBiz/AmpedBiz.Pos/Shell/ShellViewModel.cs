using AmpedBiz.Pos.Features;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AmpedBiz.Pos.Shell
{
	public class ShellViewModel : ReactiveObject, IRoutableViewModel
	{
        [Reactive] public string UrlPathSegment { get; private set; } = "Shell";

		[Reactive] public IScreen HostScreen { get; private set; }

		[Reactive] public HeaderViewModel Header { get; private set; }

        public ShellViewModel(IScreen screen, HeaderViewModel header = null)
        {
            this.HostScreen = screen;

            this.Header = header ?? Locator.CurrentMutable.GetService<HeaderViewModel>();

            this.HostScreen.Router.Navigate.Execute(Locator.CurrentMutable.GetService<PointOfSalesViewModel>());
        }
    }
}
