using Entity.Models;
using MainModule.GrpcService;
using MainModule.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainModule.ViewModels
{
    public class InputToWalletDialogViewModel : BindableBase,IDialogAware
    {
        #region template property
        public string Title => "InputToWalletDialog";

        public event Action<IDialogResult> RequestClose;

        private DelegateCommand _closeDialogCommand;

        public DelegateCommand CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand(CloseDialog));


        private DelegateCommand backCommand;

        public DelegateCommand BackCommand => backCommand ?? (backCommand = new DelegateCommand(Back));
        #endregion

        #region bindable property

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private DelegateCommand<TextBox> inputToWalletCommand;

        public DelegateCommand<TextBox> InputToWalletCommand => inputToWalletCommand ?? (inputToWalletCommand = new DelegateCommand<TextBox>(inputToWallet));

        public string WalletIdTextBox { get => walletIdTextBox; set 
            {
                Regex regex = new Regex("^\\d{0,15}$");
                if (regex.IsMatch(value))
                {
                    SetProperty(ref walletIdTextBox, value);
                }
            }
        }

        void Back()
        {
            ButtonResult result = ButtonResult.Abort;
            RaiseRequestClose(new DialogResult(result));
        }

        public string BankSelectedDisplay { 
            get {
                return BankEntityManager.GetInstance().bankEntity.BankName != null ? BankEntityManager.GetInstance().bankEntity.BankName : "";
            }
        }

        public ImageSource BankLogoDisplay { get => BankEntityManager.GetInstance().bankEntity.ImagePath != null ? BankEntityManager.GetInstance().bankEntity.ImagePath : GetImageSource("bank-icon0.png"); }

        private string walletIdTextBox;
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
        }
        #endregion


        #region command function
        protected virtual void CloseDialog()
        {
            ButtonResult result = ButtonResult.None;
            result = ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result));

        }
        private async void inputToWallet(TextBox walletIdTextBox)
        {
            //Input InValid
            //1.Open Alert Dialog
            bool isInputError = false;

            if (walletIdTextBox == null) isInputError = true;
            if (string.IsNullOrEmpty(walletIdTextBox.Text)) isInputError = true;
            if (walletIdTextBox.Text.Length != 15) isInputError = true;

            if (isInputError)
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Ignore));
                return;
            }

            //Input Valid
            //Call Api
            CustomerWalletService customerWalletService = new();
            string walletName = await customerWalletService.GetToWallet(walletIdTextBox.Text);



            //Api invalid
            //1.Open Alert Dialog

            //Api Valid
            //Fill to TransferRegion
            if (!string.IsNullOrEmpty(walletName))
            {
                WalletEntity walletTo = new()
                {
                    WalletName = walletName,
                    BankCode = BankEntityManager.GetInstance().bankEntity.BankCode,
                    WalletId = walletIdTextBox.Text,
                };
                WalletToEntityManager.GetInstance().WalletEntity = walletTo;
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
                return;
            }
            else
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Ignore));
                return;
            }


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
