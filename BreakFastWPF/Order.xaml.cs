using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace BreakFastWPF
{
    /// <summary>
    /// Cards.xaml 的互動邏輯
    /// </summary>
    public partial class Order : UserControl
    {
        //private List<Book> Books;
        //private ObservableCollection<Book> ObsBooks;
        public CartList ShoppingCart;

        public Order()
        {
            InitializeComponent();
            //Books = BookManager.GetBooks();
            //wrapitem.ItemsSource = Books; 
            //this.DataContext = Books;
            //ObservableCollection<CartList> mycart = new ObservableCollection<CartList> { };
            ObjectDataProvider menuProvider = this.Resources["Menu"] as ObjectDataProvider;
            menuProvider = Resources["ShoppingCart"] as ObjectDataProvider;
            CartList cartlist = menuProvider.Data as CartList;
            ShoppingCart = cartlist;
            ((INotifyPropertyChanged)ShoppingCart).PropertyChanged += new PropertyChangedEventHandler(ShoppingCartChanged);

        }

        public void Window1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MessageBox.Show("PropertyChanged!");
        }

        public void ShoppingCartChanged(object sender, PropertyChangedEventArgs e)
        {
            double total = 0;
            for (int x = 0; x < ShoppingCart.Count; x++)
            {
                total += ShoppingCart[x].ItemType.Price;
            }
            selcount.Text = ShoppingCart.Count.ToString();
            total_cost.Text = total.ToString();
            if (ShoppingCart.Count == 0)
                CheckOutButton.IsEnabled = false;
            else CheckOutButton.IsEnabled = true;

        }

        private void Flipper_OnIsFlippedChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void AddToShoppingCart(object sender, RoutedEventArgs e)
        {
            //selText.Text = "You select " + ((Button)sender).Tag.ToString();
            Models.Menu selItem = ((Button)sender).Tag as Models.Menu;
            ItemBase item = new CartItem(selItem.ImageUri, selItem.MenuId, selItem.Title, selItem.Price);
            ShoppingCart.Add(item);
            //ShoppingCartListBox.ScrollIntoView(item);
            //ShoppingCartListBox.SelectedItem = item;

        }

        private void CheckOut_Button(object sender, RoutedEventArgs e)
        {
            //CheckoutDialog.IsOpen = !CheckoutDialog.IsOpen;
        }

        private void RemoveFromShoppingCart(object sender, RoutedEventArgs e)
        {
            ItemBase item = ((Button)sender).Tag as ItemBase;
            ShoppingCart.Remove(item);

        }
    }
}
