using MainModule.GrpcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Teller.Core.Models;
using WPFScbOri.Models;
using WPFScbOri.Service;

namespace MainModule.ViewModels
{
    class FundDetailDialogViewModel : BindableBase,IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private string saleBoxUnitText;
        public string SaleBoxUnitText { get => saleBoxUnitText; set => SetProperty(ref saleBoxUnitText, value); }

        private string saleBoxText;
        public string SaleBoxText
        {
            get => saleBoxText;
            set
            {
                Regex regex = new Regex("[^0-9.]+");
                if (!regex.IsMatch(value))
                {
                    SetProperty(ref saleBoxText, value);
                }
            }
        }

        private string previewSaleBoxText;
        public string PreviewSaleBoxText { 
            get => previewSaleBoxText; 
            set => SetProperty(ref previewSaleBoxText, value);
        }
        private string _title = "FundDetailDialog";

        private DelegateCommand radioBut1Command;
        public DelegateCommand RadioBut1Command => radioBut1Command ?? (radioBut1Command = new DelegateCommand(RadioBut1Click));

        private DelegateCommand radioBut2Command;
        public DelegateCommand RadioBut2Command => radioBut2Command ?? (radioBut2Command = new DelegateCommand(RadioBut2Click));

        public DelegateCommand GotFocusCommand => gotFocusCommand ?? (gotFocusCommand = new DelegateCommand(GotFocus));

        private DelegateCommand gotFocusCommand;

        public DelegateCommand LostFocusCommand => lostFocusCommand ?? (lostFocusCommand = new DelegateCommand(LostFocus));

        private DelegateCommand lostFocusCommand;

        public DelegateCommand<TextBox> PreviewTextInputCommand => previewTextInputCommand ?? (previewTextInputCommand = new DelegateCommand<TextBox>(PreviewTextInput));

        public DelegateCommand<TextBox> TextChangedCommand => textChangedCommand ?? (textChangedCommand = new DelegateCommand<TextBox>(TextChanged));

        private DelegateCommand<TextBox> previewTextInputCommand;

        private DelegateCommand<TextBox> textChangedCommand;
        public DelegateCommand BackButtonCommand => backButtonCommand ?? (backButtonCommand = new DelegateCommand(BackButton));

        public DelegateCommand<ComboBox> ComboBoxFundChoice1Command => comboBoxFundChoice1Command ?? (comboBoxFundChoice1Command = new DelegateCommand<ComboBox>(comboBoxFundChoice1));

        public SellOption FundChoice1Selected { get => fundChoice1Selected; set => SetProperty(ref fundChoice1Selected, value); }
        private List<SellOption> fundChoice1ItemSource;
        public List<SellOption> FundChoice1ItemSource { get => fundChoice1ItemSource; set => SetProperty(ref fundChoice1ItemSource, value); }
        private List<SellOption> fundChoice2ItemSource;
        public List<SellOption> FundChoice2ItemSource { get => fundChoice2ItemSource; set => SetProperty(ref fundChoice2ItemSource, value); }

        private DelegateCommand backButtonCommand;

        private DelegateCommand<ComboBox> comboBoxFundChoice1Command;

        private SellOption fundChoice1Selected;
        public event Action<IDialogResult> RequestClose;
        private ObservableCollection<RedeemAccount> listAccount;
        public ObservableCollection<RedeemAccount> ListAccount { get => listAccount; set => SetProperty(ref listAccount, value); }
        private List<RedeemAccount> tempListAccount;
        public List<RedeemAccount> TempListAccount { get => tempListAccount; set => SetProperty(ref tempListAccount, value); }

        private RedeemAccount listAccountSelected;
        public RedeemAccount ListAccountSelected { get => listAccountSelected; set => SetProperty(ref listAccountSelected, value); }
        private Visibility accActive;
        public Visibility AccActive { get => accActive; set => SetProperty(ref accActive, value); }
        private Visibility accNotActive;
        public Visibility AccNotActive { get => accNotActive; set => SetProperty(ref accNotActive, value); }
        public DelegateCommand ConfirmButtonCommand => confirmButtonCommand ?? (confirmButtonCommand = new DelegateCommand(ConfirmButtonClick));

