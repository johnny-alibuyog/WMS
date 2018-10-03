using AmpedBiz.Pos.Common.Services;
using AmpedBiz.Pos.Features;
using AmpedBiz.Pos.Shell;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace AmpedBiz.Pos
{
	public class AppBootstrapper : ReactiveObject, IScreen
    {
        [Reactive] public RoutingState Router { get; private set; } = new RoutingState();

        public AppBootstrapper()
        {
            this.InitializeServices();

            this.InitializeScreens();
        }

        private void InitializeServices()
        {
            Locator.CurrentMutable.Register<IApiService>(() => new FakeApiService());
        }

        private void InitializeScreens()
        {
            // This is our Main window host
            Locator.CurrentMutable.RegisterLazySingleton<IScreen>(() => this);

            // Register Views
            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

            // Shell
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(Locator.Current.GetService<IScreen>()));
            Locator.CurrentMutable.RegisterLazySingleton<IViewFor<ShellViewModel>>(() => new ShellView(Locator.Current.GetService<ShellViewModel>()));

            // Header
            Locator.CurrentMutable.Register(() => new HeaderViewModel());
            Locator.CurrentMutable.Register<IViewFor<HeaderViewModel>>(() => new HeaderView());

            // Features
            Locator.CurrentMutable.Register(() => new PointOfSalesViewModel(Locator.Current.GetService<IScreen>(), Locator.Current.GetService<IApiService>()));
            Locator.CurrentMutable.Register<IViewFor<PointOfSalesViewModel>>(() => new PointOfSaleView());
        }
    }
}
