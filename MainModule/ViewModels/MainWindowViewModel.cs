using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Windows.Threading;
using System.Windows;
using WPFScbOri.Models;
using System.Windows.Input;
using Prism.Commands;
using Entity.Models;
using Prism.Services.Dialogs;
using WPFScbOri.Service;
using Teller.SellFund.Models;
using Teller.Core.Models;
using System.Collections.ObjectModel;
using MainModule.GrpcService;
using static Entity.Models.Enums;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using MainModule.Views;
using Entity.DTO;
using MainModule.Service;
using System.IO;
using iTextSharp.text.pdf;
using System.Diagnostics;
using iTextSharp.text;
using static iTextSharp.text.Font;
using MainModule.ViewModels.Utils;

namespace MainModule.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        IDialogService dialogService;
        public ICommand OpenSearchCustomerDialogCommand { get; set; }
        public ICommand OpenSearchCustomerTransferDialogCommand { get; set; }
        public ICommand OpenSearchEmployeeDialogCommand { get; set; }
        public ICommand OpenSelectedFundDialogCommand { get; set; }
        private string realtimeTimer;
        public string RealtimeTimer { get => realtimeTimer; set => SetProperty(ref realtimeTimer, value); }
        private string idBrn;
        private Account account;
        public Account Account{ get => account; set => SetProperty(ref account, value); }
        public string IdBrn { get { idBrn = $"ID:{AccountManager.Instance.Account.id} / BRN:{AccountManager.Instance.Account.branchCode}"; return idBrn; } }
        private string customerIdTextBox;
        public string CustomerIdTextBox { get => customerIdTextBox; set => SetProperty(ref customerIdTextBox, value); }
        public DelegateCommand<String> NavigateCommand { get; private set; }
        private string selectedSearchType;
        public string SelectedSearchType { get => selectedSearchType; set => SetProperty(ref selectedSearchType, value); }
        public string FundAccountIdTextBox { get => fundAccountIdTextBox; set => SetProperty(ref fundAccountIdTextBox, value); }
        public string CustomerName { get => customerName; set => SetProperty(ref customerName, value); }
        public string JointAccount { get => jointAccount; set => SetProperty(ref jointAccount, value); }
        public string SensitiveCustomer { get => sensitiveCustomer; set => SetProperty(ref sensitiveCustomer, value); }
        public string RiskLevel { get => riskLevel; set => SetProperty(ref riskLevel, value); }
        public string StaffIdText { get => staffIdText; set => SetProperty(ref staffIdText, value); }
        public string SecIdContent { get => secIdContent; set => SetProperty(ref secIdContent, value); }
        public string IcLicenseContent { get => icLicenseContent; set => SetProperty(ref icLicenseContent, value); }
        public string StaffRmContent { get => staffRmContent; set => SetProperty(ref staffRmContent, value); }
        public string StaffNameContent { get => staffNameContent; set => SetProperty(ref staffNameContent, value); }
        public ObservableCollection<StringModel> ComboBoxOrderByItemsSource { get => comboBoxOrderByItemsSource; set => SetProperty(ref comboBoxOrderByItemsSource, value); }
        private string fundAccountIdTextBox;
        private string customerName;
        private string jointAccount;
        private string sensitiveCustomer;
        private string riskLevel;
        private string staffIdText;
        private string secIdContent;
        private string icLicenseContent;
        private string staffRmContent;
        private string staffNameContent;
        private ObservableCollection<StringModel> comboBoxOrderByItemsSource;
        private List<SellFundTableModel> sellFundList;
        public List<SellFundTableModel> SellFundList { get => sellFundList; set => SetProperty(ref sellFundList, value); }
        private ObservableCollection<SellFundTableModel> sellFundListGrid;
        public ObservableCollection<SellFundTableModel> SellFundListGrid { get => sellFundListGrid; set => SetProperty(ref sellFundListGrid, value); }
        private bool selectFundButtonEnable;
        public bool SelectFundButtonEnable { get => selectFundButtonEnable; set => SetProperty(ref selectFundButtonEnable, value); }
        bool checkEnable1 = false;
        bool checkEnable2 = false;
        public ICommand SellFundButtonClickCommand { get; set; }
        private StringModel comboBoxOrderBySelectedItem;
        public StringModel ComboBoxOrderBySelectedItem { get => comboBoxOrderBySelectedItem; set => SetProperty(ref comboBoxOrderBySelectedItem, value); }
        public CurrentMainRegion CurrentRegion { get; set; }
        public ICommand OpenSelectFromWalletDialogCommand { get => openSelectFromWalletDialogCommand; set => SetProperty(ref openSelectFromWalletDialogCommand, value); }
        public WalletEntity FromWalletSelected { get => fromWalletSelected; set => SetProperty(ref fromWalletSelected, value); }
        public string FromWalletSelectedDisplay { get => fromWalletSelectedDisplay; set => SetProperty(ref fromWalletSelectedDisplay, value); }
        public ICommand OpenSelectBankListDialogCommand { get => openSelectBankListDialogCommand; set => SetProperty(ref openSelectBankListDialogCommand, value); }
        public BankEntity ToWalletSelected { get => toWalletSelected; set => SetProperty(ref toWalletSelected, value); }
        public string ToWalletSelectedDisplay { get => toWalletSelectedDisplay; set => SetProperty(ref toWalletSelectedDisplay, value); }
        public string AmountDisplay { 
            get => amountDisplay; 
            set {
                if (systemSetAmount)
                {
                    SetProperty(ref amountDisplay, value);
                    systemSetAmount = false;
                }
                else
                {
                    Regex regex = new Regex("[^0-9.]+");
                    if (!regex.IsMatch(value))
                    {
                        SetProperty(ref amountDisplay, value);
                    }
                }
                
            }
        }

        private ICommand openSelectFromWalletDialogCommand;
        private WalletEntity fromWalletSelected;
        private string fromWalletSelectedDisplay;
        private ICommand openSelectBankListDialogCommand;
        private BankEntity toWalletSelected;
        private string toWalletSelectedDisplay;
        private string amountDisplay;
        public DelegateCommand LostFocusCommand => lostFocusCommand ?? (lostFocusCommand = new DelegateCommand(LostFocus));

        public DelegateCommand OpenCheckLicenseImageCommand => openCheckLicenseImageCommand ?? (openCheckLicenseImageCommand = new DelegateCommand(OpenCheckLicenseImageDialog));

        public string CustomerTransferName { get => customerTransferName; set => SetProperty(ref customerTransferName, value); }
        public string CustomerTransferIdTextBox { get => customerTransferIdTextBox; set => SetProperty(ref customerTransferIdTextBox, value); }
        public DelegateCommand PreTransferCommand => preTransferCommand ?? (preTransferCommand = new DelegateCommand(PreTransfer));
        public TransactionEntity TransactionEntity { get => transactionEntity; set => SetProperty(ref transactionEntity, value); }
        public string Memo { get => memo; set => SetProperty(ref memo, value); }
        public DelegateCommand TransferCommand => transferCommand ?? (transferCommand = new DelegateCommand(Transfer));

        public DelegateCommand PrintCommand => printCommand ?? (printCommand = new DelegateCommand(Print));

        public bool CustomerHaveData { get => customerHaveData; set
            {
                SetProperty(ref customerHaveData, value);
                NotCustomerHaveData = !value;
            }
        }
        public bool NotCustomerHaveData
        {
            get => !customerHaveData;
            set => SetProperty(ref notCustomerHaveData, !value);
        }

        private DelegateCommand lostFocusCommand;
        private DelegateCommand gotFocusCommand;
        private DelegateCommand openCheckLicenseImageCommand;
        private string customerTransferName;
        private string customerTransferIdTextBox;
        private DelegateCommand preTransferCommand;
        //private WalletEntity toWalletEntity;
        //private WalletEntity fromWalletEntity;
        private TransactionEntity transactionEntity;
        private string memo;
        private DelegateCommand transferCommand;
        private DelegateCommand printCommand;
        private bool customerHaveData;
        private bool notCustomerHaveData;
        
        //MainTransferCommand
        public DelegateCommand mainTransferCommand;
        public DelegateCommand MainTransferCommand => mainTransferCommand ?? (mainTransferCommand = new DelegateCommand(GoToMainTransfer));

        public DelegateCommand GotFocusCommand => gotFocusCommand ?? (gotFocusCommand = new DelegateCommand(GotFocus));

        private bool systemSetAmount = false;
        Util util = new Util();

        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            #region injection
            _regionManager = regionManager;
            this.dialogService = dialogService;
            #endregion

            #region InitRegion
            account = AccountManager.GetInstance().Account;
            TimerCount();

            SelectFundButtonEnable = false;
            CheckSelectFundButtonEnable();

            StaffIdText = AccountManager.GetInstance().Account.id.ToString();
            CheckIcLicense(StaffIdText);

            CurrentRegion = CurrentMainRegion.WelcomeRegion;
            CustomerHaveData = false;
            #endregion

            #region Delegate Command
            OpenSearchCustomerDialogCommand = new DelegateCommand(OpenSearchCustomerDialog);
            OpenSearchCustomerTransferDialogCommand = new DelegateCommand(OpenSearchCustomerTransferDialog);
            OpenSearchEmployeeDialogCommand = new DelegateCommand(OpenSearchEmployeeDialog);
            OpenSelectedFundDialogCommand = new DelegateCommand(OpenSelectedFundDialog);
            SellFundButtonClickCommand = new DelegateCommand<string>(SellFundButtonClick);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            OpenSelectFromWalletDialogCommand = new DelegateCommand(OpenSelectFromWalletDialog);
            OpenSelectBankListDialogCommand = new DelegateCommand(OpenSelectBankListDialog);
            #endregion

        }


        void CheckSelectFundButtonEnable()
        {
            var customerDetail = CustomerDetailTransferManager.GetInstance().customerDetail;
            if (customerDetail != null && !string.IsNullOrEmpty(customerDetail.CustId))
            {
                CustomerHaveData = true;
            }


            if (!string.IsNullOrEmpty(CustomerIdTextBox))
                checkEnable1 = true;

            if (ComboBoxOrderByItemsSource != null && ComboBoxOrderByItemsSource.Count > 0)
                checkEnable2 = true;

            if (checkEnable1 && checkEnable2)
                SelectFundButtonEnable = true;
        }
        void SellFundButtonClick(string Id)
        {
            try
            {
                string fundId = Id;
                for (int i = 0; i < SellFundList.Count; i++)
                {
                    if (SellFundList[i].Id == fundId)
                    {
                        SellFundList.RemoveAt(i);
                        SellFundListGrid = new(SellFundList);
                        //SellFundListGrid.Items.Refresh();
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                CurrentMainRegion currentRegion;
                _regionManager.RequestNavigate("WelcomeRegion", navigatePath);
                Enum.TryParse(navigatePath, out currentRegion);
                CurrentRegion = currentRegion;

                if (navigatePath.Equals(nameof(TransferRegion)))
                {
                    if(CustomerDetailTransferManager.GetInstance().customerDetail != null)
                        CustomerDetailTransferManager.GetInstance().customerDetail = null;

                    if (WalletFromEntityManager.GetInstance().WalletEntity != null)
                        WalletFromEntityManager.GetInstance().WalletEntity.WalletId = null;

                    if (WalletToEntityManager.GetInstance().WalletEntity != null)
                        WalletToEntityManager.GetInstance().WalletEntity.WalletId = null;

                    if (BankEntityManager.GetInstance().bankEntity != null)
                        BankEntityManager.GetInstance().bankEntity = null;

                    CustomerTransferIdTextBox = "";
                    CustomerTransferName = "";
                    FromWalletSelectedDisplay = "";
                    ToWalletSelectedDisplay = "";
                    AmountDisplay = "";
                    Memo = "";

                    CustomerHaveData = false;
                }
            }
        }


        void OpenSearchCustomerDialog()
        {

            dialogService.Show(
                "SearchCustomerDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            CustomerIdTextBox = result.Parameters.GetValue<string>("CustId");
                            FundAccountIdTextBox = result.Parameters.GetValue<string>("Branch");
                            CustomerName = result.Parameters.GetValue<string>("AccName");
                            JointAccount = result.Parameters.GetValue<string>("JointAccount");
                            SensitiveCustomer = result.Parameters.GetValue<string>("SensitiveCustomer");
                            RiskLevel = result.Parameters.GetValue<string>("RiskLevel");


                            if (WalletFromEntityManager.GetInstance().WalletEntity != null)
                                WalletFromEntityManager.GetInstance().WalletEntity.WalletId = null;

                            if (WalletToEntityManager.GetInstance().WalletEntity != null)
                                WalletToEntityManager.GetInstance().WalletEntity.WalletId = null;

                            if (BankEntityManager.GetInstance().bankEntity != null)
                                BankEntityManager.GetInstance().bankEntity = null;

                            FromWalletSelectedDisplay = "";
                            ToWalletSelectedDisplay = "";
                            AmountDisplay = "";
                            Memo = "";

                            CheckSelectFundButtonEnable();
                        }
                        else if (result.Result == ButtonResult.Ignore)
                        {
                            string message = result.Parameters.GetValue<string>("message");
                            string defaultSearch = result.Parameters.GetValue<string>("defaultSearch");
                            OpenSearchCustomerDialog(defaultSearch);
                            dialogService.Show(
                                "AlertDialog",
                                new DialogParameters($"message={message}"),
                                    (result) => {
                                    });
                        }
                    });
        }

        void OpenSearchCustomerDialog(string defaultSearch = "")
        {
            dialogService.Show(
                "SearchCustomerDialog",
                new DialogParameters($"message={defaultSearch}"),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            CustomerIdTextBox = result.Parameters.GetValue<string>("CustId");
                            FundAccountIdTextBox = result.Parameters.GetValue<string>("Branch");
                            CustomerName = result.Parameters.GetValue<string>("AccName");
                            JointAccount = result.Parameters.GetValue<string>("JointAccount");
                            SensitiveCustomer = result.Parameters.GetValue<string>("SensitiveCustomer");
                            RiskLevel = result.Parameters.GetValue<string>("RiskLevel");

                            CheckSelectFundButtonEnable();
                        }
                        else if (result.Result == ButtonResult.Ignore)
                        {
                            string message = result.Parameters.GetValue<string>("message");
                            defaultSearch = result.Parameters.GetValue<string>("defaultSearch");
                            OpenSearchCustomerDialog(defaultSearch);
                            dialogService.Show(
                                "AlertDialog",
                                new DialogParameters($"message={message}"),
                                    (result) => {
                                    });
                        }
                    });
        }

        void OpenSearchCustomerTransferDialog()
        {

            dialogService.Show(
                "SearchCustomerTransferDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            CustomerTransferIdTextBox = CustomerDetailTransferManager.GetInstance().customerDetail.CitizenID;
                            CustomerTransferName = CustomerDetailTransferManager.GetInstance().customerDetail.AccName;

                            //reset wallet
                            if (WalletFromEntityManager.GetInstance().WalletEntity != null)
                                WalletFromEntityManager.GetInstance().WalletEntity.WalletId = null;

                            if (WalletToEntityManager.GetInstance().WalletEntity != null)
                                WalletToEntityManager.GetInstance().WalletEntity.WalletId = null;

                            if (BankEntityManager.GetInstance().bankEntity != null)
                                BankEntityManager.GetInstance().bankEntity = null;

                            FromWalletSelectedDisplay = "";
                            ToWalletSelectedDisplay = "";
                            AmountDisplay = "";
                            Memo = "";

                            CheckSelectFundButtonEnable();
                        }
                        else if (result.Result == ButtonResult.Ignore)
                        {
                            string message = result.Parameters.GetValue<string>("message");
                            string defaultSearch = result.Parameters.GetValue<string>("defaultSearch");
                            OpenSearchCustomerTransferDialog(defaultSearch);
                            dialogService.Show(
                                "AlertDialog",
                                new DialogParameters($"message={message}"),
                                    (result) => { });
                        }
                    });
        }

        void OpenSearchCustomerTransferDialog(string defaultSearch = "")
        {
            dialogService.Show(
                "SearchCustomerTransferDialog",
                new DialogParameters($"defaultSearch={defaultSearch}"),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            CustomerTransferIdTextBox = CustomerDetailTransferManager.GetInstance().customerDetail.CitizenID;
                            CustomerTransferName = CustomerDetailTransferManager.GetInstance().customerDetail.AccName;

                            //reset wallet
                            if (WalletFromEntityManager.GetInstance().WalletEntity != null)
                                WalletFromEntityManager.GetInstance().WalletEntity.WalletId = null;

                            if (WalletToEntityManager.GetInstance().WalletEntity != null)
                                WalletToEntityManager.GetInstance().WalletEntity.WalletId = null;

                            if (BankEntityManager.GetInstance().bankEntity != null)
                                BankEntityManager.GetInstance().bankEntity = null;

                            FromWalletSelectedDisplay = "";
                            ToWalletSelectedDisplay = "";
                            AmountDisplay = "";
                            Memo = "";

                            CheckSelectFundButtonEnable();
                        }
                        else if (result.Result == ButtonResult.Ignore)
                        {
                            string message = result.Parameters.GetValue<string>("message");
                            defaultSearch = result.Parameters.GetValue<string>("defaultSearch");
                            OpenSearchCustomerTransferDialog(defaultSearch);
                            dialogService.Show(
                                "AlertDialog",
                                new DialogParameters($"message={message}"),
                                    (result) => { });
                        }
                    });
        }

        void OpenSearchEmployeeDialog()
        {
            dialogService.ShowDialog(
                "SearchEmployeeDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            var _empId = result.Parameters.GetValue<string>("_empId");
                            CheckIcLicense(_empId);
                            //System.Diagnostics.Debug.WriteLine("SelectedSearchType: dialog result = " + SelectedSearchType);

                        }
                    });
        }

        void OpenSelectedFundDialog()
        {
            if (SelectFundButtonEnable)
            {
                dialogService.ShowDialog(
                "SelectFundDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            OpenFundDetailDialog();
                        }
                    });
            }
            else
            {
                dialogService.ShowDialog(
                "AlertDialog",
                new DialogParameters("message=กรุณากรอกข้อมูลลูกค้าให้ถูกต้อง"),
                    (result) => { });
            }

        }

        void OpenFundDetailDialog()
        {
            dialogService.ShowDialog(
                "FundDetailDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.Cancel)//close
                        {

                            //CheckIcLicense();
                            //System.Diagnostics.Debug.WriteLine("SelectedSearchType: dialog result = " + SelectedSearchType);
                        }
                        if (result.Result == ButtonResult.Ignore)//back
                        {
                            OpenSelectedFundDialog();
                            //CheckIcLicense();
                            //System.Diagnostics.Debug.WriteLine("SelectedSearchType: dialog result = " + SelectedSearchType);
                        }
                        if (result.Result == ButtonResult.OK)//sell
                        {
                            SetSellFundTable();
                            //CheckIcLicense();
                            //System.Diagnostics.Debug.WriteLine("SelectedSearchType: dialog result = " + SelectedSearchType);
                        }
                    });
        }

        private void OpenSelectFromWalletDialog()
        {
            dialogService.Show(
                "SelectFromWalletDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            FromWalletSelected = WalletFromEntityManager.GetInstance().WalletEntity;
                            FromWalletSelectedDisplay = SetFromWalletDisplay(FromWalletSelected);
                        }
                        else if (result.Result == ButtonResult.Ignore)
                        {
                        }
                    });

        }

        private void OpenSelectBankListDialog()
        {
            dialogService.Show(
                "SelectBankListDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK && BankEntityManager.GetInstance().bankEntity != null)
                        {
                            OpenInputToWalletDialog();
                        }
                    });

        }

        private void OpenInputToWalletDialog()
        {
            dialogService.Show(
                "InputToWalletDialog",
                new DialogParameters(),
                    (result) => {

                        var backSelected = BankEntityManager.GetInstance().bankEntity;
                        var toWalletSelected = WalletToEntityManager.GetInstance().WalletEntity;

                        if (result.Result == ButtonResult.OK && BankEntityManager.GetInstance().bankEntity != null && WalletToEntityManager.GetInstance().WalletEntity != null && !string.IsNullOrEmpty(WalletToEntityManager.GetInstance().WalletEntity.WalletName))
                        {
                            ToWalletSelectedDisplay = SetToWalletDisplay(WalletToEntityManager.GetInstance().WalletEntity);
                        }
                        else if (result.Result == ButtonResult.Ignore)
                        {
                            OpenInputToWalletDialog();
                            dialogService.Show(
                                "AlertDialog",
                                new DialogParameters($"message=กรุณาระบุเลขวอลเลตให้ถูกต้อง"),
                                    (result) => { });
                        }else if(result.Result == ButtonResult.Abort)
                        {
                            OpenSelectBankListDialog();
                        }
                    });
        }


        void OpenCheckLicenseImageDialog()
        {
            dialogService.Show(
                                "CheckLicenseImageDialog",
                                new DialogParameters($"message=กรุณาระบุเลขวอลเลตให้ถูกต้อง"),
                                    (result) => { });
        }

        public string SetFromWalletDisplay(WalletEntity walletEntity)
        {
            if (walletEntity == null || walletEntity.WalletName == null || walletEntity.WalletId == null) return "";

            string response = walletEntity.WalletName + " * " + walletEntity.WalletId + " * " + walletEntity.Balance.ToString("#,##0.00;-#,##0.00");
            return response;
        }
        public string SetToWalletDisplay(WalletEntity walletEntity)
        {
            if (walletEntity == null || walletEntity.WalletName == null || walletEntity.WalletId == null) return "";

            string response = walletEntity.WalletName + " * " + walletEntity.WalletId + " * " + BankEntityManager.GetInstance().bankEntity.BankName;
            return response;
        }

        async void PreTransfer()
        {
            DialogResult dialogResult = null;
            bool isProcess = true;
            double doubleAmount = 0;

            #region Check Parameter
            if (isProcess)
                if (string.IsNullOrEmpty(CustomerTransferIdTextBox))
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "กรุณาเลือกลูกค้า");
                    isProcess = false;
                }

            if (isProcess)
                if (string.IsNullOrEmpty(CustomerTransferName))
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "กรุณาเลือกลูกค้า");
                    isProcess = false;
                }

            if (isProcess)
                if (string.IsNullOrEmpty(FromWalletSelectedDisplay))
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "กรุณาระบุข้อมูลผู้โอน");
                    isProcess = false;
                }

            if (isProcess)
                if (string.IsNullOrEmpty(ToWalletSelectedDisplay))
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "กรุณาระบุข้อมูลผู้รับโอน");
                    isProcess = false;
                }
            if (isProcess)
                if (string.IsNullOrEmpty(AmountDisplay) || checkCountPoint())
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "กรุณาระบุจำนวนเงินที่ถูกต้อง");
                    isProcess = false;
                }
                else
                {
                    doubleAmount = Double.Parse(TrimComma(AmountDisplay));
                    if (doubleAmount <= 0)
                    {
                        dialogResult = new DialogResult(ButtonResult.Ignore);
                        dialogResult.Parameters.Add("errMessage", "กรุณาระบุจำนวนเงินที่ถูกต้อง");
                        isProcess = false;
                    }
                    if (WalletFromEntityManager.GetInstance().WalletEntity.Balance <= 0)
                    {
                        dialogResult = new DialogResult(ButtonResult.Ignore);
                        dialogResult.Parameters.Add("errMessage", "ยอดเงินไม่เพียงพอต่อการทำรายการ");
                        isProcess = false;
                    }
                }
            #endregion

            if (isProcess)
            {

                var fromWallet = WalletFromEntityManager.GetInstance().WalletEntity.WalletId;
                var toWallet = WalletToEntityManager.GetInstance().WalletEntity.WalletId;
                var bankCode = BankEntityManager.GetInstance().bankEntity.BankCode;

                TransferService transfer = new();
                WalletTransactionResponse transactionResponse = await transfer.PreTransfer(fromWallet, toWallet, bankCode, doubleAmount, Memo);

                if (transactionResponse != null)
                {
                    if (transactionResponse.returnResult != null && transactionResponse.transactionEntity != null && transactionResponse.returnResult.ResultCode == "200")
                    {
                        dialogResult = new DialogResult(ButtonResult.OK);
                        TransactionEntityManager.GetInstance().TransactionEntity = transactionResponse.transactionEntity;
                        TransactionEntity = transactionResponse.transactionEntity;

                        Navigate(nameof(WalletPreTransferRegion));
                    }
                    else
                    {
                        isProcess = false;
                        dialogResult = new DialogResult(ButtonResult.Ignore);
                        if (transactionResponse.returnResult != null)
                        {
                            dialogResult.Parameters.Add("errMessage", transactionResponse.returnResult.ResultDescription);
                        }
                        else
                        {
                            dialogResult.Parameters.Add("errMessage", "เซิฟเวอร์ไม่เปิดให้บริการ");
                        }
                    }
                }
                else
                {
                    isProcess = false;
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "เซิฟเวอร์ไม่เปิดให้บริการ");
                }

            }

            if (!isProcess) {
                if (dialogResult.Result == ButtonResult.Ignore)
                {
                    dialogService.ShowDialog(
                                    "AlertDialog",
                                    new DialogParameters($"message={dialogResult.Parameters.GetValue<string>("errMessage")}"),
                                        (result) => {
                                            //Navigate(nameof(TransferRegion));
                                        });
                    
                }
            }

            
        }

        async void Transfer()
        {
            var transactionToken = TransactionEntityManager.GetInstance().TransactionEntity.TransactionToken;
            var citizenId = CustomerDetailTransferManager.GetInstance().customerDetail.CitizenID;

            DialogResult dialogResult = null;
            bool isProcess = true;

            #region Check Parameter
            if (isProcess)
                if (string.IsNullOrEmpty(transactionToken))
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "การทำรายการผิดพลาด");
                    isProcess = false;
                }

            if (isProcess)
                if (string.IsNullOrEmpty(citizenId))
                {
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "การทำรายการผิดพลาด");
                    isProcess = false;
                }
            #endregion
            //Call Api Transfer
            if (isProcess)
            {
                TransferService transfer = new();
                WalletTransactionResponse transactionResponse = await transfer.Transfer(transactionToken, citizenId);


                if (transactionResponse != null) {
                    if (transactionResponse.transactionEntity != null && transactionResponse.returnResult != null && transactionResponse.returnResult.ResultCode == "200")  
                    {
                        //Success
                        TransactionEntityManager.GetInstance().TransactionEntity = transactionResponse.transactionEntity;
                        TransactionEntity = transactionResponse.transactionEntity;

                        Navigate(nameof(WalletTransferRegion));
                    }
                    else
                    {
                        isProcess = false;
                        dialogResult = new DialogResult(ButtonResult.Ignore);
                        if (transactionResponse.returnResult != null)
                        {
                            dialogResult.Parameters.Add("errMessage", transactionResponse.returnResult.ResultDescription);
                        }
                        else
                        {
                            dialogResult.Parameters.Add("errMessage", "เซิฟเวอร์ไม่เปิดให้บริการ");
                        }
                    }
                }
                else
                {

                    //Failed
                    isProcess = false;
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("errMessage", "เซิฟเวอร์ไม่เปิดให้บริการ");
                }
            }

            if(!isProcess)
            {
                if (dialogResult.Result == ButtonResult.Ignore)
                {
                    dialogService.ShowDialog(
                                    "AlertDialog",
                                    new DialogParameters($"message={dialogResult.Parameters.GetValue<string>("errMessage")}"),
                                        (result) => {
                                            Navigate(nameof(TransferRegion));
                                        });
                    
                }
            }





        }

        void Print()
        {
            string message = DateTime.Now.ToString(); ;
            dialogService.Show(
                "AlertWaitingDialog",
                new DialogParameters($"message={message}"),
                    (result) => {
                    });
            util.PrintPdf();
        }

        public void TimerCount()
        {
            Timer timer = new Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = false;
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        try
                        {
                            DateTime time = e.SignalTime;
                            RealtimeTimer = time.ToString("MM/dd/yyyy HH:mm:ss");
                        }
                        catch (Exception e)
                        {
                            //
                        }
                    }, null);
                }).Start();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        private async void CheckIcLicense(string _empId)
        {
            try
            {
                if (!string.IsNullOrEmpty(_empId))
                {
                    SecService sec = new();
                    SearchEmployeeResult searchEmployee = await sec.Sec(_empId);

                    if (searchEmployee != null)
                    {
                        if (searchEmployee.IcLicense)
                        {
                            StaffIdText = (searchEmployee.EmpId).ToString();
                            SecIdContent = searchEmployee.SecId;
                            IcLicenseContent = searchEmployee.IcType;
                            StaffRmContent = searchEmployee.RmId;
                            StaffNameContent = searchEmployee.EmpFirstName + " " + searchEmployee.EmpLastName;
                            var tempComboBoxOrderByItemsSource = searchEmployee.ListObjective.Select(x => new StringModel()
                            {
                                Text = x
                            }).ToList();
                            if (tempComboBoxOrderByItemsSource != null && tempComboBoxOrderByItemsSource.Count > 0)
                            {
                                ComboBoxOrderByItemsSource = new(tempComboBoxOrderByItemsSource);
                                ComboBoxOrderBySelectedItem = tempComboBoxOrderByItemsSource[0];
                            }
                        }
                    }
                    else
                    {
                        StaffIdText = "";
                        SecIdContent = "";
                        IcLicenseContent = "";
                        StaffRmContent = "";
                        StaffNameContent = "";
                        List<StringModel> tempComboBoxOrderByItemsSource = new();
                        tempComboBoxOrderByItemsSource.Add(new StringModel() { Text = "" });
                        if (tempComboBoxOrderByItemsSource != null && tempComboBoxOrderByItemsSource.Count > 0)
                        {
                            ComboBoxOrderByItemsSource = new(tempComboBoxOrderByItemsSource);
                        }
                    }
                }
                else
                {
                    StaffIdText = "";
                    SecIdContent = "";
                    IcLicenseContent = "";
                    StaffRmContent = "";
                    StaffNameContent = "";
                    List<StringModel> tempComboBoxOrderByItemsSource = new();
                    tempComboBoxOrderByItemsSource.Add(new StringModel() { Text = "" });
                    if (tempComboBoxOrderByItemsSource != null && tempComboBoxOrderByItemsSource.Count > 0)
                    {
                        ComboBoxOrderByItemsSource = new(tempComboBoxOrderByItemsSource);
                    }
                }

                CheckSelectFundButtonEnable();
            }
            catch (Exception)
            {

            }
        }

        void GoToMainTransfer()
        {
            CustomerDetailTransferManager.GetInstance().customerDetail = null;
            WalletFromEntityManager.GetInstance().WalletEntity.WalletId = null;
            WalletToEntityManager.GetInstance().WalletEntity.WalletId = null;
            BankEntityManager.GetInstance().bankEntity = null;

            CustomerTransferIdTextBox = "";
            CustomerTransferName = "";
            FromWalletSelectedDisplay = "";
            ToWalletSelectedDisplay = "";
            AmountDisplay = "";
            Memo = "";

            CustomerHaveData = false;

            Navigate(nameof(TransferRegion));
        }

        private void SetSellFundTable()
        {
            var fundDetail = FundManager.Instance;
            if (SellFundList == null) SellFundList = new();
            if (fundDetail != null)
            {
                if (fundDetail.Fund != null && fundDetail.SellFundFact != null)
                {
                    SaleType saleType = SaleType.None;
                    Enum.TryParse(fundDetail.SellFundFact.SaleType, out saleType);
                    List<SellFundTableModel> sellFundTables = SellFundList.Where(x => !string.IsNullOrEmpty(x.Id)).Select(y => new SellFundTableModel()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Volume = y.Volume,
                        Price = y.Price,
                        PrintVolume = y.PrintVolume,
                        VolumeText = y.VolumeText,
                        ErrorVolume = (y.ErrorVolume == 0m) ? null : y.ErrorVolume,
                        Fund = y.Fund,
                        SellFundFact = y.SellFundFact,
                        SaleType = y.SaleType,
                        ErrorType = y.ErrorType,
                        Index = y.Index,
                        IsButtonTable = y.IsButtonTable
                    }).ToList();
                    int countSellFund = SellFundList.Count;
                    if (sellFundTables.Count < 10)
                    {
                        decimal? volume = null;
                        decimal? price = null;
                        decimal printVolume = 0m;
                        string volumeText = string.Empty;
                        if (saleType == SaleType.Unit)
                        {
                            volume = fundDetail.SellFundFact.SaleAmount;
                            printVolume = (decimal)volume;
                        }
                        else
                        {
                            price = fundDetail.SellFundFact.SaleAmount;
                            printVolume = (decimal)price;
                        }
                        volumeText = util.ChangeDecimalToText(printVolume, saleType);
                        decimal errorVoulume = fundDetail.SellFundFact.OverAmount;
                        ErrorVolumeType errorType = ErrorVolumeType.None;
                        if (fundDetail.SellFundFact.Inactive_Fact)
                        {
                            if (fundDetail.SellFundFact.LTF_Fact)
                            {
                                errorType = ErrorVolumeType.AccountAndLTFError;
                                errorVoulume = (errorVoulume == 0m) ? (printVolume - fundDetail.Fund.allowRedeem) : errorVoulume;
                            }
                            else if (fundDetail.SellFundFact.SSF_Fact)
                            {
                                errorType = ErrorVolumeType.AccountAndSSFError;
                                errorVoulume = (errorVoulume == 0m) ? (printVolume - fundDetail.Fund.allowRedeem) : errorVoulume;
                            }
                            else if (fundDetail.SellFundFact.RMF_Fact)
                            {
                                errorType = ErrorVolumeType.AccountAndRMFError;
                            }
                            else
                            {
                                errorType = ErrorVolumeType.AccountError;
                            }
                        }
                        else
                        {
                            if (fundDetail.SellFundFact.LTF_Fact)
                            {
                                errorType = ErrorVolumeType.LTFError;
                                errorVoulume = (errorVoulume == 0m) ? (printVolume - fundDetail.Fund.allowRedeem) : errorVoulume;
                            }
                            else if (fundDetail.SellFundFact.SSF_Fact)
                            {
                                errorType = ErrorVolumeType.SSFError;
                                errorVoulume = (errorVoulume == 0m) ? (printVolume - fundDetail.Fund.allowRedeem) : errorVoulume;
                            }
                            else if (fundDetail.SellFundFact.RMF_Fact)
                            {
                                errorType = ErrorVolumeType.RMFError;
                            }
                            else
                            {
                                errorType = ErrorVolumeType.Ok;
                            }
                        }
                        sellFundTables.Add(new SellFundTableModel()
                        {
                            Id = fundDetail.Fund.fundCode,
                            Name = fundDetail.Fund.fundName,
                            Volume = volume,
                            Price = price,
                            PrintVolume = printVolume,
                            VolumeText = volumeText,
                            ErrorVolume = (errorVoulume == 0m) ? null : errorVoulume,
                            Fund = fundDetail.Fund,
                            SellFundFact = fundDetail.SellFundFact,
                            SaleType = saleType,
                            ErrorType = errorType,
                            IsButtonTable = true,
                        });
                        int newCountSellFund = sellFundTables.Count;
                        if (newCountSellFund < countSellFund)
                        {
                            for (int i = newCountSellFund; i < countSellFund; i++)
                            {
                                sellFundTables.Add(new SellFundTableModel()
                                {
                                    IsButtonTable = false
                                });
                            }
                            for (int j = 0; j < sellFundTables.Count; j++)
                            {
                                sellFundTables[j].Index = j;
                            }

                            SellFundList = sellFundTables.Select(y => new SellFundTableModel()
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Volume = y.Volume,
                                Price = y.Price,
                                PrintVolume = y.PrintVolume,
                                VolumeText = y.VolumeText,
                                ErrorVolume = (y.ErrorVolume == 0m) ? null : y.ErrorVolume,
                                Fund = y.Fund,
                                SellFundFact = y.SellFundFact,
                                SaleType = y.SaleType,
                                ErrorType = y.ErrorType,
                                Index = y.Index,
                                IsButtonTable = y.IsButtonTable
                            }).ToList();
                            SellFundListGrid = new (SellFundList);
                        }
                        else
                        {
                            for (int j = 0; j < sellFundTables.Count; j++)
                            {
                                sellFundTables[j].Index = j;
                            }
                            SellFundList = sellFundTables.Select(y => new SellFundTableModel()
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Volume = y.Volume,
                                Price = y.Price,
                                PrintVolume = y.PrintVolume,
                                VolumeText = y.VolumeText,
                                ErrorVolume = (y.ErrorVolume == 0m) ? null : y.ErrorVolume,
                                Fund = y.Fund,
                                SellFundFact = y.SellFundFact,
                                SaleType = y.SaleType,
                                ErrorType = y.ErrorType,
                                Index = y.Index,
                                IsButtonTable = y.IsButtonTable
                            }).ToList();
                            SellFundListGrid = new(SellFundList);
                        }
                    }
                }
            }
        }

        void LostFocus()
        {
            if (!string.IsNullOrEmpty(AmountDisplay))
            {
                if (AmountDisplay.IndexOf('.') < 0)
                {
                    AmountDisplay = AmountDisplay + ".00";
                }

                checkAmountInput();
            }
        }

        void GotFocus()
        {
            if (!string.IsNullOrEmpty(AmountDisplay))
            {
                AmountDisplay = TrimComma(AmountDisplay);
            }
        }

        string TrimComma(string rawstring)
        {
            string trimComma = rawstring.Replace(",", string.Empty);
            return trimComma;
        }

        bool checkCountPoint()
        {
            int countPoint = 0;
            foreach (Char item in AmountDisplay)
            {
                if (item == 46)
                {
                    countPoint++;
                }
            }

            if (countPoint > 1)
                return true;
            else
                return false;

        }

        void checkAmountInput()
        {
            if(!checkCountPoint())
            {
                systemSetAmount = true;
                decimal de1 = Convert.ToDecimal(AmountDisplay);
                string tempAmount1 = de1.ToString("0.##");
                decimal de2 = Convert.ToDecimal(tempAmount1);
                string tempAmount2 = de2.ToString("N");
                AmountDisplay = tempAmount2;
            }
        }






    }
}
