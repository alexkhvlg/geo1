using Caliburn.Micro;
using geo1.ViewModels;
using System.Windows;

namespace geo1
{
    class AppBootstrapper: BootstrapperBase {
        public AppBootstrapper() {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}