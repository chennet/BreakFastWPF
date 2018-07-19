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
    public partial class Cards : UserControl
    {
        //private List<Book> Books;
        //private ObservableCollection<Book> ObsBooks;
        public CartList ShoppingCart;
        //ObservableCollection<CartList> mycart = new ObservableCollection<CartList> { };

        public Cards()
        {
            InitializeComponent();
            //Books = BookManager.GetBooks();
            //wrapitem.ItemsSource = Books; 
            //this.DataContext = Books;
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
            total_cost.Text = total.ToString();

        }

        private void Flipper_OnIsFlippedChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void AddToShoppingCart(object sender, RoutedEventArgs e)
        {
            //selText.Text = "You select " + ((Button)sender).Tag.ToString();
            Book selbook = ((Button)sender).Tag as Book;
            ItemBase item = new CartItem(selbook.CoverImage, selbook.BookId, selbook.Title, 19.99);
            ShoppingCart.Add(item);
            ShoppingCartListBox.ScrollIntoView(item);
            ShoppingCartListBox.SelectedItem = item;

        }

        private void CheckOut_Button(object sender, RoutedEventArgs e)
        {

        }
    }
}
