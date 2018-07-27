using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;
using BreakFastWPF.Common;

namespace BreakFastWPF
{
    /// <summary>
    /// Interaction logic for TextFields.xaml
    /// </summary>
    public partial class SettingWindow : UserControl
    {
        PSCommModel pscomm;
        string passwrd = "1234567";
        [DllImport("msvcr120.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _fpreset();
        public SettingWindow()
        {
            InitializeComponent();
            pscomm = App.Current.Resources["PSCommDataSource"] as PSCommModel;
            ComPortBox.IsEnabled = true;
            DataContext = new SettingWindowModel();

            string[] ports = SerialPort.GetPortNames();

            // Display each port name to the console.
            foreach (string port in ports)
            {
                if (port.Substring(0, 3) == "COM")
                    ComPortBox.Items.Add(port);
            }
            ComPortBox.SelectedIndex = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void OpenCloseComPort(object sender, RoutedEventArgs e)
        {
            //ComPortBox.IsEnabled = false;
            string nCom = ComPortBox.Items[ComPortBox.SelectedIndex].ToString();
            //string nCom = "COM3";
            string bRate = "9600,E,8,1";
            if (OpenCloseButton.Content.ToString() == "Connect")
            {
                bool rtn = pscomm.PSOpenPort(nCom, bRate);
                if (rtn)
                {
                    Display_MsgBox("Open port [" + nCom + "] - connected.");
                    Display_MsgBox("PS machine initializing...");
                    Ps3Initial();
                }
                else { Display_MsgBox("Port [" + nCom + "] - open failed."); }
            }
            else
            {
                pscomm.PSClosePort();
                Display_MsgBox("Open port [" + nCom + "] - disconnected.");
                OpenCloseButton.Content = "Connect";
            }
            _fpreset();
        }

        private void Ps3Initial()
        {
            int rtn_code = pscomm.initPS3();
            if (rtn_code == PSCommModel.PSCons.PS_SUCCESS)
            {
                pscomm.isCommInit = true;
                Display_MsgBox("PS machine initialized.");
                OpenCloseButton.Content = "Disconnect";
            }
            else
            {
                Display_MsgBox("PS machine initial failed.(" + pscomm.PSFuncStr(rtn_code) + ")");
                pscomm.PSClosePort();
                Display_MsgBox("Connection closed.");
            }

        }

        private void Display_MsgBox(string msg)
        {
            MsgBox.Text += msg + "\n";
        }
        private void SettingWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                //PassTextBox.Password = "";
                loginhost.IsOpen = true;
            }

        }

        private void LoginCheckButtonClick(object sender, RoutedEventArgs e)
        {
            if (PassTextBox.Password == passwrd)
                loginhost.IsOpen = false;
        }
    }

}