        private DelegateCommand confirmButtonCommand;
        private Visibility layoutContent1Visibility;
        private Visibility layoutContent2Visibility;
        private Visibility layoutContent3Visibility;
        private Visibility layoutContent4Visibility;
        private Visibility layoutContent5Visibility;
        private Visibility layoutContent6Visibility;
        private Visibility layoutContent7Visibility;
        public Visibility LayoutContent1Visibility { get => layoutContent1Visibility; set => SetProperty(ref layoutContent1Visibility, value); }
        public Visibility LayoutContent2Visibility { get => layoutContent2Visibility; set => SetProperty(ref layoutContent2Visibility, value); }
        public Visibility LayoutContent3Visibility { get => layoutContent3Visibility; set => SetProperty(ref layoutContent3Visibility, value); }
        public Visibility LayoutContent4Visibility { get => layoutContent4Visibility; set => SetProperty(ref layoutContent4Visibility, value); }
        public Visibility LayoutContent5Visibility { get => layoutContent5Visibility; set => SetProperty(ref layoutContent5Visibility, value); }
        public Visibility LayoutContent6Visibility { get => layoutContent6Visibility; set => SetProperty(ref layoutContent6Visibility, value); }
        public Visibility LayoutContent7Visibility { get => layoutContent7Visibility; set => SetProperty(ref layoutContent7Visibility, value); }

        private DelegateCommand layout5ConfirmButtonCommand;
        public DelegateCommand Layout5ConfirmButtonCommand => layout5ConfirmButtonCommand ?? (layout5ConfirmButtonCommand = new DelegateCommand(Layout5ConfirmButtonClick));
        public DelegateCommand LayoutBackButtonCommand => layoutBackButtonCommand ?? (layoutBackButtonCommand = new DelegateCommand(LayoutBackButton));

        private bool radioBut1IsChecked;
        public bool RadioBut1IsChecked { get => radioBut1IsChecked; set => SetProperty(ref radioBut1IsChecked, value); }

        private bool radioBut2IsChecked;
        public bool RadioBut2IsChecked { get => radioBut2IsChecked; set => SetProperty(ref radioBut2IsChecked, value); }

        private DelegateCommand layoutBackButtonCommand;

        private DelegateCommand layout4ConfirmButtonClickCommand;
        public DelegateCommand Layout4ConfirmButtonClickCommand => layout4ConfirmButtonClickCommand ?? (layout4ConfirmButtonClickCommand = new DelegateCommand(Layout4ConfirmButtonClick));

        private Visibility comboBoxFundChoice2Visibility;

        public Visibility ComboBoxFundChoice2Visibility { get => comboBoxFundChoice2Visibility; set => SetProperty(ref comboBoxFundChoice2Visibility, value); }

        private Visibility comboBoxFundChoice2TextVisibility;

        public Visibility ComboBoxFundChoice2TextVisibility { get => comboBoxFundChoice2TextVisibility; set => SetProperty(ref comboBoxFundChoice2TextVisibility, value); }
        public Visibility AccImageVisibility { get => accImageVisibility; set => SetProperty(ref accImageVisibility, value); }
        public string AllowRedeemText { get => allowRedeemText; set => SetProperty(ref allowRedeemText, value); }
        public string FundCodeText { get => fundCodeText; set => SetProperty(ref fundCodeText, value); }
        public string FundNameText { get => fundNameText; set => SetProperty(ref fundNameText, value); }
        public bool RadioBut2IsEnabled { get => radioBut2IsEnabled; set => SetProperty(ref radioBut2IsEnabled, value); }
        public Visibility AllowRedeemGirdVisibility { get => allowRedeemGirdVisibility; set => SetProperty(ref allowRedeemGirdVisibility, value); }
        public string TotalUnitText { get => totalUnitText; set => SetProperty(ref totalUnitText, value); }
        public string TotalBahtText { get => totalBahtText; set => SetProperty(ref totalBahtText, value); }
        public string MinimumSellableUnitText { get => minimumSellableUnitText; set => SetProperty(ref minimumSellableUnitText, value); }
        public string MinimumSellableBahtText { get => minimumSellableBahtText; set => SetProperty(ref minimumSellableBahtText, value); }
        public string MinimumUnitText { get => minimumUnitText; set => SetProperty(ref minimumUnitText, value); }
        public string MinimumBahtText { get => minimumBahtText; set => SetProperty(ref minimumBahtText, value); }
        public string FundChoice1Text { get => fundChoice1Text; set => SetProperty(ref fundChoice1Text, value); }

