/* This file is no longer use */
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
        //public CartList ShoppingCart;
        CartList ShoppingCart;
        public Order()
        {
            App.Current.Properties["MenuType"] = null;
            InitializeComponent();

            //ObjectDataProvider menuProvider = this.Resources["Menu"] as ObjectDataProvider;
            //menuProvider = Resources["ShoppingCart"] as ObjectDataProvider;
            //CartList cartlist = menuProvider.Data as CartList;
            //ShoppingCart = cartlist;
            ShoppingCart = App.Current.Resources["CartListDataSource"] as CartList;
            //((INotifyPropertyChanged)ShoppingCart).PropertyChanged += new PropertyChangedEventHandler(ShoppingCartChanged);

        }

        public void Window1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MessageBox.Show("PropertyChanged!");
        }


        private void Flipper_OnIsFlippedChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void AddToShoppingCart(object sender, RoutedEventArgs e)
        {
            //selText.Text = "You select " + ((Button)sender).Tag.ToString();
            Menus selItem = ((Button)sender).Tag as Menus;
            ItemBase item = new CartItem(selItem.ImageUri, selItem.MenuId, selItem.Title, selItem.Price);
            ShoppingCart.Add(item);
            //ShoppingCartListBox.ScrollIntoView(item);
            //ShoppingCartListBox.SelectedItem = item;

        }


    }
}
