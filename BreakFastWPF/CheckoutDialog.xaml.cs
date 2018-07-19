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
        public CartList shoppingCart;
        public CartList ShoppingCart { set { shoppingCart = value; Resources["ShoppingCart"] = value; } }
        CancellationTokenSource source = new CancellationTokenSource();
        SynchronizationContext context = SynchronizationContext.Current;
        Task task;
        private PSTradModel pstrad = new PSTradModel();

        public CheckoutDialog(CartList _shoppingCart)
        {
            ShoppingCart = _shoppingCart;
            InitializeComponent();
            showListBox();
            pstrad.reqPayoutAmountReport();
        }

        public void showListBox()
        {
            int amount = 0;
            for (int x = 0; x < shoppingCart.Count; x++)
            {
                amount += (int)shoppingCart[x].ItemType.Price;
                ShoppingCartListBox.Items.Add(shoppingCart[x].ItemType.Description);
            }
            ordercount.Text = "您共訂購 " + shoppingCart.Count.ToString() + " 項";
            ordertotal.Text = amount.ToString();

        }
        public void showTotalPrice()
        {
            //totalprice.Text = shoppingCart.Count.ToString();
        }
    }
}
