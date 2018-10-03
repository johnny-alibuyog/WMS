using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AmpedBiz.Pos.Shell
{
	public class HeaderViewModel : ReactiveObject
    {
		[Reactive] public string Title { get; private set; } = "Dambanang Bayan Cooperative";
    }
}
