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
    public class SelectBankListDialogViewModel : BindableBase,IDialogAware
    {
        #region template property
        public string Title => "SelectBankListialog";

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

        public BankListEntity BankListData { get => bankListData; set => SetProperty(ref bankListData, value); }
        public ObservableCollection<BankEntity> BankListDisplay { get => bankListDisplay; set => SetProperty(ref bankListDisplay, value); }
        public DelegateCommand<string> SelectedBankCommand => selectedBankCommand ?? (selectedBankCommand = new DelegateCommand<string>(SelectedBank));


        private BankListEntity bankListData;
        private ObservableCollection<BankEntity> bankListDisplay;
        private DelegateCommand<string> selectedBankCommand;
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

            if (BankListEntityManager.GetInstance().bankListEntity.BankList == null || BankListEntityManager.GetInstance().bankListEntity.BankList.Count == 0)
            {
                SystemConfigGrpcService systemConfigGrpc = new SystemConfigGrpcService();
                BankListEntityManager.GetInstance().bankListEntity.BankList = await systemConfigGrpc.GetBankList();
            }

            if (BankListEntityManager.GetInstance().bankListEntity.BankList != null && BankListEntityManager.GetInstance().bankListEntity.BankList.Count > 0)
            {
                BankListDisplay = new ObservableCollection<BankEntity>(BankListEntityManager.GetInstance().bankListEntity.BankList);
            }
            
        }
        #endregion


        #region command function
        protected virtual void CloseDialog()
        {
            ButtonResult result = ButtonResult.None;
            result = ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result));

        }


        public BankListEntity GetMock()
        {
            BankListEntity dataList = new();

            {
                BankEntity data = new();
                data.BankCode = "002";
                data.BankName = "ธ.กรุงเทพ จำกัด (มหาชน)";
                data.BankNameEN = "BANGKOK BANK PUBLIC COMPANY LTD.";
                data.BankAbbr = "BBL";
                data.ImagePath = GetImageSource("bank-icon0.png");
                dataList.BankList.Add(data);
            }
            {
                BankEntity data = new();
                data.BankCode = "003";
                data.BankName = "ธ.กสิกรไทย จำกัด (มหาชน)";
                data.BankNameEN = "KASIKORNBANK PUBLIC COMPANY LIMITED";
                data.BankAbbr = "KBANK";
                data.ImagePath = GetImageSource("no_image.png");
                dataList.BankList.Add(data);
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
        private void SelectedBank(string bankid)
        {
            if (string.IsNullOrEmpty(bankid))
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
                return;
            }

            foreach (var item in BankListEntityManager.GetInstance().bankListEntity.BankList)
            {
                if (item.BankCode.Equals(bankid))
                {
                    BankEntityManager.GetInstance().bankEntity = item;
                    RaiseRequestClose(new DialogResult(ButtonResult.OK));
                    return;
                }
            }
        }
        #endregion
    }
}
