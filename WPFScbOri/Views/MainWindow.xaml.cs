using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Teller.Core.Models;
using Teller.SellFund.Models;
using WPFScbOri.Models;
using WPFScbOri.Service;
using Timer = System.Timers.Timer;

namespace WPFScbOri.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Set Info
            fullname.Text = $"ชื่อ-{AccountManager.Instance.Account.name} นามสกุล-{AccountManager.Instance.Account.surname}";
            idBrn.Text = $"ID:{AccountManager.Instance.Account.id} / BRN:{AccountManager.Instance.Account.branchCode}";

            BottomText.Text = AccountManager.Instance.Account.id.ToString();
            BottomText2.Text = AccountManager.Instance.Account.branchCode.ToString();
            TimerCount();
        }

        #region *** Template Member ***

        private List<RedeemAccount> ListAccount;

        private List<Fund> ListFund;

        private List<SellFundTableModel> SellFundList = new List<SellFundTableModel>();

        private Dictionary<string, string> CustomerSelected = new Dictionary<string, string>();

        private ObservableCollection<CustomerDetail> CustomerDetailLists;
        private ObservableCollection<string> SearchTypes
        {
            get
            {
                return new ObservableCollection<string>
                {
                    "เลขทะเบียนลูกค้า",
                    "บัตรประชาชน",
                    "พาสปอร์ต",
                    "ชื่อ - นามสกุล",
                    "เลขทะเบียนนิติบุคคล",
                };
            }
        }

        private class SellOption
        {
            public String optionName { get; set; }
            public int optionValue { get; set; }
            public SellOption(String optionName, int optionValue)
            {
                this.optionName = optionName;
                this.optionValue = optionValue;
            }
        }

        #endregion

        #region *** Template Function ***

        private void CheckIcLicense()
        {
            try
            {
                string userId = AccountManager.Instance.Account.id.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    SellFundMainService sellFundMainService = new SellFundMainService();
                    SearchEmployeeResult searchEmployee = sellFundMainService.CheckIcLicense(userId);
                    if (searchEmployee != null)
                    {
                        if (searchEmployee.HttpStatus == System.Net.HttpStatusCode.OK)
                        {
                            if (searchEmployee.IcLicense)
                            {
                                StaffId.Text = (searchEmployee.EmpId).ToString();
                                SecId.Content = searchEmployee.SecId;
                                IcLicense.Content = searchEmployee.IcType;
                                StaffRm.Content = searchEmployee.RmId;
                                StaffName.Content = searchEmployee.EmpFirstName + " " + searchEmployee.EmpLastName;
                                ComboBoxOrderBy.ItemsSource = searchEmployee.ListObjective.Select(x => new StringModel()
                                {
                                    Text = x
                                }).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void OpenSearchCustomerDialog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CustomerSellFundDialog.Visibility = Visibility.Visible;
            SearchCustomerComboBox.ItemsSource = SearchTypes;
        }

        private void OpenSearchEmployeeDialog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchEmployeeDialog.Visibility = Visibility.Visible;

            SearchEmployeeSerivce searchEmployeeSerivce = new SearchEmployeeSerivce();
            var BranchTextList = searchEmployeeSerivce.GetBranchText();
            if (BranchTextList.Count > 0)
            {
                ComboBoxBranch.ItemsSource = BranchTextList;
                ComboBoxBranch.SelectedItem = BranchTextList[0];
            }

            var EmpIdTextList = searchEmployeeSerivce.GetEmpIdText();
            if (EmpIdTextList.Count > 0)
            {
                UserIdBox.Visibility = Visibility.Hidden;
                ComboBoxUserId.ItemsSource = EmpIdTextList;
                ComboBoxUserId.SelectedItem = EmpIdTextList[0];
            }
        }

        private void CloseDialog_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CustomerSellFundDialog.Visibility = Visibility.Hidden;
            SearchEmployeeDialog.Visibility = Visibility.Hidden;
            SelectFundDialog.Visibility = Visibility.Hidden;
            FundDetailDialog.Visibility = Visibility.Hidden;
            CloseGraph();
        }

        private void SearchCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchCustomerComboBox.SelectedItem is null)
            {
                MessageBox.Show("โปรดเลือกประเภทการค้นหา", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (SearchCustomerTextBox.Text is null)
            {
                MessageBox.Show("โปรดกรอกข้อมูลในการค้นหา", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SearchCustomerService searchCustomerService = new SearchCustomerService();
            CustomerDetailLists = searchCustomerService.SeachCustomer(SearchCustomerComboBox.SelectedItem.ToString(), SearchCustomerTextBox.Text);

            if (CustomerDetailLists.Count() == 0)
            {
                MessageBox.Show("ไม่พบข้อมูลในระบบ", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            CustomerDetailListsGrid.ItemsSource = CustomerDetailLists;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerSellFundDialog.Visibility = Visibility.Hidden;
            SearchEmployeeDialog.Visibility = Visibility.Hidden;
        }

        private void CustomerSelected_Click(object sender, RoutedEventArgs e)
        {
            string custId = (((Button)sender).Tag).ToString();
            foreach (var i in CustomerDetailLists)
            {
                if (custId == i.CustId)
                {
                    CustomerSelected.Add("CustomerName", (i.AccName).ToString());
                    CustomerSelected.Add("CustomerNumber", (i.CustId).ToString());
                    CustomerSelected.Add("Branch", (i.Branch).ToString());
                    CustomerSelected.Add("JointAccount", (i.IsJointAccount).ToString());
                    CustomerSelected.Add("SensitiveCustomer", (i.IsSensitive).ToString());
                    CustomerSelected.Add("OrderPayment", (i.Payment).ToString());
                    CustomerSelected.Add("Age", (i.Age).ToString());

                    CustomerIdTextBox.Text = (i.CustId).ToString();
                    FundAccountIdTextBox.Text = (i.Branch).ToString();
                    CustomerName.Content = (i.AccName).ToString();
                    JointAccount.Content = (i.IsJointAccount).ToString();
                    SensitiveCustomer.Content = (i.IsSensitive).ToString();
                    RiskLevel.Content = "5";

                    FundManager.GetInstance().CustAge = (i.Age).ToString();
                    FundManager.GetInstance().CustAccount = (i.CustId).ToString();

                    CustomerSellFundDialog.Visibility = Visibility.Hidden;

                    return;
                }
            }
        }

        private void ComboBoxBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StringModel branchText = (StringModel)ComboBoxBranch.SelectedItem;
            if (branchText != null)
            {
                if (branchText.Text == "พนักงานในสาขา")
                {
                    ComboBoxUserId.Visibility = Visibility.Visible;
                    UserIdBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    ComboBoxUserId.Visibility = Visibility.Hidden;
                    UserIdBox.Visibility = Visibility.Visible;
                }
            }
        }

        private void EmployeeSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            SearchEmployeeDialog.Visibility = Visibility.Hidden;
            CheckIcLicense();
        }

        private void OpenSelectedFundDialog_Click(object sender, RoutedEventArgs e)
        {
            SelectFundDialog.Visibility = Visibility.Visible;
            GetFundGroupList();
            GetFundList();
        }

        private void GetFundGroupList()
        {
            SelectedFundService selectedFundService = new SelectedFundService();
            var ListFundGroups = selectedFundService.GetFundGroupList();

            if (ListFundGroups.Count > 0)
            {
                fundGroupNameDy.Text = "ทั้งหมด";
                ComboBoxFund.ItemsSource = ListFundGroups;
            }
        }

        private void GetFundList()
        {
            SelectedFundService selectedFundService = new SelectedFundService();
            if ((string)ComboBoxFund.SelectedItem == "ทั้งหมด")
            {
                ListFund = selectedFundService.GetFundList();
            }
            else
            {
                ListFund = selectedFundService.GetFundList(FundManager.Instance.CustAccount, (string)ComboBoxFund.SelectedItem, "");
            }

            if (ListFund.Count > 0)
            {
                for (int i = 0; i < ListFund.Count; i++)
                {
                    ListFund[i].totalBaht = Math.Round(ListFund[i].totalBaht, 2, MidpointRounding.AwayFromZero);
                    if (ListFund[i].gainAmount >= 0)
                    {
                        ListFund[i].profitStatus = true;
                        ListFund[i].totalGain = "+" + String.Format("{0:#,#.##}", ListFund[i].gainAmount) + " (+" + String.Format("{0:#,#.##}", ListFund[i].gainPercentage) + "%)";
                    }
                    else
                    {
                        ListFund[i].profitStatus = false;
                        ListFund[i].totalGain = String.Format("{0:#,#.##}", ListFund[i].gainAmount) + " (" + String.Format("{0:#,#.##}", ListFund[i].gainPercentage) + "%)";
                    }
                }
                dgFund.ItemsSource = ListFund;
            }
        }

        private void ComboBoxFund_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fundGroupNameDy.Text = (string)ComboBoxFund.SelectedItem;
            GetFundList();
        }

        private void OpenFundDetailDialog_Click(object sender, RoutedEventArgs e)
        {
            var fundCode = ((Button)sender).Tag;
            for (int i = 0; i < ListFund.Count; i++)
            {
                if (fundCode == ListFund[i].fundCode)
                {
                    FundManager.GetInstance().Fund = ListFund[i];
                    SelectFundDialog.Visibility = Visibility.Hidden;
                    FundDetailDialog.Visibility = Visibility.Visible;
                    SaleBox.Text = "0";
                    SetFundDetail();
                    SetSellOption();
                    SetAccountOption();
                }
            }
        }

        private void SetFundDetail()
        {
            string defaultUnit = "0.0000";
            string defaultBaht = "0.00";
            if (FundManager.Instance.Fund.allowRedeem == 0)
            {
                AllowRedeem.Text = "0.0000";
            }
            else
            {
                AllowRedeem.Text = String.Format("{0:#,#.0000}", FundManager.Instance.Fund.allowRedeem);
            }
            FundCode.Text = FundManager.Instance.Fund.fundCode;
            FundName.Text = FundManager.Instance.Fund.fundName;

            if (FundManager.Instance.Fund.fundGroupName == "SSF" ||
                FundManager.Instance.Fund.fundGroupName == "RMF" ||
                FundManager.Instance.Fund.fundGroupName == "LTF"
                )
            {
                RadioBut2.IsEnabled = false;
            }

            if (FundManager.Instance.Fund.fundGroupName != "SSF" && FundManager.Instance.Fund.fundGroupName != "LTF")
            {
                AllowRedeemGird.Visibility = Visibility.Hidden;
            }

            if (FundManager.Instance.Fund.totalUnit == 0)
            {
                TotalUnit.Text = defaultUnit;
            }
            else
            {
                TotalUnit.Text = String.Format("{0:#,#.####}", FundManager.Instance.Fund.totalUnit);
            }
            if (FundManager.Instance.Fund.totalBaht == 0)
            {
                TotalBaht.Text = defaultBaht;
            }
            else
            {
                TotalBaht.Text = String.Format("{0:#,#.##}", FundManager.Instance.Fund.totalBaht);
            }

            if (FundManager.Instance.Fund.minimumSellableUnit == 0)
            {
                MinimumSellableUnit.Text = defaultUnit;
            }
            else
            {
                MinimumSellableUnit.Text = String.Format("{0:#,#.####}", FundManager.Instance.Fund.minimumSellableUnit);
            }
            if (FundManager.Instance.Fund.minimumSellableBaht == 0)
            {
                MinimumSellableBaht.Text = defaultBaht;
            }
            else
            {
                MinimumSellableBaht.Text = String.Format("{0:#,#.##}", FundManager.Instance.Fund.minimumSellableBaht);
            }

            if (FundManager.Instance.Fund.minimumUnit == 0)
            {
                MinimumUnit.Text = defaultUnit;
            }
            else
            {
                MinimumUnit.Text = String.Format("{0:#,#.####}", FundManager.Instance.Fund.minimumUnit);
            }
            if (FundManager.Instance.Fund.minimumBaht == 0)
            {
                MinimumBaht.Text = defaultBaht;
            }
            else
            {
                MinimumBaht.Text = String.Format("{0:#,#.##}", FundManager.Instance.Fund.minimumBaht);
            }
        }

        private void SetSellOption()
        {
            if (ComboBoxFundChoice1.ItemsSource == null)
            {
                List<SellOption> ListSellOption = new List<SellOption>();
                ListSellOption.Add(new SellOption("T-บัญชี", 1));
                ListSellOption.Add(new SellOption("Cheque", 2));

                ComboBoxFundChoice1.ItemsSource = ListSellOption;
                ComboBoxFundChoice1.SelectedValuePath = "optionValue";
                ComboBoxFundChoice1.SelectedIndex = 0;
            }
        }

        private void SetAccountOption()
        {
            if (ComboBoxFundChoice2.ItemsSource == null)
            {
                SelectedFundService selectedFundService = new SelectedFundService();
                ListAccount = selectedFundService.SetAccountOption();

                if (ListAccount != null)
                {
                    ComboBoxFundChoice2.ItemsSource = ListAccount;
                    ComboBoxFundChoice2.SelectedValuePath = "id";
                    ComboBoxFundChoice2.SelectedIndex = 0;

                    if (ListAccount[0].status == AccStatus.ACTIVE)
                    {
                        AccActive.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AccNotActive.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void Image_MouseDown_Close(object sender, MouseButtonEventArgs e)
        {
            FundManager.GetInstance().Fund = null;
            FundManager.GetInstance().SellFundFact = null;
            FundManager.GetInstance().SellFundFact = null;
            FundDetailDialog.Visibility = Visibility.Hidden;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            FundDetailDialog.Visibility = Visibility.Hidden;
            SelectFundDialog.Visibility = Visibility.Visible;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            FundManager.GetInstance().Fund = null;
            FundManager.GetInstance().SellFundFact = null;
            FundManager.GetInstance().SellFundFact = null;
            SelectFundDialog.Visibility = Visibility.Visible;
        }

        private void ComboBoxFundChoice2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            RedeemAccount acc = (RedeemAccount)((ComboBox)sender).SelectedItem;
            for (int i = 0; i < ListAccount.Count; i++)
            {
                if (acc.id == ListAccount[i].id)
                {
                    if (ListAccount[i].status == AccStatus.ACTIVE)
                    {
                        AccActive.Visibility = Visibility.Visible;
                        AccNotActive.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        AccActive.Visibility = Visibility.Hidden;
                        AccNotActive.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void ComboBoxFundChoice1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            SellOption sellOption = (SellOption)((ComboBox)sender).SelectedItem;
            if (sellOption.optionValue == 1)
            {
                ComboBoxFundChoice2.Visibility = Visibility.Visible;
                ComboBoxFundChoice2Text.Visibility = Visibility.Visible;
                AccImage.Visibility = Visibility.Visible;
            }
            if (sellOption.optionValue == 2)
            {
                ComboBoxFundChoice2.Visibility = Visibility.Hidden;
                ComboBoxFundChoice2Text.Visibility = Visibility.Hidden;
                AccImage.Visibility = Visibility.Hidden;
            }
        }

        private void SaleBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {

            LayoutContent2.Visibility = Visibility.Hidden;
            LayoutContent4.Visibility = Visibility.Hidden;

            //Check Same Sale List
            if (FundManager.Instance.ListFundCode != null && FundManager.Instance.ListFundCode.Count > 0)
            {
                for (int i = 0; i < FundManager.Instance.ListFundCode.Count; i++)
                {
                    if (FundManager.Instance.ListFundCode[i] == FundManager.Instance.Fund.fundCode)
                    {
                        LayoutContent5.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }

            //Check Sall Amount
            if (Convert.ToDecimal(SaleBox.Text) == 0)
            {
                LayoutContent6.Visibility = Visibility.Visible;
                return;
            }

            SellFundFact sellFundFact = new SellFundFact();
            if (RadioBut1.IsChecked == true)
            {
                if (Convert.ToDecimal(SaleBox.Text) > FundManager.Instance.Fund.totalUnit)
                {
                    LayoutContent7.Visibility = Visibility.Visible;
                    return;
                }
                sellFundFact.SaleType = "Unit";
            }
            else if (RadioBut2.IsChecked == true)
            {
                if (Convert.ToDecimal(SaleBox.Text) > FundManager.Instance.Fund.totalBaht)
                {
                    LayoutContent7.Visibility = Visibility.Visible;
                    return;
                }
                sellFundFact.SaleType = "Baht";
            }

            sellFundFact.SaleAmount = Convert.ToDecimal(SaleBox.Text);

            SellOption sellOption = (SellOption)ComboBoxFundChoice1.SelectedItem;
            RedeemAccount accountOption = (RedeemAccount)ComboBoxFundChoice2.SelectedItem;
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
                LayoutContent1.Visibility = Visibility.Visible;
            }
            else if (sellFundFact.RMF_Fact == true)
            {
                LayoutContent2.Visibility = Visibility.Visible;
            }
            else if (sellFundFact.SSF_Fact == true)
            {
                LayoutContent3.Visibility = Visibility.Visible;
            }
            else if (sellFundFact.Inactive_Fact == true)
            {
                LayoutContent4.Visibility = Visibility.Visible;
            }
            else
            {
                FundDetailDialog.Visibility = Visibility.Hidden;
                SetSellFundTable();
            }
        }

        private void SetSellFundTable()
        {
            List<string> ListFundCode = SellFundList.Where(x => !string.IsNullOrEmpty(x.Id)).Select(y => y.Id).ToList();
            var fundDetail = FundManager.Instance;
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
                        volumeText = ChangeDecimalToText(printVolume, saleType);
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
                            SellFundListGrid.ItemsSource = SellFundList;
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
                            SellFundListGrid.ItemsSource = SellFundList;
                        }
                    }
                }
            }
        }

        private void Layout1_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            if (FundManager.Instance.SellFundFact.Inactive_Fact == true)
            {
                LayoutContent1.Visibility = Visibility.Hidden;
                LayoutContent4.Visibility = Visibility.Visible;
            }
            else
            {
                FundDetailDialog.Visibility = Visibility.Hidden;
                SetSellFundTable();
            }
        }

        private void Layout2_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            if (FundManager.Instance.SellFundFact.Inactive_Fact == true)
            {
                LayoutContent2.Visibility = Visibility.Hidden;
                LayoutContent4.Visibility = Visibility.Visible;
            }
            else
            {
                FundDetailDialog.Visibility = Visibility.Hidden;
                SetSellFundTable();
            }
        }

        private void Layout3_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            if (FundManager.Instance.SellFundFact.Inactive_Fact == true)
            {
                LayoutContent3.Visibility = Visibility.Hidden;
                LayoutContent4.Visibility = Visibility.Visible;
            }
            else
            {
                FundDetailDialog.Visibility = Visibility.Hidden;
                SetSellFundTable();
            }
        }

        private void Layout4_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            LayoutContent2.Visibility = Visibility.Hidden;
            LayoutContent4.Visibility = Visibility.Hidden;

            FundDetailDialog.Visibility = Visibility.Hidden;
            SetSellFundTable();
        }

        private void Layout5_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            FundManager.GetInstance().Fund = null;
            FundManager.GetInstance().SellFundFact = null;
            FundManager.GetInstance().SellFundFact = null;
            SetSellFundTable();
            FundDetailDialog.Visibility = Visibility.Hidden;
        }

        private void Layout_Back_Button_Click(object sender, RoutedEventArgs e)
        {
            LayoutContent1.Visibility = Visibility.Hidden;
            LayoutContent2.Visibility = Visibility.Hidden;
            LayoutContent3.Visibility = Visibility.Hidden;
            LayoutContent4.Visibility = Visibility.Hidden;
            LayoutContent5.Visibility = Visibility.Hidden;
            LayoutContent6.Visibility = Visibility.Hidden;
            LayoutContent7.Visibility = Visibility.Hidden;
        }

        private void SaleBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SaleBox.Text.IndexOf('.') < 0)
            {
                SaleBox.Text = SaleBox.Text + ".0000";
            }
        }

        private void SaleBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SaleBox.Text.Length >= 6)
            {
                SaleBox.Text = (SaleBox.Text).Substring(0, (SaleBox.Text).Length - 5);
            }
        }

        private void RadioBut1_Click(object sender, RoutedEventArgs e)
        {
            SaleBoxUnit.Text = "หน่วย";
        }

        private void RadioBut2_Click(object sender, RoutedEventArgs e)
        {
            SaleBoxUnit.Text = "บาท";
        }

        private string ChangeDecimalToText(decimal vol, SaleType type)
        {
            string result = string.Empty;
            string finalText = string.Empty;
            string middleText = string.Empty;
            Func<string, string> func;

            string numtext = vol.ToString("0.####");
            if (numtext.Contains('.'))
            {
                if (type == SaleType.Unit)
                {
                    middleText = "จุด";
                    finalText = "หน่วย";
                    func = new Func<string, string>(ChangeDecimalToTextAfterMiddel);
                }
                else if (type == SaleType.Baht)
                {
                    numtext = vol.ToString("0.##");
                    middleText = "บาท";
                    finalText = "สตางค์";
                    func = new Func<string, string>(ChangeDecimalToTextBeforeMiddel);
                }
                else
                {
                    return "";
                }
                string[] numtextArray = numtext.Split('.');
                if (numtextArray.Length > 1)
                {
                    result = ChangeDecimalToTextBeforeMiddel(numtextArray[0]) + middleText + func(numtextArray[1]) + finalText;
                }
                else
                {
                    result = ChangeDecimalToTextSingle(numtext, type);
                }
            }
            else
            {
                result = ChangeDecimalToTextSingle(numtext, type);
            }

            return result;
        }

        private string ChangeDecimalToTextSingle(string num, SaleType type)
        {
            string result = string.Empty;
            string finalText = string.Empty;
            if (type == SaleType.Unit)
            {
                finalText = "หน่วย";
            }
            else if (type == SaleType.Baht)
            {
                finalText = "บาทถ้วน";
            }
            else
            {
                return "";
            }
            result = ChangeDecimalToTextBeforeMiddel(num) + finalText;
            return result;
        }

        private string ChangeDecimalToTextBeforeMiddel(string num)
        {
            string result = string.Empty;

            if (Convert.ToDecimal(num) != 0m)
            {
                string[] thaiNum = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
                string[] thaiPosition = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
                char[] singleText = num.ToCharArray();
                int totalChar = singleText.Length;
                int loopCount = 0;
                for (int i = 0; i < totalChar; i++)
                {
                    int singleNum = ChangeCharToInt(singleText[totalChar - i - 1]);
                    if (singleNum == 0 && singleNum != -1)
                    {
                        loopCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                loopCount = totalChar - loopCount;
                for (int i = 0; i < loopCount; i++)
                {
                    int position = (totalChar - i - 1);
                    if (position > 1)
                    {
                        int adjustPos = (position) % 6;
                        int singleNum = ChangeCharToInt(singleText[i]);
                        if (singleNum != -1)
                        {
                            if (adjustPos == 0)
                            {
                                if (singleNum != 0)
                                {
                                    result += (thaiNum[singleNum] + thaiPosition[6]);
                                }
                                else
                                {
                                    result += thaiPosition[6];
                                }
                            }
                            else
                            {
                                if (singleNum != 0)
                                {
                                    result += (thaiNum[singleNum] + thaiPosition[adjustPos]);
                                }
                            }
                        }
                    }
                    else if (position == 1)
                    {
                        int singleNum = ChangeCharToInt(singleText[i]);
                        if (singleNum != -1 || singleNum != 0)
                        {
                            if (singleNum != 0)
                            {
                                if (singleNum == 1)
                                {
                                    result += thaiPosition[1];
                                }
                                else if (singleNum == 2)
                                {
                                    result += ("ยี่" + thaiPosition[1]);
                                }
                                else
                                {
                                    result += (thaiNum[singleNum] + thaiPosition[1]);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        int singleNum = ChangeCharToInt(singleText[i]);
                        if (singleNum != -1)
                        {
                            if (totalChar > 1 && singleNum == 1)
                            {
                                result += "เอ็ด";
                            }
                            else
                            {
                                result += thaiNum[singleNum];
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                result = "ศูนย์";
            }

            return result;
        }

        private string ChangeDecimalToTextAfterMiddel(string num)
        {
            string result = string.Empty;
            string[] thaiNum = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            char[] singleText = num.ToCharArray();
            foreach (var text in singleText)
            {
                int singleNum = ChangeCharToInt(text);
                if (singleNum != -1)
                {
                    result += thaiNum[singleNum];
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private int ChangeCharToInt(char text)
        {
            try
            {
                return Int32.Parse(text.ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void SellFundButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fundId = (((Button)sender).Tag).ToString();
                for (int i = 0; i < SellFundList.Count; i++)
                {
                    if (SellFundList[i].Id == fundId)
                    {
                        SellFundList.RemoveAt(i);
                        SellFundListGrid.ItemsSource = SellFundList;
                        SellFundListGrid.Items.Refresh();
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WelcomeMenu.Visibility = Visibility.Hidden;
            SellFundMenu.Visibility = Visibility.Hidden;
            OtherServicesMenu.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown_OpenGraph(object sender, MouseButtonEventArgs e)
        {
            SellFundMenu.Visibility = Visibility.Visible;
        }

        private void Image_MouseDown_CloseGraph(object sender, MouseButtonEventArgs e)
        {
            CloseGraph();
        }

        private void CloseGraph()
        {
            SellFundMenu.Visibility = Visibility.Hidden;
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
                            BottomText4.Text = time.ToString("MM/dd/yyyy HH:mm:ss");
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
    }
}
