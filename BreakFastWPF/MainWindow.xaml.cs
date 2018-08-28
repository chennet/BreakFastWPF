using System;
using System.Diagnostics;
using BreakFastWPF.Common;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using BreakFastWPF.Models;
using System.Windows.Controls;

namespace BreakFastWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;
        CartList ShoppingCart;
        public MainWindow()
        {
            InitializeComponent();
            ShoppingCart = App.Current.Resources["CartListDataSource"] as CartList;
            ((INotifyPropertyChanged)ShoppingCart).PropertyChanged += new PropertyChangedEventHandler(ShoppingCartChanged);

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2500);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue("歡迎進入 BreakF@st 自助點餐系統");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue);
            new DialogsViewModel();

            Snackbar = this.MainSnackbar;
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            var appMessageDialog = new AppMessageDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };

            await DialogHost.Show(appMessageDialog, "RootDialog");
        }

        private void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string stringValue)
            {
                try
                {
                    Clipboard.SetDataObject(stringValue);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }

        /* method 1 - use dialog */
        private async void AppExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var appExitDialog = new AppExitDialog
            {
                Message = { Text = ((ButtonBase)sender).Content.ToString() }
            };

            await DialogHost.Show(appExitDialog, "RootDialog");
            this.Close();
        }

        public void ShoppingCartChanged(object sender, PropertyChangedEventArgs e)
        {
            double total = 0;
            for (int x = 0; x < ShoppingCart.Count; x++)
            {
                total += ShoppingCart[x].ItemType.Price;
            }
            selcount.Text = ShoppingCart.Count.ToString();
            total_cost.Text = total.ToString() + ".-";
            if (ShoppingCart.Count == 0)
                CheckOutButton.IsEnabled = false;
            else CheckOutButton.IsEnabled = true;

        }
        private void RemoveFromShoppingCart(object sender, RoutedEventArgs e)
        {
            ItemBase item = ((Button)sender).Tag as ItemBase;
            ShoppingCart.Remove(item);

        }
        private async void CheckOut_Button(object sender, RoutedEventArgs e)
        {
            //CheckoutDialog.IsOpen = !CheckoutDialog.IsOpen;
            var checkoutDialog = new CheckoutDialog();
            await DialogHost.Show(checkoutDialog, "RootDialog");
        }


        /* method 2 - use message box::: NOT working???
        private void AppExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Method to handle the Window.Closing event.
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var response = MessageBox.Show("Do you really want to exit?", "Exiting...",
                                           MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
         */
    }
}
