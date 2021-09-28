using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFScbOri.Models;
using WPFScbOri.Views;
using Timer = System.Timers.Timer;

namespace WPFScbOri
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Account account;

        private string password;
        public Account Account
        {
            get => account;
            set => account = value;
        }
        public LoginWindow()
        {
            InitializeComponent();
            bindAccount();
            TimerCount();
        }

        private void bindAccount()
        {
            var client = new RestClient(new Uri("http://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:8888/api/accounts"));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            try
            {
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<Account> ListAccount = JsonConvert.DeserializeObject<List<Account>>(response.Content);
                    for (int i = 0; i < ListAccount.Count; i++)
                    {
                        ListAccount[i].fullname = "ชื่อ-" + ListAccount[i].name + " นามสกุล-" + ListAccount[i].surname;
                        ListAccount[i].fullinfo = "ID : " + ListAccount[i].id + "   /   BRN : " + ListAccount[i].branchCode;
                    }
                    ComboBoxAccount.ItemsSource = ListAccount;
                    ComboBoxAccount.SelectedValuePath = "id";
                }
            }
            catch (WebException ex)
            {
                // Handle error
            }
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

        private void ComboBoxAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BottomTextLine.Visibility = Visibility;
            BottomTextLine2.Visibility = Visibility;
            account = (Account)ComboBoxAccount.SelectedItem;
            LoginBoxTextTitle4.Text = (account.id).ToString();
            LoginBoxTextTitle5.Text = account.fullname;
            BottomText.Text = (account.id).ToString();
            BottomText2.Text = account.branchCode;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (account == null || string.IsNullOrEmpty(account.name) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Login Failed, username or password is not empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                AccountManager.GetInstance().Account = account;
                MainWindow newWindow = new MainWindow();
                newWindow.Show();
                this.Close();
            }
        }

        private void BoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = BoxPassword.Password;
        }
    }
}
