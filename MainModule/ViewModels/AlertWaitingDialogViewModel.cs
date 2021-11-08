using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace MainModule.ViewModels
{
    public class AlertWaitingDialogViewModel : BindableBase, IDialogAware
    {
        #region template property
        public string Title => "AlertWaitingDialog";

        public event Action<IDialogResult> RequestClose;

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        #endregion

        #region bindable property

        DateTime startTime;
        DateTime endTime;
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

            startTime = Convert.ToDateTime(Message);
            endTime = startTime.AddSeconds(3);
            TimerCount();
        }
        #endregion

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
                            //RealtimeTimer = time.ToString("MM/dd/yyyy HH:mm:ss");
                            if (time >= endTime)
                            {
                                RaiseRequestClose(new DialogResult(ButtonResult.OK));
                            }
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
