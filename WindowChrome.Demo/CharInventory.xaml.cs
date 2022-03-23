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


namespace WindowChrome.Demo
{
    /// <summary>
    /// Логика взаимодействия для CharInventory.xaml
    /// </summary>
    public partial class CharInventory : Window
    {
        public bool IsUnchet = true;
        public int CounterOfTemps = 1;
        public String ManipulatedObject = "";

        public CharInventory()
        {
            InitializeComponent();
        }

        private void CI_AddItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CI_NewItemGroup.Width == 0)
            {
                CI_AddItem.RenderTransform = new RotateTransform(45);
                CI_NewItemGroup.Width = CI_MainWindow.Width - 8;
                CI_NewItemGroup.HorizontalAlignment = HorizontalAlignment.Stretch;
                CI_AddItemBorder.Background = new SolidColorBrush(Color.FromRgb(166, 68, 68));
                Thickness NewMarign = new Thickness(0, 0, 0, 30);
                CI_ItemNames.Margin = NewMarign;
                CI_ItemWeights.Margin = NewMarign;
                CI_ItemCounts.Margin = NewMarign;
            }
            else
            {
                CI_AddItem.RenderTransform = new RotateTransform(0);
                CI_NewItemGroup.Width = 0;
                CI_AddItemBorder.Background = new SolidColorBrush(Color.FromRgb(72, 166, 68));
                Thickness NewMarign = new Thickness(0, 0, 0, 0);
                CI_NewItemGroup.HorizontalAlignment = HorizontalAlignment.Left;
                CI_ItemNames.Margin = NewMarign;
                CI_ItemWeights.Margin = NewMarign;
                CI_ItemCounts.Margin = NewMarign;
            }
        }

        private void CI_ConfurmAdding_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateRow(CI_ItemName.Text, CI_ItemCount.Text, CI_ItemWeight.Text);
        }

        public void CreateRow(String Item, String Count, String Weight)
        {
            if (Item == "") Item = "Незвестный предмет";
            if (Count == "") Count = "Незвестное количество";
            if (Weight == "") Weight = "Незвестный вес";
            if (Item == "Название предмета") Item = "Незвестный предмет";
            if (Count == "Количество") Count = "Незвестное количество";
            if (Weight == "Вес") Weight = "Незвестный вес";
            Label NewLabel = new Label();
            NewLabel.Name = "Name" + CounterOfTemps.ToString();
            CI_ItemNames.Children.Add(NewLabel);
            DockPanel.SetDock(NewLabel, Dock.Top);
            NewLabel.Content = Item;
            NewLabel.Foreground = new SolidColorBrush(Color.FromRgb(254, 211, 172));
            NewLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (IsUnchet == true) NewLabel.Background = new SolidColorBrush(Color.FromRgb(83, 83, 89));
            else NewLabel.Background = new SolidColorBrush(Color.FromRgb(57, 57, 57));

            Label NewLabel1 = new Label();
            NewLabel1.Name = "Count" + CounterOfTemps.ToString();
            CI_ItemCounts.Children.Add(NewLabel1);
            DockPanel.SetDock(NewLabel1, Dock.Top);
            NewLabel1.Content = Count;
            NewLabel1.Foreground = new SolidColorBrush(Color.FromRgb(254, 211, 172));
            NewLabel1.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (IsUnchet == true) NewLabel1.Background = new SolidColorBrush(Color.FromRgb(83, 83, 89));
            else NewLabel1.Background = new SolidColorBrush(Color.FromRgb(57, 57, 57));

            Label NewLabel2 = new Label();
            NewLabel2.Name = "Weight" + CounterOfTemps.ToString();
            CI_ItemWeights.Children.Add(NewLabel2);
            DockPanel.SetDock(NewLabel2, Dock.Top);
            NewLabel2.Content = Weight;
            NewLabel2.Foreground = new SolidColorBrush(Color.FromRgb(254, 211, 172));
            NewLabel2.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (IsUnchet == true) NewLabel2.Background = new SolidColorBrush(Color.FromRgb(83, 83, 89));
            else NewLabel2.Background = new SolidColorBrush(Color.FromRgb(57, 57, 57));

            IsUnchet = !IsUnchet;

            ContextMenu contextmenu = new ContextMenu();
            contextmenu.Name = "CM"+ CounterOfTemps.ToString();
            NewLabel2.ContextMenu = contextmenu;
            NewLabel1.ContextMenu = contextmenu;
            NewLabel.ContextMenu = contextmenu;
            MenuItem mi = new MenuItem();
            mi.Header = "Редактировать";
            MenuItem mi1 = new MenuItem();
            mi1.Header = "Удалить";
            contextmenu.Items.Add(mi);
            contextmenu.Items.Add(mi1);
            mi.Click += EditItemCM;
            mi1.Click += DeleteItemCM;

            NewLabel.ContextMenuOpening += ManipulayedObjectEvent;
            NewLabel1.ContextMenuOpening += ManipulayedObjectEvent;
            NewLabel2.ContextMenuOpening += ManipulayedObjectEvent;

            CounterOfTemps++;
        }

        private void EditItemCM(object sender, RoutedEventArgs e)
        {
            //Редактор
        }

        private void DeleteItemCM(object sender, RoutedEventArgs e)
        {
            //Загрузка и выгрузка
        }

        private void CI_MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CI_NewItemGroup.Width = CI_MainWindow.Width - 8;
        }

        private void ManipulayedObjectEvent(object sender, ContextMenuEventArgs e)
        {
            ManipulatedObject = (sender as Label).Name;
        }

        private void CI_MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CI_AddItem.RenderTransform = new RotateTransform(0);
            CI_NewItemGroup.Width = 0;
            CI_AddItemBorder.Background = new SolidColorBrush(Color.FromRgb(72, 166, 68));
            Thickness NewMarign = new Thickness(0, 0, 0, 0);
            CI_NewItemGroup.HorizontalAlignment = HorizontalAlignment.Left;
            CI_ItemNames.Margin = NewMarign;
            CI_ItemWeights.Margin = NewMarign;
            CI_ItemCounts.Margin = NewMarign;
        }
    }
}
