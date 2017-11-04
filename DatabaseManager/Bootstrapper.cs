using Microsoft.Practices.Unity;
using Prism.Unity;
using DatabaseManager.Views;
using System.Windows;

namespace DatabaseManager
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
