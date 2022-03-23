using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WindowChrome.Demo
{
    /// <summary>
    /// Логика взаимодействия для HPGenerateModalWindow.xaml
    /// </summary>
    public partial class HPGenerateModalWindow : Window
    {
        public HPGenerateModalWindow()
        {
            InitializeComponent();
        }

        private void HPMW_Roll_Click(object sender, RoutedEventArgs e)
        {
            HPMW_Unroll.IsChecked = false;
        }

        private void HPMW_Unroll_Click(object sender, RoutedEventArgs e)
        {
            HPMW_Roll.IsChecked = false;
        }

        private void HPMW_Ok_Click(object sender, RoutedEventArgs e)
        {
            int ErrorLog = 0;
            if (HPMW_Roll.IsChecked == false) ErrorLog++;
            if (HPMW_Unroll.IsChecked == false) ErrorLog++;
            if (ErrorLog != 2)
            {
                this.Close();
            } 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int GT = 0;
            if (HPMW_Roll.IsChecked == true) GT=1;
            if (HPMW_Unroll.IsChecked == true) GT = 2;
            Window MW = this.Owner;
            (MW as MainWindow).HPGenerateType = GT;
            (MW as MainWindow).GenerateHitPoints();
        }
    }
}