        private Visibility accImageVisibility;

        private string allowRedeemText;

        private string fundCodeText;

        private string fundNameText;

        private bool radioBut2IsEnabled;

        private Visibility allowRedeemGirdVisibility;

        private string totalUnitText;

        private string totalBahtText;

        private string minimumSellableUnitText;

        private string minimumSellableBahtText;

        private string minimumUnitText;

        private string minimumBahtText;

        private string fundChoice1Text;

        private DelegateCommand layout1ConfirmButtonClickCommand;
        public DelegateCommand Layout1ConfirmButtonClickCommand => layout1ConfirmButtonClickCommand ?? (layout1ConfirmButtonClickCommand = new DelegateCommand(Layout1ConfirmButtonClick));

        private DelegateCommand layout2ConfirmButtonClickCommand;
        public DelegateCommand Layout2ConfirmButtonClickCommand => layout2ConfirmButtonClickCommand ?? (layout2ConfirmButtonClickCommand = new DelegateCommand(Layout2ConfirmButtonClick));

        private DelegateCommand layout3ConfirmButtonClickCommand;
        public DelegateCommand Layout3ConfirmButtonClickCommand => layout3ConfirmButtonClickCommand ?? (layout3ConfirmButtonClickCommand = new DelegateCommand(Layout3ConfirmButtonClick));

        private DelegateCommand<ComboBox> accountChangedCommand;
        public DelegateCommand<ComboBox> AccountChangedCommand => accountChangedCommand ?? (accountChangedCommand = new DelegateCommand<ComboBox>(SetAccountOption));


