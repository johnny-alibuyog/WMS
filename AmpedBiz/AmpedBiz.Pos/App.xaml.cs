using AmpedBiz.Pos.Shell;
using ReactiveUI;
using Splat;
using System.Windows;

namespace AmpedBiz.Pos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppBootstrapper Bootstrapper;

        public static ShellView ShellView;

        public App()
        {
            Bootstrapper = new AppBootstrapper();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ShellView = (ShellView)Locator.Current.GetService<IViewFor<ShellViewModel>>();
            ShellView.Show();
        }
    }
}
