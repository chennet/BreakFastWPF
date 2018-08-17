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
        int MAX_WAIT_PAID = 5000;
        int req_to_pay = 0;
        int user_paid = 0;
        PSCommModel pscomm;
        CartList shoppingCart;
        int[] amt_paid = { 0 };
        int wait_cnt = 0;
        string ps_msg = "";
        bool cancel_trad = false;
        bool complete_trad = false;
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
                    PSStatusTextBlock.Text = "請投幣... ";
                    //pstrad.reqTransactionAmount();    //詢問PS3收到的鈔票與硬幣
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
            //當PS3開機自動確認完周邊裝置之後會自動進入狀態01H
            while (!(pGetData[0] == 01) && check_cnt > 0) { check_cnt--;  pGetData = pstrad.reqTransactionStatus(); Thread.Sleep(500); }
            if(pGetData[0] == 01)
            {
                //啟動周邊所有裝置，此時狀態會從01H進入02H
                pstrad.setChargeDeviceEnable();
                Thread.Sleep(100);
                check_cnt = MAX_CHECK_TIME;
                pGetData = pstrad.reqTransactionStatus();
                while (!(pGetData[0] == 02) && check_cnt > 0) { check_cnt--; pGetData = pstrad.reqTransactionStatus(); Thread.Sleep(500); }
                if (pGetData[0] == 02) rtn = true;
                PSStatusTextBlock.Text = "周邊裝置啟動失敗";
            }
            return rtn;
        }

        public void showListBox()
        {
            for (int x = 0; x < shoppingCart.Count; x++)
            {
                req_to_pay += (int)shoppingCart[x].ItemType.Price;
                //ShoppingCartListBox.Items.Add(shoppingCart[x].ItemType.Description);
            }
            string cmmstate = (pscomm.isCommInit) ? "Connected" : "Disconnected";
            ordercount.Text = "您共訂購 " + shoppingCart.Count.ToString() + " 項";
            ordertotal.Text = req_to_pay.ToString();

        }
        public void showTotalPrice()
        {
            //totalprice.Text = shoppingCart.Count.ToString();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
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
            int rtn = 0;
           //詢問PS3收到的鈔票與硬幣
           do
            {
                amt_paid = pstrad.reqTransactionAmount();   
                if (amt_paid != null && amt_paid.Length > 2)
                {
                    user_paid = amt_paid[0] + amt_paid[1] + amt_paid[2] + amt_paid[3];
                }
                Thread.Sleep(100);
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                backgroundWorker.ReportProgress(wait_cnt++);
            }
            while (user_paid < req_to_pay && wait_cnt <= MAX_WAIT_PAID);
            //customer paid enough cash. stop to eat coins.
            rtn = pstrad.setChargeDeviceDisable();
            Thread.Sleep(100);
            if (user_paid >= req_to_pay)
            {
                //int check_cnt = MAX_CHECK_TIME;
                int[] pGetData = pstrad.reqTransactionStatus();
                ps_msg = "投幣完成...";
                backgroundWorker.ReportProgress(0);
                wait_cnt = MAX_CHECK_TIME;
                do
                {
                    if (pGetData == null) continue;
                    if (pGetData[0] == 03) break;
                    wait_cnt--;
                    pGetData = pstrad.reqTransactionStatus();
                    Thread.Sleep(100);
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                } while (wait_cnt > 0);
                if (pGetData != null && pGetData[0] == 03)
                {
                    if (pGetData[0] == 02)
                    {
                        ps_msg = "找零計算... " + (user_paid - req_to_pay).ToString();
                        backgroundWorker.ReportProgress(0);
                    }
                    if ((user_paid - req_to_pay) > 0)
                    {
                        //使用AutoCal_Payout_Amount先行試算以目前的庫存量夠不夠被找出，此時回傳ack或Nack代表夠或不夠
                        if (1 != pstrad.AutoCalPayoutAmount(2, (user_paid - req_to_pay), 0))
                        {
                            wait_cnt = MAX_CHECK_TIME;
                            int[] pData = pstrad.reqThePayoutAmountCalResult();
                            do
                            {
                                if (pData == null) continue;
                                if (pData[0] != 1) break;
                                Thread.Sleep(100);
                                pData = pstrad.reqThePayoutAmountCalResult();
                                if (backgroundWorker.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            } while (true);

                            if (pData != null)
                            {
                                if (2 == pData[0])
                                {
                                    //足夠找零金額，開始找零
                                    ps_msg = "找零中... ";
                                    backgroundWorker.ReportProgress(0);
                                    rtn = pstrad.AutoCalPayoutAmount(1, (user_paid - req_to_pay), 0);
                                    complete_trad = true;
                                    DoShipment();
                                    do
                                    {
                                        pData = pstrad.reqTransactionStatus();
                                        if (pData == null) continue;
                                        if (pData[0] == 5) break;
                                        if (backgroundWorker.CancellationPending)
                                        {
                                            e.Cancel = true;
                                            return;
                                        }
                                    } while (true);

                                    if (1 == pstrad.setTransactionFinish())
                                    {
                                        ps_msg = "交易完成.";
                                        backgroundWorker.ReportProgress(0);
                                    }
                                }
                            }
                            else
                            {
                                ps_msg = "錯誤或不足找零";
                                backgroundWorker.ReportProgress(0);
                            }
                        }
                        else
                        {
                            ps_msg = "找零計算失敗或不足";
                            backgroundWorker.ReportProgress(0);
                        }
                    }
                    else
                    {
                        // 無須找零
                        complete_trad = true;
                        DoShipment();
                        if (1 == pstrad.setTransactionFinish())
                        {
                            ps_msg = "交易完成.";
                            backgroundWorker.ReportProgress(0);
                        }
                    }
                }
                else
                {
                    ps_msg = "投幣機錯誤!";
                    backgroundWorker.ReportProgress(0);
                    if (user_paid > 0)
                        pstrad.AutoCalPayoutAmount(1, (user_paid), 0);
                }
            }
            else
            {
                //MAX_WAIT_TIME for paid timeout
                ps_msg = "交易取消";
                cancel_trad = true;
                backgroundWorker.ReportProgress(0);
                if(user_paid > 0)
                    pstrad.AutoCalPayoutAmount(1, (user_paid), 0);
            }

        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (cancel_trad) CancelTradButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Console.WriteLine("Completed" + e.ProgressPercentage + "%");
            if (user_paid < req_to_pay)
            {
                PSStatusTextBlock.Text = "請投幣... " + wait_cnt;
                orderpaid.Text = user_paid.ToString();
            }
            else
                PSStatusTextBlock.Text = ps_msg;
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        void DoShipment()
        {
            if(complete_trad)
            {
                //Start shipment process here
            }
        }

        private void TradCancelButton(object sender, RoutedEventArgs e)
        {
            if (isPsTradable)
            {
                backgroundWorker.CancelAsync();
                pstrad.setChargeDeviceDisable();
                if (user_paid > 0)
                {
                    PSStatusTextBlock.Text = "退幣中... ";
                    pstrad.AutoCalPayoutAmount(1, (user_paid), 0);
                }
                Thread.Sleep(100);
                pstrad.setTransactionFinish();
            }
            shoppingCart.Clear();
        }
    }
}
