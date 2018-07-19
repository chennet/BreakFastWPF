//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Data;
//using BreakFastWPF.Models;
using ShowMeTheXAML;
using System.Windows;

namespace BreakFastWPF
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        //void AppStartup(object sender, StartupEventArgs args)
        //{
        //    /*
        //    Cards theWindow = new Cards();
        //    theWindow.Show();
        //
        //    ObjectDataProvider bookProvider =
        //        this.Resources["Books"] as ObjectDataProvider;
        //    bookProvider = this.Resources["ShoppingCart"] as ObjectDataProvider;
        //    CartList cartlist = bookProvider.Data as CartList;
        //    */
        //    Order theWindow = new Order();
        //    theWindow.Show();
        //
        //    ObjectDataProvider menuProvider =
        //        this.Resources["Menu"] as ObjectDataProvider;
        //    menuProvider = this.Resources["ShoppingCart"] as ObjectDataProvider;
        //    CartList cartlist = menuProvider.Data as CartList;
        //    theWindow.ShoppingCart = cartlist;
        //    ((INotifyPropertyChanged)theWindow.ShoppingCart).PropertyChanged += new PropertyChangedEventHandler(theWindow.ShoppingCartChanged);
        //
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            XamlDisplay.Init();
            //Illustration of setting culture info fully in WPF:
            /*             
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            */

            base.OnStartup(e);
        }
    }
}
