using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BreakFastWPF.Models;
using BreakFastWPF.Common;

namespace BreakFastWPF
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class CheckoutDialog : UserControl
    {
        private static BackgroundWorker backgroundWorker;
        int MAX_CHECK_TIME = 200;
        PSCommModel pscomm;
        CartList shoppingCart;
        ////public CartList shoppingCart;
        //public CartList ShoppingCart { set { shoppingCart = value; Resources["ShoppingCart"] = value; } }
        /*
        CancellationTokenSource source = new CancellationTokenSource();
        SynchronizationContext context = SynchronizationContext.Current;
        Task task;
        */
        private static PSTradModel pstrad = new PSTradModel();
        bool isPsTradable = false;
        public CheckoutDialog()
        {
            pscomm = App.Current.Resources["PSCommDataSource"] as PSCommModel;
            shoppingCart = App.Current.Resources["CartListDataSource"] as CartList;
            //ShoppingCart = _shoppingCart;
            InitializeComponent();
            showListBox();
            if (pscomm.isCommInit)
            {
                if (isPsTradable = CheckPSTradable())
                {
                    PSStatusTextBlock.Text = "PS3 Ready";
                    pstrad.reqTransactionAmount();
                    backgroundWorker = new BackgroundWorker
                    {
                        WorkerReportsProgress = true,
                        WorkerSupportsCancellation = true
                    };

                    backgroundWorker.DoWork += backgroundWorker_DoWork;
                    //For the display of operation progress to UI.    
                    backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
                    //After the completation of operation.    
                    backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                    backgroundWorker.RunWorkerAsync("Press Enter in the next 5 seconds to Cancel operation:");
                }
                else
                {
                    PSStatusTextBlock.Text = "PS3 status Failed!";
                }
            }
            else
            {
                PSStatusTextBlock.Text = "PS3 init Failed!";
                checkoutimage.Source = new BitmapImage(new Uri(@"/images/stop.png", UriKind.Relative));
            }
        }

        private bool CheckPSTradable()
        {
            bool rtn = false;
            int[] pGetData = pstrad.reqTransactionStatus();
            int check_cnt = MAX_CHECK_TIME;
            while (!(pGetData[0] == 01) && check_cnt > 0) { check_cnt--;  pGetData = pstrad.reqTransactionStatus(); Thread.Sleep(500); }
            if(pGetData[0] == 01)
            {
                pstrad.setChargeDeviceEnable();
                Thread.Sleep(100);
                check_cnt = MAX_CHECK_TIME;
                pGetData = pstrad.reqTransactionStatus();
                while (!(pGetData[0] == 01) && check_cnt > 0) { check_cnt--; pGetData = pstrad.reqTransactionStatus(); Thread.Sleep(500); }
                if (pGetData[0] == 02) rtn = true;
            }
            return rtn;
        }

        public void showListBox()
        {
            int amount = 0;
            for (int x = 0; x < shoppingCart.Count; x++)
            {
                amount += (int)shoppingCart[x].ItemType.Price;
                //ShoppingCartListBox.Items.Add(shoppingCart[x].ItemType.Description);
            }
            string cmmstate = (pscomm.isCommInit) ? "Connected" : "Disconnected";
            ordercount.Text = "您共訂購 " + shoppingCart.Count.ToString() + " 項";
            ordertotal.Text = amount.ToString();

        }
        public void showTotalPrice()
        {
            //totalprice.Text = shoppingCart.Count.ToString();
        }

        static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            /* BackgroundWorker Example
            for (int i = 0; i < 200; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                backgroundWorker.ReportProgress(i);
                Thread.Sleep(1000);
                e.Result = 1000;
            }
            */
            int[] amt = pstrad.reqTransactionAmount();

            while(true)
            { Thread.Sleep(100); }
        }

        static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Console.WriteLine("Completed" + e.ProgressPercentage + "%");
        }

        static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                Console.WriteLine("Operation Cancelled");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("Error in Process :" + e.Error);
            }
            else
            {
                Console.WriteLine("Operation Completed :" + e.Result);
            }
        }

        private void TradCancelButton(object sender, RoutedEventArgs e)
        {
            if(isPsTradable)
                backgroundWorker.CancelAsync();
            shoppingCart.Clear();
        }
    }
}
