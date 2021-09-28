using MainModule.GrpcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFScbOri.Models;
using WPFScbOri.Service;

namespace MainModule.ViewModels
{
    class SearchEmployeeDialogViewModel : BindableBase, IDialogAware
    {
        private DelegateCommand<string> _closeDialogCommand;

        private string _title = "EmployeeDialog";

        public event Action<IDialogResult> RequestClose;

        private List<StringModel> comboBoxBranchItemsSource;
        private StringModel comboBoxBranchSelectedItem;
        public DelegateCommand<string> CloseDialogCommand => _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public List<StringModel> ComboBoxBranchItemsSource { get => comboBoxBranchItemsSource; set => SetProperty(ref comboBoxBranchItemsSource, value); }
        public StringModel ComboBoxBranchSelectedItem { get => comboBoxBranchSelectedItem; set => SetProperty(ref comboBoxBranchSelectedItem, value); }
        public List<StringModel> ComboBoxUserIdItemsSource { get => comboBoxUserIdItemsSource; set => SetProperty(ref comboBoxUserIdItemsSource, value); }
        public StringModel ComboBoxUserIdSelectedItem { get => comboBoxUserIdSelectedItem; set => SetProperty(ref comboBoxUserIdSelectedItem, value); }
        public Visibility UserIdBoxVisibility { get => userIdBoxVisibility; set => SetProperty(ref userIdBoxVisibility, value); }

        private List<StringModel> comboBoxUserIdItemsSource;
        private StringModel comboBoxUserIdSelectedItem;

        private Visibility userIdBoxVisibility;

        private DelegateCommand<ComboBox> comboBoxBranchCommand;
        public DelegateCommand<ComboBox> ComboBoxBranchCommand => comboBoxBranchCommand ?? (comboBoxBranchCommand = new DelegateCommand<ComboBox>(ComboBoxBranchSelectionChanged));

        public Visibility ComboBoxUserIdVisibility { get => comboBoxUserIdVisibility; set => SetProperty(ref comboBoxUserIdVisibility, value); }

        private Visibility comboBoxUserIdVisibility;

        private DelegateCommand<Window> authenLicenseCommand;
        public DelegateCommand<Window> AuthenLicenseCommand => authenLicenseCommand ?? (authenLicenseCommand = new DelegateCommand<Window>(AuthenLicense));

        private DelegateCommand<PasswordBox> passwordChangedCommand;
        public DelegateCommand<PasswordBox> PasswordChangedCommand => passwordChangedCommand ?? (passwordChangedCommand = new DelegateCommand<PasswordBox>(PasswordChanged));

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        void PasswordChanged(PasswordBox passwordBox)
        {
            password = passwordBox.Password;
        }

        private DelegateCommand comboBoxUserIdCommand;
        public DelegateCommand ComboBoxUserIdCommand => comboBoxUserIdCommand ?? (comboBoxUserIdCommand = new DelegateCommand(ComboBoxUserIdChanged));

        private DelegateCommand<TextBox> userIdBoxChangedCommand;
        public DelegateCommand<TextBox> UserIdBoxChangedCommand => userIdBoxChangedCommand ?? (userIdBoxChangedCommand = new DelegateCommand<TextBox>(userIdBoxChanged));

        private string textBoxUserIdSelectedItem;
        public string TextBoxUserIdSelectedItem { 
            get => textBoxUserIdSelectedItem; 
            set => SetProperty(ref textBoxUserIdSelectedItem, value); }

        private void userIdBoxChanged(TextBox textBox)
        {
            if (textBox != null)
            {
                TextBoxUserIdSelectedItem = textBox.Text;

                //var aaa = ComboBoxUserIdSelectedItem;
                //var bbb = TextBoxUserIdSelectedItem;
            }
        }

        private void ComboBoxUserIdChanged()
        {
            //if (ComboBoxUserIdSelectedItem != null)
            //{
            //    var aaa = ComboBoxUserIdSelectedItem;
            //    var bbb = TextBoxUserIdSelectedItem;
            //}
        }

        protected virtual void AuthenLicense(Window parameter)
        {
            ButtonResult result = ButtonResult.None;

            string _empId = (ComboBoxUserIdSelectedItem != null && ComboBoxUserIdSelectedItem.Text != null) ?
                    ComboBoxUserIdSelectedItem.Text :
                    !string.IsNullOrEmpty(TextBoxUserIdSelectedItem) ?
                    TextBoxUserIdSelectedItem :
                    "";

            var dialogResult = new DialogResult(ButtonResult.OK);
            dialogResult.Parameters.Add("_empId", _empId);
            RaiseRequestClose(dialogResult);

        }
        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.Cancel;
                RaiseRequestClose(new DialogResult(result));
            }
            //if (parameter?.ToLower() == "submit")
            //{
                

            //}

        }
        public void ComboBoxBranchSelectionChanged(ComboBox comboBox)
        {
            StringModel branchText = (StringModel)comboBox.SelectedItem;
            if (branchText != null)
            {
                if (branchText.Text == "พนักงานในสาขา")
                {
                    ComboBoxUserIdVisibility = Visibility.Visible;
                    UserIdBoxVisibility= Visibility.Hidden;
                    SeteBranch();
                    TextBoxUserIdSelectedItem = "";
                }
                else
                {
                    ComboBoxUserIdVisibility = Visibility.Hidden;
                    UserIdBoxVisibility = Visibility.Visible;
                    TextBoxUserIdSelectedItem = "";
                    ComboBoxUserIdSelectedItem = null;
                }
            }
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

        public void OnDialogOpened(IDialogParameters parameters)
        {
            SeteBranch();
        }

        public void SeteBranch()
        {
            SearchEmployeeSerivce searchEmployeeSerivce = new SearchEmployeeSerivce();
            var BranchTextList = searchEmployeeSerivce.GetBranchText();
            if (BranchTextList.Count > 0)
            {
                ComboBoxBranchItemsSource = BranchTextList;
                ComboBoxBranchSelectedItem = BranchTextList[0];
            }

            var EmpIdTextList = searchEmployeeSerivce.GetEmpIdText();
            if (EmpIdTextList.Count > 0)
            {
                UserIdBoxVisibility = Visibility.Hidden;
                ComboBoxUserIdItemsSource = EmpIdTextList;
                ComboBoxUserIdSelectedItem = EmpIdTextList[0];
            }
        }
    }
}
