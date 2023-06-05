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
using System.Windows.Shapes;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWin.xaml
    /// </summary>
    public partial class ChangePasswordWin : Window
    {
        int IdUser;
        public ChangePasswordWin(int idUser)
        {
            InitializeComponent();
            IdUser = idUser;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordTb.Text != RepeatPasswordPb.Password)
            {
                MessageBox.Show($"Пароли не совпадают!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var edit = AppData.DB.Agents.Where(c => c.Id == IdUser).FirstOrDefault();
                edit.Password = RepeatPasswordPb.Password;
                AppData.DB.SaveChanges();
                this.Close();
                MessageBox.Show("Пароль был изменен!", "Сохранено", MessageBoxButton.OK);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
