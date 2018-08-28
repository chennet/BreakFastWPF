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

namespace BreakFastWPF
{
    /// <summary>
    /// Interaction logic for SuperWindow.xaml
    /// </summary>
    public partial class SuperWindow : Window
    {
        public static Snackbar Snackbar;
        public SuperWindow()
        {
            InitializeComponent();
            DataContext = new SuperWindowViewModel();

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

            await DialogHost.Show(appMessageDialog, "SuperDialog");
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

            await DialogHost.Show(appExitDialog, "SuperDialog");
            this.Close();
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
