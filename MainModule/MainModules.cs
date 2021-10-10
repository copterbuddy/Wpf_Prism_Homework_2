using MainModule.ViewModels;
using MainModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModule
{
    public class MainModules : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(nameof(TitleRegion), typeof(TitleRegion));
            regionManager.RegisterViewWithRegion(nameof(BottomInfoRegion), typeof(BottomInfoRegion));
            regionManager.RegisterViewWithRegion(nameof(MenuRegion), typeof(MenuRegion));
            regionManager.RegisterViewWithRegion(nameof(WelcomeRegion), typeof(WelcomeRegion));
            regionManager.RegisterViewWithRegion(nameof(OtherServiceRegion), typeof(OtherServiceRegion));
            regionManager.RegisterViewWithRegion(nameof(TransferRegion), typeof(TransferRegion));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<WelcomeRegion>();
            containerRegistry.RegisterForNavigation<OtherServiceRegion>();
            containerRegistry.RegisterForNavigation<TransferRegion>();

            containerRegistry.RegisterDialog<SearchCustomerDialog, SearchCustomerDialogViewModel>();
            containerRegistry.RegisterDialog<SearchCustomerTransferDialog, SearchCustomerTransferDialogViewModel>();
            containerRegistry.RegisterDialog<SearchEmployeeDialog, SearchEmployeeDialogViewModel>();
            containerRegistry.RegisterDialog<SelectFundDialog, SelectFundDialogViewModel>();
            containerRegistry.RegisterDialog<FundDetailDialog, FundDetailDialogViewModel>();
            containerRegistry.RegisterDialog<AlertDialog, AlertDialogViewModel>();
        }
    }
}
