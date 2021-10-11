using Entity.Models;
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
    public class SelectToWalletDialogViewModel : BindableBase,IDialogAware
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

        public WalletListEntity WalletList { get => walletList; set => SetProperty(ref walletList, value); }
        public ObservableCollection<WalletEntity> WalletListDisplay { get => walletListDisplay; set => SetProperty(ref walletListDisplay, value); }

        private WalletListEntity walletList;
        private ObservableCollection<WalletEntity> walletListDisplay;
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

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");

            WalletList = GetMock();
            WalletListDisplay = new ObservableCollection<WalletEntity>(WalletList.WalletList);
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
                WalletEntity data = new();
                data.WalletId = "1";
                data.WalletName = "ชื่อ1";
                data.WalletlImage = null;
                data.Balance = 100;
                data.BankCode = "002";
                dataList.WalletList.Add(data);
            }
            {
                WalletEntity data = new();
                data.WalletId = "2";
                data.WalletName = "ชื่อ2";
                data.WalletlImage = null;
                data.Balance = 200;
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
        #endregion
    }
}
