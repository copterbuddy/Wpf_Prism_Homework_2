using Entity.Models;
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

namespace MainModule.ViewModels
{
    public class InputToWalletDialogViewModel : BindableBase,IDialogAware
    {
        #region template property
        public string Title => "InputToWalletDialog";

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

        private DelegateCommand<TextBox> inputToWalletCommand;

        public DelegateCommand<TextBox> InputToWalletCommand => inputToWalletCommand ?? (inputToWalletCommand = new DelegateCommand<TextBox>(inputToWallet));

        public string WalletIdTextBox { get => walletIdTextBox; set 
            {
                Regex regex = new Regex("[^0-9.]+");
                if (!regex.IsMatch(value))
                {
                    SetProperty(ref walletIdTextBox, value);
                }
            }
        }

        public string BankSelectedDisplay { 
            get {
                return BankEntityManager.GetInstance().bankEntity.BankName != null ? BankEntityManager.GetInstance().bankEntity.BankName : "";
            }
        }

        private string walletIdTextBox;
        private string bankSelectedDisplay;
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
        private void inputToWallet(TextBox walletIdTextBox)
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
            WalletService walletService = new();
            string walletName = walletService.GetToWallet(walletIdTextBox.Text, BankEntityManager.GetInstance().bankEntity.BankCode);



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


        }
        #endregion
    }
}
