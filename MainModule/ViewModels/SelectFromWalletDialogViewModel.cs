using Entity.Models;
using MainModule.GrpcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainModule.ViewModels
{
    public class SelectFromWalletDialogViewModel : BindableBase,IDialogAware
    {
        #region template property
        public string Title => "SelectFromWalletDialog";

        public event Action<IDialogResult> RequestClose;

        private DelegateCommand _closeDialogCommand;

        public DelegateCommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));

        #endregion

        #region bindable property
        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public WalletListEntity WalletListData { get => walletListData; set => SetProperty(ref walletListData, value); }
        public ObservableCollection<WalletEntity> WalletListDisplay { get => walletListDisplay; set => SetProperty(ref walletListDisplay, value); }
        public DelegateCommand<string> SelectedWalletCommand => selectedWalletCommand ?? (selectedWalletCommand = new DelegateCommand<string>(SelectedWallet));


        private WalletListEntity walletListData;
        private ObservableCollection<WalletEntity> walletListDisplay;
        private DelegateCommand<string> selectedWalletCommand;
        #endregion


        #region template function
        private void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");

            CustomerWalletService customerWalletService = new();
            WalletListData = await customerWalletService.GetFromWallet(CustomerDetailTransferManager.GetInstance().customerDetail.CustId);
            WalletListDisplay = new ObservableCollection<WalletEntity>(WalletListData.WalletList);
        }
        #endregion


        #region command function
        protected virtual void CloseDialog()
        {
            ButtonResult result = ButtonResult.None;
            result = ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result));

        }


        public WalletListEntity GetMock()
        {
            WalletListEntity dataList = new();

            {
                WalletEntity data = new WalletEntity();
                data.WalletId = "110010012121211";
                data.WalletName = "นาย คุณานนต์ ทดสอบ1";
                data.WalletlImage = null;
                data.Balance = 99000;
                data.BankCode = "002";
                dataList.WalletList.Add(data);
            }
            {
                WalletEntity data = new WalletEntity();
                data.WalletId = "110010012121212";
                data.WalletName = "นายคุณานนต์ ทดสอบ2";
                data.WalletlImage = null;
                data.Balance = 1000000;
                data.BankCode = "004";
                dataList.WalletList.Add(data);
            }

            return dataList;
        }

        private ImageSource GetImageSource(string filename)
        {
            try
            {
                return new BitmapImage(new Uri($"pack://application:,,,/Entity;Component/Images/TransferImages/{filename}.png"));
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private void SelectedWallet(string walletId)
        {
            if (walletId == null)
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                return;
            }

            foreach (var item in WalletListData.WalletList)
            {
                if (item.WalletId.Equals(walletId))
                {
                    WalletFromEntityManager.GetInstance().WalletEntity = item;
                    RaiseRequestClose(new DialogResult(ButtonResult.OK));
                    return;
                }
            }
        }
        #endregion
    }
}
