using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public SettingWindow()
        {
            InitializeComponent();
            pscomm = App.Current.Resources["PSCommDataSource"] as PSCommModel;

            DataContext = new SettingWindowModel();

            string[] ports = SerialPort.GetPortNames();

            // Display each port name to the console.
            foreach (string port in ports)
            {
                if (port.Substring(0, 3) == "COM")
                    ComPortBox.Items.Add(port);
                //ComPortListBox.Items.Add(port);
                //Console.WriteLine(port);
            }
            ComPortBox.SelectedIndex = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Open_ComPort(object sender, RoutedEventArgs e)
        {
            string nCom = ComPortBox.Items[ComPortBox.SelectedIndex].ToString();
            string bRate = "9600,E,8,1";
            bool rtn = pscomm.PSOpenPort(nCom, bRate);
            if (rtn){
                pscomm.isCommInit = true;
                //Display_MsgBox("Open COM port [" + nCom + "] - Success.");
                int rtn_code = pscomm.initPS3();
            }
        }

        private void Display_MsgBox(string msg)
        {
            MsgBox.Text += msg + "\n";
        }
    }
}
