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
using System.Windows;
using System.Windows.Controls;
using Teller.Core.Models;
using WPFScbOri.Service;

namespace MainModule.ViewModels
{
    class SelectFundDialogViewModel : BindableBase,IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private DelegateCommand<ComboBox> comboBoxFundSelectionChanged;
        public DelegateCommand<ComboBox> ComboBoxFundSelectionChanged => comboBoxFundSelectionChanged ?? (comboBoxFundSelectionChanged = new DelegateCommand<ComboBox>(SelectionFund));
        
        private DelegateCommand<string> sellButtonCommand;
        public DelegateCommand<string> SellButtonCommand => sellButtonCommand ?? (sellButtonCommand = new DelegateCommand<string>(SellFundButton));

        private ObservableCollection<string> listFundGroups;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public ObservableCollection<string> ListFundGroups { get => listFundGroups; set => SetProperty(ref listFundGroups,value); }
        public ObservableCollection<Fund> ListFund { get => listFund; set => SetProperty(ref listFund, value); }

        private string _title = "FundDialog";

        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<Fund> listFund;

        private Visibility sellFundMenuVisibility;

        private string selectedGroup;
        public ObservableCollection<string> SearchTypes
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

        public string SelectedGroup { get => selectedGroup; set => SetProperty(ref selectedGroup, value); }
        public Visibility SellFundMenuVisibility { get => sellFundMenuVisibility; set => SetProperty(ref sellFundMenuVisibility, value); }

        private DelegateCommand openGraphCommand;
        public DelegateCommand OpenGraphCommand => openGraphCommand ?? (openGraphCommand = new DelegateCommand(OpenGraph));

        private DelegateCommand closeGraphCommand;
        public DelegateCommand CloseGraphCommand => closeGraphCommand ?? (closeGraphCommand = new DelegateCommand(CloseGraph));

        private void CloseGraph()
        {
            SellFundMenuVisibility = Visibility.Hidden;
        }

        void OpenGraph()
        {
            SellFundMenuVisibility = Visibility.Visible;
        }

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "close")
            {
                FundManager.GetInstance().Fund = null;
                FundManager.GetInstance().SellFundFact = null;
                FundManager.GetInstance().SellFundFact = null;

                result = ButtonResult.Cancel;
                RaiseRequestClose(new DialogResult(result));
            }
        }

        private async void GetFundGroupList()
        {
            //SelectedFundService selectedFundService = new SelectedFundService();
            FundService fundService = new();
            var tempListFundGroups = await fundService.ListFundGroups();

            if (tempListFundGroups.Count > 0)
            {
                ListFundGroups = new(tempListFundGroups);
            }
            else
            {
                ListFundGroups = new();
            }
        }

        private async void GetFundList()
        {
            //SelectedFundService selectedFundService = new SelectedFundService();
            FundService fundService = new();
            List<Fund> tempListFund = new() ;
            if (SelectedGroup == "ทั้งหมด")
            {
                tempListFund = await fundService.Funds();
            }
            else
            {
                if (FundManager.Instance != null && FundManager.Instance.CustAccount != null)
                {
                    tempListFund = await fundService.ListFundsByAccountNumber(FundManager.Instance.CustAccount, SelectedGroup, "");
                    //tempListFund = selectedFundService.GetFundList(FundManager.Instance.CustAccount, SelectedGroup, "");
                }
            }

            if (tempListFund != null && tempListFund.Count > 0)
            {
                for (int i = 0; i < tempListFund.Count; i++)
                {
                    tempListFund[i].totalBaht = Math.Round(tempListFund[i].totalBaht, 2, MidpointRounding.AwayFromZero);
                    if (tempListFund[i].gainAmount >= 0)
                    {
                        tempListFund[i].profitStatus = true;
                        tempListFund[i].totalGain = "+" + String.Format("{0:#,#.##}", tempListFund[i].gainAmount) + " (+" + String.Format("{0:#,#.##}", tempListFund[i].gainPercentage) + "%)";
                    }
                    else
                    {
                        tempListFund[i].profitStatus = false;
                        tempListFund[i].totalGain = String.Format("{0:#,#.##}", tempListFund[i].gainAmount) + " (" + String.Format("{0:#,#.##}", tempListFund[i].gainPercentage) + "%)";
                    }
                }
                ListFund = new(tempListFund);
            }
            else
            {
                ListFund = new();
            }
        }

        protected virtual void SelectionFund(ComboBox comboBox)
        {
            SelectedGroup = (string)comboBox.SelectedValue;
            GetFundList();
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
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            SelectedGroup = "ทั้งหมด";
            SellFundMenuVisibility = Visibility.Hidden;
            GetFundGroupList();
            GetFundList();
        }

        public void SellFundButton(string fundCode)
        {
            var getFundCode = "";
            if (fundCode?.ToLower() is not null)
                getFundCode = fundCode;

            for (int i = 0; i < ListFund.Count; i++)
            {
                if (getFundCode == ListFund[i].fundCode)
                {
                    FundManager.GetInstance().Fund = ListFund[i];
                    //SelectFundDialog.Visibility = Visibility.Hidden;
                    //FundDetailDialog.Visibility = Visibility.Visible;
                    //SaleBox.Text = "0";
                    //SetFundDetail();
                    //SetSellOption();
                    //SetAccountOption();
                }
            }


            var dialogReult = new DialogResult(ButtonResult.OK);
            RaiseRequestClose(dialogReult);

        }
    }
}
