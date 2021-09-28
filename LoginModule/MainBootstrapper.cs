using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoginModule
{
    class MainBootstrapper : PrismBootstrapper
    {
        IMainShellInitializer mainShellInitializer;

        public MainBootstrapper(IMainShellInitializer mainShellInitializer) : base()
        {
            this.mainShellInitializer = mainShellInitializer;
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainModule.Views.MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            mainShellInitializer?.RegisterTypes(containerRegistry);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            mainShellInitializer?.ConfigureModelCatalog(moduleCatalog);
        }
    }
}
