using System.Collections.Generic;
using Caliburn.Micro;
using geo1.ViewModels;
using System.Windows;
using geo1.Properties;

namespace geo1
{
    class AppBootstrapper: BootstrapperBase {
        public AppBootstrapper() {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {

            double width = Settings.Default.screen_width;  //Previous window width 
            double height = Settings.Default.screen_height; //Previous window height

            var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            if (width > screenWidth) width = (screenWidth - 10);
            if (height > screenHeight) height = (screenHeight - 10);

            var windowSettings = new Dictionary<string, object> {{"Width", width}, {"Height", height}};

            DisplayRootViewFor<MainViewModel>(windowSettings);
        }
    }
}