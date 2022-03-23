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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool PotokEnds = false;
        Double[] OldWIndowParams = new Double[4];
        public string[,] ArmInfo = new string[3, 6];
        public int ActiveWeappon = 0;
        public Window InventoryWindow = null;
        public String StatID = "0";
        public int HPGenerateType = 0;
        public int ClassID = 0;
        public int SubClassId = 0;
        public int RaseID = 0;
        public bool NewSpellValue = false;
        public MediaPlayer MyPlayer = new MediaPlayer();
        public string[,] LoadedCreatures = new string[2, 5000];
        public string[,] LoadedSpells = new string[2, 5000];
        public string[] LoadedProfile = new string[33000];
        public int FileRow = 0;
        public String ProfileWay = "";
        public String OldNoteText = "";
        public String NowOpenedStory = "";
        public string CurrentProfile = "";
        Label DeletetStory = null;
        Label DeletedScene = null;
        Label DeletedSpell = null;
        public String OldStoryName = "";
        public String CurrentScene = "";
        public Label DroppedLabel = null;
        public Label DeletedCombatChar = null;
        public Label DeletedChar = null;
        public Label SelectedCreature = null;
        public int SceneEditorState = 0;
        public bool MapMove = false;
        public double OldXPos = 0;
        public double OldYPos = 0;
        Border CellMap = null;
        Double OldBrdrX = 0;
        Double OldBrdrY = 0;
        Boolean SellBrush = false;
        const string nullstring = "<NULLSTRING>";
        public string LoadingTitle = null;
        public Thread myThread = null;
        public String[,] CreInCombat = new string[3, 200];
        public Label[] CreInCombatLbl = new Label[200];
        public TextPointer TPP = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ChangeImage(string PathIMG, Image SourceObj)
        {
            Image myImage3 = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(PathIMG, UriKind.Relative);
            bi3.EndInit();
            myImage3.Stretch = Stretch.Fill;
            SourceObj.Source = bi3;
        }

        private void CloseApp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void MaximizeApp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((this.Top != 0) & (this.Left != 0) & (this.Height != SystemParameters.WorkArea.Height) & (this.Width != SystemParameters.WorkArea.Width))
            {
                OldWIndowParams[0] = this.Top;
                OldWIndowParams[1] = this.Left;
                OldWIndowParams[2] = this.Height;
                OldWIndowParams[3] = this.Width;
                this.Left = 0;
                this.Top = 0;
                this.Height = SystemParameters.WorkArea.Height;
                this.Width = SystemParameters.WorkArea.Width;
            }
            else
            {
                this.Left = OldWIndowParams[1];
                this.Top = OldWIndowParams[0];
                this.Height = OldWIndowParams[2];
                this.Width = OldWIndowParams[3];
            }
        }

        private void Minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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

        public string ReturnedModificator(int Value)
        {
            string ReturnedValue = "";

            int Mod = (Value - 10) / 2;

            if (Value > 20) ReturnedValue = "+" + Mod;
            if (Value == 20) ReturnedValue = "+5";
            if (Value == 19) ReturnedValue = "+4";
            if (Value == 18) ReturnedValue = "+4";
            if (Value == 17) ReturnedValue = "+3";
            if (Value == 16) ReturnedValue = "+3";
            if (Value == 15) ReturnedValue = "+2";
            if (Value == 14) ReturnedValue = "+2";
            if (Value == 13) ReturnedValue = "+1";
            if (Value == 12) ReturnedValue = "+1";
            if (Value == 11) ReturnedValue = "0";
            if (Value == 10) ReturnedValue = "0";
            if (Value == 9) ReturnedValue = "-1";
            if (Value == 8) ReturnedValue = "-1";
            if (Value == 7) ReturnedValue = "-2";
            if (Value == 6) ReturnedValue = "-2";
            if (Value == 5) ReturnedValue = "-3";
            if (Value == 4) ReturnedValue = "-3";
            if (Value == 3) ReturnedValue = "-4";
            if (Value == 2) ReturnedValue = "-4";
            if (Value == 1) ReturnedValue = "-5";
            if (Value <= 0) ReturnedValue = "-5";
            return ReturnedValue;
        }

        public Label MC_CreateLink(String Text)
        {
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(1, 1);
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(81, 81, 81), 1.0));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(31, 31, 31), 0.0));

            Label Temp = new Label();
            RussianGroup.Children.Add(Temp);
            DockPanel.SetDock(Temp, Dock.Top);
            Temp.Height = 25;
            Temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            Temp.Margin = new Thickness(0, 0, 0, 0);
            Temp.FontFamily = new FontFamily("Bookman Old Style");
            Temp.FontSize = 12;
            Temp.Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            Temp.Background = myLinearGradientBrush;
            Temp.Content = Text;
            Temp.HorizontalContentAlignment = HorizontalAlignment.Center;
            Temp.MouseDown += LoadMonster;
            return Temp;
        }

        public void LoadMonster(object sender, MouseButtonEventArgs e)
        {
            ClearBlockStat();
            AddHeaderToBlockStat();

            String FilePath = (sender as Label).Uid;
            for (int i = 0; i != CountOfFileLines(@"Bin\Data\Monsters\Addons.dnd"); i++)
            {
                if (ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i) == FilePath.Remove(2))
                {
                    FilePath = @"Bin\Data\Monsters\" + ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i - 1) + ".dnd";
                }
            }
            String[] Lines = new String[1000];
            int MainCounter = 0;
            int DiscriptionStart = 0;
            for (int i = Int32.Parse((sender as Label).Uid.Remove(0, 3)); i != CountOfFileLines(FilePath); i++)
            {
                if (ReadCertainLine(FilePath, i) == "<Discription>") DiscriptionStart = i + 1;
                if (ReadCertainLine(FilePath, i) == "</X>") break;
                else
                {
                    Lines[MainCounter] = ReadCertainLine(FilePath, i);
                    MainCounter++;
                }
            }
            for (int i = 1; i != MainCounter; i++)
            {
                if (Lines[i].Contains("<Name>") == true)
                {
                    MC_Name.Content = Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<IMG>") == true)
                {
                    Image ImageContainer = MC_Preview;
                    ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Bin\img\Monsters\" + Lines[i].Remove(0, 5) + ".png", UriKind.Absolute));
                    ImageContainer.Source = image;
                    MC_Preview.Uid = Environment.CurrentDirectory + @"\Bin\img\Monsters\" + Lines[i].Remove(0, 5) + "_Full" + ".png";
                }
                if (Lines[i].Contains("<Mainlink>") == true)
                {
                    LinkMain.Uid = Lines[i].Remove(0, 10);
                }
                if (Lines[i].Contains("<Size>") == true)
                {
                    MC_TypeSizeView.Content = Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<Type>") == true)
                {
                    MC_TypeSizeView.Content = MC_TypeSizeView.Content.ToString() + " " + Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<View>") == true)
                {
                    MC_TypeSizeView.Content = MC_TypeSizeView.Content.ToString() + ", " + Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<Armor>") == true)
                {
                    MC_Armor.Content = "Класс доспеха: " + Lines[i].Remove(0, 7);
                }
                if (Lines[i].Contains("<ArmorType>") == true)
                {
                    MC_Armor.Content = MC_Armor.Content.ToString() + " " + Lines[i].Remove(0, 11);
                }
                if (Lines[i].Contains("<Hit>") == true)
                {
                    MC_Hit.Content = Lines[i].Remove(0, 5);
                }
                if (Lines[i].Contains("<HitDiceCount>") == true)
                {
                    MC_HitDice.Content = "(" + Lines[i].Remove(0, 14);
                }
                if (Lines[i].Contains("<HitDice>") == true)
                {
                    MC_HitDice.Content = MC_HitDice.Content.ToString() + "d" + Lines[i].Remove(0, 9) + ")";
                }
                if (Lines[i].Contains("<HitStatic>") == true)
                {
                    MC_HitDice.Content = MC_HitDice.Content.ToString().Remove(MC_HitDice.Content.ToString().Length - 1, 1) + " + " + Lines[i].Remove(0, 11) + ")";
                }
                if (Lines[i].Contains("<Walk>") == true)
                {
                    MC_Speed.Content = "Скорость: " + Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<Swim>") == true)
                {
                    MC_Speed.Content = MC_Speed.Content.ToString() + ", Плавая: " + Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<Grab>") == true)
                {
                    MC_Speed.Content = MC_Speed.Content.ToString() + ", Лазая: " + Lines[i].Remove(0, 6);
                }
                if (Lines[i].Contains("<Dig>") == true)
                {
                    MC_Speed.Content = MC_Speed.Content.ToString() + ", Копая: " + Lines[i].Remove(0, 5);
                }
                if (Lines[i].Contains("<Fly>") == true)
                {
                    MC_Speed.Content = MC_Speed.Content.ToString() + ", Летая: " + Lines[i].Remove(0, 5);
                }
                if (Lines[i].Contains("<Sil>") == true)
                {
                    MC_Sil.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                }
                if (Lines[i].Contains("<Lov>") == true)
                {
                    MC_Lov.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                }
                if (Lines[i].Contains("<Tel>") == true)
                {
                    MC_Tel.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                }
                if (Lines[i].Contains("<Int>") == true)
                {
                    MC_Int.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                }
                if (Lines[i].Contains("<Mdr>") == true)
                {
                    MC_Mdr.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                }
                if (Lines[i].Contains("<Har>") == true)
                {
                    MC_Har.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                }
                if (Lines[i].Contains("<SpellTab>") == true)
                {
                    int SpellCounter = i + 1;
                    while (Lines[SpellCounter] != "</SpellTab>")
                    {
                        CreateMCUses(" * ", Lines[SpellCounter], MC_Skills);
                        SpellCounter++;
                    }
                    i = SpellCounter;
                }
                if (Lines[i].Contains("<Chalange>") == true)
                {
                    CreateMCUses("Опасность:", Lines[i].Remove(0, 10), MC_Uses);
                }
                if (Lines[i].Contains("<Source>") == true)
                {
                    CreateMCUses("Источник:", Lines[i].Remove(0, 8), MC_Uses);
                }
                if (Lines[i].Contains("<j1>") == true)
                {
                    int end = Lines[i].Remove(0, 4).IndexOf("<j1>") + 4;
                    CreateMCUses(Lines[i].Remove(0, 4).Remove(end - 4), Lines[i].Remove(0, end + 4), MC_Uses);
                }
                if (Lines[i].Contains("<j2>") == true)
                {
                    MC_UnderSkills.Visibility = Visibility.Visible;
                    MC_Skills.Visibility = Visibility.Visible;
                    int end = Lines[i].Remove(0, 4).IndexOf("<j2>") + 4;
                    CreateMCUses(Lines[i].Remove(0, 4).Remove(end - 4), Lines[i].Remove(0, end + 4), MC_Skills);
                }
                if (Lines[i].Contains("<j3>") == true)
                {
                    MC_UnderActions.Visibility = Visibility.Visible;
                    MC_Actions.Visibility = Visibility.Visible;
                    int end = Lines[i].Remove(0, 4).IndexOf("<j3>") + 4;
                    CreateMCUses(Lines[i].Remove(0, 4).Remove(end - 4), Lines[i].Remove(0, end + 4), MC_Actions);
                }
                if (Lines[i].Contains("<jr>") == true)
                {
                    MC_UnderReactions.Visibility = Visibility.Visible;
                    MC_Reactions.Visibility = Visibility.Visible;
                    int end = Lines[i].Remove(0, 4).IndexOf("<jr>") + 4;
                    CreateMCUses(Lines[i].Remove(0, 4).Remove(end - 4), Lines[i].Remove(0, end + 4), MC_Reactions);
                }
                if (Lines[i].Contains("<jB>") == true)
                {
                    String Headd = Lines[i];
                    String Content = Lines[i];

                    Headd = Headd.Remove(0, 4);
                    Headd = Headd.Remove(Headd.IndexOf(">") + 1);
                    Headd = Headd.Remove(Headd.Length - 4, 4);

                    Content = Content.Remove(0, 4);
                    Content = Content.Remove(0, Content.IndexOf(">") + 1);
                    if (Content.Contains("[j]") == true)
                    {
                        String Content2 = Content;
                        Content2 = Content2.Remove(Content2.IndexOf("[j]"));
                        CreateBoxDiscription(Headd, Content2);
                        Content2 = Content;
                        Content2 = Content2.Remove(0, Content2.IndexOf("[j]"));
                        String jHead = Content2;
                        jHead = jHead.Remove(0, 3);
                        jHead = jHead.Remove(jHead.IndexOf("[/j]"));
                        MC_BlockDiscription.Inlines.Add("\n");
                        MC_BlockDiscription.Inlines.Add(new Run(jHead) { FontWeight = FontWeights.Bold });
                        MC_BlockDiscription.Inlines.Add("\n");
                        Content2 = Content2.Remove(0, Content2.IndexOf("[/j]") + 4);
                        MC_BlockDiscription.Inlines.Add(Content2);
                    }
                    else
                    {
                        CreateBoxDiscription(Headd, Content);
                    }
                }
                if (Lines[i].Contains("<jBT>") == true)
                {
                    String Headd = Lines[i];
                    String Content = Lines[i];

                    MC_BlockDiscription.Inlines.Add("\n");
                    Headd = Headd.Remove(0, 5);
                    Content = Headd.Remove(0, Headd.IndexOf("<jBT>") + 5);
                    Headd = Headd.Remove(Headd.IndexOf("<jBT>"));
                    MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                    MC_BlockDiscription.Inlines.Add(Content);
                }
                if (Lines[i].Contains("<Legendary>") == true)
                {
                    MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                    MC_LegendaryActions.Visibility = Visibility.Visible;
                    CreateMCUses("", Lines[i].Remove(0, 11), MC_LegendaryActions);
                }
                if (Lines[i].Contains("<lg>") == true)
                {
                    MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                    MC_LegendaryActions.Visibility = Visibility.Visible;

                    int end = Lines[i].Remove(0, 4).IndexOf("<lg>") + 4;
                    CreateMCUses(Lines[i].Remove(0, 4).Remove(end - 4), Lines[i].Remove(0, end + 4), MC_LegendaryActions);
                }
                if (Lines[i].Contains("<ceep>") == true)
                {
                    MC_UnderCeep.Visibility = Visibility.Visible;
                    MC_Ceep.Visibility = Visibility.Visible;
                    CreateMCUses("", Lines[i].Remove(0, 6), MC_Ceep);
                }
                if (Lines[i].Contains("<ca>") == true)
                {
                    MC_UnderCeepAction.Visibility = Visibility.Visible;
                    MC_CeepAction.Visibility = Visibility.Visible;
                    CreateMCUses("", Lines[i].Remove(0, 4), MC_CeepAction);
                }
                if (Lines[i].Contains("<dca>") == true)
                {
                    MC_UnderCeepAction.Visibility = Visibility.Visible;
                    MC_CeepAction.Visibility = Visibility.Visible;
                    CreateMCUses(" * ", Lines[i].Remove(0, 5), MC_CeepAction);
                }
                if (Lines[i].Contains("<ce>") == true)
                {
                    MC_UnderCeepEffect.Visibility = Visibility.Visible;
                    MC_CeepEffect.Visibility = Visibility.Visible;
                    CreateMCUses("", Lines[i].Remove(0, 4), MC_CeepEffect);
                }
                if (Lines[i].Contains("<dce>") == true)
                {
                    MC_UnderCeepEffect.Visibility = Visibility.Visible;
                    MC_CeepEffect.Visibility = Visibility.Visible;
                    CreateMCUses(" * ", Lines[i].Remove(0, 5), MC_CeepEffect);
                }
                if (Lines[i] == "<Discription>")
                {
                    for (int i2 = i + 1; i2 != MainCounter; i2++)
                    {
                        bool Special = false;
                        if (Lines[i2].Contains("<L>") == true)
                        {
                            AddDiscriptionLink(Lines[i2].Remove(0, 3).Remove(Lines[i2].Remove(0, 3).IndexOf("<")));
                            MC_Dicsription.Cursor = Cursors.Hand;
                            String s = Lines[i2].Remove(0, 3);
                            s = s.Remove(0, s.IndexOf("<") + 1);
                            s = s.Remove(s.Length - 1);
                            MC_Dicsription.Uid = s;
                            MC_Dicsription.MouseUp += GoToLink;
                            Special = true;
                        }
                        if (Lines[i2].Contains("<j>") == true)
                        {
                            MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3)) { FontWeight = FontWeights.Bold });
                            Special = true;
                        }
                        if (Lines[i2].Contains("<h>") == true)
                        {
                            MC_Dicsription.Inlines.Add("\n");
                            MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3)) { FontWeight = FontWeights.Bold });
                            Special = true;
                        }
                        if (Special == false)
                        {
                            MC_Dicsription.Inlines.Add(Lines[i2]);
                        }
                    }
                    i = MainCounter - 1;
                }
            }
        }

        public void CreateBoxDiscription(String Head, String Content)
        {
            MC_BlckDiscr.Visibility = Visibility.Visible;
            MC_BlockDiscription.Inlines.Add(new Run(Head) { FontWeight = FontWeights.Bold });
            MC_BlockDiscription.Inlines.Add("\n");
            MC_BlockDiscription.Inlines.Add(Content);
        }

        public void CreateMCUses(String Header, String Content, DockPanel SourceDockPanel)
        {
            TextBlock TB = new TextBlock();
            SourceDockPanel.Children.Add(TB);
            DockPanel.SetDock(TB, Dock.Top);
            TB.TextWrapping = TextWrapping.Wrap;
            TB.Inlines.Add(new Run(Header + " ") { FontWeight = FontWeights.Bold });
            TB.Inlines.Add(new Run(Content) { FontStyle = FontStyles.Italic });
        }

        public void AddDiscriptionLink(String Content)
        {
            MC_Dicsription.TextWrapping = TextWrapping.Wrap;
            MC_Dicsription.Inlines.Add(new Run(Content) { TextDecorations = TextDecorations.Underline });
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

        public void GoToLink(object sender, MouseButtonEventArgs e)
        {

            Process.Start((sender as TextBlock).Uid);
        }

        public void ClearBlockStat()
        {
            MC_BlckDiscr.Visibility = Visibility.Collapsed;
            MC_BlockDiscription.Inlines.Clear();
            MC_Uses.Children.Clear();
            MC_Skills.Children.Clear();
            MC_Actions.Children.Clear();
            MC_LegendaryActions.Children.Clear();
            MC_Ceep.Children.Clear();
            MC_CeepAction.Children.Clear();
            MC_CeepEffect.Children.Clear();
            MC_Reactions.Children.Clear();

            MC_UnderReactions.Visibility = Visibility.Collapsed;
            MC_Reactions.Visibility = Visibility.Collapsed;
            MC_UnderSkills.Visibility = Visibility.Collapsed;
            MC_Skills.Visibility = Visibility.Collapsed;
            MC_Actions.Visibility = Visibility.Collapsed;
            MC_UnderActions.Visibility = Visibility.Collapsed;
            MC_LegendaryActions.Visibility = Visibility.Collapsed;
            MC_Ceep.Visibility = Visibility.Collapsed;
            MC_CeepAction.Visibility = Visibility.Collapsed;
            MC_CeepEffect.Visibility = Visibility.Collapsed;
            MC_UnderLegendaryActions.Visibility = Visibility.Collapsed;
            MC_UnderCeep.Visibility = Visibility.Collapsed;
            MC_UnderCeepAction.Visibility = Visibility.Collapsed;
            MC_UnderCeepEffect.Visibility = Visibility.Collapsed;

            MC_Dicsription.Inlines.Clear();
            MC_Dicsription.Cursor = Cursors.Arrow;
            MC_Dicsription.MouseDown -= GoToLink;
        }

        public void AddHeaderToBlockStat()
        {
            String[] TempArray = new string[7];
            DockPanel[] TempDockPanel = new DockPanel[7];

            TempArray[0] = "-СПОСОБНОСТИ-";
            TempArray[1] = "-ДЕЙСТВИЯ-";
            TempArray[2] = "-ЛЕГЕНДАРНЫЕ ДЕЙСТВИЯ-";
            TempArray[3] = "-ЛОГОВО-";
            TempArray[4] = "-ДЕЙСТВИЯ ЛОГОВА-";
            TempArray[5] = "-ЭФФЕКТЫ ЛОГОВА-";
            TempArray[6] = "-РЕАКЦИИ-";
            TempDockPanel[0] = MC_Skills;
            TempDockPanel[1] = MC_Actions;
            TempDockPanel[2] = MC_LegendaryActions;
            TempDockPanel[3] = MC_Ceep;
            TempDockPanel[4] = MC_CeepAction;
            TempDockPanel[5] = MC_CeepEffect;
            TempDockPanel[6] = MC_Reactions;

            for (int i = 0; i != 7; i++)
            {
                Label UsesLbl = new Label();
                TempDockPanel[i].Children.Add(UsesLbl);
                UsesLbl.Height = MC_DiscrLabel.Height;
                UsesLbl.Width = MC_DiscrLabel.Width;
                UsesLbl.Foreground = MC_DiscrLabel.Foreground;
                UsesLbl.Effect = MC_DiscrLabel.Effect;
                UsesLbl.Content = TempArray[i];
                DockPanel.SetDock(UsesLbl, Dock.Top);
            }
        }

        private void MC_Preview_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ImageWindow taskWindow = new ImageWindow();
            taskWindow.ThisImg = (sender as Image).Uid.ToString();
            taskWindow.NameImg = MC_Name.Content.ToString();
            taskWindow.Show();
        }

        private void NM_HitFull_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Opened = false;
            if (NM_HitGroup.Visibility == Visibility.Visible) Opened = true;
            CloseAllGroups();
            if (Opened == false) NM_HitGroup.Visibility = Visibility.Visible;
        }

        public void CloseAllGroups()
        {
            NM_HitGroup.Visibility = Visibility.Collapsed;
            NM_SpeedGroup.Visibility = Visibility.Collapsed;
            NM_SpasGroup.Visibility = Visibility.Collapsed;
            NM_FeelsGroup.Visibility = Visibility.Collapsed;
            NM_SkillGroup.Visibility = Visibility.Collapsed;
        }

        private void NM_CancelHit_Click(object sender, RoutedEventArgs e)
        {
            NM_HitGroup.Visibility = Visibility.Collapsed;
        }

        private void NM_ConfurmHit_Click(object sender, RoutedEventArgs e)
        {
            bool HttDiceCountError = false;
            bool HitDiceError = false;
            bool HitStaticrror = false;
            bool TotalErrors = false;

            for (int i = 0; i != NM_HitDiceCount.Text.Length; i++)
            {
                if ((NM_HitDiceCount.Text[i] == '1') ^ (NM_HitDiceCount.Text[i] == '2') ^ (NM_HitDiceCount.Text[i] == '3') ^ (NM_HitDiceCount.Text[i] == '4') ^ (NM_HitDiceCount.Text[i] == '5')
                     ^ (NM_HitDiceCount.Text[i] == '6') ^ (NM_HitDiceCount.Text[i] == '7') ^ (NM_HitDiceCount.Text[i] == '8') ^ (NM_HitDiceCount.Text[i] == '9') ^ (NM_HitDiceCount.Text[i] == '0'))
                    Console.WriteLine("Символ ОК");
                else
                {
                    HttDiceCountError = true;
                    break;
                }
            }
            if (NM_HitDice.SelectedIndex == -1) HitDiceError = true;
            for (int i = 0; i != NM_HitStatic.Text.Length; i++)
            {
                if ((NM_HitStatic.Text[i] == '1') ^ (NM_HitStatic.Text[i] == '2') ^ (NM_HitStatic.Text[i] == '3') ^ (NM_HitStatic.Text[i] == '4') ^ (NM_HitStatic.Text[i] == '5')
                     ^ (NM_HitStatic.Text[i] == '6') ^ (NM_HitStatic.Text[i] == '7') ^ (NM_HitStatic.Text[i] == '8') ^ (NM_HitStatic.Text[i] == '9') ^ (NM_HitStatic.Text[i] == '0'))
                    Console.WriteLine("Символ ОК");
                else
                {
                    HitStaticrror = true;
                    break;
                }
            }

            if ((HttDiceCountError == false) && (HitDiceError == false) && (HitStaticrror == false)) TotalErrors = false;
            else TotalErrors = true;

            if (TotalErrors == true)
            {
                if (HttDiceCountError == true) NM_HitDiceCount.Background = new SolidColorBrush(Color.FromRgb(164, 22, 22));
                if (HitDiceError == true) NM_HitDice.Background = new SolidColorBrush(Color.FromRgb(164, 22, 22));
                if (HitStaticrror == true) NM_HitStatic.Background = new SolidColorBrush(Color.FromRgb(164, 22, 22));
            }
            else
            {
                int Dice = 0;
                if (NM_HitDice.SelectedIndex == 0) Dice = 2;
                if (NM_HitDice.SelectedIndex == 1) Dice = 3;
                if (NM_HitDice.SelectedIndex == 2) Dice = 4;
                if (NM_HitDice.SelectedIndex == 3) Dice = 5;
                if (NM_HitDice.SelectedIndex == 4) Dice = 6;
                if (NM_HitDice.SelectedIndex == 5) Dice = 10;
                if (NM_HitDice.SelectedIndex == 6) Dice = 50;
                NM_HitFull.Content = (Dice * Int32.Parse(NM_HitDiceCount.Text)) + " (" + NM_HitDiceCount.Text + NM_HitDice.Text + " + " + NM_HitStatic.Text + ")";
            }
        }

        private void NM_HitDiceCount_MouseEnter(object sender, MouseEventArgs e)
        {
            NM_HitDiceCount.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void NM_HitDice_MouseEnter(object sender, MouseEventArgs e)
        {
            NM_HitDice.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void NM_HitStatic_MouseEnter(object sender, MouseEventArgs e)
        {
            NM_HitStatic.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void NM_SpeedFull_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Opened = false;
            if (NM_SpeedGroup.Visibility == Visibility.Visible) Opened = true;
            CloseAllGroups();
            if (Opened == false) NM_SpeedGroup.Visibility = Visibility.Visible;
        }

        private void NM_Speed_LostFocus(object sender, RoutedEventArgs e)
        {
            bool Speed = false;
            bool Swim = false;
            bool Fly = false;
            bool Grab = false;
            bool Dig = false;

            if (NM_Speed.Text != "") Speed = true;
            if (NM_Swim.Text != "") Swim = true;
            if (NM_Fly.Text != "") Fly = true;
            if (NM_Grab.Text != "") Grab = true;
            if (NM_Dig.Text != "") Dig = true;

            String Total = "";
            if (Speed == true) Total = NM_Speed.Text + " фт.";
            if (Swim == true)
            {
                if (Total != "") Total = Total + ", Плавая: " + NM_Swim.Text + " фт.";
                else Total = "Плавая: " + NM_Swim.Text + " фт.";
            }
            if (Fly == true)
            {
                if (Total != "") Total = Total + ", Летая: " + NM_Fly.Text + " фт.";
                else Total = "Летая: " + NM_Fly.Text + " фт.";
            }
            if (Grab == true)
            {
                if (Total != "") Total = Total + ", Лазая: " + NM_Grab.Text + " фт.";
                else Total = "Лазая: " + NM_Grab.Text + " фт.";
            }
            if (Dig == true)
            {
                if (Total != "") Total = Total + ", Копая: " + NM_Dig.Text + " фт.";
                else Total = "Копая: " + NM_Dig.Text + " фт.";
            }

            NM_SpeedFull.Content = Total;
        }

        private void NM_SSil_LostFocus(object sender, RoutedEventArgs e)
        {
            bool S = false;
            bool L = false;
            bool T = false;
            bool I = false;
            bool M = false;
            bool H = false;

            if (NM_SSil.Text != "") S = true;
            if (NM_SLov.Text != "") L = true;
            if (NM_STel.Text != "") T = true;
            if (NM_SInt.Text != "") I = true;
            if (NM_SMdr.Text != "") M = true;
            if (NM_SHar.Text != "") H = true;

            String Total = "";
            if (S == true) Total = "СИЛ: " + NM_SSil.Text;
            if (L == true)
            {
                if (Total != "") Total = Total + ", ЛОВ: " + NM_SLov.Text;
                else Total = "ЛОВ: " + NM_SLov.Text;
            }
            if (T == true)
            {
                if (Total != "") Total = Total + ", ТЕЛ: " + NM_STel.Text;
                else Total = "ТЕЛ: " + NM_STel.Text;
            }
            if (I == true)
            {
                if (Total != "") Total = Total + ", ИНТ: " + NM_SInt.Text;
                else Total = "ИНТ: " + NM_SInt.Text;
            }
            if (M == true)
            {
                if (Total != "") Total = Total + ", МДР: " + NM_SMdr.Text;
                else Total = "МДР: " + NM_SMdr.Text;
            }
            if (H == true)
            {
                if (Total != "") Total = Total + ", ХАР: " + NM_SHar.Text;
                else Total = "ХАР: " + NM_SHar.Text;
            }

            NM_SpasFull.Content = Total;
        }

        private void NM_SpasFull_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Opened = false;
            if (NM_SpasGroup.Visibility == Visibility.Visible) Opened = true;
            CloseAllGroups();
            if (Opened == false) NM_SpasGroup.Visibility = Visibility.Visible;
        }

        private void NM_SkillsFull_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Opened = false;
            if (NM_SkillGroup.Visibility == Visibility.Visible) Opened = true;
            CloseAllGroups();
            if (Opened == false) NM_SkillGroup.Visibility = Visibility.Visible;
        }

        private void ToGeneratorLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void ToGeneratorLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        private void ToGeneratorLbl1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pages.SelectedIndex = 2;

            RussianGroup.Children.Clear();
            int i = 0;
            while (LoadedCreatures[0, i] != null)
            {
                Console.WriteLine(LoadedCreatures[0, i]);
                Label LastLbl = null;
                LastLbl = MC_CreateLink(LoadedCreatures[0, i].Remove(0, 6));
                LastLbl.Uid = LoadedCreatures[1, i];
                i++;
            }
        }

        private void ToGeneratorLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            Pages.SelectedIndex = 1;
            ToNPCGenerator.Visibility = Visibility.Visible;
            ToMonsterCreator.Visibility = Visibility.Visible;
        }

        private void ToGeneratorLbl3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pages.SelectedIndex = 3;
        }

        private void ToGeneratorLbl2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pages.SelectedIndex = 1;
        }

        public void HideAllTopButtons()
        {
            ToNPCGenerator.Visibility = Visibility.Collapsed;
            ToMonsterList.Visibility = Visibility.Collapsed;
            ToMonsterCreator.Visibility = Visibility.Collapsed;
            ToSpellBook.Visibility = Visibility.Collapsed;
            ToDMNotes.Visibility = Visibility.Collapsed;
            ToCombatPage.Visibility = Visibility.Collapsed;
            ToMap.Visibility = Visibility.Collapsed;
        }

        private void NM_FeelsFull_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Opened = false;
            if (NM_FeelsGroup.Visibility == Visibility.Visible) Opened = true;
            CloseAllGroups();
            if (Opened == false) NM_FeelsGroup.Visibility = Visibility.Visible;
        }

        private void NM_SkillsArrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border TempBrdr = null;
            Image TempArrow = null;

            if ((sender as Image) == NM_11) TempBrdr = NM_SkillDiscr;
            if ((sender as Image) == NM_22) TempBrdr = NM_ActionsDiscr;
            if ((sender as Image) == NM_33) TempBrdr = NM_LegActions;
            if ((sender as Image) == NM_44) TempBrdr = NM_Cave;
            if ((sender as Image) == NM_55) TempBrdr = NM_CaveActions;
            if ((sender as Image) == NM_66) TempBrdr = NM_CaveEffects;
            if ((sender as Image) == NM_77) TempBrdr = NM_Reactions;
            if ((sender as Image) == NM_88) TempBrdr = NM_Variants;
            if ((sender as Image) == NM_99) TempBrdr = NM_Discr;

            if ((sender as Image) == NM_11) TempArrow = NM_11;
            if ((sender as Image) == NM_22) TempArrow = NM_22;
            if ((sender as Image) == NM_33) TempArrow = NM_33;
            if ((sender as Image) == NM_44) TempArrow = NM_44;
            if ((sender as Image) == NM_55) TempArrow = NM_55;
            if ((sender as Image) == NM_66) TempArrow = NM_66;
            if ((sender as Image) == NM_77) TempArrow = NM_77;
            if ((sender as Image) == NM_88) TempArrow = NM_88;
            if ((sender as Image) == NM_99) TempArrow = NM_99;


            if (TempBrdr.Visibility == Visibility.Collapsed)
            {
                TempBrdr.Visibility = Visibility.Visible;
                ChangeImage("ArrowDown.png", TempArrow);
            }
            else
            {
                TempBrdr.Visibility = Visibility.Collapsed;
                TempBrdr.Visibility = Visibility.Collapsed;
                ChangeImage("ArrowUp.png", TempArrow);
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            NM_SkillsRich.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        private void Label_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            NM_SkillsRich.HorizontalContentAlignment = HorizontalAlignment.Right;
        }

        private void Label_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            NM_SkillsRich.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }

        private void CL_More_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CL_CharList.Width == 260)
            {
                CL_CharList.Width = 53;
                CL_Chars.Visibility = Visibility.Hidden;
                CL_CharsListLable.Visibility = Visibility.Hidden;
                CL_CharList.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                CL_More.Margin = new Thickness(0, 10, 0, 0);
            }
            else
            {
                CL_CharList.Width = 260;
                CL_Chars.Visibility = Visibility.Visible;
                CL_CharsListLable.Visibility = Visibility.Visible;
                CL_CharList.Background = new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));
                CL_More.Margin = new Thickness(0, 10, -38, 0);
            }

        }

        private void CL_More_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImage("MorePassed.png", CL_More);
        }

        private void CL_More_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImage("More_Unpassed.png", CL_More);
        }

        private void CL_NewChar_MouseEnter(object sender, MouseEventArgs e)
        {
            CL_NewChar.Background = new SolidColorBrush(Color.FromArgb(50, 151, 184, 132));
        }

        private void CL_NewChar_MouseLeave(object sender, MouseEventArgs e)
        {
            CL_NewChar.Background = new SolidColorBrush(Color.FromArgb(50, 162, 162, 162));
        }

        private void CL_CharName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateLabels();
            CL_CharNameEdit.Text = CL_CharName.Content.ToString();
            CL_CharName.Visibility = Visibility.Collapsed;
            CL_CharNameEdit.Visibility = Visibility.Visible;
        }

        private void CL_CharNameEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CL_CharNameEdit.Text != "")
                {
                    CL_CharName.Content = CL_CharNameEdit.Text;
                    CL_CharName.Visibility = Visibility.Visible;
                    CL_CharNameEdit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CL_CharNameEdit.Text = "Безимянный";
                    CL_CharName.Content = CL_CharNameEdit.Text;
                    CL_CharName.Visibility = Visibility.Visible;
                    CL_CharNameEdit.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CL_Stg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CL_StgEditor.Visibility = Visibility.Collapsed;
            CL_DexEditor.Visibility = Visibility.Collapsed;
            CL_ConEditor.Visibility = Visibility.Collapsed;
            CL_IntEditor.Visibility = Visibility.Collapsed;
            CL_WidEditor.Visibility = Visibility.Collapsed;
            CL_ChaEditor.Visibility = Visibility.Collapsed;
            CL_Stg.Visibility = Visibility.Visible;
            CL_Dex.Visibility = Visibility.Visible;
            CL_Con.Visibility = Visibility.Visible;
            CL_Int.Visibility = Visibility.Visible;
            CL_Wid.Visibility = Visibility.Visible;
            CL_Cha.Visibility = Visibility.Visible;
            TextBox Editor = null;
            Label ValLabele = null;
            if (sender == CL_Stg)
            {
                Editor = CL_StgEditor;
                ValLabele = CL_StgValue;
            }
            if (sender == CL_Dex)
            {
                Editor = CL_DexEditor;
                ValLabele = CL_DexValue;
            }
            if (sender == CL_Con)
            {
                Editor = CL_ConEditor;
                ValLabele = CL_ConValue;
            }
            if (sender == CL_Int)
            {
                Editor = CL_IntEditor;
                ValLabele = CL_IntValue;
            }
            if (sender == CL_Wid)
            {
                Editor = CL_WidEditor;
                ValLabele = CL_WidValue;
            }
            if (sender == CL_Cha)
            {
                Editor = CL_ChaEditor;
                ValLabele = CL_ChaValue;
            }

            Editor.Visibility = Visibility.Visible;
            Editor.Text = ValLabele.Content.ToString();
            (sender as Label).Visibility = Visibility.Collapsed;
        }

        private void CL_StgEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Label SLabel = null;
                Label ValLabele = null;
                if (sender == CL_StgEditor)
                {
                    SLabel = CL_Stg;
                    ValLabele = CL_StgValue;
                }
                if (sender == CL_DexEditor)
                {
                    SLabel = CL_Dex;
                    ValLabele = CL_DexValue;
                }
                if (sender == CL_ConEditor)
                {
                    SLabel = CL_Con;
                    ValLabele = CL_ConValue;
                }
                if (sender == CL_IntEditor)
                {
                    SLabel = CL_Int;
                    ValLabele = CL_IntValue;
                }
                if (sender == CL_WidEditor)
                {
                    SLabel = CL_Wid;
                    ValLabele = CL_WidValue;
                }
                if (sender == CL_ChaEditor)
                {
                    SLabel = CL_Cha;
                    ValLabele = CL_ChaValue;
                }

                if ((sender as TextBox).Text == "") (sender as TextBox).Text = "1";
                (sender as TextBox).Visibility = Visibility.Collapsed;
                ValLabele.Content = (sender as TextBox).Text;
                SLabel.Visibility = Visibility.Visible;
                int Modify = 0;

                if (Int32.Parse((sender as TextBox).Text) >= 10)
                {
                    Modify = (Int32.Parse((sender as TextBox).Text) - 10) / 2;
                }
                else
                {
                    if (Int32.Parse((sender as TextBox).Text) > 0)
                    {
                        if (Int32.Parse((sender as TextBox).Text) == 9) Modify = -1;
                        if (Int32.Parse((sender as TextBox).Text) == 8) Modify = -1;
                        if (Int32.Parse((sender as TextBox).Text) == 7) Modify = -2;
                        if (Int32.Parse((sender as TextBox).Text) == 6) Modify = -2;
                        if (Int32.Parse((sender as TextBox).Text) == 5) Modify = -3;
                        if (Int32.Parse((sender as TextBox).Text) == 4) Modify = -3;
                        if (Int32.Parse((sender as TextBox).Text) == 3) Modify = -4;
                        if (Int32.Parse((sender as TextBox).Text) == 2) Modify = -4;
                        if (Int32.Parse((sender as TextBox).Text) == 1) Modify = -5;
                    }
                    else
                    {
                        SLabel.Content = Modify = -5;
                    }
                }

                if (Modify >= 1) SLabel.Content = "+" + Modify.ToString();
                if (Modify == 0) SLabel.Content = "_0";
                if (Modify < - -1) SLabel.Content = Modify.ToString();
            }
            CalculateHitBinus();
        }

        private void CL_StgEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void CL_Rase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateLabels();
            OpenEditor(sender, CL_RaseEdit);
        }

        public void OpenEditor(object sender, object Editor)
        {
            (Editor as TextBox).Text = (sender as Label).Content.ToString();
            (sender as Label).Visibility = Visibility.Collapsed;
            (Editor as TextBox).Visibility = Visibility.Visible;
        }

        public void EditorConfurm(KeyEventArgs ee, Object sender, Object Labeled)
        {

        }

        private void CL_RaseEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CL_RaseEdit.Text != "")
                {
                    CL_Rase.Content = (sender as TextBox).Text;
                    CL_Rase.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
                else
                {
                    CL_RaseEdit.Text = "Неизвестной расы";
                    CL_Rase.Content = (sender as TextBox).Text;
                    CL_Rase.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CL_Class_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateLabels();
            OpenEditor(sender, CL_ClassEditor);
        }

        private void CL_ClassEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CL_ClassEditor.Text != "")
                {
                    CL_Class.Content = (sender as TextBox).Text;
                    CL_Class.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
                else
                {
                    CL_ClassEditor.Text = "Безклассовый";
                    CL_Class.Content = (sender as TextBox).Text;
                    CL_Class.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CL_Age_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateLabels();
            OpenEditor(sender, CL_AgeEditor);
        }

        private void CL_AgeEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CL_Age.Content = (sender as TextBox).Text;
                CL_Age.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }

            String A = CL_AgeEditor.Text;
            if (A.Length > 1)
            {
                A = A.Remove(0, A.Length - 1);
                if (Int32.Parse(A) == 0) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 1) CL_AgePreffics.Content = "год";
                if (Int32.Parse(A) == 2) CL_AgePreffics.Content = "года";
                if (Int32.Parse(A) == 3) CL_AgePreffics.Content = "года";
                if (Int32.Parse(A) == 4) CL_AgePreffics.Content = "года";
                if (Int32.Parse(A) == 5) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 6) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 7) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 8) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 9) CL_AgePreffics.Content = "лет";
            }
            else
            {
                if (A != "")
                {
                    if (Int32.Parse(A) == 0) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 1) CL_AgePreffics.Content = "год";
                    if (Int32.Parse(A) == 2) CL_AgePreffics.Content = "года";
                    if (Int32.Parse(A) == 3) CL_AgePreffics.Content = "года";
                    if (Int32.Parse(A) == 4) CL_AgePreffics.Content = "года";
                    if (Int32.Parse(A) == 5) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 6) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 7) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 8) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 9) CL_AgePreffics.Content = "лет";
                }
                else
                {
                    CL_Age.Content = "1";
                    CL_AgePreffics.Content = "год";
                }
            }
        }

        private void CL_AgeEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        public void UpdateLabels()
        {
            CL_Health.Visibility = Visibility.Visible;
            CL_HealthDice.Visibility = Visibility.Collapsed;
            CL_Armor.Visibility = Visibility.Visible;
            CL_ArmorEditor.Visibility = Visibility.Collapsed;
            CL_Speed.Visibility = Visibility.Visible;
            CL_SpeedEditor.Visibility = Visibility.Collapsed;
            CL_CharName.Visibility = Visibility.Visible;
            CL_CharNameEdit.Visibility = Visibility.Collapsed;
            CL_Rase.Visibility = Visibility.Visible;
            CL_RaseEdit.Visibility = Visibility.Collapsed;
            CL_Class.Visibility = Visibility.Visible;
            CL_ClassEditor.Visibility = Visibility.Collapsed;
            CL_Age.Visibility = Visibility.Visible;
            CL_AgeEditor.Visibility = Visibility.Collapsed;
            
        }

        private void CL_HealthEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void CL_Armor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateLabels();
            OpenEditor(sender, CL_ArmorEditor);
        }

        private void CL_ArmorEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CL_ArmorEditor.Text != "")
                {
                    CL_Armor.Content = (sender as TextBox).Text;
                    CL_Armor.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
                else
                {
                    CL_ArmorEditor.Text = "5";
                    CL_Armor.Content = (sender as TextBox).Text;
                    CL_Armor.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CL_ArmorEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void CL_Speed_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateLabels();
            OpenEditor(sender, CL_SpeedEditor);
        }

        private void CL_SpeedEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CL_SpeedEditor.Text != "")
                {
                    CL_Speed.Content = (sender as TextBox).Text;
                    CL_Speed.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
                else
                {
                    CL_SpeedEditor.Text = "0";
                    CL_Speed.Content = (sender as TextBox).Text;
                    CL_Speed.Visibility = Visibility.Visible;
                    (sender as TextBox).Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CL_SpeedEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void CL_MB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CL_BMEditor.Text = (sender as Label).Content.ToString();
            (sender as Label).Visibility = Visibility.Collapsed;
            CL_BMEditor.Visibility = Visibility.Visible;
        }

        private void CL_BMEditor_DropDownClosed(object sender, EventArgs e)
        {
            CL_MB.Content = CL_BMEditor.Text;
            CL_MB.Visibility = Visibility.Visible;
            CL_BMEditor.Visibility = Visibility.Collapsed;
            CalculateHitBinus();
        }

        private void CL_ArmName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CL_ArmsChoise.Items.Count < 2)
            {
                for (int i = 0; i != CountOfFileLines(@"Bin\Items\Arms\Arms.dnd"); i++)
                {
                    if (ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i) == "<Arm>") CL_ArmsChoise.Items.Add(ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i + 3));
                }
            }
            CL_ArmName.Visibility = Visibility.Collapsed;
            CL_ArmsChoise.Visibility = Visibility.Visible;
        }

        private void CL_ArmsChoise_DropDownClosed(object sender, EventArgs e)
        {
            for (int i = 0; i != CountOfFileLines(@"Bin\Items\Arms\Arms.dnd"); i++)
            {
                if (ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i) == CL_ArmsChoise.Text)
                {
                    ArmInfo[ActiveWeappon, 0] = ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i - 2);
                    ArmInfo[ActiveWeappon, 1] = ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i - 1);
                    ArmInfo[ActiveWeappon, 2] = ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i);
                    ArmInfo[ActiveWeappon, 3] = ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i + 1);
                    ArmInfo[ActiveWeappon, 4] = ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i + 2);
                    ArmInfo[ActiveWeappon, 5] = ReadCertainLine(@"Bin\Items\Arms\Arms.dnd", i + 3);

                }
            }

            ChangeImage(ArmInfo[ActiveWeappon, 0] + ".png", CL_ArmIcon);
            if (ArmInfo[ActiveWeappon, 1] == "1") CL_ArmSkill.Content = "Простое оружие";
            if (ArmInfo[ActiveWeappon, 1] == "2") CL_ArmSkill.Content = "Простое дальнобойное";
            if (ArmInfo[ActiveWeappon, 1] == "3") CL_ArmSkill.Content = "Воинское оружие";
            if (ArmInfo[ActiveWeappon, 1] == "4") CL_ArmSkill.Content = "Воинское дальнобойное";
            CL_ArmName.Content = ArmInfo[ActiveWeappon, 2];
            CL_ArmType.Content = ArmInfo[ActiveWeappon, 4];
            if (ArmInfo[ActiveWeappon, 5] != "none")
            {
                CL_ArmAdditionInfo.ToolTip = ArmInfo[ActiveWeappon, 5];
                CL_ArmAdditionInfo.Visibility = Visibility.Visible;
            }
            else CL_ArmAdditionInfo.Visibility = Visibility.Hidden;

            CalculateHitBinus();
            CL_ArmName.Visibility = Visibility.Visible;
            CL_ArmsChoise.Visibility = Visibility.Collapsed;
        }

        private void CL_ArmFromDex_Click(object sender, RoutedEventArgs e)
        {
            if (CL_ArmFromStg.IsChecked == true) CL_ArmFromStg.IsChecked = false;
            CalculateHitBinus();
        }

        private void CL_ArmFromStg_Click(object sender, RoutedEventArgs e)
        {
            if (CL_ArmFromDex.IsChecked == true) CL_ArmFromDex.IsChecked = false;
            CalculateHitBinus();
        }

        public void CalculateHitBinus()
        {
            if (CL_ArmFromStg.IsChecked == true)
            {
                if (Int32.Parse(CL_Stg.Content.ToString()) != 0) CL_ArmDamage.Content = "Урон:" + ArmInfo[ActiveWeappon, 3] + CL_Stg.Content;
                else CL_ArmDamage.Content = "Урон:" + ArmInfo[ActiveWeappon, 3];
                int HHit = Int32.Parse(CL_Stg.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
                if (HHit < 0) CL_ArmHit.Content = "Бонус атаки: " + HHit;
                if (HHit == 0) CL_ArmHit.Content = "Бонус атаки: +" + HHit;
                if (HHit > 0) CL_ArmHit.Content = "Бонус атаки: +" + HHit;

            }
            else if (CL_ArmFromDex.IsChecked == true)
            {
                if (Int32.Parse(CL_Dex.Content.ToString()) != 0) CL_ArmDamage.Content = "Урон:" + ArmInfo[ActiveWeappon, 3] + CL_Dex.Content;
                else CL_ArmDamage.Content = "Урон:" + ArmInfo[ActiveWeappon, 3];
                int HHit = Int32.Parse(CL_Dex.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
                if (HHit < 0) CL_ArmHit.Content = "Бонус атаки: " + HHit;
                if (HHit == 0) CL_ArmHit.Content = "Бонус атаки: +" + HHit;
                if (HHit > 0) CL_ArmHit.Content = "Бонус атаки: +" + HHit;
            }
            else
            {
                if (Int32.Parse(CL_Stg.Content.ToString()) != 0) CL_ArmDamage.Content = "Урон:" + ArmInfo[ActiveWeappon, 3] + CL_Stg.Content;
                else CL_ArmDamage.Content = "Урон:" + ArmInfo[ActiveWeappon, 3];
                int HHit = Int32.Parse(CL_Stg.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
                if (HHit < 0) CL_ArmHit.Content = "Бонус атаки: " + HHit;
                if (HHit == 0) CL_ArmHit.Content = "Бонус атаки: +" + HHit;
                if (HHit > 0) CL_ArmHit.Content = "Бонус атаки: +" + HHit;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingWindow LW = new LoadingWindow();
            LW.Show();
            this.Visibility = Visibility.Collapsed;

            LoadProfiles();
            ArmInfo[0, 0] = "Arm01";
            ArmInfo[0, 1] = "1";
            ArmInfo[0, 2] = "Боевой посох";
            ArmInfo[0, 3] = "1d6";
            ArmInfo[0, 4] = "дробящий";
            ArmInfo[0, 5] = "Универсальное (1d8)";
            ArmInfo[1, 0] = "Arm01";
            ArmInfo[1, 1] = "1";
            ArmInfo[1, 2] = "Боевой посох";
            ArmInfo[1, 3] = "1d6";
            ArmInfo[1, 4] = "дробящий";
            ArmInfo[1, 5] = "Универсальное (1d8)";
            ArmInfo[2, 0] = "Arm01";
            ArmInfo[2, 1] = "1";
            ArmInfo[2, 2] = "Боевой посох";
            ArmInfo[2, 3] = "1d6";
            ArmInfo[2, 4] = "дробящий";
            ArmInfo[2, 5] = "Универсальное (1d8)";
            CL_ArmsChoise_DropDownClosed(sender, e);
            AddCellsMouses();
        }

        private void CL_FWeappon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CL_FWeappon.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            CL_SWeappon.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            CL_TWeappon.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
            if (sender == CL_FWeappon) CL_FWeappon.Background = new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));
            if (sender == CL_SWeappon) CL_SWeappon.Background = new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));
            if (sender == CL_TWeappon) CL_TWeappon.Background = new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));

            if (sender == CL_FWeappon) ActiveWeappon = 0;
            if (sender == CL_SWeappon) ActiveWeappon = 1;
            if (sender == CL_TWeappon) ActiveWeappon = 2;

            ChangeImage(ArmInfo[ActiveWeappon, 0] + ".png", CL_ArmIcon);
            if (ArmInfo[ActiveWeappon, 1] == "1") CL_ArmSkill.Content = "Простое оружие";
            if (ArmInfo[ActiveWeappon, 1] == "2") CL_ArmSkill.Content = "Простое дальнобойное";
            if (ArmInfo[ActiveWeappon, 1] == "3") CL_ArmSkill.Content = "Воинское оружие";
            if (ArmInfo[ActiveWeappon, 1] == "4") CL_ArmSkill.Content = "Воинское дальнобойное";
            CL_ArmName.Content = ArmInfo[ActiveWeappon, 2];
            CL_ArmType.Content = ArmInfo[ActiveWeappon, 4];
            if (ArmInfo[ActiveWeappon, 5] != "none")
            {
                CL_ArmAdditionInfo.ToolTip = ArmInfo[ActiveWeappon, 5];
                CL_ArmAdditionInfo.Visibility = Visibility.Visible;
            }
            else CL_ArmAdditionInfo.Visibility = Visibility.Hidden;

            CalculateHitBinus();
            CL_ArmName.Visibility = Visibility.Visible;
            CL_ArmsChoise.Visibility = Visibility.Collapsed;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (InventoryWindow != null)
            {
                InventoryWindow.Close();
                InventoryWindow = new CharInventory();
                InventoryWindow.Owner = this;
                InventoryWindow.Show();
            }
            else
            {
                InventoryWindow = new CharInventory();
                InventoryWindow.Owner = this;
                InventoryWindow.Title = "Инвентарь " + CL_CharName.Content;
                InventoryWindow.Show();
            }
        }

        public void SelectChar(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (DeletedChar != null)
                    if (DeletedChar != (sender as Label)) SaveChar();
                CL_CharSpace.Tag = "CURRENT";
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i].Contains("<PlayerChar>" + (sender as Label).Content.ToString()) == true)
                    {
                        CL_CharList.Width = 53;
                        CL_Chars.Visibility = Visibility.Hidden;
                        CL_CharsListLable.Visibility = Visibility.Hidden;
                        CL_CharList.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                        CL_More.Margin = new Thickness(0, 10, 0, 0);
                        CL_CharSpace.Visibility = Visibility.Visible;

                        int o = i;
                        while (LoadedProfile[o] != "<PlayerCharEnd>")
                        {
                            if (LoadedProfile[o].Contains("<PlayerChar>") == true)
                            {
                                CL_CharName.Content = LoadedProfile[o].Remove(0, 12);
                            }
                            if (LoadedProfile[o].Contains("<CharRase>") == true)
                            {
                                CL_Rase.Content = LoadedProfile[o].Remove(0, 10);
                            }
                            if (LoadedProfile[o].Contains("<CharClass>") == true)
                            {
                                CL_Class.Content = LoadedProfile[o].Remove(0, 11);
                            }
                            if (LoadedProfile[o].Contains("<CharAge>") == true)
                            {
                                int p = 0;
                                string Age = "";
                                String line = LoadedProfile[o].Remove(0, 9);
                                while (line[p] != ' ')
                                {
                                    Age = Age + line[p].ToString();
                                    p++;
                                }
                                CL_Age.Content = Age;
                                CL_AgePreffics.Content = line.Remove(0, line.IndexOf(Age) + Age.Length + 1);
                            }
                            if (LoadedProfile[o].Contains("<CharHP>") == true)
                            {
                                CL_Health.Content = LoadedProfile[o].Remove(0, 8);
                            }
                            if (LoadedProfile[o].Contains("<CharHPDiceCount>") == true)
                            {
                                CP_HitDiceCount.Text = LoadedProfile[o].Remove(0, 17);
                            }
                            if (LoadedProfile[o].Contains("<CharHPDice>") == true)
                            {
                                string Ttxt = LoadedProfile[o].Remove(0, 12);
                                int SelIndex = 0;
                                if (Ttxt == "d4") SelIndex = 0;
                                if (Ttxt == "d6") SelIndex = 1;
                                if (Ttxt == "d8") SelIndex = 2;
                                if (Ttxt == "d10") SelIndex = 3;
                                if (Ttxt == "d12") SelIndex = 4;
                                if (Ttxt == "d20") SelIndex = 5;
                                CP_HitDice.SelectedIndex = SelIndex;
                            }
                            if (LoadedProfile[o].Contains("<CharSpeed>") == true)
                            {
                                CL_Speed.Content = LoadedProfile[o].Remove(0, 11);
                            }
                            if (LoadedProfile[o].Contains("<CharAC>") == true)
                            {
                                CL_Armor.Content = LoadedProfile[o].Remove(0, 8);
                            }
                            if (LoadedProfile[o].Contains("<CharStat>") == true)
                            {
                                string line = LoadedProfile[o].Remove(0, 10);
                                int StatID = 1;
                                while (line.Length != 0)
                                {
                                    int p = 0;
                                    string stat = "";
                                    if (StatID != 6)
                                    {
                                        while (line[p] != ':')
                                        {
                                            stat = stat + line[p];
                                            p++;
                                        }
                                        if (StatID == 1)
                                        {
                                            CL_StgValue.Content = stat;
                                            CL_StgEditor.Text = stat;
                                        }
                                        if (StatID == 2)
                                        {
                                            CL_DexValue.Content = stat;
                                            CL_DexEditor.Text = stat;
                                        }
                                        if (StatID == 3)
                                        {
                                            CL_ConValue.Content = stat;
                                            CL_ConEditor.Text = stat;
                                        }
                                        if (StatID == 4)
                                        {
                                            CL_IntValue.Content = stat;
                                            CL_IntEditor.Text = stat;
                                        }
                                        if (StatID == 5)
                                        {
                                            CL_WidValue.Content = stat;
                                            CL_WidEditor.Text = stat;
                                        }
                                        line = line.Remove(0, line.IndexOf(stat) + stat.Length + 1);
                                        StatID++;
                                    }
                                    else if (StatID == 6)
                                    {
                                        CL_ChaValue.Content = line;
                                        CL_ChaEditor.Text = line;
                                        line = "";
                                    }
                                }
                            }
                            if (LoadedProfile[o].Contains("<CMasterBonus>") == true)
                            {
                                CL_MB.Content = LoadedProfile[o].Remove(0, 14);
                            }
                            o++;
                        }
                    }
                }

                TextBox[] PSender = new TextBox[] {CL_StgEditor,CL_DexEditor,CL_ConEditor,CL_IntEditor,CL_WidEditor,CL_ChaEditor };
                for (int i = 0; i != 6; i++)
                {
                    Label SLabel = null;
                    Label ValLabele = null;
                    if (PSender[i] == CL_StgEditor)
                    {
                        SLabel = CL_Stg;
                        ValLabele = CL_StgValue;
                    }
                    if (PSender[i] == CL_DexEditor)
                    {
                        SLabel = CL_Dex;
                        ValLabele = CL_DexValue;
                    }
                    if (PSender[i] == CL_ConEditor)
                    {
                        SLabel = CL_Con;
                        ValLabele = CL_ConValue;
                    }
                    if (PSender[i] == CL_IntEditor)
                    {
                        SLabel = CL_Int;
                        ValLabele = CL_IntValue;
                    }
                    if (PSender[i] == CL_WidEditor)
                    {
                        SLabel = CL_Wid;
                        ValLabele = CL_WidValue;
                    }
                    if (PSender[i] == CL_ChaEditor)
                    {
                        SLabel = CL_Cha;
                        ValLabele = CL_ChaValue;
                    }

                    if ((PSender[i] as TextBox).Text == "") (PSender[i] as TextBox).Text = "1";
                    (PSender[i] as TextBox).Visibility = Visibility.Collapsed;
                    ValLabele.Content = (PSender[i] as TextBox).Text;
                    SLabel.Visibility = Visibility.Visible;
                    int Modify = 0;

                    if (Int32.Parse((PSender[i] as TextBox).Text) >= 10)
                    {
                        Modify = (Int32.Parse((PSender[i] as TextBox).Text) - 10) / 2;
                    }
                    else
                    {
                        if (Int32.Parse((PSender[i] as TextBox).Text) > 0)
                        {
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 9) Modify = -1;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 8) Modify = -1;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 7) Modify = -2;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 6) Modify = -2;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 5) Modify = -3;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 4) Modify = -3;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 3) Modify = -4;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 2) Modify = -4;
                            if (Int32.Parse((PSender[i] as TextBox).Text) == 1) Modify = -5;
                        }
                        else
                        {
                            SLabel.Content = Modify = -5;
                        }
                    }

                    if (Modify >= 1) SLabel.Content = "+" + Modify.ToString();
                    if (Modify == 0) SLabel.Content = "_0";
                    if (Modify < - -1) SLabel.Content = Modify.ToString();
                }
                CalculateHitBinus();
                CL_SaveChar.Visibility = Visibility.Collapsed;
            }
            DeletedChar = (sender as Label);
        }

        public void DeleteChar(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i] == "<PlayerChar>" + DeletedChar.Content.ToString())
                {
                    int o = i;
                    while (LoadedProfile[o] != "<PlayerCharEnd>")
                    {
                        LoadedProfile[o] = nullstring;
                        o++;
                    }
                    LoadedProfile[o] = nullstring;
                }
            }
            CL_Chars.Children.Remove(DeletedChar);
            CL_CharSpace.Tag = "NEW";
        }

        public void SaveChar()
        {
            if (CL_CharSpace.Tag.ToString() == "CURRENT")
            {
                DeleteChar(null, null);
                String[] CharLines = new string[1000];
                CharLines[0] = "<PlayerChar>" + CL_CharName.Content.ToString();
                CharLines[1] = "<CharRase>" + CL_Rase.Content.ToString();
                CharLines[2] = "<CharClass>" + CL_Class.Content.ToString();
                CharLines[3] = "<CharAge>" + CL_Age.Content.ToString() + " " + CL_AgePreffics.Content.ToString();
                CharLines[4] = "<CharHP>" + CL_Health.Content.ToString();
                CharLines[5] = "<CharHPDiceCount>" + CP_HitDiceCount.Text.ToString();
                CharLines[6] = "<CharHPDice>" + CP_HitDice.Text.ToString();
                CharLines[7] = "<CharAC>" + CL_Armor.Content.ToString();
                CharLines[8] = "<CharSpeed>" + CL_Speed.Content.ToString();
                CharLines[9] = "<CharStat>" + CL_StgValue.Content.ToString() + ":" + CL_DexValue.Content.ToString() + ":" + CL_ConValue.Content.ToString() + ":" + CL_IntValue.Content.ToString() + ":" + CL_WidValue.Content.ToString() + ":" + CL_ChaValue.Content.ToString();
                CharLines[10] = "<CMasterBonus>" + CL_MB.Content.ToString();
                CharLines[11] = "<PlayerCharSpellDif>"+CLS_SpellDificultyLbl.Content.ToString();
                CharLines[12] = "<PlayerCharSpellAtt>"+CLS_SpellAttackLbl.Content.ToString();
                CharLines[13] = "<PlayerCharSpellStat>"+CLS_SpellStatLbl.Content.ToString();
                if (CLS_SpellLevel.SelectedIndex != 0)
                {
                    CharLines[14] = "<PlayerCharCasterLevel>"+ CLS_SpellLevel.SelectedIndex;
                }
                else
                {
                    CharLines[14] = "<PlayerCharCasterLevel>1";
                }
                if (CLS_ReadySpellDP.Children.Count == 0) CharLines[15] = "<PlayerCharSpellsReady>";
                else
                {
                    
                    CharLines[15] = "<PlayerCharSpellsReady>";
                    for (int i = 0; i != CLS_ReadySpellDP.Children.Count; i++)
                    {
                        CharLines[15] = CharLines[15] + (CLS_ReadySpellDP.Children[i] as Label).Content.ToString() + ":";
                    }
                }
                CharLines[16] = "<PlayerCharItems>";
                CharLines[17] = "<PlayerCharSkills>";
                CharLines[18] = "<PlayerCharEnd>";
                PasteLinesIntoProfile(CharLines, 1000);
                CL_Chars.Children.Clear();
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i].IndexOf("<PlayerChar>") != -1)
                    {
                        Label Chars = new Label();
                        CL_Chars.Children.Add(Chars);
                        Chars.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                        Chars.Background = new SolidColorBrush(Color.FromArgb(50, 76, 76, 149));
                        DockPanel.SetDock(Chars, Dock.Top);
                        Chars.HorizontalContentAlignment = HorizontalAlignment.Center;
                        Chars.HorizontalAlignment = HorizontalAlignment.Stretch;
                        Chars.Width = Double.NaN;
                        Chars.Content = LoadedProfile[i].Remove(0, 12);
                        Chars.Margin = new Thickness(5, 5, 5, 0);
                        Chars.MouseUp += SelectChar;
                        ContextMenu CM = new ContextMenu();
                        MenuItem MI = new MenuItem();
                        MI.Header = "Удалить";
                        MI.Click += DeleteChar;
                        CM.Items.Add(MI);
                        Chars.ContextMenu = CM;
                    }
                }

                Label NewChar = new Label();
                CL_Chars.Children.Add(NewChar);
                NewChar.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                NewChar.Background = new SolidColorBrush(Color.FromArgb(51, 162, 162, 162));
                DockPanel.SetDock(NewChar, Dock.Top);
                NewChar.HorizontalContentAlignment = HorizontalAlignment.Center;
                NewChar.HorizontalAlignment = HorizontalAlignment.Stretch;
                NewChar.Width = Double.NaN;
                NewChar.Content = "Новый персонаж";
                NewChar.Margin = new Thickness(5, 5, 5, 0);
                NewChar.MouseEnter += CL_NewChar_MouseEnter;
                NewChar.MouseLeave += CL_NewChar_MouseLeave;
                NewChar.MouseDown += CL_NewChar_MouseDown;
            }
        }

        private void ToGeneratorLbl4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            Pages.SelectedIndex = 4;
            CL_Chars.Children.Clear();
            CL_CharSpace.Visibility = Visibility.Collapsed;
            CL_CharList.Width = 260;
            CL_Chars.Visibility = Visibility.Visible;
            CL_CharList.Background = new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i].IndexOf("<PlayerChar>") != -1)
                {
                    Label Chars = new Label();
                    CL_Chars.Children.Add(Chars);
                    Chars.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                    Chars.Background = new SolidColorBrush(Color.FromArgb(50, 76, 76, 149));
                    DockPanel.SetDock(Chars, Dock.Top);
                    Chars.HorizontalContentAlignment = HorizontalAlignment.Center;
                    Chars.HorizontalAlignment = HorizontalAlignment.Stretch;
                    Chars.Width = Double.NaN;
                    Chars.Content = LoadedProfile[i].Remove(0, 12);
                    Chars.Margin = new Thickness(5, 5, 5, 0);
                    Chars.MouseUp += SelectChar;
                    ContextMenu CM = new ContextMenu();
                    MenuItem MI = new MenuItem();
                    MI.Header = "Удалить";
                    MI.Click += DeleteChar;
                    CM.Items.Add(MI);
                    Chars.ContextMenu = CM;
                }
            }

            Label NewChar = new Label();
            CL_Chars.Children.Add(NewChar);
            NewChar.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            NewChar.Background = new SolidColorBrush(Color.FromArgb(51, 162, 162, 162));
            DockPanel.SetDock(NewChar, Dock.Top);
            NewChar.HorizontalContentAlignment = HorizontalAlignment.Center;
            NewChar.HorizontalAlignment = HorizontalAlignment.Stretch;
            NewChar.Width = Double.NaN;
            NewChar.Content = "Новый персонаж";
            NewChar.Margin = new Thickness(5, 5, 5, 0);
            NewChar.MouseEnter += CL_NewChar_MouseEnter;
            NewChar.MouseLeave += CL_NewChar_MouseLeave;
            NewChar.MouseDown += CL_NewChar_MouseDown;
        }

        private void NPCG_NameLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NPCG_TextFrameLeft.Visibility = Visibility.Collapsed;
            NPCG_TextFrameRight.Visibility = Visibility.Collapsed;
            NPCG_NameLbl.Visibility = Visibility.Collapsed;
            NPCG_NameEdit.Visibility = Visibility.Visible;
            NPCG_GenerateNameB.Visibility = Visibility.Visible;
            NPCG_ConfurmNameB.Visibility = Visibility.Visible;
        }

        private void NPCG_GenerateName_MouseEnter(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(127, 44, 44, 144));
        }

        private void NPCG_GenerateName_MouseLeave(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
        }

        private void NPCG_GenerateName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(38, 44, 144, 144));
        }

        private void NPCG_GenerateName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(127, 44, 44, 144));
            Random Rnd = new Random();
            int EndLine = 0;
            String Way = @"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd";
            for (int i = 0; i != CountOfFileLines(Way); i++)
            {
                if (ReadCertainLine(Way, i) == "/Name")
                {
                    EndLine = i - 1;
                    break;
                }
            }
            NPCG_NameEdit.Text = ReadCertainLine(Way, Rnd.Next(0, EndLine));
        }

        private void NPCG_RaseLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_RaseEdit.Visibility = Visibility.Visible;
        }

        private void NPCG_RaseEdit_DropDownClosed(object sender, EventArgs e)
        {
            (sender as ComboBox).Visibility = Visibility.Collapsed;
            NPCG_RaseLbl.Visibility = Visibility.Visible;
            NPCG_RaseLbl.Content = (sender as ComboBox).Text;

            NPCG_SubRaseEdit.Items.Clear();
            if (NPCG_RaseLbl.Content.ToString() == "Ааракокра") NPCG_SubRaseEdit.Items.Add("Ааракокра");
            if (NPCG_RaseLbl.Content.ToString() == "Гном")
            {
                NPCG_SubRaseEdit.Items.Add("Лесной гном");
                NPCG_SubRaseEdit.Items.Add("Скальный гном");
            }
            if (NPCG_RaseLbl.Content.ToString() == "Голиаф") NPCG_SubRaseEdit.Items.Add("Голиаф");
            if (NPCG_RaseLbl.Content.ToString() == "Дварф")
            {
                NPCG_SubRaseEdit.Items.Add("Горный дварф");
                NPCG_SubRaseEdit.Items.Add("Холмовой дварф");
            }
            if (NPCG_RaseLbl.Content.ToString() == "Дженази")
            {
                NPCG_SubRaseEdit.Items.Add("Дженази воды");
                NPCG_SubRaseEdit.Items.Add("Дженази воздуха");
                NPCG_SubRaseEdit.Items.Add("Дженази земли");
                NPCG_SubRaseEdit.Items.Add("Дженази огня");
            }
            if (NPCG_RaseLbl.Content.ToString() == "Драконорожденный")
            {
                NPCG_SubRaseEdit.Items.Add("Белый");
                NPCG_SubRaseEdit.Items.Add("Бронзовый");
                NPCG_SubRaseEdit.Items.Add("Зеленый");
                NPCG_SubRaseEdit.Items.Add("Золотой");
                NPCG_SubRaseEdit.Items.Add("Красный");
                NPCG_SubRaseEdit.Items.Add("Латунный");
                NPCG_SubRaseEdit.Items.Add("Медный");
                NPCG_SubRaseEdit.Items.Add("Серебряный");
                NPCG_SubRaseEdit.Items.Add("Синий");
                NPCG_SubRaseEdit.Items.Add("Черный");
            }
            if (NPCG_RaseLbl.Content.ToString() == "Полуорк") NPCG_SubRaseEdit.Items.Add("Полуорк");
            if (NPCG_RaseLbl.Content.ToString() == "Полурослик")
            {
                NPCG_SubRaseEdit.Items.Add("Коренастый");
                NPCG_SubRaseEdit.Items.Add("Легконогий");
            }
            if (NPCG_RaseLbl.Content.ToString() == "Полуэльф") NPCG_SubRaseEdit.Items.Add("Полуэльф");
            if (NPCG_RaseLbl.Content.ToString() == "Тифлинг") NPCG_SubRaseEdit.Items.Add("Тифлинг");
            if (NPCG_RaseLbl.Content.ToString() == "Человек") NPCG_SubRaseEdit.Items.Add("Человек");
            if (NPCG_RaseLbl.Content.ToString() == "Эльф")
            {
                NPCG_SubRaseEdit.Items.Add("Высший");
                NPCG_SubRaseEdit.Items.Add("Лесной");
                NPCG_SubRaseEdit.Items.Add("Тёмный");
            }

            NPCG_SubRaseDP.Visibility = Visibility.Visible;

            if (NPCG_RaseLbl.Content.ToString() == "Ааракокра") NPCG_SubRaseDP.Visibility = Visibility.Collapsed;
            if (NPCG_RaseLbl.Content.ToString() == "Голиаф") NPCG_SubRaseDP.Visibility = Visibility.Collapsed;
            if (NPCG_RaseLbl.Content.ToString() == "Полуорк") NPCG_SubRaseDP.Visibility = Visibility.Collapsed;
            if (NPCG_RaseLbl.Content.ToString() == "Полуэльф") NPCG_SubRaseDP.Visibility = Visibility.Collapsed;
            if (NPCG_RaseLbl.Content.ToString() == "Тифлинг") NPCG_SubRaseDP.Visibility = Visibility.Collapsed;
            if (NPCG_RaseLbl.Content.ToString() == "Человек") NPCG_SubRaseDP.Visibility = Visibility.Collapsed;

            NPCG_SubRaseEdit.SelectedIndex = 0;
            NPCG_SubRaeLbl.Content = NPCG_SubRaseEdit.Text;
        }

        private void NPCG_SubRaeLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_SubRaseEdit.Visibility = Visibility.Visible;
        }

        private void NPCG_SubRaseEdit_DropDownClosed(object sender, EventArgs e)
        {
            (sender as ComboBox).Visibility = Visibility.Collapsed;
            NPCG_SubRaeLbl.Visibility = Visibility.Visible;
            NPCG_SubRaeLbl.Content = (sender as ComboBox).Text;
        }

        private void NPCG_ClassLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_ClassEdit.Visibility = Visibility.Visible;
        }

        private void NPCG_ClassEdit_DropDownClosed(object sender, EventArgs e)
        {
            (sender as ComboBox).Visibility = Visibility.Collapsed;
            NPCG_ClassLbl.Visibility = Visibility.Visible;
            NPCG_ClassLbl.Content = (sender as ComboBox).Text;
            NPCG_SubClassDP.Visibility = Visibility.Collapsed;
            NPCG_SubClassEdit.Items.Clear();
            SelectSubclass();

            if (NPCG_ClassLbl.Content.ToString() == "Бард") ChangeImage("Bard.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Варвар") ChangeImage("Barbarian.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Воин") ChangeImage("Warrior.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Волшебник") ChangeImage("Wizzard.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Друид") ChangeImage("Druid.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Жрец") ChangeImage("Cleric.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Колдун") ChangeImage("Warlock.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Монах") ChangeImage("Monk.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Паладин") ChangeImage("Paladin.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Плут") ChangeImage("Rouge.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Следопыт") ChangeImage("Ranger.png", NPCG_ClassImage);
            if (NPCG_ClassLbl.Content.ToString() == "Чародей") ChangeImage("Sorcerer.png", NPCG_ClassImage);
        }
        //TyTb
        public void SelectSubclass()
        {
            String SLevel = NPCG_LevelLbl.Content.ToString();
            SLevel = SLevel.Remove(SLevel.IndexOf('-'), SLevel.Length - SLevel.IndexOf('-'));
            int Level = Int32.Parse(SLevel);
            NPCG_SubClassDP.Visibility = Visibility.Visible;

            if (NPCG_ClassLbl.Content.ToString() == "Бард") LoadSubclasses("Бард", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Варвар") LoadSubclasses("Варвар", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Воин") LoadSubclasses("Воин", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Волшебник") LoadSubclasses("Волшебник", Level, 2);
            if (NPCG_ClassLbl.Content.ToString() == "Друид") LoadSubclasses("Друид", Level, 2);
            if (NPCG_ClassLbl.Content.ToString() == "Жрец") LoadSubclasses("Жрец", Level, 1);
            if (NPCG_ClassLbl.Content.ToString() == "Колдун") LoadSubclasses("Колдун", Level, 1);
            if (NPCG_ClassLbl.Content.ToString() == "Монах") LoadSubclasses("Монах", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Паладин") LoadSubclasses("Паладин", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Плут") LoadSubclasses("Плут", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Следопыт") LoadSubclasses("Следопыт", Level, 3);
            if (NPCG_ClassLbl.Content.ToString() == "Чародей") LoadSubclasses("Чародей", Level, 1);
        }

        public void LoadSubclasses(String Class, int Level, int MinLevel)
        {
            String CustomText = "";
            if (MinLevel <= Level)
            {
                for (int i = 0; i != CountOfFileLines(@"Bin\Class\Subclasses.dnd"); i++)
                {
                    if (ReadCertainLine(@"Bin\Class\Subclasses.dnd", i) == "<" + Class + ">")
                    {
                        int wi = i + 1;
                        while (ReadCertainLine(@"Bin\Class\Subclasses.dnd", wi).IndexOf("<") == -1)
                        {
                            NPCG_SubClassEdit.Items.Add(ReadCertainLine(@"Bin\Class\Subclasses.dnd", wi));
                            if (CustomText == "") CustomText = ReadCertainLine(@"Bin\Class\Subclasses.dnd", wi);
                            wi++;
                        }
                        break;
                    }
                }
                NPCG_SubClassLbl.Content = CustomText;
                NPCG_SubClassEdit.SelectedIndex = 0;
            }
            else NPCG_SubClassDP.Visibility = Visibility.Collapsed;
        }

        private void NPCG_LevelLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_LevelEdit.Visibility = Visibility.Visible;
        }

        private void NPCG_LevelEdit_DropDownClosed(object sender, EventArgs e)
        {
            (sender as ComboBox).Visibility = Visibility.Collapsed;
            NPCG_LevelLbl.Visibility = Visibility.Visible;
            NPCG_LevelLbl.Content = (sender as ComboBox).Text + " - уровня";
            NPCG_SubClassDP.Visibility = Visibility.Collapsed;
            NPCG_SubClassEdit.Items.Clear();
            SelectSubclass();
        }

        private void NPCG_SubClassLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_SubClassEdit.Visibility = Visibility.Visible;
            NPCG_SubClassEdit.SelectedIndex = 0;
        }

        private void NPCG_SubClassEdit_DropDownClosed(object sender, EventArgs e)
        {
            (sender as ComboBox).Visibility = Visibility.Collapsed;
            NPCG_SubClassLbl.Visibility = Visibility.Visible;
            NPCG_SubClassLbl.Content = (sender as ComboBox).Text;
        }

        private void NPCG_ConfurmName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(127, 44, 44, 144));
            if (NPCG_NameEdit.Text != "") NPCG_NameLbl.Content = NPCG_NameEdit.Text;
            else NPCG_NameLbl.Content = "Безимянный";

            NPCG_TextFrameLeft.Visibility = Visibility.Visible;
            NPCG_TextFrameRight.Visibility = Visibility.Visible;
            NPCG_NameLbl.Visibility = Visibility.Visible;
            NPCG_NameEdit.Visibility = Visibility.Collapsed;
            NPCG_GenerateNameB.Visibility = Visibility.Collapsed;
            NPCG_ConfurmNameB.Visibility = Visibility.Collapsed;
        }

        private void NPCG_StatEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void NPCG_StatEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetStat();
            }
        }

        private void NPCG_ConfurmStat_MouseEnter(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(127, 44, 44, 144));
        }

        private void NPCG_ConfurmStat_MouseLeave(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
        }

        private void NPCG_ConfurmStat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(38, 44, 144, 144));
        }

        private void NPCG_ConfurmStat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetStat();
        }

        public void SetStat()
        {
            Label StatLbl = null;
            if (StatID == "1") StatLbl = NPCG_Stg;
            if (StatID == "2") StatLbl = NPCG_Dex;
            if (StatID == "3") StatLbl = NPCG_Con;
            if (StatID == "4") StatLbl = NPCG_Int;
            if (StatID == "5") StatLbl = NPCG_Wid;
            if (StatID == "6") StatLbl = NPCG_Cha;
            if (NPCG_StatEdit.Text == "") NPCG_StatEdit.Text = "1";
            if (Int32.Parse(NPCG_StatEdit.Text) <= 0) NPCG_StatEdit.Text = "1";
            if (Int32.Parse(NPCG_StatEdit.Text) > 209) NPCG_StatEdit.Text = "209";
            StatLbl.Content = NPCG_StatEdit.Text;
            RecalculateModify();
            NPCG_StatEditGrid.Visibility = Visibility.Collapsed;
        }

        public void RecalculateModify()
        {
            Label[] MStatsLbl = new Label[] { NPCG_ModStg, NPCG_ModDex, NPCG_ModCon, NPCG_ModInt, NPCG_ModWid, NPCG_ModCha };
            Label[] StatsLbl = new Label[] { NPCG_Stg, NPCG_Dex, NPCG_Con, NPCG_Int, NPCG_Wid, NPCG_Cha };

            for (int i = 0; i != 6; i++)
            {
                MStatsLbl[i].Content = ReturnedModificator(Int32.Parse(StatsLbl[i].Content.ToString()));
            }
        }

        private void NPCG_ModStg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Grid ParentGrid = null;
            Label ModLabel = null;

            if ((sender as Label).Tag.ToString() == "1") ParentGrid = NPCG_GridStg;
            if ((sender as Label).Tag.ToString() == "2") ParentGrid = NPCG_GridDex;
            if ((sender as Label).Tag.ToString() == "3") ParentGrid = NPCG_GridCon;
            if ((sender as Label).Tag.ToString() == "4") ParentGrid = NPCG_GridInt;
            if ((sender as Label).Tag.ToString() == "5") ParentGrid = NPCG_GridWid;
            if ((sender as Label).Tag.ToString() == "6") ParentGrid = NPCG_GridCha;

            if ((sender as Label).Tag.ToString() == "1") ModLabel = NPCG_Stg;
            if ((sender as Label).Tag.ToString() == "2") ModLabel = NPCG_Dex;
            if ((sender as Label).Tag.ToString() == "3") ModLabel = NPCG_Con;
            if ((sender as Label).Tag.ToString() == "4") ModLabel = NPCG_Int;
            if ((sender as Label).Tag.ToString() == "5") ModLabel = NPCG_Wid;
            if ((sender as Label).Tag.ToString() == "6") ModLabel = NPCG_Cha;

            StatID = (sender as Label).Tag.ToString();
            NPCG_StatEditGrid.Visibility = Visibility.Visible;
            NPCG_StatEditGrid.Margin = ParentGrid.Margin;
            NPCG_StatEdit.Text = ModLabel.Content.ToString();
            Console.WriteLine((sender as Label).Tag.ToString());
        }

        private void NPCG_MasterBonus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_MasterBonusEdit.Visibility = Visibility.Visible;
            NPCG_MasterBonusEdit.Text = (sender as Label).Content.ToString().Remove(0, 1);
        }

        private void NPCG_MasterBonusEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void NPCG_MasterBonusEdit_KeyUp(object sender, KeyEventArgs e)
        {
            int MB = 0;
            if (e.Key == Key.Enter)
            {
                if (Int32.Parse(NPCG_MasterBonusEdit.Text) > 6) MB = 6;
                else if (Int32.Parse(NPCG_MasterBonusEdit.Text) == 1) MB = 2;
                else MB = Int32.Parse(NPCG_MasterBonusEdit.Text);
                NPCG_MasterBonus.Content = "+" + MB.ToString();
                (sender as TextBox).Visibility = Visibility.Collapsed;
                NPCG_MasterBonus.Visibility = Visibility.Visible;
            }
        }

        private void NPCG_HPedit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NPCG_HPLabel.Visibility = Visibility.Visible;
                NPCG_HPedit.Visibility = Visibility.Collapsed;
                NPCG_HPLabel.Content = NPCG_HPedit.Text;
            }
        }

        private void NPCG_HPLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_HPedit.Visibility = Visibility.Visible;
            NPCG_HPedit.Text = (sender as Label).Content.ToString();
        }

        private void NPCG_ACLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_ACEdit.Visibility = Visibility.Visible;
            NPCG_ACEdit.Text = (sender as Label).Content.ToString();
        }

        private void NPCG_ACEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NPCG_ACLabel.Visibility = Visibility.Visible;
                NPCG_ACEdit.Visibility = Visibility.Collapsed;
                NPCG_ACLabel.Content = NPCG_ACEdit.Text;
            }
        }

        private void NPCG_SpeedLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (sender as Label).Visibility = Visibility.Collapsed;
            NPCG_SpeedEdit.Visibility = Visibility.Visible;
            NPCG_SpeedEdit.Text = (sender as Label).Content.ToString();
        }

        private void NPCG_SpeedEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NPCG_SpeedLabel.Visibility = Visibility.Visible;
                NPCG_SpeedEdit.Visibility = Visibility.Collapsed;
                String SpeedNew = "";
                SpeedNew = NPCG_SpeedEdit.Text;
                if (Int32.Parse(NPCG_SpeedEdit.Text) < 10) SpeedNew = "10";
                if (Int32.Parse(NPCG_SpeedEdit.Text) > 99) SpeedNew = "99";
                NPCG_SpeedLabel.Content = SpeedNew;
            }
        }

        private void NPCG_Generate_MouseEnter(object sender, MouseEventArgs e)
        {
            NPCG_GenerateBorder.Background = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255));
        }

        private void NPCG_Generate_MouseLeave(object sender, MouseEventArgs e)
        {
            NPCG_GenerateBorder.Background = new SolidColorBrush(Color.FromArgb(84, 255, 255, 255));
        }

        private void NPCG_Generate_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HPGenerateModalWindow ModalWindow = new HPGenerateModalWindow();
            ModalWindow.Owner = this;
            ModalWindow.ShowDialog();
        }

        public void GenerateHitPoints()
        {
            String S = NPCG_LevelLbl.Content.ToString();
            S = S.Remove(S.IndexOf('-'), S.Length - S.IndexOf('-'));
            int LevelM = Int32.Parse(S);
            int ConMod = Int32.Parse(NPCG_ModCon.Content.ToString());

            if (NPCG_ClassLbl.Content.ToString() == "Бард") ClassID = 1;
            if (NPCG_ClassLbl.Content.ToString() == "Варвар") ClassID = 2;
            if (NPCG_ClassLbl.Content.ToString() == "Воин") ClassID = 3;
            if (NPCG_ClassLbl.Content.ToString() == "Волшебник") ClassID = 4;
            if (NPCG_ClassLbl.Content.ToString() == "Друид") ClassID = 5;
            if (NPCG_ClassLbl.Content.ToString() == "Жрец") ClassID = 6;
            if (NPCG_ClassLbl.Content.ToString() == "Колдун") ClassID = 7;
            if (NPCG_ClassLbl.Content.ToString() == "Монах") ClassID = 8;
            if (NPCG_ClassLbl.Content.ToString() == "Паладин") ClassID = 9;
            if (NPCG_ClassLbl.Content.ToString() == "Плут") ClassID = 10;
            if (NPCG_ClassLbl.Content.ToString() == "Следопыт") ClassID = 11;
            if (NPCG_ClassLbl.Content.ToString() == "Чародей") ClassID = 12;

            if (NPCG_SubRaseDP.Visibility == Visibility.Visible)
            {
                if (NPCG_SubRaeLbl.Content.ToString() == "Лесной гном") RaseID = 21;
                if (NPCG_SubRaeLbl.Content.ToString() == "Скальный гном") RaseID = 22;
                if (NPCG_SubRaeLbl.Content.ToString() == "Горный дварф") RaseID = 41;
                if (NPCG_SubRaeLbl.Content.ToString() == "Холмовой дварф") RaseID = 42;
                if (NPCG_SubRaeLbl.Content.ToString() == "Дженази воды") RaseID = 51;
                if (NPCG_SubRaeLbl.Content.ToString() == "Дженази воздуха") RaseID = 52;
                if (NPCG_SubRaeLbl.Content.ToString() == "Дженази земли") RaseID = 53;
                if (NPCG_SubRaeLbl.Content.ToString() == "Дженази огня") RaseID = 54;
                if (NPCG_SubRaeLbl.Content.ToString() == "Белый") RaseID = 61;
                if (NPCG_SubRaeLbl.Content.ToString() == "Бронзовый") RaseID = 62;
                if (NPCG_SubRaeLbl.Content.ToString() == "Зеленый") RaseID = 63;
                if (NPCG_SubRaeLbl.Content.ToString() == "Золотой") RaseID = 64;
                if (NPCG_SubRaeLbl.Content.ToString() == "Красный") RaseID = 65;
                if (NPCG_SubRaeLbl.Content.ToString() == "Латунный") RaseID = 66;
                if (NPCG_SubRaeLbl.Content.ToString() == "Медный") RaseID = 67;
                if (NPCG_SubRaeLbl.Content.ToString() == "Серебряный") RaseID = 68;
                if (NPCG_SubRaeLbl.Content.ToString() == "Синий") RaseID = 69;
                if (NPCG_SubRaeLbl.Content.ToString() == "Черный") RaseID = 610;
                if (NPCG_SubRaeLbl.Content.ToString() == "Коренастый") RaseID = 81;
                if (NPCG_SubRaeLbl.Content.ToString() == "Легконогий") RaseID = 82;
                if (NPCG_SubRaeLbl.Content.ToString() == "Высший") RaseID = 121;
                if (NPCG_SubRaeLbl.Content.ToString() == "Лесной") RaseID = 122;
                if (NPCG_SubRaeLbl.Content.ToString() == "Тёмный") RaseID = 123;
            }
            else
            {
                if (NPCG_RaseLbl.Content.ToString() == "Ааракокра") RaseID = 1;
                if (NPCG_RaseLbl.Content.ToString() == "Голиаф") RaseID = 3;
                if (NPCG_RaseLbl.Content.ToString() == "Полуорк") RaseID = 7;
                if (NPCG_RaseLbl.Content.ToString() == "Полуэльф") RaseID = 9;
                if (NPCG_RaseLbl.Content.ToString() == "Тифлинг") RaseID = 10;
                if (NPCG_RaseLbl.Content.ToString() == "Человек") RaseID = 11;
            }

            int DiceValue = 0;
            if (ClassID == 1) DiceValue = 8;
            if (ClassID == 2) DiceValue = 12;
            if (ClassID == 3) DiceValue = 10;
            if (ClassID == 4) DiceValue = 6;
            if (ClassID == 5) DiceValue = 8;
            if (ClassID == 6) DiceValue = 8;
            if (ClassID == 7) DiceValue = 8;
            if (ClassID == 8) DiceValue = 8;
            if (ClassID == 9) DiceValue = 10;
            if (ClassID == 10) DiceValue = 8;
            if (ClassID == 11) DiceValue = 10;
            if (ClassID == 12) DiceValue = 6;

            if (HPGenerateType == 2)
            {
                int MiddleRoll = 0;
                if (DiceValue == 6) MiddleRoll = 4;
                if (DiceValue == 8) MiddleRoll = 5;
                if (DiceValue == 10) MiddleRoll = 6;
                if (DiceValue == 12) MiddleRoll = 7;

                if (LevelM == 1)
                {
                    NPCG_HPLabel.Content = (DiceValue + ConMod).ToString();
                }
                else
                {
                    int HP = DiceValue + ConMod;
                    HP = HP + ((MiddleRoll + ConMod) * (LevelM - 1));
                    NPCG_HPLabel.Content = HP.ToString();
                }
            }
            if (HPGenerateType == 1)
            {
                Random Rnd = new Random();

                if (LevelM == 1)
                {
                    NPCG_HPLabel.Content = (DiceValue + ConMod).ToString();
                }
                else
                {
                    int HP = DiceValue + ConMod;
                    for (int i = 1; i != LevelM; i++)
                    {
                        HP = HP + Rnd.Next(1, DiceValue) + ConMod;
                    }
                    NPCG_HPLabel.Content = HP.ToString();
                }
            }

            LoadUses(ClassID);
            LoadBaseSpeed();

            String Way = @"Bin\Class\";
            if (ClassID == 1) Way = Way + "Bard.dnd";
            if (ClassID == 2) Way = Way + "Barbarian.dnd";
            if (ClassID == 3) Way = Way + "Fighter.dnd";
            if (ClassID == 4) Way = Way + "Wizard.dnd";
            if (ClassID == 5) Way = Way + "Druid.dnd";
            if (ClassID == 6) Way = Way + "Cleric.dnd";
            if (ClassID == 7) Way = Way + "Warlock.dnd";
            if (ClassID == 8) Way = Way + "Monk.dnd";
            if (ClassID == 9) Way = Way + "Paladin.dnd";
            if (ClassID == 10) Way = Way + "Rogue.dnd";
            if (ClassID == 11) Way = Way + "Ranger.dnd";
            if (ClassID == 12) Way = Way + "Sorcerer.dnd";

            LoadSkills();
            int WayID = 0;
            if (NPCG_SubClassDP.Visibility == Visibility.Visible) WayID = NPCG_SubClassEdit.SelectedIndex + 1;
            GenerateClass(LevelM, WayID, Way);
        }

        public void LoadUses(int ClassID)
        {
            String Way = @"Bin\Class\";
            if (ClassID == 1) Way = Way + "Bard.dnd";
            if (ClassID == 2) Way = Way + "Barbarian.dnd";
            if (ClassID == 3) Way = Way + "Fighter.dnd";
            if (ClassID == 4) Way = Way + "Wizard.dnd";
            if (ClassID == 5) Way = Way + "Druid.dnd";
            if (ClassID == 6) Way = Way + "Cleric.dnd";
            if (ClassID == 7) Way = Way + "Warlock.dnd";
            if (ClassID == 8) Way = Way + "Monk.dnd";
            if (ClassID == 9) Way = Way + "Paladin.dnd";
            if (ClassID == 10) Way = Way + "Rogue.dnd";
            if (ClassID == 11) Way = Way + "Ranger.dnd";
            if (ClassID == 12) Way = Way + "Sorcerer.dnd";

            NPCG_UsedDP.Children.Clear();

            CreateField(NPCG_UsedDP, "Владения:", Color.FromArgb(25, 255, 255, 255), 18, HorizontalAlignment.Center);

            for (int i = 0; i != CountOfFileLines(@"Bin\Rase\" + NPCG_SubRaeLbl.Content + ".dnd"); i++)
            {
                if (ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content + ".dnd", i) == "/Use") CreateField(NPCG_UsedDP, ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content + ".dnd", i + 1), Color.FromArgb(25, 0, 0, 0), 12, HorizontalAlignment.Left);
            }
            for (int i = 0; i != CountOfFileLines(Way); i++)
            {
                if (ReadCertainLine(Way, i) == "/Use") CreateField(NPCG_UsedDP, ReadCertainLine(Way, i + 1), Color.FromArgb(25, 0, 0, 0), 12, HorizontalAlignment.Left);
            }
            NPCG_UsedDP.Height = ((NPCG_UsedDP.Children.Count - 1) * 25) + 32;
        }

        public void LoadBaseSpeed()
        {
            NPCG_FlySpeed.Content = "0";
            for (int i = 0; i != CountOfFileLines(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd"); i++)
            {
                if (ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd", i) == "/Speed") NPCG_SpeedLabel.Content = ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd", i + 1);
                if (ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd", i) == "/Fly")
                {
                    NPCG_FlySpeed.Content = ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd", i + 1);
                    NPCG_SF.Content = "Скорость   Полёт";
                }
                if (ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd", i) == "/Swim")
                {
                    NPCG_FlySpeed.Content = ReadCertainLine(@"Bin\Rase\" + NPCG_SubRaeLbl.Content.ToString() + ".dnd", i + 1);
                    NPCG_SF.Content = "Скорость Плаванье";
                }
            }
        }

        public void LoadSkills()
        {
            String RaseWay = "";
            if (RaseID == 21) RaseWay = @"Bin\Rase\Лесной гном.dnd";
            if (RaseID == 22) RaseWay = @"Bin\Rase\Скальный гном.dnd";
            if (RaseID == 41) RaseWay = @"Bin\Rase\Горный дварф.dnd";
            if (RaseID == 42) RaseWay = @"Bin\Rase\Холмовой дварф.dnd";
            if (RaseID == 51) RaseWay = @"Bin\Rase\Дженази воды.dnd";
            if (RaseID == 52) RaseWay = @"Bin\Rase\Дженази воздуха.dnd";
            if (RaseID == 53) RaseWay = @"Bin\Rase\Дженази земли.dnd";
            if (RaseID == 54) RaseWay = @"Bin\Rase\Дженази огня.dnd";
            if (RaseID == 61) RaseWay = @"Bin\Rase\Белый.dnd";
            if (RaseID == 62) RaseWay = @"Bin\Rase\Бронзовый.dnd";
            if (RaseID == 63) RaseWay = @"Bin\Rase\Зеленый.dnd";
            if (RaseID == 64) RaseWay = @"Bin\Rase\Золотой.dnd";
            if (RaseID == 65) RaseWay = @"Bin\Rase\Красный.dnd";
            if (RaseID == 66) RaseWay = @"Bin\Rase\Латунный.dnd";
            if (RaseID == 67) RaseWay = @"Bin\Rase\Медный.dnd";
            if (RaseID == 68) RaseWay = @"Bin\Rase\Серебряный.dnd";
            if (RaseID == 69) RaseWay = @"Bin\Rase\Синий.dnd";
            if (RaseID == 610) RaseWay = @"Bin\Rase\Черный.dnd";
            if (RaseID == 81) RaseWay = @"Bin\Rase\Коренастый.dnd";
            if (RaseID == 82) RaseWay = @"Bin\Rase\Легконогий.dnd";
            if (RaseID == 121) RaseWay = @"Bin\Rase\Высший.dnd";
            if (RaseID == 122) RaseWay = @"Bin\Rase\Лесной.dnd";
            if (RaseID == 123) RaseWay = @"Bin\Rase\Тёмный.dnd";
            if (RaseID == 1) RaseWay = @"Bin\Rase\Ааракокра.dnd";
            if (RaseID == 3) RaseWay = @"Bin\Rase\Голиаф.dnd";
            if (RaseID == 7) RaseWay = @"Bin\Rase\Полуорк.dnd";
            if (RaseID == 9) RaseWay = @"Bin\Rase\Полуэльф.dnd";
            if (RaseID == 10) RaseWay = @"Bin\Rase\Тифлинг.dnd";
            if (RaseID == 11) RaseWay = @"Bin\Rase\Человек.dnd";

            NPCG_SkillDP.Children.Clear();
            CreateField(NPCG_SkillDP, "Рассовые умения:", Color.FromArgb(25, 255, 255, 255), 18, HorizontalAlignment.Center);
            for (int i = 0; i != CountOfFileLines(RaseWay); i++)
            {
                if (ReadCertainLine(RaseWay, i).IndexOf("<Skill>") != -1) CreateField(NPCG_SkillDP, ReadCertainLine(RaseWay, i).Remove(0, 7), Color.FromArgb(25, 0, 0, 0), 12, HorizontalAlignment.Left);
            }
            NPCG_SkillDP.Height = ((NPCG_SkillDP.Children.Count - 1) * 25) + 32;
        }

        public void CreateField(DockPanel Parent, String Fieldtext, Color Background, int FontSize, HorizontalAlignment HCA)
        {
            Label CreatbleLabel = new Label();
            Parent.Children.Add(CreatbleLabel);
            CreatbleLabel.Content = Fieldtext;
            DockPanel.SetDock(CreatbleLabel, Dock.Top);
            CreatbleLabel.FlowDirection = FlowDirection.LeftToRight;
            CreatbleLabel.Background = new SolidColorBrush(Background);
            CreatbleLabel.Foreground = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            CreatbleLabel.FontFamily = new FontFamily("Book Antiqua");
            CreatbleLabel.FontSize = FontSize;
            CreatbleLabel.HorizontalContentAlignment = HCA;
            CreatbleLabel.ToolTip = Fieldtext;
            if (Fieldtext == "Владения:")
            {
                CreatbleLabel.MouseUp += NPCG_UsesLabel_MouseUp;
                CreatbleLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            if (Fieldtext == "Рассовые умения:")
            {
                CreatbleLabel.MouseUp += NPCG_SkillsLbl_MouseUp;
                CreatbleLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            if (Fieldtext == "Классовые умения:")
            {
                CreatbleLabel.MouseUp += NPCG_SkillsLbl1_MouseUp;
                CreatbleLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
            if (Parent == NPCG_Abilities)
                if (Fieldtext != "Классовые умения:") CreatbleLabel.MouseUp += AbilityClick;
            if (Parent == NPCG_SkillDP)
                if (Fieldtext != "Рассовые умения:") CreatbleLabel.MouseUp += Skill_Click;
        }

        public void GenerateClass(int Level, int WayID, String ClassWay)
        {
            NPCG_Abilities.Children.Clear();
            CreateField(NPCG_Abilities, "Классовые умения:", Color.FromArgb(25, 255, 255, 255), 18, HorizontalAlignment.Center);

            String Way = ClassWay;
            for (int i = 1; i != Level + 1; i++)
            {
                for (int ii = 0; ii != CountOfFileLines(Way); ii++)
                {
                    if (ReadCertainLine(Way, ii) == "<Skill>")
                    {
                        Console.WriteLine(ReadCertainLine(Way, ii + 2));
                        if (i == Int32.Parse(ReadCertainLine(Way, ii + 1).Remove(0, 7)))
                        {
                            if (ReadCertainLine(Way, ii + 2).IndexOf("<ID>") != -1)
                            {
                                if (Int32.Parse(ReadCertainLine(Way, ii + 2).Remove(0, 4)) == WayID)
                                {
                                    CreateField(NPCG_Abilities, ReadCertainLine(Way, ii + 3).Remove(0, 6), Color.FromArgb(25, 0, 0, 0), 12, HorizontalAlignment.Left);
                                }
                            }
                            else
                            {
                                CreateField(NPCG_Abilities, ReadCertainLine(Way, ii + 2).Remove(0, 6), Color.FromArgb(25, 0, 0, 0), 12, HorizontalAlignment.Left);
                            }
                        }
                    }
                }
            }
            NPCG_Abilities.Height = ((NPCG_Abilities.Children.Count - 1) * 25) + 32;
        }

        private void NPCG_UsesLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (NPCG_UsedDP.Height == 32)
            {
                NPCG_UsedDP.Height = ((NPCG_UsedDP.Children.Count - 1) * 25) + 32;
            }
            else NPCG_UsedDP.Height = 32;
        }

        private void NPCG_SkillsLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (NPCG_SkillDP.Height == 32)
            {
                NPCG_SkillDP.Height = ((NPCG_SkillDP.Children.Count - 1) * 25) + 32;
            }
            else NPCG_SkillDP.Height = 32;
        }

        private void NPCG_SkillsLbl1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (NPCG_Abilities.Height == 32)
            {
                NPCG_Abilities.Height = ((NPCG_Abilities.Children.Count - 1) * 25) + 32;
            }
            else NPCG_Abilities.Height = 32;
        }

        private void AbilityClick(object sender, MouseButtonEventArgs e)
        {
            NPCG_DiscriptionDP.Children.Clear();
            for (int i = 0; i != NPCG_Abilities.Children.Count; i++)
            {
                (NPCG_Abilities.Children[i] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
            }
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(84, 255, 255, 255));
            for (int i = 0; i != NPCG_SkillDP.Children.Count; i++)
            {
                (NPCG_SkillDP.Children[i] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
            }
            (NPCG_SkillDP.Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
            (NPCG_Abilities.Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
            Label NameLabel = new Label();
            NPCG_DiscriptionDP.Children.Add(NameLabel);
            NameLabel.FontFamily = new FontFamily("Book Antiqua");
            NameLabel.FontSize = 24;
            NameLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            NameLabel.HorizontalAlignment = HorizontalAlignment.Center;
            DockPanel.SetDock(NameLabel, Dock.Top);
            NameLabel.Content = (sender as Label).Content;

            String Way = @"Bin\Class\";
            if (ClassID == 1) Way = Way + "Bard.dnd";
            if (ClassID == 2) Way = Way + "Barbarian.dnd";
            if (ClassID == 3) Way = Way + "Fighter.dnd";
            if (ClassID == 4) Way = Way + "Wizard.dnd";
            if (ClassID == 5) Way = Way + "Druid.dnd";
            if (ClassID == 6) Way = Way + "Cleric.dnd";
            if (ClassID == 7) Way = Way + "Warlock.dnd";
            if (ClassID == 8) Way = Way + "Monk.dnd";
            if (ClassID == 9) Way = Way + "Paladin.dnd";
            if (ClassID == 10) Way = Way + "Rogue.dnd";
            if (ClassID == 11) Way = Way + "Ranger.dnd";
            if (ClassID == 12) Way = Way + "Sorcerer.dnd";

            for (int i = 0; i != CountOfFileLines(Way); i++)
            {
                if (ReadCertainLine(Way, i) == "<Name>" + (sender as Label).Content.ToString())
                {
                    int WhileCounter = i;
                    while (ReadCertainLine(Way, WhileCounter) != "<End>")
                    {
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Line>") != -1)
                        {
                            WrapPanel DiscriptionDP = new WrapPanel();
                            NPCG_DiscriptionDP.Children.Add(DiscriptionDP);
                            DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                            DiscriptionDP.Width = Double.NaN;
                            DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                            DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                            DiscriptionDP.Orientation = Orientation.Horizontal;
                            DockPanel.SetDock(DiscriptionDP, Dock.Top);
                            DiscriptionDP.Height = Double.NaN;

                            String ThisLine = ReadCertainLine(Way, WhileCounter);
                            ThisLine = ThisLine.Remove(0, 6);

                            int SymbolID = 0;
                            while (ThisLine.Length != 0)
                            {
                                String CombineText = "";
                                for (int o = 0; o != ThisLine.Length; o++)
                                {
                                    if (ThisLine[o] == ' ')
                                    {
                                        SymbolID = 1;
                                        break;
                                    }
                                    if (ThisLine[o] == '<')
                                    {
                                        if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                        if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                        if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                        break;
                                    }
                                    CombineText = CombineText + ThisLine[o];
                                }
                                if (SymbolID == 1)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                                if (SymbolID == 2)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 3);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    TextBlock TextBl = new TextBlock();
                                    DiscriptionDP.Children.Add(TextBl);
                                    TextBl.FontSize = 14;
                                    TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    TextBl.Text = CombineText;
                                    TextBl.Padding = new Thickness(3, 5, 3, 5);
                                    TextBl.TextDecorations = TextDecorations.Underline;
                                    CombineText = CombineText + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    Console.WriteLine(CombineText);
                                    Console.WriteLine(ThisLine);
                                }
                                if (SymbolID == 3)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    ThisLine = ThisLine.Remove(0, 3);
                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    String NewCombine = CombineText;
                                    while (CombineText.Length != 0)
                                    {
                                        string LinkedText = "";
                                        for (int o = 0; o != CombineText.Length; o++)
                                        {
                                            if (CombineText[o] == ' ') break;
                                            LinkedText = LinkedText + CombineText[o];
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = LinkedText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.FontWeight = FontWeights.Bold;
                                        if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                        else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                    }

                                    NewCombine = NewCombine + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                }
                                if (SymbolID == 4)
                                {
                                    Image texted = new Image();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.Height = 10;
                                    texted.Width = 10;
                                    texted.Margin = new Thickness(15, 0, 15, 0);
                                    ChangeImage("Seporator.png", texted);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                }
                            }
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Table>") != -1)
                        {
                            Grid newTable = new Grid();
                            NPCG_DiscriptionDP.Children.Add(newTable);
                            newTable.HorizontalAlignment = HorizontalAlignment.Stretch;
                            newTable.Width = Double.NaN;
                            newTable.VerticalAlignment = VerticalAlignment.Top;
                            newTable.Margin = new Thickness(25, 0, 25, 0);
                            newTable.Height = Double.NaN;
                            DockPanel.SetDock(newTable, Dock.Top);
                            int RowCount = 0;

                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); ti++)
                            {
                                RowDefinition RW = new RowDefinition();
                                newTable.RowDefinitions.Add(RW);
                                RowCount++;
                            }
                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); ti++)
                            {
                                ColumnDefinition CW = new ColumnDefinition();
                                newTable.ColumnDefinitions.Add(CW);
                            }


                            String ThiLin = ReadCertainLine(Way, WhileCounter);
                            ThiLin = ThiLin.Remove(0, 9);
                            for (int Ri = 0; Ri != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); Ri++)
                            {
                                for (int Ci = 0; Ci != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); Ci++)
                                {
                                    Border Brdr = new Border();
                                    newTable.Children.Add(Brdr);
                                    Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Brdr.VerticalAlignment = VerticalAlignment.Stretch;
                                    Brdr.Height = Double.NaN;
                                    Brdr.Width = Double.NaN;
                                    Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Brdr.BorderThickness = new Thickness(1, 1, 1, 1);
                                    if (Ci == 0) Brdr.BorderThickness = new Thickness(1, 1, 0, 1);
                                    if (Ci > 0) Brdr.BorderThickness = new Thickness(0, 1, 0, 1);
                                    if (Ci == Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()) - 1) Brdr.BorderThickness = new Thickness(0, 1, 1, 1);
                                    Grid.SetColumnSpan(Brdr, 1);
                                    Grid.SetRowSpan(Brdr, 1);
                                    Grid.SetRow(Brdr, Ri);
                                    Grid.SetColumn(Brdr, Ci);
                                    Label Lbl = new Label();
                                    newTable.Children.Add(Lbl);
                                    Lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Lbl.VerticalAlignment = VerticalAlignment.Stretch;
                                    Lbl.Height = Double.NaN;
                                    Lbl.Width = Double.NaN;
                                    Lbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Grid.SetColumnSpan(Lbl, 1);
                                    Grid.SetRowSpan(Lbl, 1);
                                    Grid.SetRow(Lbl, Ri);
                                    Grid.SetColumn(Lbl, Ci);
                                    Lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                                    Lbl.VerticalContentAlignment = VerticalAlignment.Center;
                                    int ss = 0;
                                    String AddingLine = "";
                                    if (ThiLin.Length != 0)
                                    {
                                        while (ThiLin[ss] != ';')
                                        {
                                            AddingLine = AddingLine + ThiLin[ss].ToString();
                                            ss++;
                                        }
                                    }
                                    Lbl.Content = AddingLine;
                                    ThiLin = ThiLin.Remove(0, ThiLin.IndexOf(';') + 1);
                                    if (Ci != (Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()) - 1))
                                        if (Ri == 0)
                                        {
                                            Console.WriteLine("Debug: " + RowCount);
                                            GridSplitter GS = new GridSplitter();
                                            newTable.Children.Add(GS);
                                            GS.HorizontalAlignment = HorizontalAlignment.Right;
                                            Grid.SetColumn(GS, Ci);
                                            Grid.SetRow(GS, 0);
                                            Grid.SetColumnSpan(GS, 1);
                                            Grid.SetRowSpan(GS, RowCount);
                                            GS.VerticalAlignment = VerticalAlignment.Stretch;
                                            GS.Height = double.NaN;
                                            GS.Width = 3;
                                            GS.Background = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        }
                                }
                            }
                        }
                        WhileCounter++;
                    }
                }
            }
        }

        public void Skill_Click(object sender, MouseButtonEventArgs e)
        {
            NPCG_DiscriptionDP.Children.Clear();
            for (int i = 0; i != NPCG_SkillDP.Children.Count; i++)
            {
                (NPCG_SkillDP.Children[i] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
            }
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(84, 255, 255, 255));
            (NPCG_SkillDP.Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
            for (int i = 0; i != NPCG_Abilities.Children.Count; i++)
            {
                (NPCG_Abilities.Children[i] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
            }
            (NPCG_Abilities.Children[0] as Label).Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
            Label NameLabel = new Label();
            NPCG_DiscriptionDP.Children.Add(NameLabel);
            NameLabel.FontFamily = new FontFamily("Book Antiqua");
            NameLabel.FontSize = 24;
            NameLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            NameLabel.HorizontalAlignment = HorizontalAlignment.Center;
            DockPanel.SetDock(NameLabel, Dock.Top);
            NameLabel.Content = (sender as Label).Content;

            String RaseWay = "";
            if (RaseID == 21) RaseWay = @"Bin\Rase\Лесной гном.dnd";
            if (RaseID == 22) RaseWay = @"Bin\Rase\Скальный гном.dnd";
            if (RaseID == 41) RaseWay = @"Bin\Rase\Горный дварф.dnd";
            if (RaseID == 42) RaseWay = @"Bin\Rase\Холмовой дварф.dnd";
            if (RaseID == 51) RaseWay = @"Bin\Rase\Дженази воды.dnd";
            if (RaseID == 52) RaseWay = @"Bin\Rase\Дженази воздуха.dnd";
            if (RaseID == 53) RaseWay = @"Bin\Rase\Дженази земли.dnd";
            if (RaseID == 54) RaseWay = @"Bin\Rase\Дженази огня.dnd";
            if (RaseID == 61) RaseWay = @"Bin\Rase\Белый.dnd";
            if (RaseID == 62) RaseWay = @"Bin\Rase\Бронзовый.dnd";
            if (RaseID == 63) RaseWay = @"Bin\Rase\Зеленый.dnd";
            if (RaseID == 64) RaseWay = @"Bin\Rase\Золотой.dnd";
            if (RaseID == 65) RaseWay = @"Bin\Rase\Красный.dnd";
            if (RaseID == 66) RaseWay = @"Bin\Rase\Латунный.dnd";
            if (RaseID == 67) RaseWay = @"Bin\Rase\Медный.dnd";
            if (RaseID == 68) RaseWay = @"Bin\Rase\Серебряный.dnd";
            if (RaseID == 69) RaseWay = @"Bin\Rase\Синий.dnd";
            if (RaseID == 610) RaseWay = @"Bin\Rase\Черный.dnd";
            if (RaseID == 81) RaseWay = @"Bin\Rase\Коренастый.dnd";
            if (RaseID == 82) RaseWay = @"Bin\Rase\Легконогий.dnd";
            if (RaseID == 121) RaseWay = @"Bin\Rase\Высший.dnd";
            if (RaseID == 122) RaseWay = @"Bin\Rase\Лесной.dnd";
            if (RaseID == 123) RaseWay = @"Bin\Rase\Тёмный.dnd";
            if (RaseID == 1) RaseWay = @"Bin\Rase\Ааракокра.dnd";
            if (RaseID == 3) RaseWay = @"Bin\Rase\Голиаф.dnd";
            if (RaseID == 7) RaseWay = @"Bin\Rase\Полуорк.dnd";
            if (RaseID == 9) RaseWay = @"Bin\Rase\Полуэльф.dnd";
            if (RaseID == 10) RaseWay = @"Bin\Rase\Тифлинг.dnd";
            if (RaseID == 11) RaseWay = @"Bin\Rase\Человек.dnd";

            for (int i = 0; i != CountOfFileLines(RaseWay); i++)
            {
                if (ReadCertainLine(RaseWay, i) == "<Skill>" + (sender as Label).Content.ToString())
                {
                    int WhileCounter = i;
                    while (ReadCertainLine(RaseWay, WhileCounter) != "<End>")
                    {
                        if (ReadCertainLine(RaseWay, WhileCounter).IndexOf("<Line>") != -1)
                        {
                            WrapPanel DiscriptionDP = new WrapPanel();
                            NPCG_DiscriptionDP.Children.Add(DiscriptionDP);
                            DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                            DiscriptionDP.Width = Double.NaN;
                            DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                            DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                            DiscriptionDP.Orientation = Orientation.Horizontal;
                            DockPanel.SetDock(DiscriptionDP, Dock.Top);
                            DiscriptionDP.Height = Double.NaN;

                            String ThisLine = ReadCertainLine(RaseWay, WhileCounter);
                            ThisLine = ThisLine.Remove(0, 6);

                            int SymbolID = 0;
                            while (ThisLine.Length != 0)
                            {
                                String CombineText = "";
                                for (int o = 0; o != ThisLine.Length; o++)
                                {
                                    if (ThisLine[o] == ' ')
                                    {
                                        SymbolID = 1;
                                        break;
                                    }
                                    if (ThisLine[o] == '<')
                                    {
                                        if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                        if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                        if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                        break;
                                    }
                                    CombineText = CombineText + ThisLine[o];
                                }
                                if (SymbolID == 1)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                                if (SymbolID == 2)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 3);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    TextBlock TextBl = new TextBlock();
                                    DiscriptionDP.Children.Add(TextBl);
                                    TextBl.FontSize = 14;
                                    TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    TextBl.Text = CombineText;
                                    TextBl.Padding = new Thickness(3, 5, 3, 5);
                                    TextBl.TextDecorations = TextDecorations.Underline;
                                    CombineText = CombineText + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    Console.WriteLine(CombineText);
                                    Console.WriteLine(ThisLine);
                                }
                                if (SymbolID == 3)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    ThisLine = ThisLine.Remove(0, 3);
                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    String NewCombine = CombineText;
                                    while (CombineText.Length != 0)
                                    {
                                        string LinkedText = "";
                                        for (int o = 0; o != CombineText.Length; o++)
                                        {
                                            if (CombineText[o] == ' ') break;
                                            LinkedText = LinkedText + CombineText[o];
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = LinkedText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.FontWeight = FontWeights.Bold;
                                        if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                        else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                    }

                                    NewCombine = NewCombine + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                }
                                if (SymbolID == 4)
                                {
                                    Image texted = new Image();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.Height = 10;
                                    texted.Width = 10;
                                    texted.Margin = new Thickness(15, 0, 15, 0);
                                    ChangeImage("Seporator.png", texted);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                }
                            }
                        }
                        if (ReadCertainLine(RaseWay, WhileCounter).IndexOf("<Table>") != -1)
                        {
                            Grid newTable = new Grid();
                            NPCG_DiscriptionDP.Children.Add(newTable);
                            newTable.HorizontalAlignment = HorizontalAlignment.Stretch;
                            newTable.Width = Double.NaN;
                            newTable.VerticalAlignment = VerticalAlignment.Top;
                            newTable.Margin = new Thickness(25, 0, 25, 0);
                            newTable.Height = Double.NaN;
                            DockPanel.SetDock(newTable, Dock.Top);
                            int RowCount = 0;

                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(RaseWay, WhileCounter)[7].ToString()); ti++)
                            {
                                RowDefinition RW = new RowDefinition();
                                newTable.RowDefinitions.Add(RW);
                                RowCount++;
                            }
                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(RaseWay, WhileCounter)[8].ToString()); ti++)
                            {
                                ColumnDefinition CW = new ColumnDefinition();
                                newTable.ColumnDefinitions.Add(CW);
                            }


                            String ThiLin = ReadCertainLine(RaseWay, WhileCounter);
                            ThiLin = ThiLin.Remove(0, 9);
                            for (int Ri = 0; Ri != Int32.Parse(ReadCertainLine(RaseWay, WhileCounter)[7].ToString()); Ri++)
                            {
                                for (int Ci = 0; Ci != Int32.Parse(ReadCertainLine(RaseWay, WhileCounter)[8].ToString()); Ci++)
                                {
                                    Border Brdr = new Border();
                                    newTable.Children.Add(Brdr);
                                    Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Brdr.VerticalAlignment = VerticalAlignment.Stretch;
                                    Brdr.Height = Double.NaN;
                                    Brdr.Width = Double.NaN;
                                    Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Brdr.BorderThickness = new Thickness(1, 1, 1, 1);
                                    if (Ci == 0) Brdr.BorderThickness = new Thickness(1, 1, 0, 1);
                                    if (Ci > 0) Brdr.BorderThickness = new Thickness(0, 1, 0, 1);
                                    if (Ci == Int32.Parse(ReadCertainLine(RaseWay, WhileCounter)[8].ToString()) - 1) Brdr.BorderThickness = new Thickness(0, 1, 1, 1);
                                    Grid.SetColumnSpan(Brdr, 1);
                                    Grid.SetRowSpan(Brdr, 1);
                                    Grid.SetRow(Brdr, Ri);
                                    Grid.SetColumn(Brdr, Ci);
                                    Label Lbl = new Label();
                                    newTable.Children.Add(Lbl);
                                    Lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Lbl.VerticalAlignment = VerticalAlignment.Stretch;
                                    Lbl.Height = Double.NaN;
                                    Lbl.Width = Double.NaN;
                                    Lbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Grid.SetColumnSpan(Lbl, 1);
                                    Grid.SetRowSpan(Lbl, 1);
                                    Grid.SetRow(Lbl, Ri);
                                    Grid.SetColumn(Lbl, Ci);
                                    Lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                                    Lbl.VerticalContentAlignment = VerticalAlignment.Center;
                                    int ss = 0;
                                    String AddingLine = "";
                                    if (ThiLin.Length != 0)
                                    {
                                        while (ThiLin[ss] != ';')
                                        {
                                            AddingLine = AddingLine + ThiLin[ss].ToString();
                                            ss++;
                                        }
                                    }
                                    Lbl.Content = AddingLine;
                                    ThiLin = ThiLin.Remove(0, ThiLin.IndexOf(';') + 1);
                                    if (Ci != (Int32.Parse(ReadCertainLine(RaseWay, WhileCounter)[8].ToString()) - 1))
                                        if (Ri == 0)
                                        {
                                            Console.WriteLine("Debug: " + RowCount);
                                            GridSplitter GS = new GridSplitter();
                                            newTable.Children.Add(GS);
                                            GS.HorizontalAlignment = HorizontalAlignment.Right;
                                            Grid.SetColumn(GS, Ci);
                                            Grid.SetRow(GS, 0);
                                            Grid.SetColumnSpan(GS, 1);
                                            Grid.SetRowSpan(GS, RowCount);
                                            GS.VerticalAlignment = VerticalAlignment.Stretch;
                                            GS.Height = double.NaN;
                                            GS.Width = 3;
                                            GS.Background = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        }
                                }
                            }
                        }
                        WhileCounter++;
                    }
                }
            }
        }

        public void CreateSpell(String Content, String Tag)
        {
            Label TB = new Label();
            SpellsDP.Children.Add(TB);
            TB.Content = Content;
            DockPanel.SetDock(TB, Dock.Top);
            TB.FontSize = 14;
            TB.HorizontalAlignment = HorizontalAlignment.Stretch;
            TB.HorizontalContentAlignment = HorizontalAlignment.Center;
            TB.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            TB.Tag = Tag;
            if (NewSpellValue == true)
            {
                TB.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                NewSpellValue = false;
            }
            else
            {
                TB.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                NewSpellValue = true;
            }
            TB.MouseUp += LoadSpell;
        }

        private void SL_More_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Label).Content.ToString() == "Расширенный поиск")
            {
                (sender as Label).Background = new SolidColorBrush(Color.FromArgb(122, 255, 212, 173));
                MainGrid.RowDefinitions[0].Height = new GridLength(80);
                (sender as Label).Content = "Общий поиск";
            }
            else
            {
                (sender as Label).Background = new SolidColorBrush(Color.FromArgb(255, 255, 212, 173));
                MainGrid.RowDefinitions[0].Height = new GridLength(40);
                (sender as Label).Content = "Расширенный поиск";
            }
        }

        private void SL_Search_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SpellsDP.Children.Clear();
            int i = 0;
            while (LoadedSpells[0, i] != null)
            {
                CreateSpell(LoadedSpells[0, i].Remove(0, 5), LoadedSpells[1, i]);
                Console.WriteLine(LoadedSpells[0, i]);
                i++;
            }
        }

        private void LoadSpell(object sender, MouseButtonEventArgs e)
        {
            String Way = @"Bin\Data\Spells\PHB.dnd";
            SpellDiscription.Children.Clear();
            String SpellName = "";
            if ((sender as Label).Content.ToString().IndexOf("уровень") == -1) SpellName = (sender as Label).Content.ToString();
            else SpellName = SL_SpellName.Content.ToString();

            SL_SpellName.Content = SpellName;
            int SelectedLevel = 0;

            Label[] Cells = new Label[] { SLSpellCell1, SLSpellCell2, SLSpellCell3, SLSpellCell4, SLSpellCell5, SLSpellCell6, SLSpellCell7, SLSpellCell8, SLSpellCell9, };
            if ((sender as Label).Content.ToString().IndexOf("уровень") == -1)
            {
                for (int i = 0; i != 9; i++)
                {
                    Cells[i].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Cells[i].Tag = null;
                }
            }
            else
            {
                Label[] SplCell = new Label[] { SLSpellCell1, SLSpellCell2, SLSpellCell3, SLSpellCell4, SLSpellCell5, SLSpellCell6, SLSpellCell7, SLSpellCell8, SLSpellCell9, };
                if (sender == SplCell[0]) SelectedLevel = 1;
                if (sender == SplCell[1]) SelectedLevel = 2;
                if (sender == SplCell[2]) SelectedLevel = 3;
                if (sender == SplCell[3]) SelectedLevel = 4;
                if (sender == SplCell[4]) SelectedLevel = 5;
                if (sender == SplCell[5]) SelectedLevel = 6;
                if (sender == SplCell[6]) SelectedLevel = 7;
                if (sender == SplCell[7]) SelectedLevel = 8;
                if (sender == SplCell[8]) SelectedLevel = 9;
            }

            if ((sender as Label).Content.ToString().IndexOf("уровень") == -1) FileRow = Int32.Parse((sender as Label).Tag.ToString()) - 1;
            for (int i = FileRow; i != CountOfFileLines(Way); i++)
            {
                if (ReadCertainLine(Way, i) == "<Rus>" + SpellName)
                {
                    int WhileCounter = i;

                    while (ReadCertainLine(Way, WhileCounter) != "<End>")
                    {
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Level>") != -1)
                        {
                            SL_SpellLevel.Content = "Уровень: " + ReadCertainLine(Way, WhileCounter).Remove(0, 7);
                            if ((sender as Label).Content.ToString().IndexOf("уровень") == -1) SelectedLevel = Int32.Parse(ReadCertainLine(Way, WhileCounter).Remove(0, 7));
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<School>") != -1)
                        {
                            SL_SpellSchool.Content = "Школа: " + ReadCertainLine(Way, WhileCounter).Remove(0, 8);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Casttime>") != -1)
                        {
                            SL_Spellcasttime.Text = "Время накладывания: " + ReadCertainLine(Way, WhileCounter).Remove(0, 10);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Distance>") != -1)
                        {
                            SL_SpellDistance.Content = "Дистанция: " + ReadCertainLine(Way, WhileCounter).Remove(0, 10);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Components>") != -1)
                        {
                            SL_SpellComponents.Text = "Компоненты: " + ReadCertainLine(Way, WhileCounter).Remove(0, 12);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Time>") != -1)
                        {
                            SL_SpellTime.Content = "Длительность: " + ReadCertainLine(Way, WhileCounter).Remove(0, 6);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Class>") != -1)
                        {
                            SL_SpellClass.Content = "Классы: " + ReadCertainLine(Way, WhileCounter).Remove(0, 7);
                        }

                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Line>") != -1)
                        {
                            WrapPanel DiscriptionDP = new WrapPanel();
                            SpellDiscription.Children.Add(DiscriptionDP);
                            DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                            DiscriptionDP.Width = Double.NaN;
                            DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                            DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                            DiscriptionDP.Orientation = Orientation.Horizontal;
                            DockPanel.SetDock(DiscriptionDP, Dock.Top);
                            DiscriptionDP.Height = Double.NaN;

                            String ThisLine = ReadCertainLine(Way, WhileCounter);
                            ThisLine = ThisLine.Remove(0, 6);

                            int SymbolID = 0;
                            while (ThisLine.Length != 0)
                            {
                                String CombineText = "";
                                for (int o = 0; o != ThisLine.Length; o++)
                                {
                                    if (ThisLine[o] == ' ')
                                    {
                                        SymbolID = 1;
                                        break;
                                    }
                                    if (ThisLine[o] == '<')
                                    {
                                        if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                        if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                        if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                        break;
                                    }
                                    if (ThisLine[o] == '$')
                                    {
                                        SymbolID = 5;
                                        break;
                                    }

                                    CombineText = CombineText + ThisLine[o];
                                }
                                if (SymbolID == 1)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                                if (SymbolID == 2)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 3);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    TextBlock TextBl = new TextBlock();
                                    DiscriptionDP.Children.Add(TextBl);
                                    TextBl.FontSize = 14;
                                    TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    TextBl.Text = CombineText;
                                    TextBl.Padding = new Thickness(3, 5, 3, 5);
                                    TextBl.TextDecorations = TextDecorations.Underline;
                                    CombineText = CombineText + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    Console.WriteLine(CombineText);
                                    Console.WriteLine(ThisLine);
                                }
                                if (SymbolID == 3)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    ThisLine = ThisLine.Remove(0, 3);
                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    String NewCombine = CombineText;
                                    while (CombineText.Length != 0)
                                    {
                                        string LinkedText = "";
                                        for (int o = 0; o != CombineText.Length; o++)
                                        {
                                            if (CombineText[o] == ' ') break;
                                            LinkedText = LinkedText + CombineText[o];
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = LinkedText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.FontWeight = FontWeights.Bold;
                                        if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                        else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                    }

                                    NewCombine = NewCombine + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                }
                                if (SymbolID == 4)
                                {
                                    Image texted = new Image();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.Height = 10;
                                    texted.Width = 10;
                                    texted.Margin = new Thickness(15, 0, 15, 0);
                                    ChangeImage("Seporator.png", texted);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                }
                                if (SymbolID == 5)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 1);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '$')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    //CombineText;
                                    for (int ss = WhileCounter; ss != CountOfFileLines(Way); ss++)
                                    {
                                        if (ReadCertainLine(Way, ss).IndexOf("#" + CombineText) != -1)
                                        {
                                            String[] ThisCells = new String[9];
                                            int CellsID = 0;
                                            String EditableLine = ReadCertainLine(Way, ss).Remove(0, CombineText.Length + 1);

                                            while (EditableLine.Length != 0)
                                            {
                                                for (int pos = 0; pos != EditableLine.Length; pos++)
                                                {
                                                    if (EditableLine[pos] == ':')
                                                    {
                                                        EditableLine = EditableLine.Remove(0, ThisCells[CellsID].Length + 1);
                                                        CellsID++;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        ThisCells[CellsID] = ThisCells[CellsID] + EditableLine[pos].ToString();
                                                    }
                                                }
                                            }
                                            TextBlock TextBl2 = new TextBlock();
                                            DiscriptionDP.Children.Add(TextBl2);
                                            TextBl2.FontSize = 14;
                                            TextBl2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                            TextBl2.Text = ThisCells[SelectedLevel - 1];
                                            TextBl2.Padding = new Thickness(3, 5, 3, 5);
                                            TextBl2.FontWeight = FontWeights.Bold;
                                            break;
                                        }
                                    }
                                    CombineText = CombineText + "$";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                            }
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Table>") != -1)
                        {
                            Grid newTable = new Grid();
                            NPCG_DiscriptionDP.Children.Add(newTable);
                            newTable.HorizontalAlignment = HorizontalAlignment.Stretch;
                            newTable.Width = Double.NaN;
                            newTable.VerticalAlignment = VerticalAlignment.Top;
                            newTable.Margin = new Thickness(25, 0, 25, 0);
                            newTable.Height = 100;
                            DockPanel.SetDock(newTable, Dock.Top);

                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); ti++)
                            {
                                RowDefinition RW = new RowDefinition();
                                newTable.RowDefinitions.Add(RW);
                            }
                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); ti++)
                            {
                                ColumnDefinition CW = new ColumnDefinition();
                                newTable.ColumnDefinitions.Add(CW);
                            }

                            String ThiLin = ReadCertainLine(Way, WhileCounter);
                            ThiLin = ThiLin.Remove(0, 9);
                            for (int Ri = 0; Ri != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); Ri++)
                            {
                                for (int Ci = 0; Ci != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); Ci++)
                                {
                                    Border Brdr = new Border();
                                    newTable.Children.Add(Brdr);
                                    Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Brdr.VerticalAlignment = VerticalAlignment.Stretch;
                                    Brdr.Height = Double.NaN;
                                    Brdr.Width = Double.NaN;
                                    Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Brdr.BorderThickness = new Thickness(1, 1, 1, 1);
                                    Grid.SetColumnSpan(Brdr, 1);
                                    Grid.SetRowSpan(Brdr, 1);
                                    Grid.SetRow(Brdr, Ri);
                                    Grid.SetColumn(Brdr, Ci);
                                    Label Lbl = new Label();
                                    newTable.Children.Add(Lbl);
                                    Lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Lbl.VerticalAlignment = VerticalAlignment.Stretch;
                                    Lbl.Height = Double.NaN;
                                    Lbl.Width = Double.NaN;
                                    Lbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Grid.SetColumnSpan(Lbl, 1);
                                    Grid.SetRowSpan(Lbl, 1);
                                    Grid.SetRow(Lbl, Ri);
                                    Grid.SetColumn(Lbl, Ci);
                                    Lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                                    Lbl.VerticalContentAlignment = VerticalAlignment.Center;
                                    int ss = 0;
                                    String AddingLine = "";
                                    if (ThiLin.Length != 0)
                                    {
                                        while (ThiLin[ss] != ';')
                                        {
                                            AddingLine = AddingLine + ThiLin[ss].ToString();
                                            ss++;
                                        }
                                    }
                                    Lbl.Content = AddingLine;
                                    ThiLin = ThiLin.Remove(0, ThiLin.IndexOf(';') + 1);
                                    Console.WriteLine(AddingLine);
                                    Console.WriteLine(ThiLin);
                                }
                            }
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<CellsLevel>") != -1)
                        {
                            if (SelectedLevel == Int32.Parse(SL_SpellLevel.Content.ToString().Remove(0, 9)))
                            {
                                WrapPanel DiscriptionDP = new WrapPanel();
                                SpellDiscription.Children.Add(DiscriptionDP);
                                DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                                DiscriptionDP.Width = Double.NaN;
                                DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                                DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                                DiscriptionDP.Orientation = Orientation.Horizontal;
                                DockPanel.SetDock(DiscriptionDP, Dock.Top);
                                DiscriptionDP.Height = Double.NaN;

                                String ThisLine = ReadCertainLine(Way, WhileCounter);
                                ThisLine = ThisLine.Remove(0, 12);

                                int SymbolID = 0;
                                while (ThisLine.Length != 0)
                                {
                                    String CombineText = "";
                                    for (int o = 0; o != ThisLine.Length; o++)
                                    {
                                        if (ThisLine[o] == ' ')
                                        {
                                            SymbolID = 1;
                                            break;
                                        }
                                        if (ThisLine[o] == '<')
                                        {
                                            if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                            if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                            if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                            break;
                                        }
                                        CombineText = CombineText + ThisLine[o];
                                    }
                                    if (SymbolID == 1)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    }
                                    if (SymbolID == 2)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        Console.WriteLine(CombineText);

                                        ThisLine = ThisLine.Remove(0, 3);

                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '>')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = CombineText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.TextDecorations = TextDecorations.Underline;
                                        CombineText = CombineText + ">";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                        Console.WriteLine(CombineText);
                                        Console.WriteLine(ThisLine);
                                    }
                                    if (SymbolID == 3)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        ThisLine = ThisLine.Remove(0, 3);
                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '>')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        String NewCombine = CombineText;
                                        while (CombineText.Length != 0)
                                        {
                                            string LinkedText = "";
                                            for (int o = 0; o != CombineText.Length; o++)
                                            {
                                                if (CombineText[o] == ' ') break;
                                                LinkedText = LinkedText + CombineText[o];
                                            }
                                            TextBlock TextBl = new TextBlock();
                                            DiscriptionDP.Children.Add(TextBl);
                                            TextBl.FontSize = 14;
                                            TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                            TextBl.Text = LinkedText;
                                            TextBl.Padding = new Thickness(3, 5, 3, 5);
                                            TextBl.FontWeight = FontWeights.Bold;
                                            if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                            else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                        }

                                        NewCombine = NewCombine + ">";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                    }
                                    if (SymbolID == 4)
                                    {
                                        Image texted = new Image();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.Height = 10;
                                        texted.Width = 10;
                                        texted.Margin = new Thickness(15, 0, 15, 0);
                                        ChangeImage("Seporator.png", texted);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                    }
                                }
                            }
                        }

                        WhileCounter++;
                    }
                }
            }
            SL_SpellSource.Content = "Источник: Player's Handsbook";
        }

        private void SL_SpellCellGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SL_SpellCellGrid.Width == 25)
            {
                SL_SpellCellGrid.Width = 200;
                ChangeImage("Dots2.png", SL_DotsImage);
            }
            else
            {
                SL_SpellCellGrid.Width = 25;
                ChangeImage("Dots.png", SL_DotsImage);
            }
        }

        private void SLSpellCell1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Label[] Cells = new Label[] { SLSpellCell1, SLSpellCell2, SLSpellCell3, SLSpellCell4, SLSpellCell5, SLSpellCell6, SLSpellCell7, SLSpellCell8, SLSpellCell9, };
            if ((sender as Label).Tag != null)
            {
                for (int i = 0; i != 9; i++)
                {
                    Cells[i].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Cells[i].Tag = null;
                }
            }
            else
            {
                for (int i = 0; i != 9; i++)
                {
                    Cells[i].Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Cells[i].Tag = null;
                }
                (sender as Label).Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                (sender as Label).Tag = "LAST";
            }
            LoadSpell(sender, e);
        }

        private void ToGeneratorLbl5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pages.SelectedIndex = 5;
        }

        private void SM_Storys_DropDownOpened(object sender, EventArgs e)
        {
            (SM_Storys.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
        }

        private void SM_Storys_DropDownClosed(object sender, EventArgs e)
        {
            if (SM_Storys.SelectedIndex == 1) SM_LoadStory_MouseDown();
            if (SM_Storys.SelectedIndex == 2) CreateStory();
            SM_Storys.SelectedIndex = 0;
            (SM_Storys.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
        }

        private void SM_LoadStory_MouseDown()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "История";
            dlg.DefaultExt = ".dstory"; // Default file extension
            dlg.Filter = "Dungeon Story (.dstory)|*.dstory"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
        }

        public void CreateStory()
        {
            SM_StoryNameDP.Visibility = Visibility.Visible;
            MC_StoryNameLbl.Content = "Название истории";
            NowOpenedStory = "";
        }

        private void MC_StoryNameLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Tools.IsEnabled == true)
            {
                MS_StoryNameEdt.Text = (sender as Label).Content.ToString();
                (sender as Label).Visibility = Visibility.Collapsed;
                MS_StoryNameEdt.Visibility = Visibility.Visible;
                OldStoryName = MC_StoryNameLbl.Content.ToString();
            }
        }

        private void MS_StoryNameEdt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MC_StoryNameLbl.Visibility = Visibility.Visible;
                MS_StoryNameEdt.Visibility = Visibility.Collapsed;
                MC_StoryNameLbl.Content = MS_StoryNameEdt.Text;
                MS_StoryNameEdt.Visibility = Visibility.Collapsed;
                MC_StoryNameLbl.Visibility = Visibility.Visible;

                if (NowOpenedStory == "")
                {
                    for (int i=0; i != 33000; i++)
                    {
                        if (LoadedProfile[i] == "<NULLSTRING>")
                        {
                            if (LoadedProfile[i+1] == "<NULLSTRING>")
                            {
                                LoadedProfile[i] = "<Story>" + (sender as TextBox).Text;
                                break;
                            }
                        }
                    }
                    NowOpenedStory = (sender as TextBox).Text;
                }
                else
                {
                    for (int i = 0; i != 33000; i++)
                    {
                        if (LoadedProfile[i] == "<Story>" + OldStoryName)
                        {
                            LoadedProfile[i] = "<Story>" + MS_StoryNameEdt.Text;
                            break;
                        }
                    }
                }
                NowOpenedStory = (sender as TextBox).Text;
                ToGeneratorLbl6_MouseUp(null, null);
            }
        }

        private void SM_GeneratePreview_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Paragraph CurrentParagraph = (SM_StoryEditor.Document.Blocks.FirstBlock as Paragraph);
            Inline CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
            String RunLine = "";
            SM_TextPreview.Children.Clear();

            for (int pi = 0; pi != SM_StoryEditor.Document.Blocks.Count; pi++)
            {
                WrapPanel WParagraph = new WrapPanel();
                SM_TextPreview.Children.Add(WParagraph);
                WParagraph.HorizontalAlignment = HorizontalAlignment.Stretch;
                WParagraph.VerticalAlignment = VerticalAlignment.Stretch;
                WParagraph.Margin = new Thickness(0, 10, 0, 0);
                WParagraph.Width = Double.NaN;
                WParagraph.Height = Double.NaN;
                DockPanel.SetDock(WParagraph, Dock.Top);

                CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
                for (int ri = 0; ri != CurrentParagraph.Inlines.Count; ri++)
                {
                    if (CurrentRun.GetType().ToString() == "System.Windows.Documents.Run")
                    {
                        WParagraph = new WrapPanel();
                        SM_TextPreview.Children.Add(WParagraph);
                        WParagraph.HorizontalAlignment = HorizontalAlignment.Stretch;
                        WParagraph.VerticalAlignment = VerticalAlignment.Stretch;
                        WParagraph.Margin = new Thickness(0, 0, 0, 0);
                        WParagraph.Width = Double.NaN;
                        WParagraph.Height = Double.NaN;
                        DockPanel.SetDock(WParagraph, Dock.Top);

                        RunLine = (CurrentRun as Run).Text;

                        int SymbolID = 0;
                        while (RunLine.Length != 0)
                        {
                            String CombineText = "";
                            for (int o = 0; o != RunLine.Length; o++)
                            {
                                if (RunLine[o] == ' ')
                                {
                                    SymbolID = 1;
                                    break;
                                }
                                if (RunLine[o] == '<')
                                {
                                    if (RunLine[o + 1] == 'I')
                                    {
                                        SymbolID = 2;
                                    }
                                    if (RunLine[o + 1] == 'A')
                                    {
                                        SymbolID = 3;
                                    }
                                    if (RunLine[o + 1] == 'C')
                                    {
                                        SymbolID = 4;
                                    }
                                    if (RunLine[o + 1] == 'S')
                                    {
                                        SymbolID = 5;
                                    }
                                    if (RunLine[o + 1] == 'P')
                                    {
                                        SymbolID = 6;
                                    }
                                    break;
                                }
                                CombineText = CombineText + RunLine[o];
                                if (o == RunLine.Length - 1) SymbolID = 1;
                            }
                            if (SymbolID == 1)
                            {
                                Label texted = new Label();
                                WParagraph.Children.Add(texted);
                                texted.FontSize = 14;
                                texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted.Content = CombineText;
                                texted.Padding = new Thickness(3, 5, 3, 5);
                                if ((RunLine.IndexOf(CombineText) + CombineText.Length) == RunLine.Length) RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length);
                                else RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length + 1);
                            }
                            if (SymbolID == 2)
                            {
                                Label texted = new Label();
                                WParagraph.Children.Add(texted);
                                texted.FontSize = 14;
                                texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted.Content = CombineText;
                                texted.Padding = new Thickness(3, 5, 3, 5);
                                RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length);
                                RunLine = RunLine.Remove(0, 7);
                                Console.WriteLine(RunLine);

                                int WhileCounter2 = 0;
                                String UrlImg = "";
                                while (RunLine[WhileCounter2] != ';')
                                {
                                    UrlImg = UrlImg + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                RunLine = RunLine.Remove(0, UrlImg.Length + 1);
                                Console.WriteLine(RunLine);

                                var uri = new Uri(UrlImg);
                                var bitmap = new BitmapImage(uri);
                                Image PastedImage = new Image();
                                PastedImage.Source = bitmap;
                                WParagraph.Children.Add(PastedImage);

                                string IHeight = "";
                                WhileCounter2 = 0;
                                while (RunLine[WhileCounter2] != ';')
                                {
                                    IHeight = IHeight + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                RunLine = RunLine.Remove(0, IHeight.Length + 1);

                                UrlImg = "";
                                WhileCounter2 = 0;
                                while (RunLine[WhileCounter2] != '>')
                                {
                                    UrlImg = UrlImg + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                Console.WriteLine(RunLine);

                                RunLine = RunLine.Remove(0, UrlImg.Length + 1);

                                Console.WriteLine(RunLine);
                                PastedImage.Height = Int32.Parse(IHeight);
                                PastedImage.Height = Int32.Parse(UrlImg);
                                PastedImage.HorizontalAlignment = HorizontalAlignment.Left;
                                PastedImage.VerticalAlignment = VerticalAlignment.Top;
                                DockPanel.SetDock(PastedImage, Dock.Top);
                            }
                            if (SymbolID == 3)
                            {

                                Label texted = new Label();
                                WParagraph.Children.Add(texted);
                                texted.FontSize = 14;
                                texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted.Content = CombineText;
                                texted.Padding = new Thickness(3, 5, 3, 5);
                                RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length);
                                RunLine = RunLine.Remove(0, 7);

                                int WhileCounter2 = 0;
                                String UrlImg = "";
                                while (RunLine[WhileCounter2] != '>')
                                {
                                    UrlImg = UrlImg + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                RunLine = RunLine.Remove(0, UrlImg.Length + 1);
                                Console.WriteLine(RunLine);

                                Image PastedImage = new Image();
                                WParagraph.Children.Add(PastedImage);
                                ChangeImage("PlayAudio.png", PastedImage);
                                PastedImage.Height = 20;
                                PastedImage.Height = 20;
                                PastedImage.Tag = UrlImg;
                                PastedImage.HorizontalAlignment = HorizontalAlignment.Left;
                                PastedImage.VerticalAlignment = VerticalAlignment.Top;
                                String ThisToolTip = UrlImg.Remove(0, UrlImg.LastIndexOf(@"\") + 1);
                                ThisToolTip = ThisToolTip.Remove(ThisToolTip.Length - 4, 4);
                                PastedImage.ToolTip = ThisToolTip;
                                DockPanel.SetDock(PastedImage, Dock.Top);
                                PastedImage.MouseUp += PlayAudio;
                            }
                            if (SymbolID == 4)
                            {

                                Label texted = new Label();
                                WParagraph.Children.Add(texted);
                                texted.FontSize = 14;
                                texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted.Content = CombineText;
                                texted.Padding = new Thickness(3, 5, 3, 5);
                                Console.WriteLine(RunLine);
                                RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length);
                                RunLine = RunLine.Remove(0, 5);
                                Console.WriteLine(RunLine);
                                int WhileCounter2 = 0;
                                String CreText = "";
                                while (RunLine[WhileCounter2] != '>')
                                {
                                    CreText = CreText + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                RunLine = RunLine.Remove(0, CreText.Length + 1);
                                Console.WriteLine(RunLine);

                                TextBlock texted2 = new TextBlock();
                                WParagraph.Children.Add(texted2);
                                texted2.FontSize = 14;
                                texted2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted2.Text = CreText;
                                texted2.Padding = new Thickness(3, 5, 3, 5);
                                texted2.TextDecorations = TextDecorations.Underline;
                                texted2.Tag = "Monster";
                                texted2.MouseUp += SM_LinkCiick;
                            }
                            if (SymbolID == 5)
                            {

                                Label texted = new Label();
                                WParagraph.Children.Add(texted);
                                texted.FontSize = 14;
                                texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted.Content = CombineText;
                                texted.Padding = new Thickness(3, 5, 3, 5);
                                Console.WriteLine(RunLine);
                                RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length);
                                RunLine = RunLine.Remove(0, 7);
                                Console.WriteLine(RunLine);
                                int WhileCounter2 = 0;
                                String CreText = "";
                                while (RunLine[WhileCounter2] != '>')
                                {
                                    CreText = CreText + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                RunLine = RunLine.Remove(0, CreText.Length + 1);
                                Console.WriteLine(RunLine);

                                TextBlock texted2 = new TextBlock();
                                WParagraph.Children.Add(texted2);
                                texted2.FontSize = 14;
                                texted2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted2.Text = CreText;
                                texted2.Padding = new Thickness(3, 5, 3, 5);
                                texted2.TextDecorations = TextDecorations.Underline;
                                texted2.Tag = "Spell";
                                texted2.MouseUp += SM_LinkCiick;
                            }
                            if (SymbolID == 6)
                            {

                                Label texted = new Label();
                                WParagraph.Children.Add(texted);
                                texted.FontSize = 14;
                                texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted.Content = CombineText;
                                texted.Padding = new Thickness(3, 5, 3, 5);
                                Console.WriteLine(RunLine);
                                RunLine = RunLine.Remove(RunLine.IndexOf(CombineText), CombineText.Length);
                                RunLine = RunLine.Remove(0, 6);
                                Console.WriteLine(RunLine);
                                int WhileCounter2 = 0;
                                String CreText = "";
                                while (RunLine[WhileCounter2] != '>')
                                {
                                    CreText = CreText + RunLine[WhileCounter2].ToString();
                                    WhileCounter2++;
                                }
                                RunLine = RunLine.Remove(0, CreText.Length + 1);
                                Console.WriteLine(RunLine);

                                TextBlock texted2 = new TextBlock();
                                WParagraph.Children.Add(texted2);
                                texted2.FontSize = 14;
                                texted2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                texted2.Text = CreText;
                                texted2.Padding = new Thickness(3, 5, 3, 5);
                                texted2.TextDecorations = TextDecorations.Underline;
                                texted2.Tag = "Char";
                                texted2.MouseUp += SM_LinkCiick;
                            }
                        }

                    }
                    CurrentRun = CurrentRun.NextInline;
                }
                CurrentParagraph = (CurrentParagraph.NextBlock as Paragraph);
            }
        }

        //Ссылки в Story Mode
        public void SM_LinkCiick(object sender, MouseButtonEventArgs e)
        { 
            if ((sender as TextBlock).Tag.ToString() == "Monster")
            {
                CreatureTempWindow CTW = new CreatureTempWindow();
                CTW.Title = (sender as TextBlock).Text;
                CTW.MC_BlckDiscr.Visibility = Visibility.Collapsed;
                CTW.MC_BlockDiscription.Inlines.Clear();
                CTW.MC_Uses.Children.Clear();
                CTW.MC_Skills.Children.Clear();
                CTW.MC_Actions.Children.Clear();
                CTW.MC_LegendaryActions.Children.Clear();
                CTW.MC_Ceep.Children.Clear();
                CTW.MC_CeepAction.Children.Clear();
                CTW.MC_CeepEffect.Children.Clear();
                CTW.MC_Reactions.Children.Clear();

                CTW.MC_UnderReactions.Visibility = Visibility.Collapsed;
                CTW.MC_Reactions.Visibility = Visibility.Collapsed;
                CTW.MC_UnderSkills.Visibility = Visibility.Collapsed;
                CTW.MC_Skills.Visibility = Visibility.Collapsed;
                CTW.MC_Actions.Visibility = Visibility.Collapsed;
                CTW.MC_UnderActions.Visibility = Visibility.Collapsed;
                CTW.MC_LegendaryActions.Visibility = Visibility.Collapsed;
                CTW.MC_Ceep.Visibility = Visibility.Collapsed;
                CTW.MC_CeepAction.Visibility = Visibility.Collapsed;
                CTW.MC_CeepEffect.Visibility = Visibility.Collapsed;
                CTW.MC_UnderLegendaryActions.Visibility = Visibility.Collapsed;
                CTW.MC_UnderCeep.Visibility = Visibility.Collapsed;
                CTW.MC_UnderCeepAction.Visibility = Visibility.Collapsed;
                CTW.MC_UnderCeepEffect.Visibility = Visibility.Collapsed;

                CTW.MC_Dicsription.Inlines.Clear();
                CTW.MC_Dicsription.Cursor = Cursors.Arrow;
                CTW.MC_Dicsription.MouseDown -= GoToLink;

                String[] TempArray = new string[7];
                DockPanel[] TempDockPanel = new DockPanel[7];

                TempArray[0] = "-СПОСОБНОСТИ-";
                TempArray[1] = "-ДЕЙСТВИЯ-";
                TempArray[2] = "-ЛЕГЕНДАРНЫЕ ДЕЙСТВИЯ-";
                TempArray[3] = "-ЛОГОВО-";
                TempArray[4] = "-ДЕЙСТВИЯ ЛОГОВА-";
                TempArray[5] = "-ЭФФЕКТЫ ЛОГОВА-";
                TempArray[6] = "-РЕАКЦИИ-";
                TempDockPanel[0] = CTW.MC_Skills;
                TempDockPanel[1] = CTW.MC_Actions;
                TempDockPanel[2] = CTW.MC_LegendaryActions;
                TempDockPanel[3] = CTW.MC_Ceep;
                TempDockPanel[4] = CTW.MC_CeepAction;
                TempDockPanel[5] = CTW.MC_CeepEffect;
                TempDockPanel[6] = CTW.MC_Reactions;

                for (int i = 0; i != 7; i++)
                {
                    Label UsesLbl = new Label();
                    TempDockPanel[i].Children.Add(UsesLbl);
                    UsesLbl.Height = MC_DiscrLabel.Height;
                    UsesLbl.Width = MC_DiscrLabel.Width;
                    UsesLbl.Foreground = MC_DiscrLabel.Foreground;
                    UsesLbl.Effect = MC_DiscrLabel.Effect;
                    UsesLbl.Content = TempArray[i];
                    DockPanel.SetDock(UsesLbl, Dock.Top);

                }

                int CreaID = 0;
                for (int i=0; i!= 5000; i++)
                {
                    if (LoadedCreatures[0, i] != null)
                    {
                        if (LoadedCreatures[0, i] == "<Name>"+(sender as TextBlock).Text) 
                        {
                            CreaID = i;
                        } 
                    }
                }

                String FilePath = LoadedCreatures[1, CreaID];

                for (int i = 0; i != CountOfFileLines(@"Bin\Data\Monsters\Addons.dnd"); i++)
                {
                    if (ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i) == FilePath.Remove(2))
                    {
                        FilePath = @"Bin\Data\Monsters\" + ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i - 1) + ".dnd";
                    }
                }
                String[] Lines = new String[1000];
                int MainCounter = 0;
                int DiscriptionStart = 0;

                for (int i = Int32.Parse(LoadedCreatures[1, CreaID].Remove(0, 3)); i != CountOfFileLines(FilePath); i++)
                {
                    if (ReadCertainLine(FilePath, i) == "<Discription>") DiscriptionStart = i + 1;
                    if (ReadCertainLine(FilePath, i) == "</X>") break;
                    else
                    {
                        Lines[MainCounter] = ReadCertainLine(FilePath, i);
                        MainCounter++;
                    }
                }
                for (int i = 1; i != MainCounter; i++)
                {
                    if (Lines[i].Contains("<Name>") == true)
                    {
                        CTW.MC_Name.Content = Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<IMG>") == true)
                    {
                        Image ImageContainer = CTW.MC_Preview;
                        ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Bin\img\Monsters\" + Lines[i].Remove(0, 5) + ".png", UriKind.Absolute));
                        ImageContainer.Source = image;
                        CTW.MC_Preview.Uid = Environment.CurrentDirectory + @"\Bin\img\Monsters\" + Lines[i].Remove(0, 5) + "_Full" + ".png";
                    }
                    if (Lines[i].Contains("<Mainlink>") == true)
                    {
                        CTW.LinkMain.Uid = Lines[i].Remove(0, 10);
                    }
                    if (Lines[i].Contains("<Size>") == true)
                    {
                        CTW.MC_TypeSizeView.Content = Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<Type>") == true)
                    {
                        CTW.MC_TypeSizeView.Content = CTW.MC_TypeSizeView.Content.ToString() + " " + Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<View>") == true)
                    {
                        CTW.MC_TypeSizeView.Content = CTW.MC_TypeSizeView.Content.ToString() + ", " + Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<Armor>") == true)
                    {
                        CTW.MC_Armor.Content = "Класс доспеха: " + Lines[i].Remove(0, 7);
                    }
                    if (Lines[i].Contains("<ArmorType>") == true)
                    {
                        CTW.MC_Armor.Content = CTW.MC_Armor.Content.ToString() + " " + Lines[i].Remove(0, 11);
                    }
                    if (Lines[i].Contains("<Hit>") == true)
                    {
                        CTW.MC_Hit.Content = Lines[i].Remove(0, 5);
                    }
                    if (Lines[i].Contains("<HitDiceCount>") == true)
                    {
                        CTW.MC_HitDice.Content = "(" + Lines[i].Remove(0, 14);
                    }
                    if (Lines[i].Contains("<HitDice>") == true)
                    {
                        CTW.MC_HitDice.Content = CTW.MC_HitDice.Content.ToString() + "d" + Lines[i].Remove(0, 9) + ")";
                    }
                    if (Lines[i].Contains("<HitStatic>") == true)
                    {
                        CTW.MC_HitDice.Content = CTW.MC_HitDice.Content.ToString().Remove(CTW.MC_HitDice.Content.ToString().Length - 1, 1) + " + " + Lines[i].Remove(0, 11) + ")";
                    }
                    if (Lines[i].Contains("<Walk>") == true)
                    {
                        CTW.MC_Speed.Content = "Скорость: " + Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<Swim>") == true)
                    {
                        CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Плавая: " + Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<Grab>") == true)
                    {
                        CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Лазая: " + Lines[i].Remove(0, 6);
                    }
                    if (Lines[i].Contains("<Dig>") == true)
                    {
                        CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Копая: " + Lines[i].Remove(0, 5);
                    }
                    if (Lines[i].Contains("<Fly>") == true)
                    {
                        CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Летая: " + Lines[i].Remove(0, 5);
                    }
                    if (Lines[i].Contains("<Sil>") == true)
                    {
                        CTW.MC_Sil.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                    }
                    if (Lines[i].Contains("<Lov>") == true)
                    {
                        CTW.MC_Lov.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                    }
                    if (Lines[i].Contains("<Tel>") == true)
                    {
                        CTW.MC_Tel.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                    }
                    if (Lines[i].Contains("<Int>") == true)
                    {
                        CTW.MC_Int.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                    }
                    if (Lines[i].Contains("<Mdr>") == true)
                    {
                        CTW.MC_Mdr.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                    }
                    if (Lines[i].Contains("<Har>") == true)
                    {
                        CTW.MC_Har.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                    }
                    if (Lines[i].Contains("<SpellTab>") == true)
                    {
                        int SpellCounter = i + 1;
                        while (Lines[SpellCounter] != "</SpellTab>")
                        {
                            TextBlock TB = new TextBlock();
                            CTW.MC_Skills.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(" * " + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[SpellCounter]) { FontStyle = FontStyles.Italic });
                            SpellCounter++;
                        }
                        i = SpellCounter;
                    }
                    if (Lines[i].Contains("<Chalange>") == true)
                    {
                        TextBlock TB = new TextBlock();
                        CTW.MC_Uses.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run("Опасность:" + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 10)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<Source>") == true)
                    {
                        TextBlock TB = new TextBlock();
                        CTW.MC_Uses.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run("Источник:" + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 8)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<j1>") == true)
                    {
                        int end = Lines[i].Remove(0, 4).IndexOf("<j1>") + 4;
                        TextBlock TB = new TextBlock();
                        CTW.MC_Uses.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                        Console.WriteLine(Lines[i]);
                    }
                    if (Lines[i].Contains("<j2>") == true)
                    {
                        CTW.MC_UnderSkills.Visibility = Visibility.Visible;
                        CTW.MC_Skills.Visibility = Visibility.Visible;
                        int end = Lines[i].Remove(0, 4).IndexOf("<j2>") + 4;
                        TextBlock TB = new TextBlock();
                        CTW.MC_Skills.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<j3>") == true)
                    {
                        CTW.MC_UnderActions.Visibility = Visibility.Visible;
                        CTW.MC_Actions.Visibility = Visibility.Visible;
                        int end = Lines[i].Remove(0, 4).IndexOf("<j3>") + 4;
                        TextBlock TB = new TextBlock();
                        CTW.MC_Actions.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<jr>") == true)
                    {
                        CTW.MC_UnderReactions.Visibility = Visibility.Visible;
                        CTW.MC_Reactions.Visibility = Visibility.Visible;
                        int end = Lines[i].Remove(0, 4).IndexOf("<jr>") + 4;
                        TextBlock TB = new TextBlock();
                        CTW.MC_Reactions.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<jB>") == true)
                    {
                        String Headd = Lines[i];
                        String Content = Lines[i];

                        Headd = Headd.Remove(0, 4);
                        Headd = Headd.Remove(Headd.IndexOf(">") + 1);
                        Headd = Headd.Remove(Headd.Length - 4, 4);

                        Content = Content.Remove(0, 4);
                        Content = Content.Remove(0, Content.IndexOf(">") + 1);
                        if (Content.Contains("[j]") == true)
                        {
                            String Content2 = Content;
                            Content2 = Content2.Remove(Content2.IndexOf("[j]"));
                            CTW.MC_BlckDiscr.Visibility = Visibility.Visible;
                            CTW.MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                            CTW.MC_BlockDiscription.Inlines.Add("\n");
                            CTW.MC_BlockDiscription.Inlines.Add(Content2);
                            Content2 = Content;
                            Content2 = Content2.Remove(0, Content2.IndexOf("[j]"));
                            String jHead = Content2;
                            jHead = jHead.Remove(0, 3);
                            jHead = jHead.Remove(jHead.IndexOf("[/j]"));
                            CTW.MC_BlockDiscription.Inlines.Add("\n");
                            CTW.MC_BlockDiscription.Inlines.Add(new Run(jHead) { FontWeight = FontWeights.Bold });
                            CTW.MC_BlockDiscription.Inlines.Add("\n");
                            Content2 = Content2.Remove(0, Content2.IndexOf("[/j]") + 4);
                            CTW.MC_BlockDiscription.Inlines.Add(Content2);
                        }
                        else
                        {
                            CTW.MC_BlckDiscr.Visibility = Visibility.Visible;
                            CTW.MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                            CTW.MC_BlockDiscription.Inlines.Add("\n");
                            CTW.MC_BlockDiscription.Inlines.Add(Content);
                        }
                    }
                    if (Lines[i].Contains("<jBT>") == true)
                    {
                        String Headd = Lines[i];
                        String Content = Lines[i];

                        CTW.MC_BlockDiscription.Inlines.Add("\n");
                        Headd = Headd.Remove(0, 5);
                        Content = Headd.Remove(0, Headd.IndexOf("<jBT>") + 5);
                        Headd = Headd.Remove(Headd.IndexOf("<jBT>"));
                        CTW.MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                        CTW.MC_BlockDiscription.Inlines.Add(Content);
                    }
                    if (Lines[i].Contains("<Legendary>") == true)
                    {
                        CTW.MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                        CTW.MC_LegendaryActions.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_LegendaryActions.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 11)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<lg>") == true)
                    {
                        MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                        MC_LegendaryActions.Visibility = Visibility.Visible;

                        int end = Lines[i].Remove(0, 4).IndexOf("<lg>") + 4;
                        CTW.MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                        CTW.MC_LegendaryActions.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_LegendaryActions.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<ceep>") == true)
                    {
                        CTW.MC_UnderCeep.Visibility = Visibility.Visible;
                        CTW.MC_Ceep.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_Ceep.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 6)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<ca>") == true)
                    {
                        CTW.MC_UnderCeepAction.Visibility = Visibility.Visible;
                        CTW.MC_CeepAction.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_CeepAction.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<dca>") == true)
                    {
                        CTW.MC_UnderCeepAction.Visibility = Visibility.Visible;
                        CTW.MC_CeepAction.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_CeepAction.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(" * " + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 5)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<ce>") == true)
                    {
                        CTW.MC_UnderCeepEffect.Visibility = Visibility.Visible;
                        CTW.MC_CeepEffect.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_CeepEffect.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 4)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i].Contains("<dce>") == true)
                    {
                        CTW.MC_UnderCeepEffect.Visibility = Visibility.Visible;
                        CTW.MC_CeepEffect.Visibility = Visibility.Visible;
                        TextBlock TB = new TextBlock();
                        CTW.MC_CeepEffect.Children.Add(TB);
                        DockPanel.SetDock(TB, Dock.Top);
                        TB.TextWrapping = TextWrapping.Wrap;
                        TB.Inlines.Add(new Run(" * " + " ") { FontWeight = FontWeights.Bold });
                        TB.Inlines.Add(new Run(Lines[i].Remove(0, 5)) { FontStyle = FontStyles.Italic });
                    }
                    if (Lines[i] == "<Discription>")
                    {
                        for (int i2 = i + 1; i2 != MainCounter; i2++)
                        {
                            bool Special = false;
                            if (Lines[i2].Contains("<L>") == true)
                            {
                                CTW.MC_Dicsription.TextWrapping = TextWrapping.Wrap;
                                CTW.MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3).Remove(Lines[i2].Remove(0, 3).IndexOf("<"))) { TextDecorations = TextDecorations.Underline });

                                CTW.MC_Dicsription.Cursor = Cursors.Hand;
                                String s = Lines[i2].Remove(0, 3);
                                s = s.Remove(0, s.IndexOf("<") + 1);
                                s = s.Remove(s.Length - 1);
                                CTW.MC_Dicsription.Uid = s;
                                CTW.MC_Dicsription.MouseUp += GoToLink;
                                Special = true;
                            }
                            if (Lines[i2].Contains("<j>") == true)
                            {
                                CTW.MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3)) { FontWeight = FontWeights.Bold });
                                Special = true;
                            }
                            if (Lines[i2].Contains("<h>") == true)
                            {
                                CTW.MC_Dicsription.Inlines.Add("\n");
                                CTW.MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3)) { FontWeight = FontWeights.Bold });
                                Special = true;
                            }
                            if (Special == false)
                            {
                                CTW.MC_Dicsription.Inlines.Add(Lines[i2]);
                            }
                        }
                        i = MainCounter - 1;
                    }
                }
                CTW.Show();
            }
            if ((sender as TextBlock).Tag.ToString() == "Spell")
            {
                SpellNewWindow SNW = new SpellNewWindow();
                String Way = @"Bin\Data\Spells\PHB.dnd";
                String SpellName = "";
                SpellName = (sender as TextBlock).Text;

                int NewFileRow = 0;
                SNW.SL_SpellName.Content = SpellName;
                for (int i=0; i != 5000; i++)
                {
                    if (LoadedSpells[0,i]!= null)
                    {
                        if (LoadedSpells[0, 1] == (sender as TextBlock).Text) NewFileRow = Int32.Parse(LoadedSpells[1, i]);
                    }
                }

                SNW.FileRow = NewFileRow;
                for (int i = NewFileRow; i != CountOfFileLines(Way); i++)
                {
                    if (ReadCertainLine(Way, i) == "<Rus>" + SpellName)
                    {
                        int WhileCounter = i;

                        while (ReadCertainLine(Way, WhileCounter) != "<End>")
                        {
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Level>") != -1)
                            {
                                SNW.SL_SpellLevel.Content = "Уровень: " + ReadCertainLine(Way, WhileCounter).Remove(0, 7);
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<School>") != -1)
                            {
                                SNW.SL_SpellSchool.Content = "Школа: " + ReadCertainLine(Way, WhileCounter).Remove(0, 8);
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Casttime>") != -1)
                            {
                                SNW.SL_Spellcasttime.Text = "Время накладывания: " + ReadCertainLine(Way, WhileCounter).Remove(0, 10);
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Distance>") != -1)
                            {
                                SNW.SL_SpellDistance.Content = "Дистанция: " + ReadCertainLine(Way, WhileCounter).Remove(0, 10);
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Components>") != -1)
                            {
                                SNW.SL_SpellComponents.Text = "Компоненты: " + ReadCertainLine(Way, WhileCounter).Remove(0, 12);
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Time>") != -1)
                            {
                                SNW.SL_SpellTime.Content = "Длительность: " + ReadCertainLine(Way, WhileCounter).Remove(0, 6);
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Class>") != -1)
                            {
                                SNW.SL_SpellClass.Content = "Классы: " + ReadCertainLine(Way, WhileCounter).Remove(0, 7);
                            }

                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Line>") != -1)
                            {
                                WrapPanel DiscriptionDP = new WrapPanel();
                                SNW.SpellDiscription.Children.Add(DiscriptionDP);
                                DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                                DiscriptionDP.Width = Double.NaN;
                                DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                                DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                                DiscriptionDP.Orientation = Orientation.Horizontal;
                                DockPanel.SetDock(DiscriptionDP, Dock.Top);
                                DiscriptionDP.Height = Double.NaN;

                                String ThisLine = ReadCertainLine(Way, WhileCounter);
                                ThisLine = ThisLine.Remove(0, 6);

                                int SymbolID = 0;
                                while (ThisLine.Length != 0)
                                {
                                    String CombineText = "";
                                    for (int o = 0; o != ThisLine.Length; o++)
                                    {
                                        if (ThisLine[o] == ' ')
                                        {
                                            SymbolID = 1;
                                            break;
                                        }
                                        if (ThisLine[o] == '<')
                                        {
                                            if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                            if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                            if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                            break;
                                        }
                                        if (ThisLine[o] == '$')
                                        {
                                            SymbolID = 5;
                                            break;
                                        }

                                        CombineText = CombineText + ThisLine[o];
                                    }
                                    if (SymbolID == 1)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    }
                                    if (SymbolID == 2)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        Console.WriteLine(CombineText);

                                        ThisLine = ThisLine.Remove(0, 3);

                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '>')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = CombineText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.TextDecorations = TextDecorations.Underline;
                                        CombineText = CombineText + ">";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                        Console.WriteLine(CombineText);
                                        Console.WriteLine(ThisLine);
                                    }
                                    if (SymbolID == 3)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        ThisLine = ThisLine.Remove(0, 3);
                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '>')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        String NewCombine = CombineText;
                                        while (CombineText.Length != 0)
                                        {
                                            string LinkedText = "";
                                            for (int o = 0; o != CombineText.Length; o++)
                                            {
                                                if (CombineText[o] == ' ') break;
                                                LinkedText = LinkedText + CombineText[o];
                                            }
                                            TextBlock TextBl = new TextBlock();
                                            DiscriptionDP.Children.Add(TextBl);
                                            TextBl.FontSize = 14;
                                            TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                            TextBl.Text = LinkedText;
                                            TextBl.Padding = new Thickness(3, 5, 3, 5);
                                            TextBl.FontWeight = FontWeights.Bold;
                                            if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                            else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                        }

                                        NewCombine = NewCombine + ">";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                    }
                                    if (SymbolID == 4)
                                    {
                                        Image texted = new Image();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.Height = 10;
                                        texted.Width = 10;
                                        texted.Margin = new Thickness(15, 0, 15, 0);
                                        ChangeImage("Seporator.png", texted);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                    }
                                    if (SymbolID == 5)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        Console.WriteLine(CombineText);

                                        ThisLine = ThisLine.Remove(0, 1);

                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '$')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        //CombineText;
                                        for (int ss = WhileCounter; ss != CountOfFileLines(Way); ss++)
                                        {
                                            if (ReadCertainLine(Way, ss).IndexOf("#" + CombineText) != -1)
                                            {
                                                String[] ThisCells = new String[9];
                                                int CellsID = 0;
                                                String EditableLine = ReadCertainLine(Way, ss).Remove(0, CombineText.Length + 1);

                                                while (EditableLine.Length != 0)
                                                {
                                                    for (int pos = 0; pos != EditableLine.Length; pos++)
                                                    {
                                                        if (EditableLine[pos] == ':')
                                                        {
                                                            EditableLine = EditableLine.Remove(0, ThisCells[CellsID].Length + 1);
                                                            CellsID++;
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            ThisCells[CellsID] = ThisCells[CellsID] + EditableLine[pos].ToString();
                                                        }
                                                    }
                                                }
                                                TextBlock TextBl2 = new TextBlock();
                                                DiscriptionDP.Children.Add(TextBl2);
                                                TextBl2.FontSize = 14;
                                                TextBl2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                                TextBl2.Text = ThisCells[0];
                                                TextBl2.Padding = new Thickness(3, 5, 3, 5);
                                                TextBl2.FontWeight = FontWeights.Bold;
                                                break;
                                            }
                                        }
                                        CombineText = CombineText + "$";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    }
                                }
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<Table>") != -1)
                            {
                                Grid newTable = new Grid();
                                NPCG_DiscriptionDP.Children.Add(newTable);
                                newTable.HorizontalAlignment = HorizontalAlignment.Stretch;
                                newTable.Width = Double.NaN;
                                newTable.VerticalAlignment = VerticalAlignment.Top;
                                newTable.Margin = new Thickness(25, 0, 25, 0);
                                newTable.Height = 100;
                                DockPanel.SetDock(newTable, Dock.Top);

                                for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); ti++)
                                {
                                    RowDefinition RW = new RowDefinition();
                                    newTable.RowDefinitions.Add(RW);
                                }
                                for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); ti++)
                                {
                                    ColumnDefinition CW = new ColumnDefinition();
                                    newTable.ColumnDefinitions.Add(CW);
                                }

                                String ThiLin = ReadCertainLine(Way, WhileCounter);
                                ThiLin = ThiLin.Remove(0, 9);
                                for (int Ri = 0; Ri != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); Ri++)
                                {
                                    for (int Ci = 0; Ci != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); Ci++)
                                    {
                                        Border Brdr = new Border();
                                        newTable.Children.Add(Brdr);
                                        Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                                        Brdr.VerticalAlignment = VerticalAlignment.Stretch;
                                        Brdr.Height = Double.NaN;
                                        Brdr.Width = Double.NaN;
                                        Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        Brdr.BorderThickness = new Thickness(1, 1, 1, 1);
                                        Grid.SetColumnSpan(Brdr, 1);
                                        Grid.SetRowSpan(Brdr, 1);
                                        Grid.SetRow(Brdr, Ri);
                                        Grid.SetColumn(Brdr, Ci);
                                        Label Lbl = new Label();
                                        newTable.Children.Add(Lbl);
                                        Lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                                        Lbl.VerticalAlignment = VerticalAlignment.Stretch;
                                        Lbl.Height = Double.NaN;
                                        Lbl.Width = Double.NaN;
                                        Lbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        Grid.SetColumnSpan(Lbl, 1);
                                        Grid.SetRowSpan(Lbl, 1);
                                        Grid.SetRow(Lbl, Ri);
                                        Grid.SetColumn(Lbl, Ci);
                                        Lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                                        Lbl.VerticalContentAlignment = VerticalAlignment.Center;
                                        int ss = 0;
                                        String AddingLine = "";
                                        if (ThiLin.Length != 0)
                                        {
                                            while (ThiLin[ss] != ';')
                                            {
                                                AddingLine = AddingLine + ThiLin[ss].ToString();
                                                ss++;
                                            }
                                        }
                                        Lbl.Content = AddingLine;
                                        ThiLin = ThiLin.Remove(0, ThiLin.IndexOf(';') + 1);
                                        Console.WriteLine(AddingLine);
                                        Console.WriteLine(ThiLin);
                                    }
                                }
                            }
                            if (ReadCertainLine(Way, WhileCounter).IndexOf("<CellsLevel>") != -1)
                            {
                                WrapPanel DiscriptionDP = new WrapPanel();
                                SNW.SpellDiscription.Children.Add(DiscriptionDP);
                                DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                                DiscriptionDP.Width = Double.NaN;
                                DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                                DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                                DiscriptionDP.Orientation = Orientation.Horizontal;
                                DockPanel.SetDock(DiscriptionDP, Dock.Top);
                                DiscriptionDP.Height = Double.NaN;

                                String ThisLine = ReadCertainLine(Way, WhileCounter);
                                ThisLine = ThisLine.Remove(0, 12);

                                int SymbolID = 0;
                                while (ThisLine.Length != 0)
                                {
                                    String CombineText = "";
                                    for (int o = 0; o != ThisLine.Length; o++)
                                    {
                                        if (ThisLine[o] == ' ')
                                        {
                                            SymbolID = 1;
                                            break;
                                        }
                                        if (ThisLine[o] == '<')
                                        {
                                            if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                            if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                            if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                            break;
                                        }
                                        CombineText = CombineText + ThisLine[o];
                                    }
                                    if (SymbolID == 1)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    }
                                    if (SymbolID == 2)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        Console.WriteLine(CombineText);

                                        ThisLine = ThisLine.Remove(0, 3);

                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '>')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = CombineText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.TextDecorations = TextDecorations.Underline;
                                        CombineText = CombineText + ">";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                        Console.WriteLine(CombineText);
                                        Console.WriteLine(ThisLine);
                                    }
                                    if (SymbolID == 3)
                                    {
                                        Label texted = new Label();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.FontSize = 14;
                                        texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        texted.Content = CombineText;
                                        texted.Padding = new Thickness(3, 5, 3, 5);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                        ThisLine = ThisLine.Remove(0, 3);
                                        CombineText = "";
                                        int WC = 0;
                                        while (ThisLine[WC] != '>')
                                        {
                                            CombineText = CombineText + ThisLine[WC];
                                            WC++;
                                        }
                                        String NewCombine = CombineText;
                                        while (CombineText.Length != 0)
                                        {
                                            string LinkedText = "";
                                            for (int o = 0; o != CombineText.Length; o++)
                                            {
                                                if (CombineText[o] == ' ') break;
                                                LinkedText = LinkedText + CombineText[o];
                                            }
                                            TextBlock TextBl = new TextBlock();
                                            DiscriptionDP.Children.Add(TextBl);
                                            TextBl.FontSize = 14;
                                            TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                            TextBl.Text = LinkedText;
                                            TextBl.Padding = new Thickness(3, 5, 3, 5);
                                            TextBl.FontWeight = FontWeights.Bold;
                                            if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                            else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                        }

                                        NewCombine = NewCombine + ">";
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                    }
                                    if (SymbolID == 4)
                                    {
                                        Image texted = new Image();
                                        DiscriptionDP.Children.Add(texted);
                                        texted.Height = 10;
                                        texted.Width = 10;
                                        texted.Margin = new Thickness(15, 0, 15, 0);
                                        ChangeImage("Seporator.png", texted);
                                        ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                    }
                                }
                            }

                            WhileCounter++;
                        }
                    }
                }
                SNW.SL_SpellSource.Content = "Источник: Player's Handsbook";
                SNW.Title = (sender as TextBlock).Text;
                SNW.Show();
            }
            if ((sender as TextBlock).Tag.ToString() == "Char")
            {
                HideAllTopButtons();
                Pages.SelectedIndex = 4;
                CL_Chars.Children.Clear();
                CL_CharSpace.Visibility = Visibility.Collapsed;
                CL_CharList.Width = 260;
                CL_Chars.Visibility = Visibility.Visible;
                CL_CharList.Background = new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i].IndexOf("<PlayerChar>") != -1)
                    {
                        Label Chars = new Label();
                        CL_Chars.Children.Add(Chars);
                        Chars.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                        Chars.Background = new SolidColorBrush(Color.FromArgb(50, 76, 76, 149));
                        DockPanel.SetDock(Chars, Dock.Top);
                        Chars.HorizontalContentAlignment = HorizontalAlignment.Center;
                        Chars.HorizontalAlignment = HorizontalAlignment.Stretch;
                        Chars.Width = Double.NaN;
                        Chars.Content = LoadedProfile[i].Remove(0, 12);
                        Chars.Margin = new Thickness(5, 5, 5, 0);
                    }
                }
                Label NewChar = new Label();
                CL_Chars.Children.Add(NewChar);
                NewChar.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                NewChar.Background = new SolidColorBrush(Color.FromArgb(51, 162, 162, 162));
                DockPanel.SetDock(NewChar, Dock.Top);
                NewChar.HorizontalContentAlignment = HorizontalAlignment.Center;
                NewChar.HorizontalAlignment = HorizontalAlignment.Stretch;
                NewChar.Width = Double.NaN;
                NewChar.Content = "Новый персонаж";
                NewChar.Margin = new Thickness(5, 5, 5, 0);
                NewChar.MouseEnter += CL_NewChar_MouseEnter;
                NewChar.MouseLeave += CL_NewChar_MouseLeave;
                NewChar.MouseDown += CL_NewChar_MouseDown;
            }
        }

        private void SM_GeneratePreview_MouseEnter(object sender, MouseEventArgs e)
        {
            SM_GeneratePreview.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
        }

        private void SM_GeneratePreview_MouseLeave(object sender, MouseEventArgs e)
        {
            SM_GeneratePreview.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
        }

        private void SM_AddCombo_DropDownOpened(object sender, EventArgs e)
        {
            (SM_AddCombo.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
        }

        private void SM_AddCombo_DropDownClosed(object sender, EventArgs e)
        {
            Paragraph CurrentParagraph = (SM_StoryEditor.Document.Blocks.FirstBlock as Paragraph);
            Inline CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);

            if (SM_AddCombo.SelectedIndex == 1)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".png"; // Default file extension
                dlg.Filter = "Png (*.png)|*.png|Jpg (*.jpg)|*.jpg";
                Nullable<bool> result = dlg.ShowDialog();

                for (int pi = 0; pi != SM_StoryEditor.Document.Blocks.Count; pi++)
                {
                    CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
                    for (int ri = 0; ri != CurrentParagraph.Inlines.Count; ri++)
                    {
                        if (CurrentRun.GetType().ToString() == "System.Windows.Documents.Run")
                        {
                            if ((CurrentRun as Run).Text.IndexOf(TPP.GetTextInRun(LogicalDirection.Forward)) != -1)
                            {
                                String TempLine = (CurrentRun as Run).Text;
                                if (TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) == 0) TempLine = TempLine + "<Image " + dlg.FileName + ";200;200>";
                                else TempLine = TempLine.Insert(TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)), "<Image " + dlg.FileName + ";200;200>");
                                (CurrentRun as Run).Text = TempLine;
                            }
                        }
                        CurrentRun = CurrentRun.NextInline;
                    }
                    CurrentParagraph = (CurrentParagraph.NextBlock as Paragraph);
                }
            }
            if (SM_AddCombo.SelectedIndex == 2)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".mp3"; // Default file extension
                dlg.Filter = "Mp3 (*.mp3)|*.mp3|Wav (*.wav)|*.wav";
                Nullable<bool> result = dlg.ShowDialog();

                for (int pi = 0; pi != SM_StoryEditor.Document.Blocks.Count; pi++)
                {
                    CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
                    for (int ri = 0; ri != CurrentParagraph.Inlines.Count; ri++)
                    {
                        if (CurrentRun.GetType().ToString() == "System.Windows.Documents.Run")
                        {
                            if ((CurrentRun as Run).Text.IndexOf(TPP.GetTextInRun(LogicalDirection.Forward)) != -1)
                            {
                                String TempLine = (CurrentRun as Run).Text;
                                if (TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) == 0) TempLine = TempLine + "<Audio " + dlg.FileName + ">";
                                else TempLine = TempLine.Insert(TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)), "<Audio " + dlg.FileName + ">");
                                (CurrentRun as Run).Text = TempLine;
                            }
                        }
                        CurrentRun = CurrentRun.NextInline;
                    }
                    CurrentParagraph = (CurrentParagraph.NextBlock as Paragraph);
                }
            }
            (SM_AddCombo.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
            SM_AddCombo.SelectedIndex = 0;
        }

        public void PlayAudio(object sender, MouseButtonEventArgs e)
        {
            Thickness PlayT = new Thickness(1, 0, 0, 0);
            Thickness StopT = new Thickness(0, 0, 0, 0);

            if ((sender as Image).Margin == StopT)
            {
                MyPlayer.Open(new Uri((sender as Image).Tag.ToString()));
                MyPlayer.Play();
                ChangeImage("StopAudio.png", (sender as Image));
                (sender as Image).Margin = PlayT;
            }
            else
            {
                MyPlayer.Stop();
                (sender as Image).Margin = StopT;
                ChangeImage("PlayAudio.png", (sender as Image));
            }
        }

        private void SM_LoadCombo_DropDownOpened(object sender, EventArgs e)
        {
            (SM_LoadCombo.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
        }

        private void SM_LoadCombo_DropDownClosed(object sender, EventArgs e)
        {
            if (SM_LoadCombo.SelectedIndex == 2)
            {
                SM_AddToStoryGrid.Visibility = Visibility.Visible;
                StoryEditorWindow.IsEnabled = false;
                SM_AddingDP.Children.Clear();
                int i = 0;
                while (LoadedCreatures[0, i] != null)
                {
                    Label LastLbl = null;
                    LastLbl = SM_CreateLink(LoadedCreatures[0, i].Remove(0, 6));
                    LastLbl.Uid = LoadedCreatures[1, i];
                    i++;
                }
            }
            if (SM_LoadCombo.SelectedIndex == 3)
            {
                SM_AddToStoryGrid.Visibility = Visibility.Visible;
                StoryEditorWindow.IsEnabled = false;
                SM_AddingDP.Children.Clear();
                int i = 0;
                while (LoadedSpells[0, i] != null)
                {
                    SM_CreateSpell(LoadedSpells[0, i].Remove(0, 5), LoadedSpells[1, i]);
                    i++;
                }
            }
            if (SM_LoadCombo.SelectedIndex == 4)
            {
                SM_AddToStoryGrid.Visibility = Visibility.Visible;
                StoryEditorWindow.IsEnabled = false;
                SM_AddingDP.Children.Clear();
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i].IndexOf("<PlayerChar>") != -1)
                    {
                        Label Chars = new Label();
                        SM_AddingDP.Children.Add(Chars);
                        Chars.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                        Chars.Background = new SolidColorBrush(Color.FromArgb(50, 76, 76, 149));
                        DockPanel.SetDock(Chars, Dock.Top);
                        Chars.HorizontalContentAlignment = HorizontalAlignment.Center;
                        Chars.HorizontalAlignment = HorizontalAlignment.Stretch;
                        Chars.Width = Double.NaN;
                        Chars.Content = LoadedProfile[i].Remove(0, 12);
                        Chars.Margin = new Thickness(5, 5, 5, 0);
                        Chars.MouseUp += SM_AddChar;
                    }
                }
            }
            if (SM_AddingDP.Children.Count == 0)
            {
                MessageBox.Show("Нет сохраненных данных");
                SM_AddToStoryGrid.Visibility = Visibility.Collapsed;
                StoryEditorWindow.IsEnabled = true;
                SM_AddingDP.Children.Clear();
            }
            (SM_LoadCombo.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
            SM_LoadCombo.SelectedIndex = 0;
        }

        public Label SM_CreateLink(String Text)
        {
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(1, 1);
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(81, 81, 81), 1.0));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(31, 31, 31), 0.0));

            Label Temp = new Label();
            SM_AddingDP.Children.Add(Temp);
            DockPanel.SetDock(Temp, Dock.Top);
            Temp.Height = 25;
            Temp.HorizontalAlignment = HorizontalAlignment.Stretch;
            Temp.Margin = new Thickness(0, 0, 0, 0);
            Temp.FontFamily = new FontFamily("Bookman Old Style");
            Temp.FontSize = 12;
            Temp.Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            Temp.Background = myLinearGradientBrush;
            Temp.Content = Text;
            Temp.HorizontalContentAlignment = HorizontalAlignment.Center;
            Temp.MouseDown += SM_CreatureClick;
            return Temp;
        }

        public void SM_CreateSpell(String Content, String Tag)
        {
            Label TB = new Label();
            SM_AddingDP.Children.Add(TB);
            TB.Content = Content;
            DockPanel.SetDock(TB, Dock.Top);
            TB.FontSize = 14;
            TB.HorizontalAlignment = HorizontalAlignment.Stretch;
            TB.HorizontalContentAlignment = HorizontalAlignment.Center;
            TB.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            TB.Tag = Tag;
            if (NewSpellValue == true)
            {
                TB.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                NewSpellValue = false;
            }
            else
            {
                TB.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                NewSpellValue = true;
            }
            TB.MouseUp += SM_AddSpell;
        }

        public void SM_CreatureClick(object sender, MouseEventArgs e)
        {
            Paragraph CurrentParagraph = (SM_StoryEditor.Document.Blocks.FirstBlock as Paragraph);
            Inline CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);

            for (int pi = 0; pi != SM_StoryEditor.Document.Blocks.Count; pi++)
            {
                CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
                for (int ri = 0; ri != CurrentParagraph.Inlines.Count; ri++)
                {
                    if (CurrentRun.GetType().ToString() == "System.Windows.Documents.Run")
                    {
                        if ((CurrentRun as Run).Text.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) != -1)
                        {
                            String TempLine = (CurrentRun as Run).Text;
                            if (TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) == 0) TempLine = TempLine + "<Crt " +(sender as Label).Content.ToString()+">";
                            else TempLine = TempLine.Insert(TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)), "<Crt " + (sender as Label).Content.ToString() + ">");
                            (CurrentRun as Run).Text = TempLine;
                        }
                    }
                    CurrentRun = CurrentRun.NextInline;
                }
                CurrentParagraph = (CurrentParagraph.NextBlock as Paragraph);
            }
            SM_AddToStoryGrid.Visibility = Visibility.Collapsed;
            StoryEditorWindow.IsEnabled = true;
        }

        public void SM_AddChar(object sender, MouseEventArgs e)
        {
            Paragraph CurrentParagraph = (SM_StoryEditor.Document.Blocks.FirstBlock as Paragraph);
            Inline CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);

            for (int pi = 0; pi != SM_StoryEditor.Document.Blocks.Count; pi++)
            {
                CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
                for (int ri = 0; ri != CurrentParagraph.Inlines.Count; ri++)
                {
                    if (CurrentRun.GetType().ToString() == "System.Windows.Documents.Run")
                    {
                        if ((CurrentRun as Run).Text.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) != -1)
                        {
                            String TempLine = (CurrentRun as Run).Text;
                            if (TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) == 0) TempLine = TempLine + "<Pers " + (sender as Label).Content.ToString() + ">";
                            else TempLine = TempLine.Insert(TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)), "<Pers " + (sender as Label).Content.ToString() + ">");
                            (CurrentRun as Run).Text = TempLine;
                        }
                    }
                    CurrentRun = CurrentRun.NextInline;
                }
                CurrentParagraph = (CurrentParagraph.NextBlock as Paragraph);
            }
            SM_AddToStoryGrid.Visibility = Visibility.Collapsed;
            StoryEditorWindow.IsEnabled = true;
        }

        public void SM_AddSpell(object sender, MouseEventArgs e)
        {
            Paragraph CurrentParagraph = (SM_StoryEditor.Document.Blocks.FirstBlock as Paragraph);
            Inline CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);

            for (int pi = 0; pi != SM_StoryEditor.Document.Blocks.Count; pi++)
            {
                CurrentRun = (CurrentParagraph.Inlines.FirstInline as Run);
                for (int ri = 0; ri != CurrentParagraph.Inlines.Count; ri++)
                {
                    if (CurrentRun.GetType().ToString() == "System.Windows.Documents.Run")
                    {
                        if ((CurrentRun as Run).Text.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) != -1)
                        {
                            String TempLine = (CurrentRun as Run).Text;
                            if (TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)) == 0) TempLine = TempLine + "<Spell " + (sender as Label).Content.ToString() + ">";
                            else TempLine = TempLine.Insert(TempLine.IndexOf(SM_StoryEditor.CaretPosition.GetTextInRun(LogicalDirection.Forward)), "<Spell " + (sender as Label).Content.ToString() + ">");
                            (CurrentRun as Run).Text = TempLine;
                        }
                    }
                    CurrentRun = CurrentRun.NextInline;
                }
                CurrentParagraph = (CurrentParagraph.NextBlock as Paragraph);
            }
            SM_AddToStoryGrid.Visibility = Visibility.Collapsed;
            StoryEditorWindow.IsEnabled = true;
        }

        private void ToGeneratorLbl6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            Pages.SelectedIndex = 6;
            SM_SceneListSB.Children.Clear();
            NowOpenedStory = "";
            SM_StoryNameDP.Visibility = Visibility.Collapsed;
            SM_Storys.IsEnabled = true;
            SM_TopGrid.IsEnabled = true;
            SM_StoryNameDP.Visibility = Visibility.Collapsed;
            MC_StoryNameLbl.Visibility = Visibility.Visible;
            MS_StoryNameEdt.Visibility = Visibility.Collapsed;
            MC_StoryNameLbl.Content = "Название истории";
            SM_Lbl2.Content = "Добро пожаловать в редактор историй";
            SM_Lbl1.Content = "Для начала работы выберете историю или создайте новую.";
            SM_SceneListSB.Children.Clear();

            bool Chet = false;
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i].IndexOf("<Story>") != -1)
                {
                    Label StoryNames = new Label();
                    SM_SceneListSB.Children.Add(StoryNames);
                    StoryNames.HorizontalAlignment = HorizontalAlignment.Stretch;
                    DockPanel.SetDock(StoryNames, Dock.Top);
                    StoryNames.HorizontalContentAlignment = HorizontalAlignment.Center;
                    StoryNames.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));

                    if (Chet == true)
                    {
                        StoryNames.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                        Chet = false;
                    }
                    else
                    {
                        StoryNames.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                        Chet = true;
                    }
                    StoryNames.Content = LoadedProfile[i].Remove(0, 7);
                    ContextMenu CMX = new ContextMenu();
                    MenuItem MI = new MenuItem();
                    CMX.Items.Add(MI);
                    MI.Header = "Удалить";
                    StoryNames.ContextMenu = CMX;
                    MI.Click += DeleteStory;
                    StoryNames.MouseUp += StoryClick;
                }
            }
            if (SM_SceneListSB.Children.Count == 0)
            {
                Label StoryNames = new Label();
                SM_SceneListSB.Children.Add(StoryNames);
                StoryNames.HorizontalAlignment = HorizontalAlignment.Stretch;
                DockPanel.SetDock(StoryNames, Dock.Top);
                StoryNames.HorizontalContentAlignment = HorizontalAlignment.Center;
                StoryNames.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                if (Chet == true)
                {
                    StoryNames.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                    Chet = false;
                }
                else
                {
                    StoryNames.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                    Chet = true;
                }
                StoryNames.Content = "Нет доступных историй";
            }
        }

        public void DeleteStory(object sender, RoutedEventArgs e)
        {
            if (DeletetStory.Content.ToString() == MC_StoryNameLbl.Content.ToString())
            {
                SM_StoryNameDP.Visibility = Visibility.Collapsed;
            }         
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i] == "<Story>" + DeletetStory.Content.ToString())
                {
                    LoadedProfile[i] = "<NULLSTRING>";
                }
                if (LoadedProfile[i] == "<Scene>" + DeletetStory.Content.ToString())
                {
                    int o = i;
                    while (LoadedProfile[o] != "<SceneEnd>")
                    {
                        LoadedProfile[o] = "<NULLSTRING>";
                        o++;
                    }
                    LoadedProfile[o] = "<NULLSTRING>";
                }
            }
            ToGeneratorLbl6_MouseUp(null, null);
        }

        private void WP_LoadProfile_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
        }

        private void WP_LoadProfile_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
        }

        private void WP_CreateProfile_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(25, 81, 255, 0));
        }

        private void WP_CreateProfile_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Background = new SolidColorBrush(Color.FromArgb(12, 81, 255, 0));
        }

        private void WP_LoadProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (WP_ProfileListDP.Visibility == Visibility.Collapsed) WP_ProfileListDP.Visibility = Visibility.Visible;
            else WP_ProfileListDP.Visibility = Visibility.Collapsed;
        }

        private void WP_CreateProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WP_ProfileListDP.IsEnabled = false;
            WP_NewProfilePanel.Visibility = Visibility.Visible;
        }

        private void WP_HideNewProfilePanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WP_ProfileListDP.IsEnabled = true;
            WP_NewProfilePanel.Visibility = Visibility.Collapsed;
            WP_NewProfileName.IsEnabled = true;
            WP_NewProfilePlayer.IsEnabled = true;
            WP_NewProfileDiscription.IsEnabled = true;
            WP_SaveNewProfile.Visibility = Visibility.Visible;
            WP_NewProfileName.Text = "";
            WP_NewProfilePlayer.Text = "";
            WP_NewProfileDiscription.Text = "Описание профиля";
        }

        private void WP_SaveNewProfile_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int Errors = 0;
            if (WP_NewProfileName.Text == "") Errors++;
            if (WP_NewProfilePlayer.Text == "") Errors++;
            string[] Lines = new string[3000];

            if (Errors != 2)
            {
                string curFile = @"Bin\PlayerData\Profiles.pdnd";
                if (File.Exists(curFile) == true)
                {
                    for (int i = 0; i != CountOfFileLines(@"Bin\PlayerData\Profiles.pdnd"); i++)
                    {
                        Lines[i] = ReadCertainLine(@"Bin\PlayerData\Profiles.pdnd", i);
                    }
                }

                using (StreamWriter sw = new StreamWriter(@"Bin\PlayerData\" + WP_NewProfileName.Text + ".pdnd", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(WP_NewProfileName.Text);
                    sw.WriteLine(WP_NewProfilePlayer.Text);
                    if (WP_NewProfileDiscription.Text == "Описание профиля") sw.WriteLine("NONE");
                    else sw.WriteLine(WP_NewProfileDiscription.Text);
                    sw.WriteLine("<Note>Не забывай про концентрацию заклинаний!");
                }
                using (StreamWriter sw = new StreamWriter(@"Bin\PlayerData\Profiles.pdnd", false, System.Text.Encoding.Default))
                {
                    int hh = 0;
                    while (Lines[hh] != null)
                    {
                        sw.WriteLine(Lines[hh]);
                        hh++;
                    }
                    sw.WriteLine(WP_NewProfileName.Text);
                }
                WP_ProfileListDP.IsEnabled = true;
                WP_NewProfilePanel.Visibility = Visibility.Collapsed;
                WP_NewProfileName.Text = "";
                WP_NewProfilePlayer.Text = "";
                WP_NewProfileDiscription.Text = "Описание профиля";
                WP_NewProfileName.IsEnabled = true;
                WP_NewProfilePlayer.IsEnabled = true;
                WP_NewProfileDiscription.IsEnabled = true;
                WP_SaveNewProfile.Visibility = Visibility.Visible;
                LoadProfiles();
            }
        }

        public void LoadProfiles()
        {
            string curFile = @"Bin\PlayerData\Profiles.pdnd";
            if (File.Exists(curFile) == true)
            {
                WP_ProfileListDP.Children.Clear();
                Label CPL = new Label();
                WP_ProfileListDP.Children.Add(CPL);
                CPL.FontFamily = new FontFamily("Bookman Old Style");
                CPL.FontSize = 20;
                CPL.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                CPL.Background = new SolidColorBrush(Color.FromArgb(12, 81, 255, 0));
                CPL.MouseEnter += WP_CreateProfile_MouseEnter;
                CPL.MouseLeave += WP_CreateProfile_MouseLeave;
                CPL.MouseUp += WP_CreateProfile_MouseUp;
                CPL.VerticalAlignment = VerticalAlignment.Stretch;
                DockPanel.SetDock(CPL, Dock.Bottom);
                CPL.Content = "Создать профиль";

                for (int i = 0; i != CountOfFileLines(@"Bin\PlayerData\Profiles.pdnd"); i++)
                {
                    if (ReadCertainLine(@"Bin\PlayerData\Profiles.pdnd", i) != "")
                    {
                        Label PL = new Label();
                        WP_ProfileListDP.Children.Add(PL);
                        PL.FontFamily = new FontFamily("Bookman Old Style");
                        PL.FontSize = 20;
                        PL.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                        PL.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                        PL.VerticalAlignment = VerticalAlignment.Stretch;
                        PL.HorizontalContentAlignment = HorizontalAlignment.Center;
                        ContextMenu CM = new ContextMenu();
                        MenuItem MI = new MenuItem();
                        MI.Header = "Удалить";
                        MI.Click += DeleteProfile;
                        CM.Items.Add(MI);
                        PL.ContextMenu = CM;
                        DockPanel.SetDock(PL, Dock.Bottom);
                        PL.Content = ReadCertainLine(@"Bin\PlayerData\Profiles.pdnd", i);
                        PL.MouseUp += LoadCetrainProfile;
                        PL.MouseDoubleClick += LoadCetrainProfileInfo;
                    }
                }
            }
        }

        public void DeleteProfile(object sender, RoutedEventArgs e)
        {
            String[] Profiles = new string[1000];
            for (int i = 0; i != CountOfFileLines(@"Bin\PlayerData\Profiles.pdnd"); i++)
            {
                if (ReadCertainLine(@"Bin\PlayerData\Profiles.pdnd", i) != CurrentProfile) Profiles[i] = ReadCertainLine(@"Bin\PlayerData\Profiles.pdnd", i);
            }
            using (StreamWriter sw = new StreamWriter(@"Bin\PlayerData\Profiles.pdnd", false, System.Text.Encoding.Default))
            {
                for (int i = 0; i != 1000; i++)
                {
                    if (Profiles[i] != null) sw.WriteLine(Profiles[i]);
                }
            }
            File.Delete(@"Bin\PlayerData\" + CurrentProfile + ".pdnd");
            LoadProfiles();
        }

        private void LoadCetrainProfile(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (ProfileWay != "")
                {
                    LoadingTitle = "Идёт сохранение...";
                    MainWindowLoadLabel.Visibility = Visibility.Visible;
                    for (int i = 0; i != 33000; i++)
                    {
                        if (LoadedProfile[i].IndexOf("<Note>") != -1) LoadedProfile[i] = nullstring;
                    }
                    for (int NoteI = 0; NoteI != DN_WrapPanel.Children.Count; NoteI++)
                    {
                        for (int i = 0; i != 33000; i++)
                        {
                            if (LoadedProfile[i] == nullstring)
                            {
                                LoadedProfile[i] = "<Note>" + ((DN_WrapPanel.Children[NoteI] as Grid).Children[1] as TextBox).Text;
                                break;
                            }
                        }
                    }

                    using (StreamWriter sw = new StreamWriter(ProfileWay, false, System.Text.Encoding.Default))
                    {
                        for (int i = 0; i != 33000; i++)
                        {
                            if (LoadedProfile[i] != nullstring)
                            {
                                sw.WriteLine(LoadedProfile[i]);
                            }
                        }
                    }
                }

                DN_WrapPanel.Children.Clear();
                WP_Lbl1.Content = "Здравствуй, " + ReadCertainLine(@"Bin\PlayerData\" + (sender as Label).Content + ".pdnd", 1) + ".";
                Random rnd = new Random();
                WP_Lbl2.Content = ReadCertainLine(@"Bin\PlayerData\Greetings.pdnd", rnd.Next(0, CountOfFileLines(@"Bin\PlayerData\Greetings.pdnd")));
                ProfileWay = @"Bin\PlayerData\" + (sender as Label).Content + ".pdnd";
                WP_ProfileListDP.Visibility = Visibility.Collapsed;
                MainTools.IsEnabled = true;
                AdditionTools.IsEnabled = true;

                this.Cursor = Cursors.Wait;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                timer.Start();
                myThread = new Thread(new ThreadStart(LoadProfile));
                myThread.Start();
                LoadingTitle = "Загрузка.";
                MainWindowLoadLabel.Visibility = Visibility.Visible;
                
                this.Cursor = Cursors.Arrow;
            }
            CurrentProfile = (sender as Label).Content.ToString();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (MainWindowLoadLabel.Content.ToString() == LoadingTitle + ".") MainWindowLoadLabel.Content = LoadingTitle + "..";
            else if (MainWindowLoadLabel.Content.ToString() == LoadingTitle + "..") MainWindowLoadLabel.Content = LoadingTitle + "...";
            else MainWindowLoadLabel.Content = LoadingTitle + ".";

            if (PotokEnds == true)
            {
                MainWindowLoadLabel.Visibility = Visibility.Collapsed;
                myThread.Abort();
            }
        }

        public void LoadProfile()
        {
            for (int i = 0; i != 33000; i++)
            {
                LoadedProfile[i] = "<NULLSTRING>";
            }
            for (int i = 0; i != CountOfFileLines(ProfileWay); i++)
            {
                LoadedProfile[i] = ReadCertainLine(ProfileWay, i);
            }
            PotokEnds = true;
        }

        private void LoadCetrainProfileInfo(object sender, MouseButtonEventArgs e)
        {
            WP_ProfileListDP.IsEnabled = false;
            WP_NewProfilePanel.Visibility = Visibility.Visible;

            WP_NewProfileName.Text = ReadCertainLine(@"Bin\PlayerData\" + (sender as Label).Content.ToString() + ".pdnd", 0);
            WP_NewProfilePlayer.Text = ReadCertainLine(@"Bin\PlayerData\" + (sender as Label).Content.ToString() + ".pdnd", 1);
            WP_NewProfileDiscription.Text = ReadCertainLine(@"Bin\PlayerData\" + (sender as Label).Content.ToString() + ".pdnd", 2);

            WP_NewProfileName.IsEnabled = false;
            WP_NewProfilePlayer.IsEnabled = false;
            WP_NewProfileDiscription.IsEnabled = false;
            WP_SaveNewProfile.Visibility = Visibility.Collapsed;
        }

        private void ToGeneratorLbl7_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            Pages.SelectedIndex = 5;
            ToMonsterList.Visibility = Visibility.Visible;
            ToSpellBook.Visibility = Visibility.Visible;
        }

        private void MainLogo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            Pages.SelectedIndex = 0;
        }

        private void ToGeneratorLbl8_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            ToDMNotes.Visibility = Visibility.Visible;
            ToCombatPage.Visibility = Visibility.Visible;
            ToMap.Visibility = Visibility.Visible;
            Pages.SelectedIndex = 7;
            LoadNotes();
        }

        private void ToGeneratorLbl9_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToDMNotes.Visibility = Visibility.Visible;
            Pages.SelectedIndex = 7;
            LoadNotes();
        }

        private void DN_AddNote_Click(object sender, RoutedEventArgs e)
        {
            Grid Grd = new Grid();
            DN_WrapPanel.Children.Add(Grd);
            Grd.Height = 200;
            Grd.Width = 200;
            Grd.MaxWidth = 200;
            Grd.MaxHeight = 200;
            Grd.MinHeight = 200;
            Grd.MinWidth = 200;
            Image Img = new Image();
            Grd.Children.Add(Img);
            Img.VerticalAlignment = VerticalAlignment.Stretch;
            Img.HorizontalAlignment = HorizontalAlignment.Stretch;
            ChangeImage("Note.png", Img);
            TextBox Tb = new TextBox();
            Grd.Children.Add(Tb);
            Tb.BorderBrush = null;
            Tb.Background = null;
            Tb.TextWrapping = TextWrapping.Wrap;
            Tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            Tb.VerticalAlignment = VerticalAlignment.Stretch;
            Tb.Margin = new Thickness(32, 50, 38, 84);
            Tb.HorizontalContentAlignment = HorizontalAlignment.Center;
            Tb.VerticalContentAlignment = VerticalAlignment.Center;
            Tb.Text = "Ваша новая заметка";
            Image Img2 = new Image();
            Grd.Children.Add(Img2);
            Img2.VerticalAlignment = VerticalAlignment.Top;
            Img2.HorizontalAlignment = HorizontalAlignment.Right;
            Img2.Height = 25;
            Img2.Width = 25;
            Img2.Margin = new Thickness(0, 30, 30, 0);
            ChangeImage("Colse.png", Img2);
            Img2.MouseUp += DeleteNote;
        }

        private void DeleteNote(object sender, MouseButtonEventArgs e)
        {
            Grid Grd = (sender as Image).Parent as Grid;
            DN_WrapPanel.Children.Remove(Grd);
        }

        public void LoadNotes()
        {
            if (DN_WrapPanel.Children.Count == 0)
            {
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i].IndexOf("<Note>") != -1)
                    {
                        Grid Grd = new Grid();
                        DN_WrapPanel.Children.Add(Grd);
                        Grd.Height = 200;
                        Grd.Width = 200;
                        Grd.MaxWidth = 200;
                        Grd.MaxHeight = 200;
                        Grd.MinHeight = 200;
                        Grd.MinWidth = 200;
                        Image Img = new Image();
                        Grd.Children.Add(Img);
                        Img.VerticalAlignment = VerticalAlignment.Stretch;
                        Img.HorizontalAlignment = HorizontalAlignment.Stretch;
                        ChangeImage("Note.png", Img);
                        TextBox Tb = new TextBox();
                        Grd.Children.Add(Tb);
                        Tb.BorderBrush = null;
                        Tb.Background = null;
                        Tb.TextWrapping = TextWrapping.Wrap;
                        Tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                        Tb.VerticalAlignment = VerticalAlignment.Stretch;
                        Tb.Margin = new Thickness(32, 50, 38, 84);
                        Tb.HorizontalContentAlignment = HorizontalAlignment.Center;
                        Tb.VerticalContentAlignment = VerticalAlignment.Center;
                        Tb.Text = LoadedProfile[i].Remove(0, 6);
                        Image Img2 = new Image();
                        Grd.Children.Add(Img2);
                        Img2.VerticalAlignment = VerticalAlignment.Top;
                        Img2.HorizontalAlignment = HorizontalAlignment.Right;
                        Img2.Height = 25;
                        Img2.Width = 25;
                        Img2.Margin = new Thickness(0, 30, 30, 0);
                        ChangeImage("Colse.png", Img2);
                        Img2.MouseUp += DeleteNote;
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ProfileWay != "")
            {
                SaveChar();
                Pages.SelectedIndex = 0;
                LoadingTitle = "Идёт сохранение...";
                MainWindowLoadLabel.Visibility = Visibility.Visible;
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i].IndexOf("<Note>") != -1) LoadedProfile[i] = nullstring;
                }
                for (int NoteI = 0; NoteI != DN_WrapPanel.Children.Count; NoteI++)
                {
                    for (int i = 0; i != 33000; i++)
                    {
                        if (LoadedProfile[i] == nullstring)
                        {
                            LoadedProfile[i] = "<Note>" + ((DN_WrapPanel.Children[NoteI] as Grid).Children[1] as TextBox).Text;
                            break;
                        }
                    }
                }

                using (StreamWriter sw = new StreamWriter(ProfileWay, false, System.Text.Encoding.Default))
                {
                    for (int i = 0; i != 33000; i++)
                    {
                        if (LoadedProfile[i] != nullstring)
                        {
                            sw.WriteLine(LoadedProfile[i]);
                        }
                    }
                }
            }
        }

        public void StoryClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SM_Storys.IsEnabled = false;
                bool Chet = true;
                SM_StoryNameDP.Visibility = Visibility.Visible;
                NowOpenedStory = (sender as Label).Content.ToString();
                MC_StoryNameLbl.Visibility = Visibility.Visible;
                MS_StoryNameEdt.Visibility = Visibility.Collapsed;
                MC_StoryNameLbl.Content = (sender as Label).Content;
                SM_Lbl2.Content = "Выберете сцену из приключения " + MC_StoryNameLbl.Content;
                SM_Lbl1.Content = "ПКМ по сцене чтобы перейти к редактированию";
                SM_SceneListSB.Children.Clear();
                for (int i=0; i != 33000; i++)
                {
                    if (LoadedProfile[i] == "<Scene>" + NowOpenedStory)
                    {
                        Label StoryNames2 = new Label();
                        SM_SceneListSB.Children.Add(StoryNames2);
                        StoryNames2.HorizontalAlignment = HorizontalAlignment.Stretch;
                        DockPanel.SetDock(StoryNames2, Dock.Top);
                        StoryNames2.HorizontalContentAlignment = HorizontalAlignment.Center;
                        StoryNames2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                        ContextMenu CM = new ContextMenu();
                        MenuItem MII = new MenuItem();
                        MII.Header = "Удалить";
                        MenuItem MII2 = new MenuItem();
                        MII2.Header = "Редактировать";
                        CM.Items.Add(MII);
                        CM.Items.Add(MII2);
                        MII.Click += DeleteScene;
                        MII2.Click += EditScene;
                        StoryNames2.ContextMenu = CM;
                        if (Chet == true)
                        {
                            StoryNames2.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                            Chet = false;
                        }
                        else
                        {
                            StoryNames2.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                            Chet = true;
                        }
                        StoryNames2.Content = LoadedProfile[i+1];
                        StoryNames2.MouseUp += SceneClick;
                    }
                }

                Label StoryNames = new Label();
                SM_SceneListSB.Children.Add(StoryNames);
                StoryNames.HorizontalAlignment = HorizontalAlignment.Stretch;
                DockPanel.SetDock(StoryNames, Dock.Top);
                StoryNames.HorizontalContentAlignment = HorizontalAlignment.Center;
                StoryNames.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));

                if (Chet == true)
                {
                    StoryNames.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                    Chet = false;
                }
                else
                {
                    StoryNames.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                    Chet = true;
                }
                StoryNames.Content = "Создать сцену";
                StoryNames.MouseUp += CreateNewScene;

                Label BackToStoryList = new Label();
                SM_SceneListSB.Children.Add(BackToStoryList);
                BackToStoryList.HorizontalAlignment = HorizontalAlignment.Stretch;
                DockPanel.SetDock(BackToStoryList, Dock.Top);
                BackToStoryList.HorizontalContentAlignment = HorizontalAlignment.Center;
                BackToStoryList.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                BackToStoryList.Background = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
                BackToStoryList.Content = "К списку историй";
                BackToStoryList.MouseUp += ToGeneratorLbl6_MouseUp;
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                DeletetStory = (sender as Label);
            }
        }

        public void CreateNewScene(object sender, MouseButtonEventArgs e)
        {
            StoryEditorWindow.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            SM_GridSplitter.Visibility = Visibility.Visible;
            SM_TopGrid.IsEnabled = false;
            SM_SceneListSB.IsEnabled = false;
            StoryEditorWindow.Visibility = Visibility.Visible;
            SM_Lbl1.Visibility = Visibility.Collapsed;
            SM_Lbl2.Visibility = Visibility.Collapsed;
            SM_TextPreview.Children.Clear();
            SM_StoryEditor.Document.Blocks.Clear();
            SM_StoryEditor.Document.Blocks.Add(new Paragraph(new Run("Начните писать сюда свою историю")));
            SM_SceneName.Text = "Название сцены";
            SceneEditorState = 0;
        }

        private void SM_SceneName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SM_SceneName.Text == "") SM_SceneName.Text = "Название сцены";
        }

        private void SM_SaveScene_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SceneEditorState == 0)
            {
                if (SM_SceneName.Text != "")
                    if (SM_SceneName.Text != "Название сцены")
                    {
                        string[] LoadedProfile1 = new string[33000];
                        string[] LoadedProfile2 = new string[33000];
                        for (int i = 3; i != 33000; i++)
                        {
                            LoadedProfile1[i - 3] = LoadedProfile[i];
                        }
                        LoadedProfile2[0] = "<Scene>" + NowOpenedStory;
                        LoadedProfile2[1] = SM_SceneName.Text;
                        int CountOfLines = 2;
                        Block ThisParagraph = SM_StoryEditor.Document.Blocks.FirstBlock;
                        Inline ThisInline = (ThisParagraph as Paragraph).Inlines.FirstInline;
                        for (int Pi = 0; Pi != SM_StoryEditor.Document.Blocks.Count; Pi++)
                        {
                            ThisInline = (ThisParagraph as Paragraph).Inlines.FirstInline;
                            for (int Ri = 0; Ri != (ThisParagraph as Paragraph).Inlines.Count; Ri++)
                            {
                                if (ThisInline != null)
                                {
                                    Console.WriteLine(ThisInline.GetType().ToString());
                                    if (ThisInline.GetType().ToString() == "System.Windows.Documents.Run")
                                    {
                                        LoadedProfile2[CountOfLines] = (ThisInline as Run).Text;
                                        Console.WriteLine((ThisInline as Run).Text);
                                        ThisInline = ThisInline.NextInline;
                                        CountOfLines++;
                                    }
                                }
                            }
                            ThisParagraph = ThisParagraph.NextBlock;
                        }
                        LoadedProfile2[CountOfLines] = "<SceneEnd>";
                        CountOfLines++;
                        for (int i = 0; i != CountOfLines; i++)
                        {
                            LoadedProfile[i + 3] = LoadedProfile2[i];
                        }
                        for (int i = CountOfLines + 3; i != 33000; i++)
                        {
                            LoadedProfile[i] = LoadedProfile1[i-3- CountOfLines];
                        }
                        Label lbl = new Label();
                        lbl.Content = NowOpenedStory;
                        StoryClick(lbl, e);
                        SM_Unsave_MouseUp(null, null);
                        lbl.Content = SM_SceneName;
                        SceneClick(lbl, e);
                    }
            }
            if (SceneEditorState == 1)
            {
                DeleteScene(null, null);
                if (SM_SceneName.Text != "")
                    if (SM_SceneName.Text != "Название сцены")
                    {
                        string[] LoadedProfile1 = new string[33000];
                        string[] LoadedProfile2 = new string[33000];
                        for (int i = 3; i != 33000; i++)
                        {
                            LoadedProfile1[i - 3] = LoadedProfile[i];
                        }
                        LoadedProfile2[0] = "<Scene>" + NowOpenedStory;
                        LoadedProfile2[1] = SM_SceneName.Text;
                        int CountOfLines = 2;
                        Block ThisParagraph = SM_StoryEditor.Document.Blocks.FirstBlock;
                        Inline ThisInline = (ThisParagraph as Paragraph).Inlines.FirstInline;
                        for (int Pi = 0; Pi != SM_StoryEditor.Document.Blocks.Count; Pi++)
                        {
                            ThisInline = (ThisParagraph as Paragraph).Inlines.FirstInline;
                            for (int Ri = 0; Ri != (ThisParagraph as Paragraph).Inlines.Count; Ri++)
                            {
                                if (ThisInline != null)
                                {
                                    if (ThisInline.GetType().ToString() == "System.Windows.Documents.Run")
                                    {
                                        LoadedProfile2[CountOfLines] = (ThisInline as Run).Text;
                                        ThisInline = ThisInline.NextInline;
                                        CountOfLines++;
                                    }
                                }
                            }
                            ThisParagraph = ThisParagraph.NextBlock;
                        }
                        LoadedProfile2[CountOfLines] = "<SceneEnd>";
                        CountOfLines++;
                        for (int i = 0; i != CountOfLines; i++)
                        {
                            LoadedProfile[i + 3] = LoadedProfile2[i];
                        }
                        for (int i = CountOfLines + 3; i != 33000; i++)
                        {
                            LoadedProfile[i] = LoadedProfile1[i - 3 - CountOfLines];
                        }
                        Label lbl = new Label();
                        lbl.Content = NowOpenedStory;
                        StoryClick(lbl, e);
                        SM_Unsave_MouseUp(null, null);
                        lbl.Content = SM_SceneName;
                        SceneClick(lbl, e);
                    }
            }
        }

        private void SceneClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                SM_GridSplitter.Visibility = Visibility.Collapsed;
                StoryEditorWindow.RowDefinitions[0].Height = new GridLength(0);
                SM_TopGrid.IsEnabled = false;
                SM_SceneListSB.IsEnabled = true;
                StoryEditorWindow.Visibility = Visibility.Visible;
                SM_Lbl1.Visibility = Visibility.Collapsed;
                SM_Lbl2.Visibility = Visibility.Collapsed;

                CurrentScene = "<Scene>" + NowOpenedStory;
                SM_StoryEditor.Document.Blocks.Clear();
                Paragraph Prgrp = new Paragraph();
                SM_StoryEditor.Document.Blocks.Add(Prgrp);
                for (int i = 0; i != 33000; i++)
                {
                    if (LoadedProfile[i] == CurrentScene)
                    {
                        if (LoadedProfile[i+1] == (sender as Label).Content.ToString())
                        {
                            int Wi = i + 2;
                            while (LoadedProfile[Wi] != "<SceneEnd>")
                            {
                                Prgrp.Inlines.Add(LoadedProfile[Wi]);
                                Wi++;
                            }
                        }
                    }
                }
                SM_GeneratePreview_MouseUp(null, null);
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                DeletedScene = (sender as Label);
            }
        }

        private void SM_Unsave_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SM_GridSplitter.Visibility = Visibility.Visible;
            StoryEditorWindow.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            SM_TopGrid.IsEnabled = false;
            SM_SceneListSB.IsEnabled = true;
            StoryEditorWindow.Visibility = Visibility.Collapsed;
            SM_Lbl1.Visibility = Visibility.Visible;
            SM_Lbl2.Visibility = Visibility.Visible;
        }

        public void DeleteScene(object sender, RoutedEventArgs e)
        {
            SM_StoryNameDP.Visibility = Visibility.Visible;
            MC_StoryNameLbl.Visibility = Visibility.Visible;
            MS_StoryNameEdt.Visibility = Visibility.Collapsed;
            SM_Lbl2.Content = "Выберете сцену из приключения " + MC_StoryNameLbl.Content;
            SM_Lbl1.Content = "ПКМ по сцене чтобы перейти к редактированию";
            SM_SceneListSB.Children.Clear();
            for (int i = 0; i !=33000; i++)
            {
                if (LoadedProfile[i] == "<Scene>" + NowOpenedStory)
                {
                    if (LoadedProfile[i+1] == DeletedScene.Content.ToString())
                    {
                        int o = i;
                        while (LoadedProfile[o] != "<SceneEnd>")
                        {
                            LoadedProfile[o] = nullstring;
                            o++;
                        }
                        LoadedProfile[o] = nullstring;
                    }
                }
            }

            SM_Storys.IsEnabled = false;
            bool Chet = true;
            SM_StoryNameDP.Visibility = Visibility.Visible;
            MC_StoryNameLbl.Visibility = Visibility.Visible;
            MS_StoryNameEdt.Visibility = Visibility.Collapsed;
            SM_Lbl2.Content = "Выберете сцену из приключения " + MC_StoryNameLbl.Content;
            SM_Lbl1.Content = "ПКМ по сцене чтобы перейти к редактированию";
            SM_SceneListSB.Children.Clear();
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i] == "<Scene>" + NowOpenedStory)
                {
                    Label StoryNames2 = new Label();
                    SM_SceneListSB.Children.Add(StoryNames2);
                    StoryNames2.HorizontalAlignment = HorizontalAlignment.Stretch;
                    DockPanel.SetDock(StoryNames2, Dock.Top);
                    StoryNames2.HorizontalContentAlignment = HorizontalAlignment.Center;
                    StoryNames2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                    ContextMenu CM = new ContextMenu();
                    MenuItem MII = new MenuItem();
                    MII.Header = "Удалить";
                    MenuItem MII2 = new MenuItem();
                    MII2.Header = "Редактировать";
                    CM.Items.Add(MII);
                    CM.Items.Add(MII2);
                    MII.Click += DeleteScene;
                    MII2.Click += EditScene;
                    StoryNames2.ContextMenu = CM;
                    if (Chet == true)
                    {
                        StoryNames2.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                        Chet = false;
                    }
                    else
                    {
                        StoryNames2.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                        Chet = true;
                    }
                    StoryNames2.Content = LoadedProfile[i + 1];
                    StoryNames2.MouseUp += SceneClick;
                }
            }

            Label StoryNames = new Label();
            SM_SceneListSB.Children.Add(StoryNames);
            StoryNames.HorizontalAlignment = HorizontalAlignment.Stretch;
            DockPanel.SetDock(StoryNames, Dock.Top);
            StoryNames.HorizontalContentAlignment = HorizontalAlignment.Center;
            StoryNames.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));

            if (Chet == true)
            {
                StoryNames.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                Chet = false;
            }
            else
            {
                StoryNames.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                Chet = true;
            }
            StoryNames.Content = "Создать сцену";
            StoryNames.MouseUp += CreateNewScene;

            Label BackToStoryList = new Label();
            SM_SceneListSB.Children.Add(BackToStoryList);
            BackToStoryList.HorizontalAlignment = HorizontalAlignment.Stretch;
            DockPanel.SetDock(BackToStoryList, Dock.Top);
            BackToStoryList.HorizontalContentAlignment = HorizontalAlignment.Center;
            BackToStoryList.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            BackToStoryList.Background = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
            BackToStoryList.Content = "К списку историй";
            BackToStoryList.MouseUp += ToGeneratorLbl6_MouseUp;

        }

        public void EditScene(object sender, RoutedEventArgs e)
        {
            SceneEditorState = 1;
            SM_GridSplitter.Visibility = Visibility.Visible;
            StoryEditorWindow.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            SM_TopGrid.IsEnabled = false;
            SM_SceneListSB.IsEnabled = false;
            StoryEditorWindow.Visibility = Visibility.Visible;
            SM_Lbl1.Visibility = Visibility.Collapsed;
            SM_Lbl2.Visibility = Visibility.Collapsed;
            CurrentScene = DeletedScene.Content.ToString();
            SM_StoryEditor.Document.Blocks.Clear();
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i] == "<Scene>" + NowOpenedStory)
                {
                    if (LoadedProfile[i+1] == CurrentScene)
                    {
                        Paragraph Prgrp = new Paragraph();
                        SM_SceneName.Text = DeletedScene.Content.ToString();
                        int o = i + 2;
                        while (LoadedProfile[o] != "<SceneEnd>")
                        {
                            Run Rn = new Run();
                            Rn.Text = LoadedProfile[o];
                            Prgrp.Inlines.Add(Rn);
                            o++;
                        }
                        SM_StoryEditor.Document.Blocks.Add(Prgrp);
                    }
                }
            }
            ShowProfileinConsole();
        }

        public void ShowProfileinConsole()
        {
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i] != nullstring) Console.WriteLine(DateTime.Now.Minute +"::"+ LoadedProfile[i]);
            }
        }

        private void CL_NewChar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveChar();
            CL_SaveChar.Visibility = Visibility.Visible;
            CL_CharSpace.Tag = "NEW";
            CL_CharList.Width = 53;
            CL_Chars.Visibility = Visibility.Hidden;
            CL_CharsListLable.Visibility = Visibility.Hidden;
            CL_CharList.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            CL_More.Margin = new Thickness(0, 10, 0, 0);
            CL_CharSpace.Visibility = Visibility.Visible;
        }

        private void CL_SaveChar_MouseUp(object sender, MouseButtonEventArgs e)
        {
            String[] CharLines = new string[1000];
            CharLines[0] = "<PlayerChar>" + CL_CharName.Content.ToString();
            CharLines[1] = "<CharRase>" + CL_Rase.Content.ToString();
            CharLines[2] = "<CharClass>" + CL_Class.Content.ToString();
            CharLines[3] = "<CharAge>" + CL_Age.Content.ToString()+" "+CL_AgePreffics.Content.ToString();
            CharLines[4] = "<CharHP>" + CL_Health.Content.ToString();
            CharLines[5] = "<CharAC>" + CL_Armor.Content.ToString();
            CharLines[6] = "<CharSpeed>" + CL_Speed.Content.ToString();
            CharLines[7] = "<CharStat>" + CL_StgValue.Content.ToString() + ":" + CL_DexValue.Content.ToString() + ":" + CL_ConValue.Content.ToString() + ":" + CL_IntValue.Content.ToString() + ":" + CL_WidValue.Content.ToString() + ":" + CL_ChaValue.Content.ToString();
            CharLines[8] = "<CMasterBonus>" + CL_MB.Content.ToString();
            CharLines[9] = "<PlayerCharSpellDif>";
            CharLines[10] = "<PlayerCharSpellAtt>";
            CharLines[11] = "<PlayerCharSpellStat>СИЛ";
            CharLines[12] = "<PlayerCharCasterLevel>1";
            CharLines[13] = "<PlayerCharSpellsReady>";
            CharLines[14] = "<PlayerCharItems>";
            CharLines[15] = "<PlayerCharSkills>";
            CharLines[16] = "<PlayerCharEnd>";
            PasteLinesIntoProfile(CharLines, 1000);
            ToGeneratorLbl4_MouseUp(sender, null);
        }

        public void PasteLinesIntoProfile(String[] AddedLines,int MassValue)
        {
            string[] LoadedProfile1 = new string[33000];
            for (int i = 3; i != 33000; i++)
            {
                LoadedProfile1[i - 3] = LoadedProfile[i];
            }
            int CountOfLines = 0;
            for (int i=0; i!= MassValue; i++)
            {
                if (AddedLines[i] != null)
                    if (AddedLines[i] != "") CountOfLines++;
            }
            for (int i = 0; i != CountOfLines; i++)
            {
                LoadedProfile[i + 3] = AddedLines[i];
            }
            for (int i = CountOfLines + 3; i != 33000; i++)
            {
                LoadedProfile[i] = LoadedProfile1[i - 3 - CountOfLines];
            }
        }

        private void CharsLbl_MouseEnter(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromRgb(255, 246, 238));
        }

        private void CharsLbl_MouseLeave(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromRgb(255, 212, 173));
        }

        private void CharsLbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromRgb(93, 93, 93));
        }

        public void CharLoadBP()
        {
            CP_MainGrid.ColumnDefinitions[0].MinWidth = 200;
            CP_TempListDock.MaxWidth = 400;
            CP_TempList.Visibility = Visibility.Visible;
            CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(200);
            CP_GS1.Visibility = Visibility.Visible;
            CP_TempListDock.Children.Clear();
            for (int i = 0; i != 33000; i++)
            {
                if (LoadedProfile[i].IndexOf("<PlayerChar>") != -1)
                {
                    Label CharsLbl = new Label();
                    CP_TempListDock.Children.Add(CharsLbl);
                    DockPanel.SetDock(CharsLbl, Dock.Top);
                    CharsLbl.Margin = new Thickness(0, 5, 0, 0);
                    CharsLbl.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                    CharsLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                    CharsLbl.Content = LoadedProfile[i].Remove(0, 12);
                    CharsLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                    CharsLbl.FontFamily = new FontFamily("Bookman Old Style");
                    CharsLbl.FontSize = 12;
                    CharsLbl.MouseDown += TempMoveDown;
                }
            }
            CP_GS1.Tag = "CharsOpen";
        }

        private void CharsLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Creatble = false;
            if (CP_GS1.Tag.ToString() == "null")
            {
                CharLoadBP();
                Creatble = true;
            }
            if (CP_GS1.Tag.ToString() == "CreOpen")
            {
                CharLoadBP();
                Creatble = true;
            }
            if (CP_GS1.Tag.ToString() == "CreClose")
            {
                CharLoadBP();
                Creatble = true;
            }

            if (Creatble == false)
            {
                if (CP_GS1.Tag.ToString() == "CharsOpen")
                {
                    CP_MainGrid.ColumnDefinitions[0].MinWidth = 0;
                    CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
                    CP_MainGrid.ColumnDefinitions[0].MaxWidth = 0;
                    CP_TempListDock.MaxWidth = 0;
                    CP_TempList.Visibility = Visibility.Collapsed;
                    CP_GS1.Visibility = Visibility.Collapsed;
                    CP_GS1.Tag = "CharsClose";
                }
                else
            if (CP_GS1.Tag.ToString() == "CharsClose")
                {
                    CP_MainGrid.ColumnDefinitions[0].MinWidth = 200;
                    CP_TempList.Visibility = Visibility.Visible;
                    CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(200);
                    CP_GS1.Visibility = Visibility.Visible;
                    CP_GS1.Tag = "CharsOpen";
                }
            }
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromRgb(255, 212, 173));
        }

        public void TempMoveDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.Background = new SolidColorBrush(Color.FromRgb(90, 90, 90));
            DroppedLabel = sender as Label;
            DragDrop.DoDragDrop(lbl, lbl.Content, DragDropEffects.Copy);
        }

        private void CP_CombatList_Drop(object sender, DragEventArgs e)
        {
            DroppedLabel.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
            if (DroppedLabel.Parent != (sender as ScrollViewer).Content)
            {
                //Новый интерфейс панели существа
                Grid CharMainGrid = new Grid();
                CP_CombatListDock.Children.Add(CharMainGrid);
                CharMainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                CharMainGrid.Width = Double.NaN;
                CharMainGrid.FlowDirection = FlowDirection.LeftToRight;
                CharMainGrid.Height = Double.NaN;
                DockPanel.SetDock(CharMainGrid, Dock.Top);
                CharMainGrid.Margin = new Thickness(0, 5, 0, 0);
                CharMainGrid.Background = new SolidColorBrush(Color.FromArgb(55, 0, 0, 0));
                RowDefinition RW1 = new RowDefinition();
                RowDefinition RW2 = new RowDefinition();
                CharMainGrid.RowDefinitions.Add(RW1);
                CharMainGrid.RowDefinitions.Add(RW2);
                RW1.Height = new GridLength(5,GridUnitType.Pixel);
                RW2.Height = new GridLength(1,GridUnitType.Star);
                //Child 0
                Label CharHealth = new Label();
                CharMainGrid.Children.Add(CharHealth);
                CharHealth.Content = "";
                CharHealth.HorizontalAlignment = HorizontalAlignment.Stretch;
                CharHealth.Width = Double.NaN;
                CharHealth.Height = 5;
                CharHealth.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                Grid.SetRow(CharHealth, 0);
                Grid.SetRowSpan(CharHealth, 1);
                //Child 1
                Label AddedLbl = new Label();
                CharMainGrid.Children.Add(AddedLbl);
                AddedLbl.Background = new SolidColorBrush(Color.FromArgb(75, 0, 0, 0));
                AddedLbl.Content = "0";
                AddedLbl.Height = 26;
                AddedLbl.Width = 26;
                AddedLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                AddedLbl.HorizontalAlignment = HorizontalAlignment.Left;
                AddedLbl.VerticalAlignment = VerticalAlignment.Top;
                AddedLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                AddedLbl.MouseDoubleClick += CharInicDC;
                Grid.SetRow(AddedLbl, 1);
                Grid.SetRowSpan(AddedLbl, 1);
                //Child 2
                TextBox TB = new TextBox();
                CharMainGrid.Children.Add(TB);
                TB.Background = new SolidColorBrush(Color.FromArgb(15, 0, 0, 0));
                TB.Text = "0";
                TB.Height = 26;
                TB.HorizontalContentAlignment = HorizontalAlignment.Center;
                TB.HorizontalAlignment = HorizontalAlignment.Left;
                TB.Width = 26;
                TB.VerticalAlignment = VerticalAlignment.Top;
                TB.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                TB.Visibility = Visibility.Collapsed;
                TB.PreviewTextInput += CL_StgEditor_PreviewTextInput;
                TB.KeyUp += CharInicDCAccept;
                TB.LostFocus += TB_InicLostFocus;
                Grid.SetRow(TB, 1);
                Grid.SetRowSpan(TB, 1);
                //Child 3
                Label AddedLbl2 = new Label();
                CharMainGrid.Children.Add(AddedLbl2);
                AddedLbl2.Background = new SolidColorBrush(Color.FromArgb(15, 0, 0, 0));
                AddedLbl2.Content = DroppedLabel.Content;
                AddedLbl2.Height = 26;
                AddedLbl2.Margin = new Thickness(26, 0, 0, 0);
                AddedLbl2.HorizontalContentAlignment = HorizontalAlignment.Center;
                AddedLbl2.HorizontalAlignment = HorizontalAlignment.Stretch;
                AddedLbl2.Width = Double.NaN;
                AddedLbl2.VerticalAlignment = VerticalAlignment.Top;
                AddedLbl2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                AddedLbl2.MouseDoubleClick += CombatCharMouseUp;
                Grid.SetRow(AddedLbl2, 1);
                Grid.SetRowSpan(AddedLbl2, 1);
                ContextMenu CM = new ContextMenu();
                MenuItem MI = new MenuItem();
                MI.Header = "Удалить";
                MI.Click += RemoveCharFromCombat;
                CM.Items.Add(MI);
                AddedLbl2.ContextMenu = CM;
                if (CP_GS1.Tag.ToString() == "CharsOpen") AddedLbl2.Tag = "Char";
                if (CP_GS1.Tag.ToString() == "CreOpen")  AddedLbl2.Tag = "Monster";
                //Child 4
                Border CharBorder = new Border();
                CharMainGrid.Children.Add(CharBorder);
                CharBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
                CharBorder.VerticalAlignment = VerticalAlignment.Stretch;
                CharBorder.Visibility = Visibility.Collapsed;
                CharBorder.Width = Double.NaN;
                CharBorder.Height = Double.NaN;
                CharBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                Grid.SetRow(CharBorder, 0);
                Grid.SetRowSpan(CharBorder, 2);
                CharBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(126, 126, 126));
            }
        }

        private void TB_InicLostFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "") (sender as TextBox).Text = "0";
            ((sender as TextBox).Parent as Grid).Children[1].Visibility = Visibility.Visible;
            (((sender as TextBox).Parent as Grid).Children[1] as Label).Content = (sender as TextBox).Text;
            (sender as TextBox).Visibility = Visibility.Collapsed;
        }

        public void OpenCharInfo(object sender, MouseButtonEventArgs e)
        {
            CP_StackInfo.Visibility = Visibility.Visible;
            CP_CharName.Content = (sender as Label).Content;
            if (e.ChangedButton == MouseButton.Left)
            {
                SelectedCreature = sender as Label;
                if ((sender as Label).Tag.ToString() == "Char")
                {
                    for (int i = 0; i != 200; i++)
                    {
                        if (CreInCombatLbl[i] != null)
                            if (CreInCombatLbl[i] == sender as Label)
                            {
                                for (int i2 = 0; i2 != 33000; i2++)
                                {
                                    if (LoadedProfile[i2] != nullstring)
                                    {
                                        if (LoadedProfile[i2].Contains("<CharStat>") == true)
                                        {
                                            string line = LoadedProfile[i2].Remove(0, 10);
                                            int StatID = 1;
                                            while (line.Length != 0)
                                            {
                                                int p = 0;
                                                string stat = "";
                                                if (StatID != 6)
                                                {
                                                    while (line[p] != ':')
                                                    {
                                                        stat = stat + line[p];
                                                        p++;
                                                    }
                                                    if (StatID == 1)
                                                    {
                                                        CP_StgValue.Content = stat;
                                                    }
                                                    if (StatID == 2)
                                                    {
                                                        CP_StgValue1.Content = stat;
                                                    }
                                                    if (StatID == 3)
                                                    {
                                                        CP_StgValue2.Content = stat;
                                                    }
                                                    if (StatID == 4)
                                                    {
                                                        CP_StgValue3.Content = stat;
                                                    }
                                                    if (StatID == 5)
                                                    {
                                                        CP_StgValue4.Content = stat;
                                                    }
                                                    line = line.Remove(0, line.IndexOf(stat) + stat.Length + 1);
                                                    StatID++;
                                                }
                                                else if (StatID == 6)
                                                {
                                                    CP_StgValue5.Content = line;
                                                    line = "";
                                                }
                                            }
                                        }
                                    }
                                }
                                CP_HP.Text = CreInCombat[1, i];
                            }
                    }
                }
                if ((sender as Label).Tag.ToString() == "Monster")
                {
                    int CreaID = 0;
                    for (int i2 = 0; i2 != 5000; i2++)
                    {
                        if (LoadedCreatures[0, i2] != null)
                        {
                            if (LoadedCreatures[0, i2] == "<Name>" + (sender as Label).Content)
                            {
                                CreaID = i2;
                            }
                        }
                    }

                    String FilePath = LoadedCreatures[1, CreaID];

                    for (int i2 = 0; i2 != CountOfFileLines(@"Bin\Data\Monsters\Addons.dnd"); i2++)
                    {
                        if (ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i2) == FilePath.Remove(2))
                        {
                            FilePath = @"Bin\Data\Monsters\" + ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i2 - 1) + ".dnd";
                        }
                    }
                    String[] Lines = new String[1000];
                    int MainCounter = 0;
                    int DiscriptionStart = 0;

                    for (int i2 = Int32.Parse(LoadedCreatures[1, CreaID].Remove(0, 3)); i2 != CountOfFileLines(FilePath); i2++)
                    {
                        if (ReadCertainLine(FilePath, i2) == "<Discription>") DiscriptionStart = i2 + 1;
                        if (ReadCertainLine(FilePath, i2) == "</X>") break;
                        else
                        {
                            Lines[MainCounter] = ReadCertainLine(FilePath, i2);
                            MainCounter++;
                        }
                    }
                    for (int i2 = 1; i2 != MainCounter; i2++)
                    {
                        if (Lines[i2].Contains("<Sil>") == true)
                        {
                            CP_StgValue.Content = Lines[i2].Remove(0, 5);
                        }
                        if (Lines[i2].Contains("<Lov>") == true)
                        {
                            CP_StgValue1.Content = Lines[i2].Remove(0, 5);
                        }
                        if (Lines[i2].Contains("<Tel>") == true)
                        {
                            CP_StgValue2.Content = Lines[i2].Remove(0, 5);
                        }
                        if (Lines[i2].Contains("<Int>") == true)
                        {
                            CP_StgValue3.Content = Lines[i2].Remove(0, 5);
                        }
                        if (Lines[i2].Contains("<Mdr>") == true)
                        {
                            CP_StgValue4.Content = Lines[i2].Remove(0, 5);
                        }
                        if (Lines[i2].Contains("<Har>") == true)
                        {
                            CP_StgValue5.Content = Lines[i2].Remove(0, 5);
                        }
                    }

                    for (int i = 0; i != 200; i++)
                    {
                        if (CreInCombatLbl[i] == SelectedCreature)
                        {
                            CP_HP.Text = CreInCombat[1, i];
                        }
                    }
                }

                for (int i = 0; i != CP_InfoStatsDock.Children.Count; i++)
                {
                    string Ti = ReturnedModificator(Int32.Parse(((CP_InfoStatsDock.Children[i] as Grid).Children[1] as Label).Content.ToString()));
                    ((CP_InfoStatsDock.Children[i] as Grid).Children[0] as Label).Content = Ti;
                }
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                DeletedCombatChar = sender as Label;
            }
        }

        public void LoadCreHP(int Slot)
        {
            int CreaID = 0;
            for (int i = 0; i != 5000; i++)
            {
                if (LoadedCreatures[0, i] != null)
                {
                    if (LoadedCreatures[0, i] == "<Name>" + DroppedLabel.Content)
                    {
                        CreaID = i;
                    }
                }
            }

            String FilePath = LoadedCreatures[1, CreaID];

            for (int i = 0; i != CountOfFileLines(@"Bin\Data\Monsters\Addons.dnd"); i++)
            {
                if (ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i) == FilePath.Remove(2))
                {
                    FilePath = @"Bin\Data\Monsters\" + ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i - 1) + ".dnd";
                }
            }
            String[] Lines = new String[1000];
            int MainCounter = 0;
            int DiscriptionStart = 0;

            for (int i = Int32.Parse(LoadedCreatures[1, CreaID].Remove(0, 3)); i != CountOfFileLines(FilePath); i++)
            {
                if (ReadCertainLine(FilePath, i) == "<Discription>") DiscriptionStart = i + 1;
                if (ReadCertainLine(FilePath, i) == "</X>") break;
                else
                {
                    Lines[MainCounter] = ReadCertainLine(FilePath, i);
                    MainCounter++;
                }
            }
            for (int i = 1; i != MainCounter; i++)
            {
                if (Lines[i].Contains("<Hit>") == true)
                {
                    CreInCombat[1, Slot] = Lines[i].Remove(0, 5);
                }
                if (Lines[i].Contains("<HitDiceCount>") == true)
                {
                    CreInCombat[2, Slot] = Lines[i].Remove(0, 14);
                }
                if (Lines[i].Contains("<HitDice>") == true)
                {
                    CreInCombat[2, Slot] = CreInCombat[2, Slot] + "d" + Lines[i].Remove(0, 9) + ")";
                }
                if (Lines[i].Contains("<HitStatic>") == true)
                {
                    CreInCombat[2, Slot] = CreInCombat[2, Slot].ToString().Remove(CreInCombat[2, Slot].ToString().Length - 1, 1) + " + " + Lines[i].Remove(0, 11);
                }
            }
        }

        private void CharInicDC(object sender, MouseButtonEventArgs e)
        {
            (((sender as Label).Parent as Grid).Children[2] as TextBox).Visibility = Visibility.Visible;
            (((sender as Label).Parent as Grid).Children[2] as TextBox).Text = (sender as Label).Content.ToString();
            (sender as Label).Visibility = Visibility.Collapsed;
        }

        private void CharInicDCAccept(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((sender as TextBox).Parent as Grid).Children[1].Visibility = Visibility.Visible;
                (((sender as TextBox).Parent as Grid).Children[1] as Label).Content = (sender as TextBox).Text;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
        }

        private void ToGeneratorLbl10_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pages.SelectedIndex = 8;
        }

        public void CombatCharMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right) DeletedCombatChar = sender as Label;
            if (e.ChangedButton == MouseButton.Left)
            {
                if ((sender as Label).Tag.ToString() == "Char")
                {
                    HideAllTopButtons();
                    Pages.SelectedIndex = 4;
                    CL_Chars.Children.Clear();
                    CL_CharSpace.Visibility = Visibility.Collapsed;
                    CL_CharList.Width = 260;
                    CL_Chars.Visibility = Visibility.Visible;
                    CL_CharList.Background = new SolidColorBrush(Color.FromArgb(10, 255, 255, 255));
                    for (int i = 0; i != 33000; i++)
                    {
                        if (LoadedProfile[i].IndexOf("<PlayerChar>") != -1)
                        {
                            Label Chars = new Label();
                            CL_Chars.Children.Add(Chars);
                            Chars.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                            Chars.Background = new SolidColorBrush(Color.FromArgb(50, 76, 76, 149));
                            DockPanel.SetDock(Chars, Dock.Top);
                            Chars.HorizontalContentAlignment = HorizontalAlignment.Center;
                            Chars.HorizontalAlignment = HorizontalAlignment.Stretch;
                            Chars.Width = Double.NaN;
                            Chars.Content = LoadedProfile[i].Remove(0, 12);
                            Chars.Margin = new Thickness(5, 5, 5, 0);
                        }
                    }
                    Label NewChar = new Label();
                    CL_Chars.Children.Add(NewChar);
                    NewChar.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                    NewChar.Background = new SolidColorBrush(Color.FromArgb(51, 162, 162, 162));
                    DockPanel.SetDock(NewChar, Dock.Top);
                    NewChar.HorizontalContentAlignment = HorizontalAlignment.Center;
                    NewChar.HorizontalAlignment = HorizontalAlignment.Stretch;
                    NewChar.Width = Double.NaN;
                    NewChar.Content = "Новый персонаж";
                    NewChar.Margin = new Thickness(5, 5, 5, 0);
                    NewChar.MouseEnter += CL_NewChar_MouseEnter;
                    NewChar.MouseLeave += CL_NewChar_MouseLeave;
                    NewChar.MouseDown += CL_NewChar_MouseDown;
                }
                if ((sender as Label).Tag.ToString() == "Monster")
                {
                    CreatureTempWindow CTW = new CreatureTempWindow();
                    CTW.Title = (sender as Label).Content.ToString();
                    CTW.MC_BlckDiscr.Visibility = Visibility.Collapsed;
                    CTW.MC_BlockDiscription.Inlines.Clear();
                    CTW.MC_Uses.Children.Clear();
                    CTW.MC_Skills.Children.Clear();
                    CTW.MC_Actions.Children.Clear();
                    CTW.MC_LegendaryActions.Children.Clear();
                    CTW.MC_Ceep.Children.Clear();
                    CTW.MC_CeepAction.Children.Clear();
                    CTW.MC_CeepEffect.Children.Clear();
                    CTW.MC_Reactions.Children.Clear();

                    CTW.MC_UnderReactions.Visibility = Visibility.Collapsed;
                    CTW.MC_Reactions.Visibility = Visibility.Collapsed;
                    CTW.MC_UnderSkills.Visibility = Visibility.Collapsed;
                    CTW.MC_Skills.Visibility = Visibility.Collapsed;
                    CTW.MC_Actions.Visibility = Visibility.Collapsed;
                    CTW.MC_UnderActions.Visibility = Visibility.Collapsed;
                    CTW.MC_LegendaryActions.Visibility = Visibility.Collapsed;
                    CTW.MC_Ceep.Visibility = Visibility.Collapsed;
                    CTW.MC_CeepAction.Visibility = Visibility.Collapsed;
                    CTW.MC_CeepEffect.Visibility = Visibility.Collapsed;
                    CTW.MC_UnderLegendaryActions.Visibility = Visibility.Collapsed;
                    CTW.MC_UnderCeep.Visibility = Visibility.Collapsed;
                    CTW.MC_UnderCeepAction.Visibility = Visibility.Collapsed;
                    CTW.MC_UnderCeepEffect.Visibility = Visibility.Collapsed;

                    CTW.MC_Dicsription.Inlines.Clear();
                    CTW.MC_Dicsription.Cursor = Cursors.Arrow;
                    CTW.MC_Dicsription.MouseDown -= GoToLink;

                    String[] TempArray = new string[7];
                    DockPanel[] TempDockPanel = new DockPanel[7];

                    TempArray[0] = "-СПОСОБНОСТИ-";
                    TempArray[1] = "-ДЕЙСТВИЯ-";
                    TempArray[2] = "-ЛЕГЕНДАРНЫЕ ДЕЙСТВИЯ-";
                    TempArray[3] = "-ЛОГОВО-";
                    TempArray[4] = "-ДЕЙСТВИЯ ЛОГОВА-";
                    TempArray[5] = "-ЭФФЕКТЫ ЛОГОВА-";
                    TempArray[6] = "-РЕАКЦИИ-";
                    TempDockPanel[0] = CTW.MC_Skills;
                    TempDockPanel[1] = CTW.MC_Actions;
                    TempDockPanel[2] = CTW.MC_LegendaryActions;
                    TempDockPanel[3] = CTW.MC_Ceep;
                    TempDockPanel[4] = CTW.MC_CeepAction;
                    TempDockPanel[5] = CTW.MC_CeepEffect;
                    TempDockPanel[6] = CTW.MC_Reactions;

                    for (int i = 0; i != 7; i++)
                    {
                        Label UsesLbl = new Label();
                        TempDockPanel[i].Children.Add(UsesLbl);
                        UsesLbl.Height = MC_DiscrLabel.Height;
                        UsesLbl.Width = MC_DiscrLabel.Width;
                        UsesLbl.Foreground = MC_DiscrLabel.Foreground;
                        UsesLbl.Effect = MC_DiscrLabel.Effect;
                        UsesLbl.Content = TempArray[i];
                        DockPanel.SetDock(UsesLbl, Dock.Top);

                    }

                    int CreaID = 0;
                    for (int i = 0; i != 5000; i++)
                    {
                        if (LoadedCreatures[0, i] != null)
                        {
                            if (LoadedCreatures[0, i] == "<Name>" + (sender as Label).Content)
                            {
                                CreaID = i;
                            }
                        }
                    }

                    String FilePath = LoadedCreatures[1, CreaID];

                    for (int i = 0; i != CountOfFileLines(@"Bin\Data\Monsters\Addons.dnd"); i++)
                    {
                        if (ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i) == FilePath.Remove(2))
                        {
                            FilePath = @"Bin\Data\Monsters\" + ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i - 1) + ".dnd";
                        }
                    }
                    String[] Lines = new String[1000];
                    int MainCounter = 0;
                    int DiscriptionStart = 0;

                    for (int i = Int32.Parse(LoadedCreatures[1, CreaID].Remove(0, 3)); i != CountOfFileLines(FilePath); i++)
                    {
                        if (ReadCertainLine(FilePath, i) == "<Discription>") DiscriptionStart = i + 1;
                        if (ReadCertainLine(FilePath, i) == "</X>") break;
                        else
                        {
                            Lines[MainCounter] = ReadCertainLine(FilePath, i);
                            MainCounter++;
                        }
                    }
                    for (int i = 1; i != MainCounter; i++)
                    {
                        if (Lines[i].Contains("<Name>") == true)
                        {
                            CTW.MC_Name.Content = Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<IMG>") == true)
                        {
                            Image ImageContainer = CTW.MC_Preview;
                            ImageSource image = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Bin\img\Monsters\" + Lines[i].Remove(0, 5) + ".png", UriKind.Absolute));
                            ImageContainer.Source = image;
                            CTW.MC_Preview.Uid = Environment.CurrentDirectory + @"\Bin\img\Monsters\" + Lines[i].Remove(0, 5) + "_Full" + ".png";
                        }
                        if (Lines[i].Contains("<Mainlink>") == true)
                        {
                            CTW.LinkMain.Uid = Lines[i].Remove(0, 10);
                        }
                        if (Lines[i].Contains("<Size>") == true)
                        {
                            CTW.MC_TypeSizeView.Content = Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<Type>") == true)
                        {
                            CTW.MC_TypeSizeView.Content = CTW.MC_TypeSizeView.Content.ToString() + " " + Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<View>") == true)
                        {
                            CTW.MC_TypeSizeView.Content = CTW.MC_TypeSizeView.Content.ToString() + ", " + Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<Armor>") == true)
                        {
                            CTW.MC_Armor.Content = "Класс доспеха: " + Lines[i].Remove(0, 7);
                        }
                        if (Lines[i].Contains("<ArmorType>") == true)
                        {
                            CTW.MC_Armor.Content = CTW.MC_Armor.Content.ToString() + " " + Lines[i].Remove(0, 11);
                        }
                        if (Lines[i].Contains("<Hit>") == true)
                        {
                            CTW.MC_Hit.Content = Lines[i].Remove(0, 5);
                        }
                        if (Lines[i].Contains("<HitDiceCount>") == true)
                        {
                            CTW.MC_HitDice.Content = "(" + Lines[i].Remove(0, 14);
                        }
                        if (Lines[i].Contains("<HitDice>") == true)
                        {
                            CTW.MC_HitDice.Content = CTW.MC_HitDice.Content.ToString() + "d" + Lines[i].Remove(0, 9) + ")";
                        }
                        if (Lines[i].Contains("<HitStatic>") == true)
                        {
                            CTW.MC_HitDice.Content = CTW.MC_HitDice.Content.ToString().Remove(CTW.MC_HitDice.Content.ToString().Length - 1, 1) + " + " + Lines[i].Remove(0, 11) + ")";
                        }
                        if (Lines[i].Contains("<Walk>") == true)
                        {
                            CTW.MC_Speed.Content = "Скорость: " + Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<Swim>") == true)
                        {
                            CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Плавая: " + Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<Grab>") == true)
                        {
                            CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Лазая: " + Lines[i].Remove(0, 6);
                        }
                        if (Lines[i].Contains("<Dig>") == true)
                        {
                            CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Копая: " + Lines[i].Remove(0, 5);
                        }
                        if (Lines[i].Contains("<Fly>") == true)
                        {
                            CTW.MC_Speed.Content = CTW.MC_Speed.Content.ToString() + ", Летая: " + Lines[i].Remove(0, 5);
                        }
                        if (Lines[i].Contains("<Sil>") == true)
                        {
                            CTW.MC_Sil.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                        }
                        if (Lines[i].Contains("<Lov>") == true)
                        {
                            CTW.MC_Lov.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                        }
                        if (Lines[i].Contains("<Tel>") == true)
                        {
                            CTW.MC_Tel.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                        }
                        if (Lines[i].Contains("<Int>") == true)
                        {
                            CTW.MC_Int.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                        }
                        if (Lines[i].Contains("<Mdr>") == true)
                        {
                            CTW.MC_Mdr.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                        }
                        if (Lines[i].Contains("<Har>") == true)
                        {
                            CTW.MC_Har.Content = Lines[i].Remove(0, 5) + "(" + ReturnedModificator(Int32.Parse(Lines[i].Remove(0, 5))) + ")";
                        }
                        if (Lines[i].Contains("<SpellTab>") == true)
                        {
                            int SpellCounter = i + 1;
                            while (Lines[SpellCounter] != "</SpellTab>")
                            {
                                TextBlock TB = new TextBlock();
                                CTW.MC_Skills.Children.Add(TB);
                                DockPanel.SetDock(TB, Dock.Top);
                                TB.TextWrapping = TextWrapping.Wrap;
                                TB.Inlines.Add(new Run(" * " + " ") { FontWeight = FontWeights.Bold });
                                TB.Inlines.Add(new Run(Lines[SpellCounter]) { FontStyle = FontStyles.Italic });
                                SpellCounter++;
                            }
                            i = SpellCounter;
                        }
                        if (Lines[i].Contains("<Chalange>") == true)
                        {
                            TextBlock TB = new TextBlock();
                            CTW.MC_Uses.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run("Опасность:" + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 10)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<Source>") == true)
                        {
                            TextBlock TB = new TextBlock();
                            CTW.MC_Uses.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run("Источник:" + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 8)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<j1>") == true)
                        {
                            int end = Lines[i].Remove(0, 4).IndexOf("<j1>") + 4;
                            TextBlock TB = new TextBlock();
                            CTW.MC_Uses.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                            Console.WriteLine(Lines[i]);
                        }
                        if (Lines[i].Contains("<j2>") == true)
                        {
                            CTW.MC_UnderSkills.Visibility = Visibility.Visible;
                            CTW.MC_Skills.Visibility = Visibility.Visible;
                            int end = Lines[i].Remove(0, 4).IndexOf("<j2>") + 4;
                            TextBlock TB = new TextBlock();
                            CTW.MC_Skills.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<j3>") == true)
                        {
                            CTW.MC_UnderActions.Visibility = Visibility.Visible;
                            CTW.MC_Actions.Visibility = Visibility.Visible;
                            int end = Lines[i].Remove(0, 4).IndexOf("<j3>") + 4;
                            TextBlock TB = new TextBlock();
                            CTW.MC_Actions.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<jr>") == true)
                        {
                            CTW.MC_UnderReactions.Visibility = Visibility.Visible;
                            CTW.MC_Reactions.Visibility = Visibility.Visible;
                            int end = Lines[i].Remove(0, 4).IndexOf("<jr>") + 4;
                            TextBlock TB = new TextBlock();
                            CTW.MC_Reactions.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<jB>") == true)
                        {
                            String Headd = Lines[i];
                            String Content = Lines[i];

                            Headd = Headd.Remove(0, 4);
                            Headd = Headd.Remove(Headd.IndexOf(">") + 1);
                            Headd = Headd.Remove(Headd.Length - 4, 4);

                            Content = Content.Remove(0, 4);
                            Content = Content.Remove(0, Content.IndexOf(">") + 1);
                            if (Content.Contains("[j]") == true)
                            {
                                String Content2 = Content;
                                Content2 = Content2.Remove(Content2.IndexOf("[j]"));
                                CTW.MC_BlckDiscr.Visibility = Visibility.Visible;
                                CTW.MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                                CTW.MC_BlockDiscription.Inlines.Add("\n");
                                CTW.MC_BlockDiscription.Inlines.Add(Content2);
                                Content2 = Content;
                                Content2 = Content2.Remove(0, Content2.IndexOf("[j]"));
                                String jHead = Content2;
                                jHead = jHead.Remove(0, 3);
                                jHead = jHead.Remove(jHead.IndexOf("[/j]"));
                                CTW.MC_BlockDiscription.Inlines.Add("\n");
                                CTW.MC_BlockDiscription.Inlines.Add(new Run(jHead) { FontWeight = FontWeights.Bold });
                                CTW.MC_BlockDiscription.Inlines.Add("\n");
                                Content2 = Content2.Remove(0, Content2.IndexOf("[/j]") + 4);
                                CTW.MC_BlockDiscription.Inlines.Add(Content2);
                            }
                            else
                            {
                                CTW.MC_BlckDiscr.Visibility = Visibility.Visible;
                                CTW.MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                                CTW.MC_BlockDiscription.Inlines.Add("\n");
                                CTW.MC_BlockDiscription.Inlines.Add(Content);
                            }
                        }
                        if (Lines[i].Contains("<jBT>") == true)
                        {
                            String Headd = Lines[i];
                            String Content = Lines[i];

                            CTW.MC_BlockDiscription.Inlines.Add("\n");
                            Headd = Headd.Remove(0, 5);
                            Content = Headd.Remove(0, Headd.IndexOf("<jBT>") + 5);
                            Headd = Headd.Remove(Headd.IndexOf("<jBT>"));
                            CTW.MC_BlockDiscription.Inlines.Add(new Run(Headd) { FontWeight = FontWeights.Bold });
                            CTW.MC_BlockDiscription.Inlines.Add(Content);
                        }
                        if (Lines[i].Contains("<Legendary>") == true)
                        {
                            CTW.MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                            CTW.MC_LegendaryActions.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_LegendaryActions.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 11)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<lg>") == true)
                        {
                            MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                            MC_LegendaryActions.Visibility = Visibility.Visible;

                            int end = Lines[i].Remove(0, 4).IndexOf("<lg>") + 4;
                            CTW.MC_UnderLegendaryActions.Visibility = Visibility.Visible;
                            CTW.MC_LegendaryActions.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_LegendaryActions.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4).Remove(end - 4) + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, end + 4)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<ceep>") == true)
                        {
                            CTW.MC_UnderCeep.Visibility = Visibility.Visible;
                            CTW.MC_Ceep.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_Ceep.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 6)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<ca>") == true)
                        {
                            CTW.MC_UnderCeepAction.Visibility = Visibility.Visible;
                            CTW.MC_CeepAction.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_CeepAction.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<dca>") == true)
                        {
                            CTW.MC_UnderCeepAction.Visibility = Visibility.Visible;
                            CTW.MC_CeepAction.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_CeepAction.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(" * " + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 5)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<ce>") == true)
                        {
                            CTW.MC_UnderCeepEffect.Visibility = Visibility.Visible;
                            CTW.MC_CeepEffect.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_CeepEffect.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run("" + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 4)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i].Contains("<dce>") == true)
                        {
                            CTW.MC_UnderCeepEffect.Visibility = Visibility.Visible;
                            CTW.MC_CeepEffect.Visibility = Visibility.Visible;
                            TextBlock TB = new TextBlock();
                            CTW.MC_CeepEffect.Children.Add(TB);
                            DockPanel.SetDock(TB, Dock.Top);
                            TB.TextWrapping = TextWrapping.Wrap;
                            TB.Inlines.Add(new Run(" * " + " ") { FontWeight = FontWeights.Bold });
                            TB.Inlines.Add(new Run(Lines[i].Remove(0, 5)) { FontStyle = FontStyles.Italic });
                        }
                        if (Lines[i] == "<Discription>")
                        {
                            for (int i2 = i + 1; i2 != MainCounter; i2++)
                            {
                                bool Special = false;
                                if (Lines[i2].Contains("<L>") == true)
                                {
                                    CTW.MC_Dicsription.TextWrapping = TextWrapping.Wrap;
                                    CTW.MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3).Remove(Lines[i2].Remove(0, 3).IndexOf("<"))) { TextDecorations = TextDecorations.Underline });

                                    CTW.MC_Dicsription.Cursor = Cursors.Hand;
                                    String s = Lines[i2].Remove(0, 3);
                                    s = s.Remove(0, s.IndexOf("<") + 1);
                                    s = s.Remove(s.Length - 1);
                                    CTW.MC_Dicsription.Uid = s;
                                    CTW.MC_Dicsription.MouseUp += GoToLink;
                                    Special = true;
                                }
                                if (Lines[i2].Contains("<j>") == true)
                                {
                                    CTW.MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3)) { FontWeight = FontWeights.Bold });
                                    Special = true;
                                }
                                if (Lines[i2].Contains("<h>") == true)
                                {
                                    CTW.MC_Dicsription.Inlines.Add("\n");
                                    CTW.MC_Dicsription.Inlines.Add(new Run(Lines[i2].Remove(0, 3)) { FontWeight = FontWeights.Bold });
                                    Special = true;
                                }
                                if (Special == false)
                                {
                                    CTW.MC_Dicsription.Inlines.Add(Lines[i2]);
                                }
                            }
                            i = MainCounter - 1;
                        }
                    }
                    CTW.Show();
                }
            }
        }

        public void RemoveCharFromCombat(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i != 200; i++)
            {
                if (CreInCombatLbl[i] != null)
                {
                    if (CreInCombatLbl[i] == DeletedCombatChar)
                    {
                        CreInCombat[0, i] = null;
                        CreInCombat[1, i] = null;
                        CreInCombat[2, i] = null;
                        CreInCombatLbl[i] = null;
                    }
                }
            }
            CP_CombatListDock.Children.Remove((DeletedCombatChar.Parent as Grid));
            CP_StackInfo.Visibility = Visibility.Collapsed;
        }

        public void CreLoad()
        {
            CP_MainGrid.ColumnDefinitions[0].MinWidth = 200;
            CP_TempListDock.MaxWidth = 400;
            CP_TempList.Visibility = Visibility.Visible;
            CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(200);
            CP_GS1.Visibility = Visibility.Visible;
            CP_TempListDock.Children.Clear();

            TextBox MonsterSerach = new TextBox();
            CP_TempListDock.Children.Add(MonsterSerach);
            DockPanel.SetDock(MonsterSerach, Dock.Top);
            MonsterSerach.Margin = new Thickness(0, 5, 0, 0);
            MonsterSerach.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
            MonsterSerach.Text = "Поиск существа";
            CharsLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            MonsterSerach.FontFamily = new FontFamily("Bookman Old Style");
            MonsterSerach.FontSize = 12;
            MonsterSerach.MouseDown += TempMoveDown;
            MonsterSerach.FlowDirection = FlowDirection.LeftToRight;
            MonsterSerach.HorizontalContentAlignment = HorizontalAlignment.Center;
            MonsterSerach.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            MonsterSerach.KeyUp += MonsterSearchKeyUp;

            int i = 0;
            while (LoadedCreatures[0, i] != null)
            {
                Label CharsLbl = new Label();
                CP_TempListDock.Children.Add(CharsLbl);
                DockPanel.SetDock(CharsLbl, Dock.Top);
                CharsLbl.Margin = new Thickness(0, 5, 0, 0);
                CharsLbl.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                CharsLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                CharsLbl.Content = LoadedCreatures[0, i].Remove(0, 6);
                CharsLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                CharsLbl.FontFamily = new FontFamily("Bookman Old Style");
                CharsLbl.FontSize = 12;
                CharsLbl.MouseDown += TempMoveDown;
                i++;
            }
            CP_GS1.Tag = "CreOpen";
        }

        private void CreaturesLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bool Creatble = false;
            if (CP_GS1.Tag.ToString() == "null")
            {
                CreLoad();
                Creatble = true;
            }
            if (CP_GS1.Tag.ToString() == "CharsOpen")
            {
                CreLoad();
                Creatble = true;
            }
            if (CP_GS1.Tag.ToString() == "CharsClose")
            {
                CreLoad();
                Creatble = true;
            }


            if (Creatble == false)
            {
                if (CP_GS1.Tag.ToString() == "CreOpen")
                {
                    CP_MainGrid.ColumnDefinitions[0].MinWidth = 0;
                    CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
                    CP_MainGrid.ColumnDefinitions[0].MaxWidth = 0;
                    CP_TempList.Visibility = Visibility.Collapsed;
                    CP_GS1.Visibility = Visibility.Collapsed;
                    CP_GS1.Tag = "CreClose";
                }
                else
            if (CP_GS1.Tag.ToString() == "CreClose")
                {
                    CP_MainGrid.ColumnDefinitions[0].MinWidth = 200;
                    CP_TempList.Visibility = Visibility.Visible;
                    CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(200);
                    CP_GS1.Visibility = Visibility.Visible;
                    CP_GS1.Tag = "CreOpen";
                }
            }
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromRgb(255, 212, 173));
        }

        private void MonsterSearchKeyUp(object sender, KeyEventArgs e)
        {
            if ((sender as TextBox).Text != "")
            {
                for (int i = 1; i != CP_TempListDock.Children.Count; i++)
                {
                    if ((CP_TempListDock.Children[i] as Label).Content.ToString().IndexOf((sender as TextBox).Text) == -1)
                    {
                        CP_TempListDock.Children[i].Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                for (int i = 0; i != CP_TempListDock.Children.Count; i++)
                {
                    CP_TempListDock.Children[i].Visibility = Visibility.Visible;
                }
            }
        }

        private void Header_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((this.Top != 0) & (this.Left != 0) & (this.Height != SystemParameters.WorkArea.Height) & (this.Width != SystemParameters.WorkArea.Width))
            {
                OldWIndowParams[0] = this.Top;
                OldWIndowParams[1] = this.Left;
                OldWIndowParams[2] = this.Height;
                OldWIndowParams[3] = this.Width;
                this.Left = 0;
                this.Top = 0;
                this.Height = SystemParameters.WorkArea.Height;
                this.Width = SystemParameters.WorkArea.Width;
            }
            else
            {
                this.Left = OldWIndowParams[1];
                this.Top = OldWIndowParams[0];
                this.Height = OldWIndowParams[2];
                this.Width = OldWIndowParams[3];
            }
        }

        private void CL_StgEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            Label SLabel = null;
            Label ValLabele = null;
            if (sender == CL_StgEditor)
            {
                SLabel = CL_Stg;
                ValLabele = CL_StgValue;
            }
            if (sender == CL_DexEditor)
            {
                SLabel = CL_Dex;
                ValLabele = CL_DexValue;
            }
            if (sender == CL_ConEditor)
            {
                SLabel = CL_Con;
                ValLabele = CL_ConValue;
            }
            if (sender == CL_IntEditor)
            {
                SLabel = CL_Int;
                ValLabele = CL_IntValue;
            }
            if (sender == CL_WidEditor)
            {
                SLabel = CL_Wid;
                ValLabele = CL_WidValue;
            }
            if (sender == CL_ChaEditor)
            {
                SLabel = CL_Cha;
                ValLabele = CL_ChaValue;
            }

            if ((sender as TextBox).Text == "") (sender as TextBox).Text = "1";
            (sender as TextBox).Visibility = Visibility.Collapsed;
            ValLabele.Content = (sender as TextBox).Text;
            SLabel.Visibility = Visibility.Visible;
            int Modify = 0;

            if (Int32.Parse((sender as TextBox).Text) >= 10)
            {
                Modify = (Int32.Parse((sender as TextBox).Text) - 10) / 2;
            }
            else
            {
                if (Int32.Parse((sender as TextBox).Text) > 0)
                {
                    if (Int32.Parse((sender as TextBox).Text) == 9) Modify = -1;
                    if (Int32.Parse((sender as TextBox).Text) == 8) Modify = -1;
                    if (Int32.Parse((sender as TextBox).Text) == 7) Modify = -2;
                    if (Int32.Parse((sender as TextBox).Text) == 6) Modify = -2;
                    if (Int32.Parse((sender as TextBox).Text) == 5) Modify = -3;
                    if (Int32.Parse((sender as TextBox).Text) == 4) Modify = -3;
                    if (Int32.Parse((sender as TextBox).Text) == 3) Modify = -4;
                    if (Int32.Parse((sender as TextBox).Text) == 2) Modify = -4;
                    if (Int32.Parse((sender as TextBox).Text) == 1) Modify = -5;
                }
                else
                {
                    SLabel.Content = Modify = -5;
                }
            }

            if (Modify >= 1) SLabel.Content = "+" + Modify.ToString();
            if (Modify == 0) SLabel.Content = "_0";
            if (Modify < - -1) SLabel.Content = Modify.ToString();
            CalculateHitBinus();
        }

        private void MP_LoadMap_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".png"; // Default file extension
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                Nullable<bool> result = dlg.ShowDialog();
                if (dlg.FileName != "")
                {
                    var uri = new Uri(dlg.FileName);
                    var bitmap = new BitmapImage(uri);
                    MP_MapImage.Source = bitmap;
                }
            }
        }

        private void MP_MapImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //MP_MapScroll.CanContentScroll = false;
            //MP_MapImage.Height = MP_MapImage.Height+100;
           // MP_MapScroll.CanContentScroll = true;
        }

        private void MP_MapSizeDown_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MP_MapImage.Height >= 1000)
            {
                MP_MapImage.Height = MP_MapImage.Height - 250;
            }
            else
            {
                MP_MapImage.Height = MP_MapImage.Height - 25;
            }
        }

        private void MP_MapSizeUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MP_MapImage.Height >= 1000)
            {
                MP_MapImage.Height = MP_MapImage.Height + 250;
            }
            else
            {
                MP_MapImage.Height = MP_MapImage.Height + 25;
            }
        }

        private void MP_LoadMap_MouseEnter(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(52, 255, 255, 255));
        }

        private void MP_LoadMap_MouseLeave(object sender, MouseEventArgs e)
        {
            ((sender as Label).Parent as Border).Background = new SolidColorBrush(Color.FromArgb(52, 0, 0, 0));
        }

        private void MP_Rows_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MP_Rows.Text == "") MP_Rows.Text = "0";
            if (MP_Rows.Text != "0")
            {
                if (MP_Colums.Text == "0")
                {
                    MP_GridDP.Children.Clear();
                    for (int i = 0; i != Int32.Parse(MP_Rows.Text); i++)
                    {

                        Border Brdr = new Border();
                        DockPanel DP = new DockPanel();
                        DP.Children.Add(Brdr);
                        Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                        Brdr.Width = Double.NaN;
                        Brdr.Height = 25;
                        Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        if (i == 0) Brdr.BorderThickness = new Thickness(2, 2, 2, 2);
                        if (i != 0) Brdr.BorderThickness = new Thickness(2, 0, 2, 2);
                        MP_GridDP.Children.Add(DP);
                        DP.HorizontalAlignment = HorizontalAlignment.Stretch;
                        DockPanel.SetDock(DP, Dock.Top);
                        DP.Height = 25;
                        DP.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                    }
                }
                if (MP_Colums.Text != "0")
                {
                    MP_GridDP.Children.Clear();
                    for (int i = 0; i != Int32.Parse(MP_Rows.Text); i++)
                    {
                        DockPanel DP = new DockPanel();
                        MP_GridDP.Children.Add(DP);
                        DP.HorizontalAlignment = HorizontalAlignment.Left;
                        DockPanel.SetDock(DP, Dock.Top);
                        DP.Height = Double.NaN;
                        DP.Width = Double.NaN;
                        DP.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                        for (int ii=0; ii != Int32.Parse(MP_Colums.Text); ii++)
                        {
                            Border Brdr = new Border();
                            DP.Children.Add(Brdr);
                            Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                            Brdr.Width = Int32.Parse(MP_GridSize.Text);
                            Brdr.Height = Int32.Parse(MP_GridSize.Text);
                            Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                            Brdr.MouseEnter += MP_CellMouseEnter;
                            Brdr.MouseLeave += MP_CellMouseLeave;
                            Brdr.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                            Brdr.Tag = "0";
                            Brdr.MouseDown += MP_CellMouseDown;
                            Brdr.MouseUp += MP_CellMouseUp;
                            Brdr.MouseMove += Map_CellMouseMove;
                            if (i== 0)
                            {
                                if (ii != (Int32.Parse(MP_Colums.Text) - 1)) Brdr.BorderThickness = new Thickness(2, 2, 0, 2);
                                else Brdr.BorderThickness = new Thickness(2, 2, 2, 2);
                            }
                            if (i != 0)
                            {
                                if (ii != (Int32.Parse(MP_Colums.Text) -1)) Brdr.BorderThickness = new Thickness(2,0, 0, 2);
                                else Brdr.BorderThickness = new Thickness(2, 0, 2, 2);
                            }
                        }
                    }
                }
            }
        }

        private void MP_Rows_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MP_Rows_LostFocus(null,null);
            }
        }

        private void MP_GridUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MP_GridDP.Margin = new Thickness(MP_GridDP.Margin.Left, MP_GridDP.Margin.Top - 1, 0, 0);
        }

        private void MP_GridRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MP_GridDP.Margin = new Thickness(MP_GridDP.Margin.Left+1, MP_GridDP.Margin.Top, 0, 0);
        }

        private void MP_GridLeft_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MP_GridDP.Margin = new Thickness(MP_GridDP.Margin.Left-1, MP_GridDP.Margin.Top, 0, 0);
        }

        private void MP_GridDown_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MP_GridDP.Margin = new Thickness(MP_GridDP.Margin.Left, MP_GridDP.Margin.Top+1, 0, 0);
        }

        private void MP_CalculateGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int GridHeight = 0;
            int GridWidth = 0;

            GridHeight = (int)MP_MapImage.ActualHeight / Int32.Parse(MP_GridSize.Text);
            GridWidth = (int)MP_MapImage.ActualWidth / Int32.Parse(MP_GridSize.Text);
            MP_Rows.Text = GridHeight.ToString();
            MP_Colums.Text = GridWidth.ToString();
            MP_Rows_LostFocus(null, null);
        }

        private void MP_GridClear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MP_GridDP.Children.Clear();
        }

        private void MP_GridCellSize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MP_MapImage.Tag = "Cell";
        }

        private void MP_MapImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MP_MapImage.Tag.ToString() != "null")
            {
                MP_MapImage.Tag = "null";
                MP_GridSize.Text = (CellMap.Height.ToString());
                Map_MainGrid.Children.Remove(CellMap);
            }
        }

        private void MP_MapImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MP_MapImage.Tag.ToString() == "Cell")
            {
                MP_GridDP.Children.Clear();
                CellMap = new Border();
                Map_MainGrid.Children.Add(CellMap);
                CellMap.BorderThickness = new Thickness(1, 1, 1, 1);
                CellMap.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                CellMap.VerticalAlignment = VerticalAlignment.Top;
                CellMap.HorizontalAlignment = HorizontalAlignment.Left;
                CellMap.Height = 25;
                CellMap.Width = 25;
                CellMap.Margin = new Thickness(e.GetPosition(Map_MainGrid).X+1, e.GetPosition(Map_MainGrid).Y+1, 0, 0);
                OldBrdrX = e.GetPosition(Map_MainGrid).X + 1;
                OldBrdrY = e.GetPosition(Map_MainGrid).Y + 1;
                MP_MapImage.Tag = "Cell1";
            }
        }

        private void MP_MapImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (MP_MapImage.Tag.ToString() == "Cell1")
            {
                if ((e.GetPosition(Map_MainGrid).Y - OldBrdrX) > 2) CellMap.Height = e.GetPosition(Map_MainGrid).Y - OldBrdrX-1;
                else CellMap.Height = 1;
                if ((e.GetPosition(Map_MainGrid).X - OldBrdrX) > 2) CellMap.Width = e.GetPosition(Map_MainGrid).X - OldBrdrX-1;
                else CellMap.Width = 1;
            }
        }

        private void MP_CellMouseEnter(object sender, MouseEventArgs e)
        {
            if ((sender as Border).Tag.ToString() == "0")
            {
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(127, 255, 255, 255));
            }
        }

        private void MP_CellMouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as Border).Tag.ToString() == "0")
            {
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            }
        }

        private void MP_CellMouseDown(object sender, MouseButtonEventArgs e)
        {
            SellBrush = true;
            if ((sender as Border).Tag.ToString() == "0")
            {
                (sender as Border).Tag = "1";
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            else
            {
                (sender as Border).Tag = "0";
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            }
        }

        private void MP_CellMouseUp(object sender, MouseButtonEventArgs e)
        {
            SellBrush = false;
        }

        private void Map_CellMouseMove(object sender, MouseEventArgs e)
        {
            if (SellBrush == true)
            {
                if ((sender as Border).Tag.ToString() == "0")
                {
                    (sender as Border).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    (sender as Border).Tag = "1";
                }
            }
        }

        private void MP_ClearBrush_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i=0;i != MP_GridDP.Children.Count; i++)
            {
                for (int ii=0; ii!= (MP_GridDP.Children[i] as DockPanel).Children.Count; ii++)
                {
                    ((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    ((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Tag = "0";
                }
            }
        }

        private void MP_ReverseColor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i != MP_GridDP.Children.Count; i++)
            {
                for (int ii = 0; ii != (MP_GridDP.Children[i] as DockPanel).Children.Count; ii++)
                {
                    if (((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Tag.ToString() == "0")
                    {
                        ((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Tag = "1";
                        ((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    }
                    else
                    {
                        ((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Tag = "0";
                        ((MP_GridDP.Children[i] as DockPanel).Children[ii] as Border).Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                    }
                }
            }
        }

        private void MS_StoryNameEdt_LostFocus(object sender, RoutedEventArgs e)
        {
            //
        }

        private void CL_CharNameEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CL_CharNameEdit.Text != "")
            {
                CL_CharName.Content = CL_CharNameEdit.Text;
                CL_CharName.Visibility = Visibility.Visible;
                CL_CharNameEdit.Visibility = Visibility.Collapsed;
            }
            else
            {
                CL_CharNameEdit.Text = "Безимянный";
                CL_CharName.Content = CL_CharNameEdit.Text;
                CL_CharName.Visibility = Visibility.Visible;
                CL_CharNameEdit.Visibility = Visibility.Collapsed;
            }
        }

        private void CL_RaseEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CL_RaseEdit.Text != "")
            {
                CL_Rase.Content = (sender as TextBox).Text;
                CL_Rase.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
            else
            {
                CL_RaseEdit.Text = "Неизвестной расы";
                CL_Rase.Content = (sender as TextBox).Text;
                CL_Rase.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
        }

        private void CL_ClassEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CL_ClassEditor.Text != "")
            {
                CL_Class.Content = (sender as TextBox).Text;
                CL_Class.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
            else
            {
                CL_ClassEditor.Text = "Безклассовый";
                CL_Class.Content = (sender as TextBox).Text;
                CL_Class.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
        }

        private void CL_AgeEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            CL_Age.Content = (sender as TextBox).Text;
            CL_Age.Visibility = Visibility.Visible;
            (sender as TextBox).Visibility = Visibility.Collapsed;

            String A = CL_AgeEditor.Text;
            if (A.Length > 1)
            {
                A = A.Remove(0, A.Length - 1);
                if (Int32.Parse(A) == 0) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 1) CL_AgePreffics.Content = "год";
                if (Int32.Parse(A) == 2) CL_AgePreffics.Content = "года";
                if (Int32.Parse(A) == 3) CL_AgePreffics.Content = "года";
                if (Int32.Parse(A) == 4) CL_AgePreffics.Content = "года";
                if (Int32.Parse(A) == 5) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 6) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 7) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 8) CL_AgePreffics.Content = "лет";
                if (Int32.Parse(A) == 9) CL_AgePreffics.Content = "лет";
            }
            else
            {
                if (A != "")
                {
                    if (Int32.Parse(A) == 0) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 1) CL_AgePreffics.Content = "год";
                    if (Int32.Parse(A) == 2) CL_AgePreffics.Content = "года";
                    if (Int32.Parse(A) == 3) CL_AgePreffics.Content = "года";
                    if (Int32.Parse(A) == 4) CL_AgePreffics.Content = "года";
                    if (Int32.Parse(A) == 5) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 6) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 7) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 8) CL_AgePreffics.Content = "лет";
                    if (Int32.Parse(A) == 9) CL_AgePreffics.Content = "лет";
                }
                else
                {
                    CL_Age.Content = "1";
                    CL_AgePreffics.Content = "год";
                }
            }
        }

        private void CL_ArmorEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CL_ArmorEditor.Text != "")
            {
                CL_Armor.Content = (sender as TextBox).Text;
                CL_Armor.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
            else
            {
                CL_ArmorEditor.Text = "5";
                CL_Armor.Content = (sender as TextBox).Text;
                CL_Armor.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
        }

        private void CL_SpeedEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CL_SpeedEditor.Text != "")
            {
                CL_Speed.Content = (sender as TextBox).Text;
                CL_Speed.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
            else
            {
                CL_SpeedEditor.Text = "0";
                CL_Speed.Content = (sender as TextBox).Text;
                CL_Speed.Visibility = Visibility.Visible;
                (sender as TextBox).Visibility = Visibility.Collapsed;
            }
        }

        private void CLS_SpellLevelHelp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CLS_HelpGrid.Visibility = Visibility.Visible;
        }

        private void CLS_HelpGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CLS_HelpGrid.Visibility = Visibility.Collapsed;
        }

        private void CLS_SpellLevel_DropDownOpened(object sender, EventArgs e)
        {
            (CLS_SpellLevel.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
        }

        private void CLS_SpellLevel_DropDownClosed(object sender, EventArgs e)
        {
            (CLS_SpellLevel.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
            Border[,] Cells = new Border[10, 5];
            Cells[1, 1] = CLS_SG11;
            Cells[1, 2] = CLS_SG12;
            Cells[1, 3] = CLS_SG13;
            Cells[1, 4] = CLS_SG14;
            Cells[2, 1] = CLS_SG21;
            Cells[2, 2] = CLS_SG22;
            Cells[2, 3] = CLS_SG23;
            Cells[2, 4] = CLS_SG24;
            Cells[3, 1] = CLS_SG31;
            Cells[3, 2] = CLS_SG32;
            Cells[3, 3] = CLS_SG33;
            Cells[3, 4] = CLS_SG34;
            Cells[4, 1] = CLS_SG41;
            Cells[4, 2] = CLS_SG42;
            Cells[4, 3] = CLS_SG43;
            Cells[4, 4] = CLS_SG44;
            Cells[5, 1] = CLS_SG51;
            Cells[5, 2] = CLS_SG52;
            Cells[5, 3] = CLS_SG53;
            Cells[5, 4] = CLS_SG54;
            Cells[6, 1] = CLS_SG61;
            Cells[6, 2] = CLS_SG62;
            Cells[6, 3] = CLS_SG63;
            Cells[6, 4] = CLS_SG64;
            Cells[7, 1] = CLS_SG71;
            Cells[7, 2] = CLS_SG72;
            Cells[7, 3] = CLS_SG73;
            Cells[7, 4] = CLS_SG74;
            Cells[8, 1] = CLS_SG81;
            Cells[8, 2] = CLS_SG82;
            Cells[8, 3] = CLS_SG83;
            Cells[8, 4] = CLS_SG84;
            Cells[9, 1] = CLS_SG91;
            Cells[9, 2] = CLS_SG92;
            Cells[9, 3] = CLS_SG93;
            Cells[9, 4] = CLS_SG94;
            String[] CurrentCells = new string[10];
            for (int R = 1; R != 10; R++)
            {
                for (int C = 1; C != 5; C++)
                {
                    Cells[R, C].Visibility = Visibility.Hidden;
                }
            }
            CurrentCells[1] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[0].ToString();
            CurrentCells[2] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[1].ToString();
            CurrentCells[3] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[2].ToString();
            CurrentCells[4] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[3].ToString();
            CurrentCells[5] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[4].ToString();
            CurrentCells[6] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[5].ToString();
            CurrentCells[7] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[6].ToString();
            CurrentCells[8] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[7].ToString();
            CurrentCells[9] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[8].ToString();
            for (int i = 1; i != 10; i++)
            {
                for (int ii=1; ii!= Int32.Parse(CurrentCells[i])+1; ii++)
                {
                    Console.WriteLine(CurrentCells[i]);
                    if (Cells[i, ii] != null) Cells[i, ii].Visibility = Visibility.Visible;
                }
            }
        }

        public void AddCellsMouses()
        {
            Border[,] Cells = new Border[10, 5];
            Cells[1, 1] = CLS_SG11;
            Cells[1, 2] = CLS_SG12;
            Cells[1, 3] = CLS_SG13;
            Cells[1, 4] = CLS_SG14;
            Cells[2, 1] = CLS_SG21;
            Cells[2, 2] = CLS_SG22;
            Cells[2, 3] = CLS_SG23;
            Cells[2, 4] = CLS_SG24;
            Cells[3, 1] = CLS_SG31;
            Cells[3, 2] = CLS_SG32;
            Cells[3, 3] = CLS_SG33;
            Cells[3, 4] = CLS_SG34;
            Cells[4, 1] = CLS_SG41;
            Cells[4, 2] = CLS_SG42;
            Cells[4, 3] = CLS_SG43;
            Cells[4, 4] = CLS_SG44;
            Cells[5, 1] = CLS_SG51;
            Cells[5, 2] = CLS_SG52;
            Cells[5, 3] = CLS_SG53;
            Cells[5, 4] = CLS_SG54;
            Cells[6, 1] = CLS_SG61;
            Cells[6, 2] = CLS_SG62;
            Cells[6, 3] = CLS_SG63;
            Cells[6, 4] = CLS_SG64;
            Cells[7, 1] = CLS_SG71;
            Cells[7, 2] = CLS_SG72;
            Cells[7, 3] = CLS_SG73;
            Cells[7, 4] = CLS_SG74;
            Cells[8, 1] = CLS_SG81;
            Cells[8, 2] = CLS_SG82;
            Cells[8, 3] = CLS_SG83;
            Cells[8, 4] = CLS_SG84;
            Cells[9, 1] = CLS_SG91;
            Cells[9, 2] = CLS_SG92;
            Cells[9, 3] = CLS_SG93;
            Cells[9, 4] = CLS_SG94;

            for (int i = 1; i != 10; i++)
            {
                for (int ii = 1; ii != 5; ii++)
                {
                    Cells[i, ii].MouseUp += CLS_SG11_MouseUp;
                    Cells[i, ii].MouseEnter += CLS_SG11_MouseEnter;
                    Cells[i, ii].MouseLeave += CLS_SG11_MouseLeave;
                    Cells[i, ii].Tag = "Uncheck";
                }
            }
        }

        private void CLS_SG11_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = new SolidColorBrush(Color.FromArgb(64,81,166,49));
        }

        private void CLS_SG11_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((sender as Border).Tag.ToString() == "Uncheck") (sender as Border).Background = new SolidColorBrush(Color.FromArgb(64, 255, 255, 255));
            else (sender as Border).Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));
        }

        private void CLS_SG11_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Border).Tag.ToString() == "Uncheck")
            {
                ((sender as Border).Child as Image).Visibility = Visibility.Visible;
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));
                (sender as Border).Tag = "Check";
            }
            else
            {
                ((sender as Border).Child as Image).Visibility = Visibility.Collapsed;
                (sender as Border).Background = new SolidColorBrush(Color.FromArgb(64, 255, 255, 255));
                (sender as Border).Tag = "Uncheck";
            }
        }

        private void CLS_SpellStatLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CLS_SpellStatLbl.Visibility = Visibility.Collapsed;
            CLS_StatCB.Visibility = Visibility.Visible;
        }

        private void CLS_StatCB_DropDownClosed(object sender, EventArgs e)
        {
            CLS_SpellStatLbl.Visibility = Visibility.Visible;
            CLS_StatCB.Visibility = Visibility.Collapsed;
            CLS_SpellStatLbl.Content = (CLS_StatCB.SelectedItem as ComboBoxItem).Content;
            Label Stat = null;
            if (CLS_StatCB.SelectedIndex == 0) Stat = CL_Stg;
            if (CLS_StatCB.SelectedIndex == 1) Stat = CL_Dex;
            if (CLS_StatCB.SelectedIndex == 2) Stat = CL_Con;
            if (CLS_StatCB.SelectedIndex == 3) Stat = CL_Int;
            if (CLS_StatCB.SelectedIndex == 4) Stat = CL_Wid;
            if (CLS_StatCB.SelectedIndex == 5) Stat = CL_Cha;

            int SD = 8;
            SD = SD + Int32.Parse(Stat.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
            CLS_SpellDificultyLbl.Content = SD.ToString();

            int SAB = 0;
            SAB = Int32.Parse(CL_MB.Content.ToString()) + Int32.Parse(Stat.Content.ToString());
            string BM = "0";
            if (SAB > -1) BM = "+";
            if (SAB == -1) BM = "";
            if (SAB < -1) BM = "";
            CLS_SpellAttackLbl.Content = "d20" + BM + SAB.ToString();
        }

        private void CLS_SDPlus1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CLS_SD_Stat2.Visibility == Visibility.Collapsed)
                if (CLS_SD_Stat2_CB.Visibility == Visibility.Collapsed)
                {
                    CLS_SD_Stat2.Visibility = Visibility.Visible;
                    CLS_SDPlus2.Visibility = Visibility.Visible;
                }
        }

        private void CLS_SpellDificultyLbl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CLS_SpellDificultyDP.Visibility == Visibility.Collapsed)
            {
                CLS_SpellDificultyDP.Visibility = Visibility.Visible;
                CLS_SpellDificultyLbl.Visibility = Visibility.Collapsed;
            }
            else
            {
                CLS_SpellDificultyDP.Visibility = Visibility.Collapsed;
                CLS_SpellDificultyLbl.Visibility = Visibility.Visible;
            }
        }

        private void CLS_SD_Stat1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CLS_SD_Stat1.Visibility = Visibility.Collapsed;
            CLS_SD_Stat1_CB.Visibility = Visibility.Visible;
        }

        private void CLS_SD_Stat1_CB_DropDownClosed(object sender, EventArgs e)
        {
            CLS_SD_Stat1.Content = (CLS_SD_Stat1_CB.SelectedItem as ComboBoxItem).Content;
            CLS_SD_Stat1.Visibility = Visibility.Visible;
            CLS_SD_Stat1_CB.Visibility = Visibility.Collapsed;
        }

        private void CLS_SD_Stat2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CLS_SD_Stat2.Visibility = Visibility.Collapsed;
            CLS_SD_Stat2_CB.Visibility = Visibility.Visible;
        }

        private void CLS_SD_Stat2_CB_DropDownClosed(object sender, EventArgs e)
        {
            CLS_SD_Stat2.Content = (CLS_SD_Stat2_CB.SelectedItem as ComboBoxItem).Content;
            CLS_SD_Stat2.Visibility = Visibility.Visible;
            CLS_SD_Stat2_CB.Visibility = Visibility.Collapsed;
        }

        private void CLS_SDPlus2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CLS_SD_Stat2.Visibility = Visibility.Collapsed;
            CLS_SDPlus2.Visibility = Visibility.Collapsed;
            CLS_SD_Stat2_CB.Visibility = Visibility.Collapsed;
        }

        private void CLS_SD_Confurm_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CLS_SD_Stat2.Visibility == Visibility.Visible)
            {
                int SD = 8;
                Label StatLbl = null;
                Label StatLbl2 = null;
                if (CLS_SD_Stat1.Content.ToString() == "СИЛ") StatLbl = CL_Stg;
                if (CLS_SD_Stat1.Content.ToString() == "ЛОВ") StatLbl = CL_Dex;
                if (CLS_SD_Stat1.Content.ToString() == "ТЕЛ") StatLbl = CL_Con;
                if (CLS_SD_Stat1.Content.ToString() == "ИНТ") StatLbl = CL_Int;
                if (CLS_SD_Stat1.Content.ToString() == "МДР") StatLbl = CL_Wid;
                if (CLS_SD_Stat1.Content.ToString() == "ХАР") StatLbl = CL_Cha;
                if (CLS_SD_Stat2.Content.ToString() == "СИЛ") StatLbl2 = CL_Stg;
                if (CLS_SD_Stat2.Content.ToString() == "ЛОВ") StatLbl2 = CL_Dex;
                if (CLS_SD_Stat2.Content.ToString() == "ТЕЛ") StatLbl2 = CL_Con;
                if (CLS_SD_Stat2.Content.ToString() == "ИНТ") StatLbl2 = CL_Int;
                if (CLS_SD_Stat2.Content.ToString() == "МДР") StatLbl2 = CL_Wid;
                if (CLS_SD_Stat2.Content.ToString() == "ХАР") StatLbl2 = CL_Cha;

                SD = SD + Int32.Parse(StatLbl.Content.ToString()) + Int32.Parse(StatLbl2.Content.ToString())+ Int32.Parse(CL_MB.Content.ToString());
                CLS_SpellDificultyLbl.Content = SD.ToString();
            }
            else
            {
                int SD = 8;
                Label StatLbl = null;
                if (CLS_SD_Stat1.Content.ToString() == "СИЛ") StatLbl = CL_Stg;
                if (CLS_SD_Stat1.Content.ToString() == "ЛОВ") StatLbl = CL_Dex;
                if (CLS_SD_Stat1.Content.ToString() == "ТЕЛ") StatLbl = CL_Con;
                if (CLS_SD_Stat1.Content.ToString() == "ИНТ") StatLbl = CL_Int;
                if (CLS_SD_Stat1.Content.ToString() == "МДР") StatLbl = CL_Wid;
                if (CLS_SD_Stat1.Content.ToString() == "ХАР") StatLbl = CL_Cha;

                SD = SD + Int32.Parse(StatLbl.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
                CLS_SpellDificultyLbl.Content = SD.ToString();
            }

            CLS_SpellDificultyDP.Visibility = Visibility.Collapsed;
            CLS_SpellDificultyLbl.Visibility = Visibility.Visible;
        }

        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (CL_SaveChar.Visibility == Visibility.Collapsed)
            {
                bool FirstEnter = true;
                for (int ip = 0; ip != 33000; ip++)
                {
                    if (LoadedProfile[ip].IndexOf("<PlayerCharSpellDif>") != -1)
                    {
                        if (LoadedProfile[ip].Length == 20) FirstEnter = true;
                        else FirstEnter = false;
                    }
                }

                if (FirstEnter == true)
                {
                    int i = 0;
                    while (LoadedSpells[0, i] != null)
                    {
                        CreateSpellCL(LoadedSpells[0, i].Remove(0, 5), LoadedSpells[1, i], CLS_Spells);
                        i++;
                    }
                    int SAB = 0;
                    SAB = Int32.Parse(CL_MB.Content.ToString()) + Int32.Parse(CL_Stg.Content.ToString());
                    string BM = "0";
                    if (SAB > -1) BM = "+";
                    if (SAB == -1) BM = "";
                    if (SAB < -1) BM = "";
                    CLS_SpellAttackLbl.Content = "d20" + BM + SAB.ToString();
                    if (CLS_SD_Stat2.Visibility == Visibility.Visible)
                    {
                        int SD = 8;
                        Label StatLbl = null;
                        Label StatLbl2 = null;
                        if (CLS_SD_Stat1.Content.ToString() == "СИЛ") StatLbl = CL_Stg;
                        if (CLS_SD_Stat1.Content.ToString() == "ЛОВ") StatLbl = CL_Dex;
                        if (CLS_SD_Stat1.Content.ToString() == "ТЕЛ") StatLbl = CL_Con;
                        if (CLS_SD_Stat1.Content.ToString() == "ИНТ") StatLbl = CL_Int;
                        if (CLS_SD_Stat1.Content.ToString() == "МДР") StatLbl = CL_Wid;
                        if (CLS_SD_Stat1.Content.ToString() == "ХАР") StatLbl = CL_Cha;
                        if (CLS_SD_Stat2.Content.ToString() == "СИЛ") StatLbl2 = CL_Stg;
                        if (CLS_SD_Stat2.Content.ToString() == "ЛОВ") StatLbl2 = CL_Dex;
                        if (CLS_SD_Stat2.Content.ToString() == "ТЕЛ") StatLbl2 = CL_Con;
                        if (CLS_SD_Stat2.Content.ToString() == "ИНТ") StatLbl2 = CL_Int;
                        if (CLS_SD_Stat2.Content.ToString() == "МДР") StatLbl2 = CL_Wid;
                        if (CLS_SD_Stat2.Content.ToString() == "ХАР") StatLbl2 = CL_Cha;

                        SD = SD + Int32.Parse(StatLbl.Content.ToString()) + Int32.Parse(StatLbl2.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
                        CLS_SpellDificultyLbl.Content = SD.ToString();
                    }
                    else
                    {
                        int SD = 8;
                        Label StatLbl = null;
                        if (CLS_SD_Stat1.Content.ToString() == "СИЛ") StatLbl = CL_Stg;
                        if (CLS_SD_Stat1.Content.ToString() == "ЛОВ") StatLbl = CL_Dex;
                        if (CLS_SD_Stat1.Content.ToString() == "ТЕЛ") StatLbl = CL_Con;
                        if (CLS_SD_Stat1.Content.ToString() == "ИНТ") StatLbl = CL_Int;
                        if (CLS_SD_Stat1.Content.ToString() == "МДР") StatLbl = CL_Wid;
                        if (CLS_SD_Stat1.Content.ToString() == "ХАР") StatLbl = CL_Cha;

                        SD = SD + Int32.Parse(StatLbl.Content.ToString()) + Int32.Parse(CL_MB.Content.ToString());
                        CLS_SpellDificultyLbl.Content = SD.ToString();
                    }
                    CLS_SpellDificultyDP.Visibility = Visibility.Collapsed;
                    CLS_SpellDificultyLbl.Visibility = Visibility.Visible;
                    Pages.SelectedIndex = 10;
                    CLS_CharName.Content = CL_CharName.Content;
                }
                else
                {
                    int i = 0;
                    while (LoadedSpells[0, i] != null)
                    {
                        CreateSpellCL(LoadedSpells[0, i].Remove(0, 5), LoadedSpells[1, i], CLS_Spells);
                        i++;
                    }
                    for (int ip = 0; ip != 33000; ip++)
                    {
                        if (LoadedProfile[ip] == "<PlayerChar>"+CL_CharName.Content.ToString())
                        {
                            int o = ip;
                            while (LoadedProfile[o] != "<PlayerCharEnd>")
                            {
                                if (LoadedProfile[o].IndexOf("<PlayerCharSpellDif>") != -1)
                                {
                                    CLS_SpellDificultyLbl.Content = LoadedProfile[o].Remove(0, 20);
                                }
                                if (LoadedProfile[o].IndexOf("<PlayerCharSpellAtt>") != -1)
                                {
                                    CLS_SpellAttackLbl.Content = LoadedProfile[o].Remove(0, 20);
                                }
                                if (LoadedProfile[o].IndexOf("<PlayerCharSpellStat>") != -1)
                                {
                                    CLS_SpellStatLbl.Content = LoadedProfile[o].Remove(0, 21);
                                }
                                if (LoadedProfile[o].IndexOf("<PlayerCharCasterLevel>") != -1)
                                {
                                    String SpellCastLevel = "1";
                                    SpellCastLevel = LoadedProfile[o].Remove(0, 23);
                                    CLS_SpellLevel.SelectedIndex = Int32.Parse(SpellCastLevel);
                                }
                                if (LoadedProfile[o].IndexOf("<PlayerCharSpellsReady>") != -1)
                                {
                                    CLS_ReadySpellDP.Children.Clear();
                                    if (LoadedProfile[o].Length != 23)
                                    {
                                        int op = 0;
                                        string line = LoadedProfile[o].Remove(0,23);
                                        while (line.Length != 0)
                                        {
                                            for (int ess=0;ess!= line.Length; ess++)
                                            {
                                                if (line[ess] == ':')
                                                {
                                                    string SpellName = line.Remove(ess, line.Length - ess);
                                                    line = line.Remove(0, SpellName.Length+1);
                                                    for (int SI=0; SI != 5000; SI++)
                                                    {
                                                        if (LoadedSpells[0,SI] != null)
                                                            if (LoadedSpells[0, SI] == "<Rus>"+ SpellName) CreateSpellCL(SpellName, LoadedSpells[1, SI], CLS_ReadySpellDP);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                o++;
                            }
                        }
                    }
                    CLS_SpellDificultyDP.Visibility = Visibility.Collapsed;
                    CLS_SpellDificultyLbl.Visibility = Visibility.Visible;
                    Pages.SelectedIndex = 10;
                    CLS_CharName.Content = CL_CharName.Content;
                }
            }
        }

        private void CLS_CharName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CL_CharSpace.Tag = "CURRENT";
            SaveChar();
            Pages.SelectedIndex = 4;
        }

        private void CLS_SClass_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ((sender as ComboBox).Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
        }

        public void CreateSpellCL(String Content, String Tag,DockPanel Pparent)
        {
            Label TB = new Label();
            Pparent.Children.Add(TB);
            TB.Content = Content;
            DockPanel.SetDock(TB, Dock.Top);
            TB.FontSize = 14;
            TB.HorizontalAlignment = HorizontalAlignment.Stretch;
            TB.HorizontalContentAlignment = HorizontalAlignment.Center;
            TB.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            TB.Tag = Tag;
            if (NewSpellValue == true)
            {
                TB.Background = new SolidColorBrush(Color.FromArgb(25, 255, 255, 255));
                NewSpellValue = false;
            }
            else
            {
                TB.Background = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
                NewSpellValue = true;
            }
            TB.MouseDoubleClick += LoadSpellCL;
            if (Pparent != CLS_ReadySpellDP) TB.MouseDown += CLS_SpellMouseDown;
            else
            {
                ContextMenu CM = new ContextMenu();
                MenuItem MI = new MenuItem();
                CM.Items.Add(MI);
                MI.Header = "Удалить";
                MI.Click += DeleteSpellFromReady;
                TB.MouseDown += SpellReadyClick;
                TB.ContextMenu = CM;
            }
        }

        public void DeleteSpellFromReady(object sender, RoutedEventArgs e)
        {
            CLS_ReadySpellDP.Children.Remove(DeletedSpell);
        }

        public void SpellReadyClick(Object sender, MouseButtonEventArgs e)
        {
            DeletedSpell = (sender as Label);
        }

        public void LoadSpellCL(Object sender, MouseButtonEventArgs e)
        {
            SpellNewWindow SNW = new SpellNewWindow();
            String Way = @"Bin\Data\Spells\PHB.dnd";
            String SpellName = "";
            SpellName = (sender as Label).Content.ToString();

            int NewFileRow = 0;
            SNW.SL_SpellName.Content = SpellName;
            for (int i = 0; i != 5000; i++)
            {
                if (LoadedSpells[0, i] != null)
                {
                    if (LoadedSpells[0, 1] == (sender as Label).Content.ToString()) NewFileRow = Int32.Parse(LoadedSpells[1, i]);
                }
            }

            SNW.FileRow = NewFileRow;
            for (int i = NewFileRow; i != CountOfFileLines(Way); i++)
            {
                if (ReadCertainLine(Way, i) == "<Rus>" + SpellName)
                {
                    int WhileCounter = i;

                    while (ReadCertainLine(Way, WhileCounter) != "<End>")
                    {
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Level>") != -1)
                        {
                            SNW.SL_SpellLevel.Content = "Уровень: " + ReadCertainLine(Way, WhileCounter).Remove(0, 7);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<School>") != -1)
                        {
                            SNW.SL_SpellSchool.Content = "Школа: " + ReadCertainLine(Way, WhileCounter).Remove(0, 8);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Casttime>") != -1)
                        {
                            SNW.SL_Spellcasttime.Text = "Время накладывания: " + ReadCertainLine(Way, WhileCounter).Remove(0, 10);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Distance>") != -1)
                        {
                            SNW.SL_SpellDistance.Content = "Дистанция: " + ReadCertainLine(Way, WhileCounter).Remove(0, 10);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Components>") != -1)
                        {
                            SNW.SL_SpellComponents.Text = "Компоненты: " + ReadCertainLine(Way, WhileCounter).Remove(0, 12);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Time>") != -1)
                        {
                            SNW.SL_SpellTime.Content = "Длительность: " + ReadCertainLine(Way, WhileCounter).Remove(0, 6);
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Class>") != -1)
                        {
                            SNW.SL_SpellClass.Content = "Классы: " + ReadCertainLine(Way, WhileCounter).Remove(0, 7);
                        }

                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Line>") != -1)
                        {
                            WrapPanel DiscriptionDP = new WrapPanel();
                            SNW.SpellDiscription.Children.Add(DiscriptionDP);
                            DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                            DiscriptionDP.Width = Double.NaN;
                            DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                            DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                            DiscriptionDP.Orientation = Orientation.Horizontal;
                            DockPanel.SetDock(DiscriptionDP, Dock.Top);
                            DiscriptionDP.Height = Double.NaN;

                            String ThisLine = ReadCertainLine(Way, WhileCounter);
                            ThisLine = ThisLine.Remove(0, 6);

                            int SymbolID = 0;
                            while (ThisLine.Length != 0)
                            {
                                String CombineText = "";
                                for (int o = 0; o != ThisLine.Length; o++)
                                {
                                    if (ThisLine[o] == ' ')
                                    {
                                        SymbolID = 1;
                                        break;
                                    }
                                    if (ThisLine[o] == '<')
                                    {
                                        if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                        if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                        if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                        break;
                                    }
                                    if (ThisLine[o] == '$')
                                    {
                                        SymbolID = 5;
                                        break;
                                    }

                                    CombineText = CombineText + ThisLine[o];
                                }
                                if (SymbolID == 1)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                                if (SymbolID == 2)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 3);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    TextBlock TextBl = new TextBlock();
                                    DiscriptionDP.Children.Add(TextBl);
                                    TextBl.FontSize = 14;
                                    TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    TextBl.Text = CombineText;
                                    TextBl.Padding = new Thickness(3, 5, 3, 5);
                                    TextBl.TextDecorations = TextDecorations.Underline;
                                    CombineText = CombineText + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    Console.WriteLine(CombineText);
                                    Console.WriteLine(ThisLine);
                                }
                                if (SymbolID == 3)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    ThisLine = ThisLine.Remove(0, 3);
                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    String NewCombine = CombineText;
                                    while (CombineText.Length != 0)
                                    {
                                        string LinkedText = "";
                                        for (int o = 0; o != CombineText.Length; o++)
                                        {
                                            if (CombineText[o] == ' ') break;
                                            LinkedText = LinkedText + CombineText[o];
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = LinkedText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.FontWeight = FontWeights.Bold;
                                        if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                        else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                    }

                                    NewCombine = NewCombine + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                }
                                if (SymbolID == 4)
                                {
                                    Image texted = new Image();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.Height = 10;
                                    texted.Width = 10;
                                    texted.Margin = new Thickness(15, 0, 15, 0);
                                    ChangeImage("Seporator.png", texted);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                }
                                if (SymbolID == 5)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 1);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '$')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    //CombineText;
                                    for (int ss = WhileCounter; ss != CountOfFileLines(Way); ss++)
                                    {
                                        if (ReadCertainLine(Way, ss).IndexOf("#" + CombineText) != -1)
                                        {
                                            String[] ThisCells = new String[9];
                                            int CellsID = 0;
                                            String EditableLine = ReadCertainLine(Way, ss).Remove(0, CombineText.Length + 1);

                                            while (EditableLine.Length != 0)
                                            {
                                                for (int pos = 0; pos != EditableLine.Length; pos++)
                                                {
                                                    if (EditableLine[pos] == ':')
                                                    {
                                                        EditableLine = EditableLine.Remove(0, ThisCells[CellsID].Length + 1);
                                                        CellsID++;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        ThisCells[CellsID] = ThisCells[CellsID] + EditableLine[pos].ToString();
                                                    }
                                                }
                                            }
                                            TextBlock TextBl2 = new TextBlock();
                                            DiscriptionDP.Children.Add(TextBl2);
                                            TextBl2.FontSize = 14;
                                            TextBl2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                            TextBl2.Text = ThisCells[0];
                                            TextBl2.Padding = new Thickness(3, 5, 3, 5);
                                            TextBl2.FontWeight = FontWeights.Bold;
                                            break;
                                        }
                                    }
                                    CombineText = CombineText + "$";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                            }
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<Table>") != -1)
                        {
                            Grid newTable = new Grid();
                            NPCG_DiscriptionDP.Children.Add(newTable);
                            newTable.HorizontalAlignment = HorizontalAlignment.Stretch;
                            newTable.Width = Double.NaN;
                            newTable.VerticalAlignment = VerticalAlignment.Top;
                            newTable.Margin = new Thickness(25, 0, 25, 0);
                            newTable.Height = 100;
                            DockPanel.SetDock(newTable, Dock.Top);

                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); ti++)
                            {
                                RowDefinition RW = new RowDefinition();
                                newTable.RowDefinitions.Add(RW);
                            }
                            for (int ti = 0; ti != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); ti++)
                            {
                                ColumnDefinition CW = new ColumnDefinition();
                                newTable.ColumnDefinitions.Add(CW);
                            }

                            String ThiLin = ReadCertainLine(Way, WhileCounter);
                            ThiLin = ThiLin.Remove(0, 9);
                            for (int Ri = 0; Ri != Int32.Parse(ReadCertainLine(Way, WhileCounter)[7].ToString()); Ri++)
                            {
                                for (int Ci = 0; Ci != Int32.Parse(ReadCertainLine(Way, WhileCounter)[8].ToString()); Ci++)
                                {
                                    Border Brdr = new Border();
                                    newTable.Children.Add(Brdr);
                                    Brdr.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Brdr.VerticalAlignment = VerticalAlignment.Stretch;
                                    Brdr.Height = Double.NaN;
                                    Brdr.Width = Double.NaN;
                                    Brdr.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Brdr.BorderThickness = new Thickness(1, 1, 1, 1);
                                    Grid.SetColumnSpan(Brdr, 1);
                                    Grid.SetRowSpan(Brdr, 1);
                                    Grid.SetRow(Brdr, Ri);
                                    Grid.SetColumn(Brdr, Ci);
                                    Label Lbl = new Label();
                                    newTable.Children.Add(Lbl);
                                    Lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    Lbl.VerticalAlignment = VerticalAlignment.Stretch;
                                    Lbl.Height = Double.NaN;
                                    Lbl.Width = Double.NaN;
                                    Lbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    Grid.SetColumnSpan(Lbl, 1);
                                    Grid.SetRowSpan(Lbl, 1);
                                    Grid.SetRow(Lbl, Ri);
                                    Grid.SetColumn(Lbl, Ci);
                                    Lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                                    Lbl.VerticalContentAlignment = VerticalAlignment.Center;
                                    int ss = 0;
                                    String AddingLine = "";
                                    if (ThiLin.Length != 0)
                                    {
                                        while (ThiLin[ss] != ';')
                                        {
                                            AddingLine = AddingLine + ThiLin[ss].ToString();
                                            ss++;
                                        }
                                    }
                                    Lbl.Content = AddingLine;
                                    ThiLin = ThiLin.Remove(0, ThiLin.IndexOf(';') + 1);
                                    Console.WriteLine(AddingLine);
                                    Console.WriteLine(ThiLin);
                                }
                            }
                        }
                        if (ReadCertainLine(Way, WhileCounter).IndexOf("<CellsLevel>") != -1)
                        {
                            WrapPanel DiscriptionDP = new WrapPanel();
                            SNW.SpellDiscription.Children.Add(DiscriptionDP);
                            DiscriptionDP.HorizontalAlignment = HorizontalAlignment.Stretch;
                            DiscriptionDP.Width = Double.NaN;
                            DiscriptionDP.VerticalAlignment = VerticalAlignment.Top;
                            DiscriptionDP.Margin = new Thickness(25, 0, 25, 0);
                            DiscriptionDP.Orientation = Orientation.Horizontal;
                            DockPanel.SetDock(DiscriptionDP, Dock.Top);
                            DiscriptionDP.Height = Double.NaN;

                            String ThisLine = ReadCertainLine(Way, WhileCounter);
                            ThisLine = ThisLine.Remove(0, 12);

                            int SymbolID = 0;
                            while (ThisLine.Length != 0)
                            {
                                String CombineText = "";
                                for (int o = 0; o != ThisLine.Length; o++)
                                {
                                    if (ThisLine[o] == ' ')
                                    {
                                        SymbolID = 1;
                                        break;
                                    }
                                    if (ThisLine[o] == '<')
                                    {
                                        if (ThisLine[o + 1] == 'l') SymbolID = 2;
                                        if (ThisLine[o + 1] == 'b') SymbolID = 3;
                                        if (ThisLine[o + 1] == 'd') SymbolID = 4;
                                        break;
                                    }
                                    CombineText = CombineText + ThisLine[o];
                                }
                                if (SymbolID == 1)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    if ((ThisLine.IndexOf(CombineText) + CombineText.Length) == ThisLine.Length) ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    else ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                }
                                if (SymbolID == 2)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    Console.WriteLine(CombineText);

                                    ThisLine = ThisLine.Remove(0, 3);

                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    TextBlock TextBl = new TextBlock();
                                    DiscriptionDP.Children.Add(TextBl);
                                    TextBl.FontSize = 14;
                                    TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    TextBl.Text = CombineText;
                                    TextBl.Padding = new Thickness(3, 5, 3, 5);
                                    TextBl.TextDecorations = TextDecorations.Underline;
                                    CombineText = CombineText + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length + 1);
                                    Console.WriteLine(CombineText);
                                    Console.WriteLine(ThisLine);
                                }
                                if (SymbolID == 3)
                                {
                                    Label texted = new Label();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.FontSize = 14;
                                    texted.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                    texted.Content = CombineText;
                                    texted.Padding = new Thickness(3, 5, 3, 5);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), CombineText.Length);
                                    ThisLine = ThisLine.Remove(0, 3);
                                    CombineText = "";
                                    int WC = 0;
                                    while (ThisLine[WC] != '>')
                                    {
                                        CombineText = CombineText + ThisLine[WC];
                                        WC++;
                                    }
                                    String NewCombine = CombineText;
                                    while (CombineText.Length != 0)
                                    {
                                        string LinkedText = "";
                                        for (int o = 0; o != CombineText.Length; o++)
                                        {
                                            if (CombineText[o] == ' ') break;
                                            LinkedText = LinkedText + CombineText[o];
                                        }
                                        TextBlock TextBl = new TextBlock();
                                        DiscriptionDP.Children.Add(TextBl);
                                        TextBl.FontSize = 14;
                                        TextBl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                                        TextBl.Text = LinkedText;
                                        TextBl.Padding = new Thickness(3, 5, 3, 5);
                                        TextBl.FontWeight = FontWeights.Bold;
                                        if ((CombineText.IndexOf(LinkedText) + (LinkedText.Length + 1)) > CombineText.Length) CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length);
                                        else CombineText = CombineText.Remove(CombineText.IndexOf(LinkedText), LinkedText.Length + 1);
                                    }

                                    NewCombine = NewCombine + ">";
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(NewCombine), NewCombine.Length + 1);
                                }
                                if (SymbolID == 4)
                                {
                                    Image texted = new Image();
                                    DiscriptionDP.Children.Add(texted);
                                    texted.Height = 10;
                                    texted.Width = 10;
                                    texted.Margin = new Thickness(15, 0, 15, 0);
                                    ChangeImage("Seporator.png", texted);
                                    ThisLine = ThisLine.Remove(ThisLine.IndexOf(CombineText), 3);
                                }
                            }
                        }

                        WhileCounter++;
                    }
                }
            }
            SNW.SL_SpellSource.Content = "Источник: Player's Handsbook";
            SNW.Title = (sender as Label).Content.ToString();
            SNW.Show();
        }

        public void CLS_SpellMouseDown(Object sender, MouseButtonEventArgs e)
        {
            DroppedLabel = sender as Label;
            DragDrop.DoDragDrop(DroppedLabel, DroppedLabel.Content, DragDropEffects.Copy);
        }

        private void CLS_ReadySpellDP_Drop(object sender, DragEventArgs e)
        {
            if (CLS_ReadySpellDP.Children.Count < Int32.Parse(CLS_ReadySpellTB.Text))
            {
                CreateSpellCL(DroppedLabel.Content.ToString(), DroppedLabel.Tag.ToString(), CLS_ReadySpellDP);
            }
        }

        private void CLS_SpellLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CLS_SpellLevel.SelectedIndex != 0)
            {
                (CLS_SpellLevel.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
                Border[,] Cells = new Border[10, 5];
                Cells[1, 1] = CLS_SG11;
                Cells[1, 2] = CLS_SG12;
                Cells[1, 3] = CLS_SG13;
                Cells[1, 4] = CLS_SG14;
                Cells[2, 1] = CLS_SG21;
                Cells[2, 2] = CLS_SG22;
                Cells[2, 3] = CLS_SG23;
                Cells[2, 4] = CLS_SG24;
                Cells[3, 1] = CLS_SG31;
                Cells[3, 2] = CLS_SG32;
                Cells[3, 3] = CLS_SG33;
                Cells[3, 4] = CLS_SG34;
                Cells[4, 1] = CLS_SG41;
                Cells[4, 2] = CLS_SG42;
                Cells[4, 3] = CLS_SG43;
                Cells[4, 4] = CLS_SG44;
                Cells[5, 1] = CLS_SG51;
                Cells[5, 2] = CLS_SG52;
                Cells[5, 3] = CLS_SG53;
                Cells[5, 4] = CLS_SG54;
                Cells[6, 1] = CLS_SG61;
                Cells[6, 2] = CLS_SG62;
                Cells[6, 3] = CLS_SG63;
                Cells[6, 4] = CLS_SG64;
                Cells[7, 1] = CLS_SG71;
                Cells[7, 2] = CLS_SG72;
                Cells[7, 3] = CLS_SG73;
                Cells[7, 4] = CLS_SG74;
                Cells[8, 1] = CLS_SG81;
                Cells[8, 2] = CLS_SG82;
                Cells[8, 3] = CLS_SG83;
                Cells[8, 4] = CLS_SG84;
                Cells[9, 1] = CLS_SG91;
                Cells[9, 2] = CLS_SG92;
                Cells[9, 3] = CLS_SG93;
                Cells[9, 4] = CLS_SG94;
                String[] CurrentCells = new string[10];
                for (int R = 1; R != 10; R++)
                {
                    for (int C = 1; C != 5; C++)
                    {
                        Cells[R, C].Visibility = Visibility.Hidden;
                    }
                }
                CurrentCells[1] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[0].ToString();
                CurrentCells[2] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[1].ToString();
                CurrentCells[3] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[2].ToString();
                CurrentCells[4] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[3].ToString();
                CurrentCells[5] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[4].ToString();
                CurrentCells[6] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[5].ToString();
                CurrentCells[7] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[6].ToString();
                CurrentCells[8] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[7].ToString();
                CurrentCells[9] = ReadCertainLine(@"Bin\Data\Spells\SpellCells.dnd", CLS_SpellLevel.SelectedIndex - 1)[8].ToString();
                for (int i = 1; i != 10; i++)
                {
                    for (int ii = 1; ii != Int32.Parse(CurrentCells[i]) + 1; ii++)
                    {
                        Console.WriteLine(CurrentCells[i]);
                        if (Cells[i, ii] != null) Cells[i, ii].Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void CP_RollIniciativeLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            for (int i=1;i!= CP_CombatListDock.Children.Count; i++)
            {
                if (((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Tag.ToString() == "Char")
                {
                    Random Rnd = new Random();
                    for (int i2 = 0; i2 != 33000; i2++)
                    {
                        if (LoadedProfile[i2] != nullstring)
                        {
                            if (LoadedProfile[i2].IndexOf("<PlayerChar>" + ((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Content.ToString()) != -1)
                            {
                                int o = i2;
                                while (LoadedProfile[o] != "<PlayerCharEnd>")
                                {
                                    if (LoadedProfile[o].Contains("<CharStat>") == true)
                                    {
                                        string line = LoadedProfile[o].Remove(0, 10);
                                        int StatID = 1;
                                        while (line.Length != 0)
                                        {
                                            int p = 0;
                                            string stat = "";
                                            if (StatID != 6)
                                            {
                                                while (line[p] != ':')
                                                {
                                                    stat = stat + line[p];
                                                    p++;
                                                }
                                                if (StatID == 1)
                                                {
                                                }
                                                if (StatID == 2)
                                                {
                                                    string LovModCP = ReturnedModificator(Int32.Parse(stat));
                                                    ((CP_CombatListDock.Children[i] as Grid).Children[2] as TextBox).Text = (Rnd.Next(1, 20) + Int32.Parse(LovModCP)).ToString();
                                                }
                                                line = line.Remove(0, line.IndexOf(stat) + stat.Length + 1);
                                                StatID++;
                                            }
                                            else if (StatID == 6)
                                            {
                                                line = "";
                                            }
                                        }
                                    }
                                    o++;
                                }
                            }
                        }
                    }
                }
                if (((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Tag.ToString() == "Monster")
                {
                    Random Rnd = new Random();
                    int CreaID = 0;
                    for (int i2 = 0; i2 != 5000; i2++)
                    {
                        if (LoadedCreatures[0, i2] != null)
                        {
                            if (LoadedCreatures[0, i2] == "<Name>" + ((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Content)
                            {
                                CreaID = i;
                            }
                        }
                    }

                    String FilePath = LoadedCreatures[1, CreaID];

                    for (int i2 = 0; i2 != CountOfFileLines(@"Bin\Data\Monsters\Addons.dnd"); i2++)
                    {
                        if (ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i2) == FilePath.Remove(2))
                        {
                            FilePath = @"Bin\Data\Monsters\" + ReadCertainLine(@"Bin\Data\Monsters\Addons.dnd", i2 - 1) + ".dnd";
                        }
                    }
                    String[] Lines = new String[1000];
                    int MainCounter = 0;
                    int DiscriptionStart = 0;

                    for (int i2 = Int32.Parse(LoadedCreatures[1, CreaID].Remove(0, 3)); i2 != CountOfFileLines(FilePath); i2++)
                    {
                        if (ReadCertainLine(FilePath, i2) == "<Discription>") DiscriptionStart = i2 + 1;
                        if (ReadCertainLine(FilePath, i2) == "</X>") break;
                        else
                        {
                            Lines[MainCounter] = ReadCertainLine(FilePath, i2);
                            MainCounter++;
                        }
                    }
                    for (int i2 = 1; i2 != MainCounter; i2++)
                    {
                        if (Lines[i2].Contains("<Lov>") == true)
                        {
                            ((CP_CombatListDock.Children[i] as Grid).Children[2] as TextBox).Text = (Rnd.Next(1, 20) + Int32.Parse(ReturnedModificator(Int32.Parse(Lines[i2].Remove(0, 5))))).ToString();
                        }
                    }
                }
                ((CP_CombatListDock.Children[i] as Grid).Children[1] as Label).Content = ((CP_CombatListDock.Children[i] as Grid).Children[2] as TextBox).Text;
            }
        }

        public void SortCombat()
        {
            int[] iniciatives = new int[400];
            String[,] CharIt = new String[2, 400];
            int InicI = 0;
            for (int i = 0; i != 400; i++)
            {
                iniciatives[i] = 1488;
            }
            for (int i = 1; i != CP_CombatListDock.Children.Count; i++)
            {
                iniciatives[InicI] = Int32.Parse(((CP_CombatListDock.Children[i] as Grid).Children[2] as TextBox).Text);
                CharIt[0, InicI] = ((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Content.ToString();
                CharIt[1, InicI] = ((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Tag.ToString();
                InicI++;
            }
            int InicLenght = 0;
            for (int i = 0; i != 400; i++)
            {
                if (iniciatives[i] != 1488)
                {
                    InicLenght++;
                }
            }
            int[] Trueiniciatives = new int[400];
            String[,] TrueCharIt = new String[2, 400];
            int MaxPos = 0;
            int MaxValue = 0;

            for (int i = 0; i != InicLenght; i++)
            {
                MaxPos = 0;
                MaxValue = 0;

                for (int i2 = 0; i2 != InicLenght; i2++)
                {
                    if (iniciatives[i2] > MaxValue)
                    {
                        MaxValue = iniciatives[i2];
                        MaxPos = i2;
                        Console.WriteLine(i + ":" + MaxValue);
                    }
                }
                TrueCharIt[0, i] = CharIt[0, MaxPos];
                TrueCharIt[1, i] = CharIt[1, MaxPos];
                Trueiniciatives[i] = MaxValue;
                iniciatives[MaxPos] = 0;
            }
            for (int i = 0; i != 200; i++)
            {
                CreInCombat[0, i] = null;
                CreInCombat[1, i] = null;
                CreInCombatLbl[i] = null;
            }
            CP_CombatListDock.Children.Clear();
            Label HeaderL = new Label();
            CP_CombatListDock.Children.Add(HeaderL);
            HeaderL.FlowDirection = FlowDirection.LeftToRight;
            HeaderL.HorizontalContentAlignment = HorizontalAlignment.Center;
            HeaderL.Background = new SolidColorBrush(Color.FromRgb(249, 207, 169));
            DockPanel.SetDock(HeaderL, Dock.Top);
            HeaderL.HorizontalAlignment = HorizontalAlignment.Stretch;
            HeaderL.VerticalAlignment = VerticalAlignment.Stretch;
            HeaderL.Height = Double.NaN;
            HeaderL.Width = Double.NaN;
            HeaderL.FontFamily = new FontFamily("Book Antiqua");
            HeaderL.FontSize = 14;
            HeaderL.Content = "Участвующие в бою:";
            CP_MainGrid.ColumnDefinitions[0].MinWidth = 0;
            CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
            CP_MainGrid.ColumnDefinitions[0].MaxWidth = 0;
            CP_GS1.Visibility = Visibility.Collapsed;
            CP_GS1.Tag = "CharsClose";
            for (int i = 0; i != InicLenght; i++)
            {
                CreateCharFields(TrueCharIt[0, i], TrueCharIt[1, i], Trueiniciatives[i]);
            }
        }

        private void CP_StartFightLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CP_TempListDock.IsEnabled = false;
            SortCombat();
            ((CP_CombatListDock.Children[1] as Grid).Children[4] as Border).Visibility = Visibility.Visible;
            OpenCharInfo((CP_CombatListDock.Children[1] as Grid).Children[3] as Label, e);
            CP_TurnBrdr.Visibility = Visibility.Visible;
        }

        public void CreateCharFields(string Caption,string Tag2,int Iniciative)
        {
            //Новый интерфейс панели существа
            Grid CharMainGrid = new Grid();
            CP_CombatListDock.Children.Add(CharMainGrid);
            CharMainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            CharMainGrid.Width = Double.NaN;
            CharMainGrid.FlowDirection = FlowDirection.LeftToRight;
            CharMainGrid.Height = Double.NaN;
            DockPanel.SetDock(CharMainGrid, Dock.Top);
            CharMainGrid.Margin = new Thickness(0, 5, 0, 0);
            CharMainGrid.Background = new SolidColorBrush(Color.FromArgb(55, 0, 0, 0));
            RowDefinition RW1 = new RowDefinition();
            RowDefinition RW2 = new RowDefinition();
            CharMainGrid.RowDefinitions.Add(RW1);
            CharMainGrid.RowDefinitions.Add(RW2);
            RW1.Height = new GridLength(5, GridUnitType.Pixel);
            RW2.Height = new GridLength(1, GridUnitType.Star);
            //Child 0
            Label CharHealth = new Label();
            CharMainGrid.Children.Add(CharHealth);
            CharHealth.Content = "";
            CharHealth.HorizontalAlignment = HorizontalAlignment.Stretch;
            CharHealth.Width = Double.NaN;
            CharHealth.Height = 5;
            CharHealth.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            Grid.SetRow(CharHealth, 0);
            Grid.SetRowSpan(CharHealth, 1);
            //Child 1
            Label AddedLbl = new Label();
            CharMainGrid.Children.Add(AddedLbl);
            AddedLbl.Background = new SolidColorBrush(Color.FromArgb(75, 0, 0, 0));
            AddedLbl.Content = Iniciative.ToString();
            AddedLbl.Height = 26;
            AddedLbl.Width = 26;
            AddedLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            AddedLbl.HorizontalAlignment = HorizontalAlignment.Left;
            AddedLbl.VerticalAlignment = VerticalAlignment.Top;
            AddedLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            AddedLbl.MouseDoubleClick += CharInicDC;
            Grid.SetRow(AddedLbl, 1);
            Grid.SetRowSpan(AddedLbl, 1);
            //Child 2
            TextBox TB = new TextBox();
            CharMainGrid.Children.Add(TB);
            TB.Background = new SolidColorBrush(Color.FromArgb(15, 0, 0, 0));
            TB.Text = Iniciative.ToString();
            TB.Height = 26;
            TB.HorizontalContentAlignment = HorizontalAlignment.Center;
            TB.HorizontalAlignment = HorizontalAlignment.Left;
            TB.Width = 26;
            TB.VerticalAlignment = VerticalAlignment.Top;
            TB.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            TB.Visibility = Visibility.Collapsed;
            TB.PreviewTextInput += CL_StgEditor_PreviewTextInput;
            TB.KeyUp += CharInicDCAccept;
            TB.LostFocus += TB_InicLostFocus;
            Grid.SetRow(TB, 1);
            Grid.SetRowSpan(TB, 1);
            //Child 3
            Label AddedLbl2 = new Label();
            CharMainGrid.Children.Add(AddedLbl2);
            AddedLbl2.Background = new SolidColorBrush(Color.FromArgb(15, 0, 0, 0));
            AddedLbl2.Content = Caption;
            AddedLbl2.Height = 26;
            AddedLbl2.Margin = new Thickness(26, 0, 0, 0);
            AddedLbl2.HorizontalContentAlignment = HorizontalAlignment.Center;
            AddedLbl2.HorizontalAlignment = HorizontalAlignment.Stretch;
            AddedLbl2.Width = Double.NaN;
            AddedLbl2.VerticalAlignment = VerticalAlignment.Top;
            AddedLbl2.Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
            AddedLbl2.MouseDoubleClick += CombatCharMouseUp;
            AddedLbl2.MouseUp += OpenCharInfo;
            Grid.SetRow(AddedLbl2, 1);
            Grid.SetRowSpan(AddedLbl2, 1);
            ContextMenu CM = new ContextMenu();
            MenuItem MI = new MenuItem();
            MI.Header = "Удалить";
            MI.Click += RemoveCharFromCombat;
            CM.Items.Add(MI);
            AddedLbl2.ContextMenu = CM;
            AddedLbl2.Tag = Tag2;
            if (Tag2 == "Char")
            {
                for (int i = 0; i != 200; i++)
                {
                    if (CreInCombat[0, i] == null)
                    {
                        CreInCombat[0, i] = Caption;
                        for (int i2 = 0; i2 != 33000; i2++)
                        {
                            if (LoadedProfile[i2] != nullstring)
                            {
                                if (LoadedProfile[i2] == "<PlayerChar>" + Caption)
                                {
                                    int FinHP = i2;
                                    while (LoadedProfile[FinHP].IndexOf("<CharAC>") == -1)
                                    {
                                        if (LoadedProfile[FinHP].IndexOf("<CharHPDiceCount>") != -1)
                                        {
                                            CreInCombat[2, i] = LoadedProfile[FinHP].Remove(0, 17);
                                        }
                                        if (LoadedProfile[FinHP].IndexOf("<CharHPDice>") != -1)
                                        {
                                            CreInCombat[2, i] = CreInCombat[2, i] + LoadedProfile[FinHP].Remove(0, 12);
                                        }
                                        if (LoadedProfile[FinHP].IndexOf("<CharHP>") != -1)
                                        {
                                            CreInCombat[1, i] = LoadedProfile[FinHP].Remove(0, 8);
                                            CharHealth.Tag = LoadedProfile[FinHP].Remove(0, 8);
                                        }
                                        FinHP++;
                                    }
                                }
                            }
                        }
                        CreInCombatLbl[i] = AddedLbl2;
                        break;
                    }
                }
            }
            if (Tag2 == "Monster")
            {
                for (int i = 0; i != 200; i++)
                {
                    if (CreInCombat[0, i] == null)
                    {
                        CreInCombat[0, i] = Caption;
                        LoadCreHP(i);
                        CharHealth.Tag = CreInCombat[1, i];
                        CreInCombatLbl[i] = AddedLbl2;
                        break;
                    }
                }
            }
            Console.WriteLine(Tag2);
            //Child 4
            Border CharBorder = new Border();
            CharMainGrid.Children.Add(CharBorder);
            CharBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
            CharBorder.VerticalAlignment = VerticalAlignment.Stretch;
            CharBorder.Width = Double.NaN;
            CharBorder.Height = Double.NaN;
            CharBorder.BorderThickness = new Thickness(1, 2, 1, 2);
            CharBorder.Visibility = Visibility.Collapsed;
            Grid.SetRow(CharBorder, 0);
            Grid.SetRowSpan(CharBorder, 2);
            CharBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            
        }

        private void CP_HP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                for (int i = 0; i != 200; i++)
                {
                    if (CreInCombatLbl[i] != null)
                        if (CreInCombatLbl[i] == SelectedCreature)
                        {
                            CreInCombat[1, i] = CP_HP.Text;
                        }
                }
                for (int i = 1; i != CP_CombatListDock.Children.Count; i++)
                {
                    if (((CP_CombatListDock.Children[i] as Grid).Children[3] as Label) == SelectedCreature)
                    {
                        Double FullHP = Int32.Parse(((CP_CombatListDock.Children[i] as Grid).Children[0] as Label).Tag.ToString());
                        int CurentHP = Int32.Parse((sender as TextBox).Text);
                        Double Procent = FullHP / 100;
                        Procent = CurentHP / Procent;
                        Double FullWidth = (CP_CombatListDock.Children[i] as Grid).ActualWidth;
                        double NewWidth = (FullWidth / 100)* Procent;
                        Console.WriteLine(NewWidth);
                        Console.WriteLine(FullWidth);
                        ((CP_CombatListDock.Children[i] as Grid).Children[0] as Label).HorizontalAlignment = HorizontalAlignment.Left;
                        ((CP_CombatListDock.Children[i] as Grid).Children[0] as Label).Width = NewWidth;
                        if (CurentHP <= 0) ((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Foreground = new SolidColorBrush(Color.FromRgb(89, 49, 49));
                        if (CurentHP > 0) ((CP_CombatListDock.Children[i] as Grid).Children[3] as Label).Foreground = new SolidColorBrush(Color.FromRgb(255, 212, 173));
                    }
                }
                TraversalRequest TRQ = new TraversalRequest(FocusNavigationDirection.Next);
                CP_FocusTB.Visibility = Visibility.Visible;
                CP_HP.MoveFocus(TRQ);
                CP_FocusTB.Visibility = Visibility.Collapsed;
            }
        }

        private void CP_HP_LostFocus(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i != 200; i++)
            {
                if (CreInCombatLbl[i] != null)
                    if (CreInCombatLbl[i] == SelectedCreature) CreInCombat[1, i] = CP_HP.Text;
            }
            TraversalRequest TRQ = new TraversalRequest(FocusNavigationDirection.Next);
            CP_FocusTB.Visibility = Visibility.Visible;
            CP_HP.MoveFocus(TRQ);
            CP_FocusTB.Visibility = Visibility.Collapsed;
        }

        private void SM_StoryEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            TPP = SM_StoryEditor.CaretPosition;
        }

        private void CL_Health_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CL_HealthDice.Visibility = Visibility.Visible;
            (sender as Label).Visibility = Visibility.Collapsed;
            CP_StaticHealth.Text = (sender as Label).Content.ToString();
        }

        private void Label_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            CL_HealthDice.Visibility = Visibility.Collapsed;
            CL_Health.Visibility = Visibility.Visible;
            int FullHP = 0;
            int ConMod = 0;
            int Dice = 0;
            ConMod = Int32.Parse(CL_ConValue.Content.ToString());
            ConMod = Int32.Parse(ReturnedModificator(ConMod));
            if (CP_HitDiceCount.Text == "") CP_HitDiceCount.Text ="1";
            if (CP_HitDice.SelectedIndex == 0) Dice = 3;
            if (CP_HitDice.SelectedIndex == 1) Dice = 5;
            if (CP_HitDice.SelectedIndex == 2) Dice = 6;
            if (CP_HitDice.SelectedIndex == 3) Dice = 8;
            if (CP_HitDice.SelectedIndex == 4) Dice = 12;
            FullHP = Int32.Parse(CP_HitDiceCount.Text) * Dice + (Int32.Parse(CP_HitDiceCount.Text) * ConMod);
            if (CP_StaticHealth.Text != "") CL_Health.Content = CP_StaticHealth.Text;
            else CL_Health.Content = FullHP.ToString();
        }

        private void CP_GenerateHelathLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int CreID = 0;
            String NewHp = "0";
            for (int i = 0; i != 200; i++)
            {
                if (CreInCombatLbl[i] == SelectedCreature)
                {
                    CreID = i;
                    String LineT = CreInCombat[2, i];
                    if (LineT.Contains("+") == true)
                    {
                        int HitDiceCount = 0;
                        int HitDice = 0;
                        int HitPlus = 0;
                        String TempLine = "";
                        int Counter = 0;
                        while (LineT[Counter] != 'd')
                        {
                            TempLine = TempLine + LineT[Counter].ToString();
                            Counter++;
                        }
                        LineT = LineT.Remove(0, Counter+1);
                        HitDiceCount = Int32.Parse(TempLine);

                        TempLine = "";
                        Counter = 0;
                        while (LineT[Counter] != '+')
                        {
                            TempLine = TempLine + LineT[Counter].ToString();
                            Counter++;
                        }
                        LineT = LineT.Remove(0, Counter+1);
                        HitDice = Int32.Parse(TempLine);

                        HitPlus = Int32.Parse(LineT);
                        CP_HP.Text = GenerateHealth(HitDiceCount, HitDice, HitPlus).ToString();
                        NewHp = GenerateHealth(HitDiceCount, HitDice, HitPlus).ToString();
                    }
                    else
                    if (LineT.Contains("-") == true)
                    {
                        int HitDiceCount = 0;
                        int HitDice = 0;
                        int HitMinus = 0;
                        String TempLine = "";
                        int Counter = 0;
                        while (LineT[Counter] != 'd')
                        {
                            TempLine = TempLine + LineT[Counter].ToString();
                            Counter++;
                        }
                        LineT = LineT.Remove(0, Counter + 1);
                        HitDiceCount = Int32.Parse(TempLine);

                        TempLine = "";
                        Counter = 0;
                        while (LineT[Counter] != '-')
                        {
                            TempLine = TempLine + LineT[Counter].ToString();
                            Counter++;
                        }
                        LineT = LineT.Remove(0, Counter + 1);
                        HitDice = Int32.Parse(TempLine);

                        HitMinus = Int32.Parse(LineT);
                        CP_HP.Text = GenerateHealth(HitDiceCount, HitDice, HitMinus).ToString();
                        NewHp = GenerateHealth(HitDiceCount, HitDice, HitMinus).ToString();
                    }
                    else
                    if (LineT.Contains("+") == false)
                        if (LineT.Contains("-") == false)
                        {
                            int HitDiceCount = 0;
                            int HitDice = 0;

                            String TempLine = "";
                            int Counter = 0;
                            while (LineT[Counter] != 'd')
                            {
                                TempLine = TempLine + LineT[Counter].ToString();
                                Counter++;
                            }
                            LineT = LineT.Remove(0, Counter + 1);
                            HitDiceCount = Int32.Parse(TempLine);
                            HitDice = Int32.Parse(LineT);
                            CP_HP.Text = GenerateHealth(HitDiceCount, HitDice, 0).ToString();
                            NewHp = GenerateHealth(HitDiceCount, HitDice, 0).ToString();
                        }
                }
            }
            CreInCombat[1, CreID] = NewHp;
        }

        public int GenerateHealth(int HDC,int HD, int SH)
        {
            Random rnd = new Random();
            int FullHP = 0;
            for (int i=0; i!= HDC; i++)
            {
                FullHP = FullHP + rnd.Next(1, HD);
            }
            FullHP = FullHP + SH;
            return (FullHP);
        }

        private void CP_StopFightLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CP_StackInfo.Visibility = Visibility.Collapsed;
            CP_TempListDock.IsEnabled = true;
            CP_CombatListDock.Children.Clear();
            Label HeaderL = new Label();
            CP_CombatListDock.Children.Add(HeaderL);
            HeaderL.FlowDirection = FlowDirection.LeftToRight;
            HeaderL.HorizontalContentAlignment = HorizontalAlignment.Center;
            HeaderL.Background = new SolidColorBrush(Color.FromRgb(249, 207, 169));
            DockPanel.SetDock(HeaderL, Dock.Top);
            HeaderL.HorizontalAlignment = HorizontalAlignment.Stretch;
            HeaderL.VerticalAlignment = VerticalAlignment.Stretch;
            HeaderL.Height = Double.NaN;
            HeaderL.Width = Double.NaN;
            HeaderL.FontFamily = new FontFamily("Book Antiqua");
            HeaderL.FontSize = 14;
            HeaderL.Content = "Участвующие в бою:";
            CP_MainGrid.ColumnDefinitions[0].MinWidth = 0;
            CP_MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
            CP_MainGrid.ColumnDefinitions[0].MaxWidth = 0;
            CP_GS1.Visibility = Visibility.Collapsed;
            CP_GS1.Tag = "CharsClose";
            CP_TurnBrdr.Visibility = Visibility.Collapsed;
        }

        private void ToMapClick_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAllTopButtons();
            ToMap.Visibility = Visibility.Visible;
            ToDMNotes.Visibility = Visibility.Visible;
            ToCombatPage.Visibility = Visibility.Visible;
            Pages.SelectedIndex = 9;
        }

        private void CP_TurnLbl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int NowCre = 0;
            for (int i=1; i!= CP_CombatListDock.Children.Count; i++)
            {
                if (((CP_CombatListDock.Children[i] as Grid).Children[4] as Border).Visibility == Visibility.Visible) 
                    if (NowCre == 0) NowCre = i;
                ((CP_CombatListDock.Children[i] as Grid).Children[4] as Border).Visibility = Visibility.Collapsed;
            }
            if (NowCre+1 != CP_CombatListDock.Children.Count)
            {
                ((CP_CombatListDock.Children[NowCre + 1] as Grid).Children[4] as Border).Visibility = Visibility.Visible;
                OpenCharInfo((CP_CombatListDock.Children[NowCre + 1] as Grid).Children[3] as Label, e);
            }
            else
            {
                ((CP_CombatListDock.Children[1] as Grid).Children[4] as Border).Visibility = Visibility.Visible;
                OpenCharInfo((CP_CombatListDock.Children[1] as Grid).Children[3] as Label, e);
            }
        }
    }
}
