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
    /// Логика взаимодействия для ProfileWin.xaml
    /// </summary>
    public partial class ProfileWin : Window
    {
        int IdUser;
        int viewPassword = 0;
        public ProfileWin(int idUser)
        {
            InitializeComponent();
            IdUser = idUser;
            GetData(idUser);
            RealtorsCb.SelectedIndex = idUser - 1;
            if (AppData.DB.Agents.First(c => c.Id == idUser).IdRole == 1)
            {
                DataUserLbl.Visibility = Visibility.Collapsed;
                RealtorsCb.Visibility = Visibility.Visible;
            }
        }

        private void GetData(int combobox)
        {
            AppData.DB.Orders.Load();
            AppData.DB.StatusOrders.Load();
            AppData.DB.Clients.Load();
            AppData.DB.Agents.Load();
            AppData.DB.TypeRealEstateObjects.Load();
            AppData.DB.Address.Load();
            AppData.DB.TypeOrders.Load();
            AppData.DB.Contracts.Load();
            var data = AppData.DB.Orders.ToList();
            var data1 = AppData.DB.Contracts.Local.ToList();
            UserFullNameTb.Text = AppData.DB.Agents.First(c => c.Id == combobox).FullName;
            PhoneTb.Text = AppData.DB.Agents.First(c => c.Id == combobox).Phone;
            ShareTb.Text = AppData.DB.Agents.First(c => c.Id == combobox).Share.ToString();
            UserPasswordTb.Text = AppData.DB.Agents.First(c => c.Id == combobox).Password;
            UserPasswordPb.Password = AppData.DB.Agents.First(c => c.Id == combobox).Password;
            RealtorsCb.ItemsSource = AppData.DB.Agents.Select(c => c.LastName + " " + c.FirstName).ToList();
            data = data.Where(c => c.Agents.Id == combobox).ToList();
            ApplicationsDg.ItemsSource = data;
            data1 = data1.Where(c => c.Agents.Id == combobox).ToList();
            ContractsDg.ItemsSource = data1;
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ViewPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (viewPassword == 0)
            {
                UserPasswordPb.Visibility = Visibility.Collapsed;
                UserPasswordTb.Visibility = Visibility.Visible;
                viewPassword = 1;
            }
            else if (viewPassword == 1)
            {
                UserPasswordTb.Visibility = Visibility.Collapsed;
                UserPasswordPb.Visibility = Visibility.Visible;
                viewPassword = 0;
            }
        }

        private void UserPasswordPb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RealtorsCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetData(RealtorsCb.SelectedIndex + 1);
        }
    }
}
