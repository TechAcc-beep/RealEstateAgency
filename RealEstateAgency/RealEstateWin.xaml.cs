using RealEstateAgency.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для RealEstateWin.xaml
    /// </summary>
    public partial class RealEstateWin : Window
    {
        public RealEstateWin()
        {
            InitializeComponent();
            GetData(null);
        }

        private void GetData(string combobox)
        {
            AppData.DB.RealEstateObjects.Load();
            AppData.DB.Address.Load();
            AppData.DB.TypeRealEstateObjects.Load();
            AppData.DB.AttributesName.Load();
            AppData.DB.AttributesRealEstateObjects.Load();
            if (combobox == null)
            {
                var data = AppData.DB.RealEstateObjects.Local.ToBindingList();
                ObjectDg.ItemsSource = data;
            }
            else
            {
                var data = AppData.DB.RealEstateObjects.Local.ToList();
                switch (combobox)
                {
                    case "По адресу":
                        data = data.Where(c => c.Address.Title.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ObjectDg.ItemsSource = data;
                        break;
                    case "По типу":
                        data = data.Where(c => c.TypeRealEstateObjects.Title.ToLower().StartsWith(SearchCb.Text.ToLower())).ToList();
                        ObjectDg.ItemsSource = data;
                        break;
                }
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddRealEstateBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditRealEstateWin addEditRealEstateWin = new AddEditRealEstateWin(null);
            addEditRealEstateWin.ShowDialog();
            ObjectDg.ItemsSource = null;
            GetData(null);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            RealEstateObjects cId = ObjectDg.SelectedItem as RealEstateObjects;
            if (ObjectDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Contracts.Any(c => c.RealEstateObject == ObjectDg.SelectedIndex + 1) == true || AppData.DB.Orders.Any(c => c.IdAddress == cId.IdAddress) == true)
            {
                MessageBox.Show($"Этот объект недвижимости нельзя удалить. \nОн задействован в других таблицах!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show($"Вы точно хотите удалить элемент {ObjectDg.SelectedIndex + 1}?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.DB.RealEstateObjects.RemoveRange(AppData.DB.RealEstateObjects.Where(p => p.Id == cId.Id));
                    AppData.DB.Address.RemoveRange(AppData.DB.Address.Where(p => p.Id == cId.IdAddress));
                    AppData.DB.AttributesRealEstateObjects.RemoveRange(AppData.DB.AttributesRealEstateObjects.Where(p => p.IdObject == cId.Id));
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ObjectDg.ItemsSource = null;
                    GetData(null);
                }
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ObjectDg.ItemsSource = null;
            GetData(((ComboBoxItem)SearchByCb.SelectedValue).Content.ToString());
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchTb.Text = string.Empty;
            SearchCb.Text = string.Empty;
            SearchByCb.Text = string.Empty;
            GetData(null);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ObjectDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                AddEditRealEstateWin addEditRealEstateWin = new AddEditRealEstateWin(ObjectDg.SelectedItem as RealEstateObjects);
                addEditRealEstateWin.ShowDialog();
                ObjectDg.ItemsSource = null;
                GetData(null);
            }
        }

        private void SearchByCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchByCb.SelectedIndex == 1 || SearchByCb.SelectedIndex == 2)
            {
                SearchCb.Visibility = Visibility.Visible;
                SearchTb.Visibility = Visibility.Collapsed;
                switch (SearchByCb.SelectedIndex)
                {
                    case (1):
                        SearchCb.ItemsSource = AppData.DB.TypeRealEstateObjects.Select(c => c.Title).ToList();
                        break;
                }
            }
            else
            {
                SearchCb.Visibility = Visibility.Collapsed;
                SearchTb.Visibility = Visibility.Visible;
            }
        }
    }
}
