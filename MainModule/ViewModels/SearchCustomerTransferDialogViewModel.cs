using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Entity.Models;
using MainModule.GrpcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Teller.Core.Models;
using WPFScbOri.Models;
using static Entity.Models.Enums;

namespace MainModule.ViewModels
{
    public class SearchCustomerTransferDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private DelegateCommand<ComboBox> selectionSearchTypeCommand;
        public DelegateCommand<ComboBox> SelectionSearchTypeCommand => selectionSearchTypeCommand ?? (selectionSearchTypeCommand = new DelegateCommand<ComboBox>(SelectionSearchType));
        //CustomerDetailListsCommond
        private DelegateCommand<string> customerDetailListsCommond;
        public DelegateCommand<string> CustomerDetailListsCommond => customerDetailListsCommond ?? (customerDetailListsCommond = new DelegateCommand<string>(GetCustId));

        private DelegateCommand<TextBox> searchCustomerTextBox;
        public DelegateCommand<TextBox> SearchCustomerTextBox => searchCustomerTextBox ?? (searchCustomerTextBox = new DelegateCommand<TextBox>(SearchCustomerTextBoxRef));

        private string selectedSearchType;
        public string SelectedSearchType { get => selectedSearchType; set => SetProperty(ref selectedSearchType, value); }

        private string searchCustomerTextBoxString;
        public string SearchCustomerTextBoxString { get => searchCustomerTextBoxString; set => SetProperty(ref searchCustomerTextBoxString, value); }

        private string _title = "CustomerDialog";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<CustomerDetail> customerDetailLists;
        public ObservableCollection<CustomerDetail> CustomerDetailLists { get => customerDetailLists; set => SetProperty(ref customerDetailLists, value); }

        public CurrentMainRegion CurrentRegion = CurrentMainRegion.None;

        public class OnDialogOpenedEntity
        {
            public CurrentMainRegion CurrentRegion { get; set; }
            public string DefaultSearch { get; set; }
        }

        public ObservableCollection<string> SearchTypes
        {
            get
            {
                return new ObservableCollection<string>
                {
                    "บัตรประชาชน",
                    "พาสปอร์ต",
                    "ชื่อ - นามสกุล",
                };
            }
        }


        public async virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;
            string _close = "close";
            string _search = "search";

            #region Close
            if (parameter?.ToLower() == _close)
            {

                result = ButtonResult.Cancel;
                RaiseRequestClose(new DialogResult(result));
            }
            #endregion

            #region Search
            if (parameter?.ToLower() == _search)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    //dialogService.ShowDialog("AlertDialog", new DialogParameters("message=Username หรือ Password ไม่ถูกต้อง"),
                    //(r) => { System.Diagnostics.Debug.WriteLine("LoginWindowModel: dialog result = " + r.Result); });
                    //MessageBox.Show("โปรดเลือกประเภทการค้นหา", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    var dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("message", "โปรดเลือกประเภทการค้นหา");
                    RaiseRequestClose(dialogResult);
                    return;
                }

                if (SelectedSearchType is null)
                {
                    //MessageBox.Show("โปรดกรอกข้อมูลในการค้นหา", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    var dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("message", "โปรดเลือกประเภทการค้นหา");
                    RaiseRequestClose(dialogResult);
                    return;
                }

                //SeachCustomer(SelectedSearchType, SearchCustomerTextBoxString);
                CustomerService searchCustomerService = new();
                CustomerDetail Cust = await Task.Run(() => searchCustomerService.SeachCustomerTransfer(SelectedSearchType, SearchCustomerTextBoxString));
                List<CustomerDetail> listCust = new();
                listCust.Add(Cust);

                if (listCust == null || listCust.Count == 0)
                {
                    //MessageBox.Show("ไม่พบข้อมูลในระบบ", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    var dialogResult = new DialogResult(ButtonResult.Ignore);
                    dialogResult.Parameters.Add("message", "ไม่พบข้อมูลในระบบ");
                    dialogResult.Parameters.Add("defaultSearch", SelectedSearchType);
                    RaiseRequestClose(dialogResult);
                    return;
                }
                else
                {
                    foreach (var item in listCust)
                    {
                        if (item.CitizenIdCardImagePath == null)
                            item.CitizenIdCardImagePath = GetImageSource("citizenId_image");
                        else
                            item.CitizenIdCardImagePath = GetImageSource("no_image");

                        if (item.SignedSignatureImagePath == null)
                            item.SignedSignatureImagePath = GetImageSource("sign_image");
                        else
                            item.CitizenIdCardImagePath = GetImageSource("no_image");

                    }

                    CustomerDetailLists = new(listCust);
                }
            }
            #endregion

            //SelectedSearchType = null;

        }

        public virtual void GetCustId(string parameter)
        {
            if (!string.IsNullOrEmpty(parameter?.ToLower()))
            {
                string custId = parameter;
                if (int.TryParse(parameter?.ToLower(), out _))
                {
                    var dialogResult = new DialogResult(ButtonResult.OK);
                    foreach (var i in CustomerDetailLists)
                    {
                        if (custId == i.CustId)
                        {

                            dialogResult.Parameters.Add("AccName", (i.AccName).ToString());
                            dialogResult.Parameters.Add("CustId", (i.CustId).ToString());
                            dialogResult.Parameters.Add("Branch", (i.Branch).ToString());
                            dialogResult.Parameters.Add("JointAccount", (i.IsJointAccount).ToString());
                            dialogResult.Parameters.Add("SensitiveCustomer", (i.IsSensitive).ToString());
                            dialogResult.Parameters.Add("OrderPayment", (i.Payment).ToString());
                            dialogResult.Parameters.Add("Age", (i.Age).ToString());
                            dialogResult.Parameters.Add("RiskLevel", "5");

                            FundManager.GetInstance().CustAge = (i.Age).ToString();
                            FundManager.GetInstance().CustAccount = (i.CustId).ToString();

                            RaiseRequestClose(dialogResult);
                            return;
                        }
                    }

                }
            }
        }

        public virtual void SelectionSearchType(ComboBox parameter)
        {
            if (parameter != null && parameter.SelectedValue != null)
                SelectedSearchType = parameter.SelectedValue.ToString();
        }

        public virtual void SearchCustomerTextBoxRef(TextBox parameter)
        {
            SearchCustomerTextBoxString = parameter.Text;
        }
        private ImageSource GetImageSource(string filename)
        {
            try
            {
                return new BitmapImage(new Uri($"pack://application:,,,/Entity;Component/Images/TransferImages/{filename}.png"));
            }
            catch(Exception e)
            {
                return null;
            }
        }


        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var _defaultSearch = parameters.GetValue<string>("defaultSearch");
            if (!string.IsNullOrEmpty(_defaultSearch))
            {
                foreach (var item in SearchTypes)
                {
                    if (item == _defaultSearch)
                    {
                        SelectedSearchType = item;
                    }
                }
                if (string.IsNullOrEmpty(SelectedSearchType))
                {
                    SelectedSearchType = SearchTypes[0];
                }

            }
            else
            {
                SelectedSearchType = SearchTypes[0];
            }

            SearchCustomerTextBoxString = "000015663527";
        }

    }
}
