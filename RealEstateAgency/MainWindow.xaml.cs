using RealEstateAgency.model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int viewPassword = 0;
        int introWin = 0;
        public MainWindow()
        {
            InitializeComponent();
            PasswordTb.Visibility = Visibility.Collapsed;
            UserComboBox();
        }

        public void UserComboBox()
        {
            UserCb.ItemsSource = AppData.DB.Agents.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            UserCb.SelectedIndex = 0;
        }

        private void ViewPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (viewPassword == 0)
            {
                PasswordPb.Visibility = Visibility.Collapsed;
                PasswordTb.Visibility = Visibility.Visible;
                PasswordTb.Text = PasswordPb.Password;
                viewPassword = 1;
            }
            else if (viewPassword == 1)
            {
                PasswordTb.Visibility = Visibility.Collapsed;
                PasswordPb.Visibility = Visibility.Visible;
                PasswordPb.Password = PasswordTb.Text;
                viewPassword = 0;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Закрыть приложение?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentUser = AppData.DB.Agents.FirstOrDefault(c => c.Id == UserCb.SelectedIndex + 1 && (c.Password == PasswordPb.Password || c.Password == PasswordTb.Text));
                if (currentUser != null)
                {
                    ApplicationsWin applicationsWin = new ApplicationsWin(UserCb.SelectedIndex + 1);
                    applicationsWin.Show();
                    Hide();
                }
                else
                {
                    throw new Exception("Неверный пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UserComboBox();
            PasswordPb.Password = string.Empty;
            PasswordTb.Text = string.Empty;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (introWin == 0)
            {
                Hide();
                WelcomeWin welcomeWin = new WelcomeWin();
                welcomeWin.ShowDialog();
                introWin = 1;
            }
        }
    }
}
