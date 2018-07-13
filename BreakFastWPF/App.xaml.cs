using BreakFastWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BreakFastWPF
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        void AppStartup(object sender, StartupEventArgs args)
        {
            /*
            Cards theWindow = new Cards();
            theWindow.Show();

            ObjectDataProvider bookProvider =
                this.Resources["Books"] as ObjectDataProvider;
            bookProvider = this.Resources["ShoppingCart"] as ObjectDataProvider;
            CartList cartlist = bookProvider.Data as CartList;
            */
            Order theWindow = new Order();
            theWindow.Show();

            ObjectDataProvider menuProvider =
                this.Resources["Menu"] as ObjectDataProvider;
            menuProvider = this.Resources["ShoppingCart"] as ObjectDataProvider;
            CartList cartlist = menuProvider.Data as CartList;
            theWindow.ShoppingCart = cartlist;
            ((INotifyPropertyChanged)theWindow.ShoppingCart).PropertyChanged += new PropertyChangedEventHandler(theWindow.ShoppingCartChanged);

        }
    }
}
