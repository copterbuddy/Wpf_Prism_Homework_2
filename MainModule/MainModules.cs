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
            regionManager.RegisterViewWithRegion(nameof(WalletPreTransferRegion), typeof(WalletPreTransferRegion));
            regionManager.RegisterViewWithRegion(nameof(WalletTransferRegion), typeof(WalletTransferRegion));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<WelcomeRegion>();
            containerRegistry.RegisterForNavigation<OtherServiceRegion>();
            containerRegistry.RegisterForNavigation<TransferRegion>();
            containerRegistry.RegisterForNavigation<WalletPreTransferRegion>();
            containerRegistry.RegisterForNavigation<WalletTransferRegion>();

            containerRegistry.RegisterDialog<SearchCustomerDialog, SearchCustomerDialogViewModel>();
            containerRegistry.RegisterDialog<SearchCustomerTransferDialog, SearchCustomerTransferDialogViewModel>();
            containerRegistry.RegisterDialog<SearchEmployeeDialog, SearchEmployeeDialogViewModel>();
            containerRegistry.RegisterDialog<SelectFundDialog, SelectFundDialogViewModel>();
            containerRegistry.RegisterDialog<FundDetailDialog, FundDetailDialogViewModel>();
            containerRegistry.RegisterDialog<AlertDialog, AlertDialogViewModel>();
            containerRegistry.RegisterDialog<AlertWaitingDialog, AlertWaitingDialogViewModel>();
            containerRegistry.RegisterDialog<SelectFromWalletDialog, SelectFromWalletDialogViewModel>();
            containerRegistry.RegisterDialog<SelectBankListDialog, SelectBankListDialogViewModel>();
            containerRegistry.RegisterDialog<InputToWalletDialog, InputToWalletDialogViewModel>();
            containerRegistry.RegisterDialog<CheckLicenseImageDialog, CheckLicenseImageDialogViewModel>();
        }
    }
}
