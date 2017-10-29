using Microsoft.Practices.Unity;
using Prism.Unity;
using DatabaseManager.Views;
using System.Windows;
using DatabaseManager.ViewModels;

namespace DatabaseManager
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Container.RegisterType<MainWindowViewModel>();
            Container.RegisterType<ViewBaseViewModel>();
            Application.Current.MainWindow?.Show();
        }
    }
}
