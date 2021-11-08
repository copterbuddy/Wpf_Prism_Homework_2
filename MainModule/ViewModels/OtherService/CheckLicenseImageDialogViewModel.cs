using Entity.Models;
using MainModule.GrpcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static Entity.Models.Enums;

namespace MainModule.ViewModels
{
    public class CheckLicenseImageDialogViewModel : BindableBase,IDialogAware
    {
        #region template property
        public string Title => "CheckLicenseImageDialog";

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

        public ImageSource CustomerImageCitizenDisplay { get => customerImageCitizenDisplay; set => SetProperty(ref customerImageCitizenDisplay, value); }

        private ImageSource customerImageCitizenDisplay;
        public ImageSource CustomerImageSignDisplay { get => customerImageSignDisplay; set => SetProperty(ref customerImageSignDisplay, value); }

        private ImageSource customerImageSignDisplay;
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

            CustomerImageCitizenDisplay = CustomerDetailTransferManager.GetInstance().customerDetail.CitizenIdCardImagePath;
            CustomerImageSignDisplay = CustomerDetailTransferManager.GetInstance().customerDetail.SignedSignatureImagePath;

            LogGrpcService logGrpcService = new();
            logGrpcService.AddActivityLog(ActivityType.CheckSign.GetHashCode(), ActivityType.CheckSign.ToString(), PageCode.PAGE001.ToString(), PageName.TRANSFER_PAGE.ToString());
        }
        #endregion


        #region command function
        protected virtual void CloseDialog()
        {
            ButtonResult result = ButtonResult.None;
            result = ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result));

        }
        #endregion
    }
}
