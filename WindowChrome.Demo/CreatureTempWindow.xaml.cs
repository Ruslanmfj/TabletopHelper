using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;
using System.Windows.Threading;
using Microsoft.Win32;

namespace WindowChrome.Demo
{
    /// <summary>
    /// Логика взаимодействия для CreatureTempWindow.xaml
    /// </summary>
    public partial class CreatureTempWindow : Window
    {
        public CreatureTempWindow()
        {
            InitializeComponent();
        }

        private void CTW_Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void MC_Preview_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageWindow taskWindow = new ImageWindow();
            taskWindow.ThisImg = (sender as Image).Uid.ToString();
            taskWindow.NameImg = MC_Name.Content.ToString();
            taskWindow.Show();
        }

        public void GoToLink(object sender, MouseButtonEventArgs e)
        {

            Process.Start((sender as TextBlock).Uid);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DockPanel thisTB = null;

            if ((sender as Border) == MC_UnderUses) thisTB = MC_Uses;
            if ((sender as Border) == MC_UnderSkills) thisTB = MC_Skills;
            if ((sender as Border) == MC_UnderActions) thisTB = MC_Actions;

            if ((sender as Border) == MC_UnderLegendaryActions) thisTB = MC_LegendaryActions;
            if ((sender as Border) == MC_UnderCeep) thisTB = MC_Ceep;
            if ((sender as Border) == MC_UnderCeepAction) thisTB = MC_CeepAction;
            if ((sender as Border) == MC_UnderCeepEffect) thisTB = MC_CeepEffect;

            if ((sender as Border).Height == 10)
            {
                (sender as Border).Height = 33;
                thisTB.Visibility = Visibility.Collapsed;
            }
            else
            {
                (sender as Border).Height = 10;
                thisTB.Visibility = Visibility.Visible;
            }
        }
    }
}