        private void Layout3ConfirmButtonClick()
        {
            if (FundManager.Instance.SellFundFact.Inactive_Fact == true)
            {
                LayoutContent3Visibility = Visibility.Hidden;
                LayoutContent4Visibility = Visibility.Visible;
            }
            else
            {
                //FundDetailDialog.Visibility = Visibility.Hidden;
                //SetSellFundTable();
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
        }
        private void Layout2ConfirmButtonClick()
        {
            if (FundManager.Instance.SellFundFact.Inactive_Fact == true)
            {
                LayoutContent2Visibility = Visibility.Hidden;
                LayoutContent4Visibility = Visibility.Visible;
            }
            else
            {
                //FundDetailDialog.Visibility = Visibility.Hidden;
                //SetSellFundTable();
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
        }

        private void comboBoxFundChoice1(ComboBox comboBox)
        {
            if (comboBox.SelectedValue != null)
            {
                FundChoice1Selected = (SellOption)comboBox.SelectedValue;
                SellOption sellOption = (SellOption)FundChoice1Selected;
                if (sellOption.optionValue == 1)
                {
                    ComboBoxFundChoice2Visibility = Visibility.Visible;
                    ComboBoxFundChoice2TextVisibility = Visibility.Visible;
                    AccImageVisibility = Visibility.Visible;
                }
                if (sellOption.optionValue == 2)
                {
                    ComboBoxFundChoice2Visibility = Visibility.Hidden;
                    ComboBoxFundChoice2TextVisibility = Visibility.Hidden;
                    AccImageVisibility = Visibility.Hidden;
                }
            }
            
        }

        void LayoutBackButton()
        {
            LayoutContent1Visibility = Visibility.Hidden;
            LayoutContent2Visibility = Visibility.Hidden;
            LayoutContent3Visibility = Visibility.Hidden;
            LayoutContent4Visibility = Visibility.Hidden;
            LayoutContent5Visibility = Visibility.Hidden;
            LayoutContent6Visibility = Visibility.Hidden;
            LayoutContent7Visibility = Visibility.Hidden;
        }

        private void Layout1ConfirmButtonClick()
        {
            if (FundManager.Instance.SellFundFact.Inactive_Fact == true)
            {
                LayoutContent1Visibility = Visibility.Hidden;
                LayoutContent4Visibility = Visibility.Visible;
            }
            else
            {
                //FundDetailDialog.Visibility = Visibility.Hidden;
                //SetSellFundTable();
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
        }

        private void Layout4ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            LayoutContent2Visibility = Visibility.Hidden;
            LayoutContent4Visibility = Visibility.Hidden;

            //FundDetailDialog.Visibility = Visibility.Hidden;
            //SetSellFundTable();
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        private void Layout5ConfirmButtonClick()
        {
            FundManager.GetInstance().Fund = null;
            FundManager.GetInstance().SellFundFact = null;
            FundManager.GetInstance().SellFundFact = null;

            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        void ConfirmButtonClick()
        {
            //Check Same Sale List
            if (FundManager.Instance.ListFundCode != null && FundManager.Instance.ListFundCode.Count > 0)
            {
                for (int i = 0; i < FundManager.Instance.ListFundCode.Count; i++)
                {
                    if (FundManager.Instance.ListFundCode[i] == FundManager.Instance.Fund.fundCode)
                    {
                        LayoutContent5Visibility = Visibility.Visible;
                        return;
                    }
                }
            }

            //Check Sall Amount
            if (Convert.ToDecimal(SaleBoxText) == 0)
            {
                LayoutContent6Visibility = Visibility.Visible;
                return;
            }

            SellFundFact sellFundFact = new SellFundFact();
            if (RadioBut1IsChecked == true)
            {
                if (Convert.ToDecimal(SaleBoxText) > FundManager.Instance.Fund.totalUnit)
                {
                    LayoutContent7Visibility = Visibility.Visible;
                    return;
                }
                sellFundFact.SaleType = "Unit";
            }
            else if (RadioBut2IsChecked == true)
            {
                if (Convert.ToDecimal(SaleBoxText) > FundManager.Instance.Fund.totalBaht)
                {
                    LayoutContent7Visibility = Visibility.Visible;
                    return;
                }
                sellFundFact.SaleType = "Baht";
            }

            sellFundFact.SaleAmount = Convert.ToDecimal(SaleBoxText);

            SellOption sellOption = (SellOption)FundChoice1Selected;
            RedeemAccount accountOption = (RedeemAccount)ListAccountSelected;
            if (FundManager.Instance.Fund.fundGroupName == "LTF")
            {
                if (FundManager.Instance.Fund.allowRedeem > 0 && sellFundFact.SaleAmount > FundManager.Instance.Fund.allowRedeem)
                {
                    sellFundFact.LTF_Fact = true;
                }
            }

            if (FundManager.Instance.Fund.fundGroupName == "SSF")
            {
                if (FundManager.Instance.Fund.allowRedeem > 0 && sellFundFact.SaleAmount > FundManager.Instance.Fund.allowRedeem)
                {
                    sellFundFact.SSF_Fact = true;
                }
            }

            if (FundManager.Instance.Fund.fundGroupName == "RMF")
            {
                string cusAge = FundManager.Instance.CustAge;
                if (cusAge != null && cusAge != "")
                {
                    if (Int32.Parse(cusAge) < 55)
                    {
                        sellFundFact.RMF_Fact = true;
                    }
                }
            }

            if (sellOption.optionValue == 1)
            {
                sellFundFact.RecieveType = "Account";
                sellFundFact.RecieveAccount = accountOption.id;
                if (accountOption.status == AccStatus.ACTIVE)
                {
                    sellFundFact.Inactive_Fact = false;
                }
                else if (accountOption.status == AccStatus.INACTIVE)
                {
                    sellFundFact.Inactive_Fact = true;
                }
            }
            else if (sellOption.optionValue == 2)
            {
                sellFundFact.RecieveType = "Cheque";
                sellFundFact.RecieveAccount = null;
            }

            FundManager.GetInstance().SellFundFact = sellFundFact;

            if (sellFundFact.LTF_Fact == true)
            {
                LayoutContent1Visibility = Visibility.Visible;
            }
            else if (sellFundFact.RMF_Fact == true)
            {
                LayoutContent2Visibility = Visibility.Visible;
            }
            else if (sellFundFact.SSF_Fact == true)
            {
                LayoutContent3Visibility = Visibility.Visible;
            }
            else if (sellFundFact.Inactive_Fact == true)
            {
                LayoutContent4Visibility = Visibility.Visible;
            }
            else
            {
                //FundDetailDialog.Visibility = Visibility.Hidden;
                //SetSellFundTable();
                RaiseRequestClose(new DialogResult(ButtonResult.OK));
            }
        }

        private void SetSellOption()
        {
            if (FundChoice1ItemSource == null)
            {
                List<SellOption> ListSellOption = new List<SellOption>();
                ListSellOption.Add(new SellOption("T-บัญชี", 1));
                ListSellOption.Add(new SellOption("Cheque", 2));

                FundChoice1ItemSource = ListSellOption;
                FundChoice1Selected = ListSellOption[0];
            }
        }

        private void SetAccountOption(ComboBox comboBox)
        {
            if (comboBox != null && comboBox.SelectedItem != null && FundChoice2ItemSource == null)
            {

                if (ListAccount != null)
                {
                    ListAccountSelected = (RedeemAccount)comboBox.SelectedItem;

                    if (ListAccountSelected.status == AccStatus.ACTIVE)
                    {
                        AccActive = Visibility.Visible;
                        AccNotActive = Visibility.Hidden;
                    }
                    else
                    {
                        AccNotActive = Visibility.Visible;
                        AccActive = Visibility.Hidden;
                    }
                }
            }
        }

        private async void SetAccountOptionInit()
        {
            if (FundChoice2ItemSource == null)
            {
                //SelectedFundService selectedFundService = new SelectedFundService();
                //TempListAccount = selectedFundService.SetAccountOption();
                AccountService accountService = new();
                var TempListAccount = await accountService.ListRedeemAccount(AccountManager.GetInstance().Account.id.ToString());

                if (TempListAccount != null)
                {
                    ListAccount = new(TempListAccount);
                    ListAccountSelected = ListAccount[0];

                    if (ListAccountSelected.status == AccStatus.ACTIVE)
                    {
                        AccActive = Visibility.Visible;
                        AccNotActive = Visibility.Hidden;
                    }
                    else
                    {
                        AccNotActive = Visibility.Visible;
                        AccActive = Visibility.Hidden;
                    }
                }
            }
        }

        public class SellOption
        {
            public String optionName { get; set; }
            public int optionValue { get; set; }
            public SellOption(String optionName, int optionValue)
            {
                this.optionName = optionName;
                this.optionValue = optionValue;
            }
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            SaleBoxUnitText = "หน่วย";
            SaleBoxText = "0";
            FundChoice1Text = "Select Account";
            AccActive = Visibility.Hidden;
            AccNotActive = Visibility.Hidden;
            LayoutContent1Visibility = Visibility.Hidden;
            LayoutContent2Visibility = Visibility.Hidden;
            LayoutContent3Visibility = Visibility.Hidden;
            LayoutContent4Visibility = Visibility.Hidden;
            LayoutContent5Visibility = Visibility.Hidden;
            LayoutContent6Visibility = Visibility.Hidden;
            LayoutContent7Visibility = Visibility.Hidden;
            RadioBut1IsChecked = true;
            RadioBut2IsChecked = false;
            RadioBut2IsEnabled = false;
            SetFundDetail();
            SetSellOption();
            SetAccountOptionInit();
        }

        private void SetFundDetail()
        {
            string defaultUnit = "0.0000";
            string defaultBaht = "0.00";
            if (FundManager.Instance.Fund.allowRedeem == 0)
            {
                AllowRedeemText = "0.0000";
            }
            else
            {
                AllowRedeemText = String.Format("{0:#,#.0000}", FundManager.Instance.Fund.allowRedeem);
            }
            FundCodeText = FundManager.Instance.Fund.fundCode;
            FundNameText = FundManager.Instance.Fund.fundName;

            if (FundManager.Instance.Fund.fundGroupName == "SSF" ||
                FundManager.Instance.Fund.fundGroupName == "RMF" ||
                FundManager.Instance.Fund.fundGroupName == "LTF"
                )
            {
                RadioBut2IsEnabled = false;
            }

            if (FundManager.Instance.Fund.fundGroupName != "SSF" && FundManager.Instance.Fund.fundGroupName != "LTF")
            {
                AllowRedeemGirdVisibility = Visibility.Hidden;
            }

            if (FundManager.Instance.Fund.totalUnit == 0)
            {
                TotalUnitText = defaultUnit;
            }
            else
            {
                TotalUnitText = String.Format("{0:#,#.####}", FundManager.Instance.Fund.totalUnit);
            }
            if (FundManager.Instance.Fund.totalBaht == 0)
            {
                TotalBahtText = defaultBaht;
            }
            else
            {
                TotalBahtText = String.Format("{0:#,#.##}", FundManager.Instance.Fund.totalBaht);
            }

            if (FundManager.Instance.Fund.minimumSellableUnit == 0)
            {
                MinimumSellableUnitText = defaultUnit;
            }
            else
            {
                MinimumSellableUnitText = String.Format("{0:#,#.####}", FundManager.Instance.Fund.minimumSellableUnit);
            }
            if (FundManager.Instance.Fund.minimumSellableBaht == 0)
            {
                MinimumSellableBahtText = defaultBaht;
            }
            else
            {
                MinimumSellableBahtText = String.Format("{0:#,#.##}", FundManager.Instance.Fund.minimumSellableBaht);
            }

            if (FundManager.Instance.Fund.minimumUnit == 0)
            {
                MinimumUnitText = defaultUnit;
            }
            else
            {
                MinimumUnitText = String.Format("{0:#,#.####}", FundManager.Instance.Fund.minimumUnit);
            }
            if (FundManager.Instance.Fund.minimumBaht == 0)
            {
                MinimumBahtText = defaultBaht;
            }
            else
            {
                MinimumBahtText = String.Format("{0:#,#.##}", FundManager.Instance.Fund.minimumBaht);
            }
        }

        private void Layout4ConfirmButtonClick()
        {
            //FundDetailDialog.Visibility = Visibility.Hidden;
            //SetSellFundTable();
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        void BackButton()
        {
            ButtonResult result = ButtonResult.None;

            result = ButtonResult.Ignore;
            RaiseRequestClose(new DialogResult(result));
        }



        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "close") {
                result = ButtonResult.Cancel;
                RaiseRequestClose(new DialogResult(result));
            }

        }

        void RadioBut1Click()
        {
            SaleBoxUnitText = "หน่วย";
        }

        void RadioBut2Click()
        {
            SaleBoxUnitText = "บาท";
        }

        void GotFocus()
        {
            if (SaleBoxText.Length >= 6)
            {
                SaleBoxText = (SaleBoxText).Substring(0, (SaleBoxText).Length - 5);
            }
        }

        void LostFocus()
        {
            if (SaleBoxText.IndexOf('.') < 0)
            {
                SaleBoxText = SaleBoxText + ".0000";
            }
        }

        void PreviewTextInput(TextBox saleBox)
        {
            //Regex regex = new Regex("[^0-9.]+");
            //if (regex.IsMatch(saleBox))
            //{
            //    SaleBoxText = saleBox;
            //}
            //else
            //{
            //    SaleBoxText = "";
            //}
        }

        void TextChanged(TextBox saleBox)
        {

        }

        

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
            //
        }
    }
}
