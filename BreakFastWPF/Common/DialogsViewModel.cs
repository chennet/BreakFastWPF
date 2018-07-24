using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BreakFastWPF.Common;
using BreakFastWPF.Models;
using MaterialDesignThemes.Wpf;

namespace BreakFastWPF.Common
{
    public class DialogsViewModel : INotifyPropertyChanged
    {
        private CartList shoppingCart;
        public CartList ShoppingCart { set { shoppingCart = value; } }
        public DialogsViewModel()
        {
            //Sample 4
            //OpenSample4DialogCommand = new AnotherCommandImplementation(OpenSample4Dialog);
            AcceptSample4DialogCommand = new AnotherCommandImplementation(AcceptSample4Dialog);
            CancelSample4DialogCommand = new AnotherCommandImplementation(CancelSample4Dialog);
            OpenLoginDialogCommand = new AnotherCommandImplementation(OpenLoginDialog);
            AcceptLoginDialogCommand = new AnotherCommandImplementation(AcceptLoginDialog);
        }

        #region SAMPLE 3

        public ICommand RunDialogCommand => new AnotherCommandImplementation(ExecuteRunDialog);

        public ICommand RunCheckOutCommand => new AnotherCommandImplementation(ExecuteRunCheckOutDialog);

        private async void ExecuteRunDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new LoginDialog
            {
                DataContext = new AppDialogViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private async void ExecuteRunCheckOutDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            //this.ShoppingCart = (CartList)o;
            var view = new CheckoutDialog()
            {
                DataContext = new AppDialogViewModel()
            };
            //view.ShoppingCart = shoppingCart;
            //view.showTotalPrice();
            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new AppProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region SAMPLE 4

        //pretty much ignore all the stuff provided, and manage everything via custom commands and a binding for .IsOpen
        public ICommand OpenSample4DialogCommand { get; }
        public ICommand AcceptSample4DialogCommand { get; }
        public ICommand CancelSample4DialogCommand { get; }
        public ICommand OpenLoginDialogCommand { get; }
        public ICommand AcceptLoginDialogCommand { get; }

        private bool _isSample4DialogOpen;
        private object _sample4Content;

        private bool _isLoginDialogOpen;
        private object _loginContent;

        public bool IsSample4DialogOpen
        {
            get { return _isSample4DialogOpen; }
            set
            {
                if (_isSample4DialogOpen == value) return;
                _isSample4DialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoginDialogOpen
        {
            get { return _isLoginDialogOpen; }
            set
            {
                if (_isLoginDialogOpen == value) return;
                _isLoginDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public object Sample4Content
        {
            get { return _sample4Content; }
            set
            {
                if (_sample4Content == value) return;
                _sample4Content = value;
                OnPropertyChanged();
            }
        }

        public object LoginContent
        {
            get { return _loginContent; }
            set
            {
                if (_loginContent == value) return;
                _loginContent = value;
                OnPropertyChanged();
            }
        }

        /*
                private void OpenSample4Dialog(object obj)
                {
                    Sample4Content = new Sample4Dialog();
                    IsSample4DialogOpen = true;
                }
        */
        private void CancelSample4Dialog(object obj)
        {
            IsSample4DialogOpen = false;
        }

        private void AcceptSample4Dialog(object obj)
        {
            //pretend to do something for 3 seconds, then close
            Sample4Content = new AppProgressDialog();
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => IsSample4DialogOpen = false, null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OpenLoginDialog(object obj)
        {
            LoginContent = new LoginDialog();
            IsLoginDialogOpen = true;
        }

        private void AcceptLoginDialog(object obj)
        {
            //pretend to do something for 3 seconds, then close
            LoginContent = new AppProgressDialog();
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => IsLoginDialogOpen = false, null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
