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
using System.Threading;

namespace WindowChrome.Demo
{
    /// <summary>
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public int MainLogoRotate = 0;
        public MainWindow MW = null;
        public LoadingWindow LW = null;
        public String NowLoad = "";
        public Thread myThread = null;
        public bool PotokEnds = false;

        public LoadingWindow()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MW = App.Current.MainWindow as MainWindow;
            LW = this;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();

            myThread = new Thread(new ThreadStart(LoadSkills));
            myThread.Start();
        }
        public int CountOfFileLines(String FilePath)
        {
            int ReturnLineCount = 0;
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            using (StreamReader sr = new StreamReader(FilePath, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ReturnLineCount = ReturnLineCount + 1;
                }
            }
            return ReturnLineCount;
        }

        public string ReadCertainLine(string filer, int Line)
        {
            string s = "";
            using (StreamReader str = new StreamReader(filer, Encoding.Default))
            {
                for (int i = 0; i <= Line; i++)
                {
                    s = str.ReadLine();
                }
            }
            return s;
        }

        public void LoadSkills()
        {
            String PathF = @"Bin\Data\Monsters\Addons.dnd";
            String PathF2 = @"Bin\Data\Monsters\";
            int CrtID = 0;
            for (int i = 0; i != CountOfFileLines(PathF); i = i + 2)
            {
                PathF2 = @"Bin\Data\Monsters\" + ReadCertainLine(PathF, i) + ".dnd";
                for (int i2 = 0; i2 != CountOfFileLines(PathF2); i2++)
                {
                    if (ReadCertainLine(PathF2, i2).Contains("<Name>") == true)
                    {
                        MW.LoadedCreatures[0,CrtID] = ReadCertainLine(PathF2, i2);
                        MW.LoadedCreatures[1, CrtID] = (i2 - 1).ToString();
                        MW.LoadedCreatures[1, CrtID] = ReadCertainLine(PathF, i + 1) + "_" + MW.LoadedCreatures[1, CrtID].ToString();
                        CrtID++;
                        NowLoad = "Загурзка существа:" + ReadCertainLine(PathF2, i2).Remove(0,6);
                    }
                }
            }
            String Way = @"Bin\Data\Spells\PHB.dnd";
            int SplId = 0;
            for (int i = 0; i != CountOfFileLines(Way); i++)
            {
                if (ReadCertainLine(Way, i).IndexOf("<Rus>") != -1)
                {
                    MW.LoadedSpells[0, SplId] = ReadCertainLine(Way, i);
                    MW.LoadedSpells[1, SplId] = i.ToString();
                    NowLoad = "Загурзка заклинания:" + ReadCertainLine(Way, i).Remove(0, 5);
                    SplId++;
                }
            }
            PotokEnds = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            LoadingWindow LW = (LoadingWindow)this.Owner;
            LP_LogoS.RenderTransform = new RotateTransform(MainLogoRotate);
            MainLogoRotate++;
            LP_Info.Content = NowLoad;
            if (MainLogoRotate == 360) MainLogoRotate = 1;
            if (PotokEnds == true) 
            {
                myThread.Abort();
                this.Close();
                MW.Visibility = Visibility.Visible;
            }
        }
    }
}
