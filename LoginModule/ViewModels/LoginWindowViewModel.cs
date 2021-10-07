using Entity.Models;
using LoginModule.GrpcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFScbOri.Models;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using Timer = System.Timers.Timer;
using System.Collections.ObjectModel;
using RestSharp;
using Newtonsoft.Json;

namespace LoginModule.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        #region Variable
        IMainShellInitializer shellInitializer;

        IDialogService dialogService;

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        public ICommand SelectionUsernameCommand { get; set; }

        private Account account;
        public Account Account
        {
            get => account;
            set
            {
                SetProperty(ref account, value);
            }
        }

        private Account accountSelected;
        public Account AccountSelected {
            get => accountSelected;
            set=> SetProperty(ref accountSelected,value);
        }

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

        private ObservableCollection<Account> listAccount;
        public ObservableCollection<Account> ListAccount
        {
            get => listAccount;
            set => SetProperty(ref listAccount, value);
        }

        private string realtimeTimer;
        public string RealtimeTimer
        {
            get => realtimeTimer;
            set => SetProperty(ref realtimeTimer, value);
        }

        private Visibility bottomTextLine;
        public Visibility BottomTextLine
        {
            get => bottomTextLine;
            set => SetProperty(ref bottomTextLine, value);
        }

        private Visibility bottomTextLine2;
        public Visibility BottomTextLine2
        {
            get => bottomTextLine2;
            set => SetProperty(ref bottomTextLine2, value);
        }
        #endregion

        public LoginWindowViewModel(IMainShellInitializer mainShellInitializer, IDialogService dialogService)
        {
            this.shellInitializer = mainShellInitializer;
            this.dialogService = dialogService;

            PasswordChangedCommand = new DelegateCommand<PasswordBox>(PasswordChanged);
            LoginCommand = new DelegateCommand<Window>(Login);
            SelectionUsernameCommand = new DelegateCommand<ComboBox>(SelectionUsername);
            BottomTextLine = Visibility.Hidden;
            BottomTextLine2 = Visibility.Hidden;

            bindAccount();
            TimerCount();
        }


        void SelectionUsername(ComboBox comboBox)
        {
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                BottomTextLine = Visibility.Visible;
                BottomTextLine2 = Visibility.Visible;
                AccountSelected = (Account)comboBox.SelectedValue;
            }
        }

        void Login(Window loginWindow)
        {
            
            if (AccountSelected == null || /*string.IsNullOrEmpty(AccountSelected.name) ||*/ string.IsNullOrEmpty(password))//Cop
            {
                //Login Failed
                dialogService.ShowDialog(
                "LoginDialog",
                new DialogParameters(),
                    (result) => {
                        if (result.Result == ButtonResult.OK)
                        {
                            //CustomerIdTextBox = result.Parameters.GetValue<string>("CustId");
                            //FundAccountIdTextBox = result.Parameters.GetValue<string>("Branch");
                            //CustomerName = result.Parameters.GetValue<string>("AccName");
                            //JointAccount = result.Parameters.GetValue<string>("JointAccount");
                            //SensitiveCustomer = result.Parameters.GetValue<string>("SensitiveCustomer");
                            //RiskLevel = result.Parameters.GetValue<string>("RiskLevel");
                            //System.Diagnostics.Debug.WriteLine("SelectedSearchType: dialog result = " + SelectedSearchType);
                        }
                    });

                //MessageBox.Show("Login Failed, username or password is not empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                //Login Pass
                AccountManager.GetInstance().Account = AccountSelected;
                //MainWindow newWindow = new MainWindow();
                //newWindow.Show();
                new MainBootstrapper(shellInitializer).Run();
                loginWindow.Close();
            }
        }

        private async void bindAccount()
        {
            var client = new RestClient(new Uri("http://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:8888/api/accounts"));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            try
            {
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<Account> tempListAccount = JsonConvert.DeserializeObject<List<Account>>(response.Content);
                    ObservableCollection<Account> ListAccount = new(tempListAccount);
                    for (int i = 0; i < ListAccount.Count; i++)
                    {
                        ListAccount[i].fullname = "ชื่อ-" + ListAccount[i].name + " นามสกุล-" + ListAccount[i].surname;
                        ListAccount[i].fullinfo = "ID : " + ListAccount[i].id + "   /   BRN : " + ListAccount[i].branchCode;
                    }
                    //ComboBoxAccount.ItemsSource = ListAccount;
                    //ComboBoxAccount.SelectedValuePath = "id";
                    this.ListAccount = ListAccount;
                }
            }
            catch (WebException ex)
            {
                // Handle error
            }
            #region GRPC
            //APIServices apiServices = new();

            //try
            //{
            //    List<Account> response = await apiServices.GetAccountList();

            //    ObservableCollection<Account> tempListAccount = new ObservableCollection<Account>(response);
            //    if (tempListAccount != null && tempListAccount.Count > 0)
            //    {
            //        for (int i = 0; i < tempListAccount.Count; i++)
            //        {
            //            tempListAccount[i].fullname = "ชื่อ-" + tempListAccount[i].name + " นามสกุล-" + tempListAccount[i].surname;
            //            tempListAccount[i].fullinfo = "ID : " + tempListAccount[i].id + "   /   BRN : " + tempListAccount[i].branchCode;
            //        }
            //        ListAccount = tempListAccount;
            //    }
            //}
            //catch (WebException ex)
            //{
            //    // Handle error
            //}
            #endregion

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
                            //BottomText4.Text = time.ToString("MM/dd/yyyy HH:mm:ss");
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
    }
}
