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
    /// Логика взаимодействия для SpellNewWindow.xaml
    /// </summary>
    public partial class SpellNewWindow : Window
    {
        public int FileRow = 0;

        public SpellNewWindow()
        {
            InitializeComponent();
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
    }
}
