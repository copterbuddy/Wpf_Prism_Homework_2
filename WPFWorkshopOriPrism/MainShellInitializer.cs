using LoginModule;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWorkshopOriPrism
{
    class MainShellInitializer : IMainShellInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        public void ConfigureModelCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<LoginModule.LoginModules>();
            moduleCatalog.AddModule<MainModule.MainModules>();
        }
    }
}
