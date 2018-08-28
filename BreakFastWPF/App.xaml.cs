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
using System.Windows.Forms;
using System.Windows.Media;
using Database;
using Microsoft.EntityFrameworkCore;

namespace BreakFastWPF
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            InitializeComponent();

            using (DatabaseContext dbContext = new DatabaseContext())
            {
                dbContext.Database.Migrate();
            }
            //If multi screens available, show SuperWindow to screen #2
            //if (Screen.AllScreens.Length > 1)
            //{
                SuperWindow swin = new SuperWindow();
            //    Screen s1 = Screen.AllScreens[1];
            //    System.Drawing.Rectangle r1 = s1.WorkingArea;
            //    swin.WindowState = System.Windows.WindowState.Normal;
            //    swin.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            //    swin.Top = r1.Top;
            //    swin.Left = r1.Left;

                swin.Show();
                swin.WindowState = System.Windows.WindowState.Maximized;
            //}
        }

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
