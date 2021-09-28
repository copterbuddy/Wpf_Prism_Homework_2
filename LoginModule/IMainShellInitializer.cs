using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginModule
{
    public interface IMainShellInitializer
    {
        void RegisterTypes(IContainerRegistry containerRegistry);

        void ConfigureModelCatalog(IModuleCatalog moduleCatalog);
    }
}
