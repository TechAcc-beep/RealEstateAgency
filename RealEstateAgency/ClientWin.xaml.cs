using RealEstateAgency.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ClientWin.xaml
    /// </summary>
    public partial class ClientWin : Window
    {
        int changeTrigegr;
        public ClientWin()
        {
            InitializeComponent();
            GetData(null);
            changeTrigegr = 0;
        }

        private void GetData(string textbox)
        {
            AppData.DB.Clients.Load();
            if (textbox == "")
            {
                var data = AppData.DB.Clients.Local.ToBindingList();
                ClientsDg.ItemsSource = data;
            }
            else
            {
                var data = AppData.DB.Clients.Local.ToList();
                data = data.Where(c => c.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) ||
                        c.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                ClientsDg.ItemsSource = data;
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Orders.Any(c => c.Client == ClientsDg.SelectedIndex + 1) == true || AppData.DB.Contracts.Any(c => c.Clients.Id == ClientsDg.SelectedIndex + 1 
            || c.Clients1.Id == ClientsDg.SelectedIndex + 1) == true || AppData.DB.RealEstateObjects.Any(c => c.Clients.Id == ClientsDg.SelectedIndex + 1) == true)
            {
                MessageBox.Show($"Этого клиента нельзя удалить. \nОн задействован в других таблицах!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Clients cId = ClientsDg.SelectedItem as Clients;
                if (MessageBox.Show($"Вы точно хотите удалить элемент {ClientsDg.SelectedIndex + 1}?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.DB.Clients.RemoveRange(AppData.DB.Clients.Where(p => p.Id == cId.Id));
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ClientsDg.ItemsSource = null;
                    GetData(null);
                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LastNameTb.Text == "" || FirstNameTb.Text == "" || MiddleNameTb.Text == "")
            {
                if (LastNameTb.Text == "")
                {
                    MessageBox.Show($"Фамилия не заполнена!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LastNameTb.BorderBrush = Brushes.Red;
                }
                else if (FirstNameTb.Text == "")
                {
                    MessageBox.Show($"Имя не заполнено!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    FirstNameTb.BorderBrush = Brushes.Red;
                }
                else if (MiddleNameTb.Text == "")
                {
                    MessageBox.Show($"Отчество не заполнено!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MiddleNameTb.BorderBrush = Brushes.Red;
                }
            }
            else if (PhoneTb.Text == "" && EmailTb.Text == "")
            {
                MessageBox.Show($"Заполните телефон или почту!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (PhoneTb.Text != "" && PhoneTb.Text.Length != 11)
            {
                MessageBox.Show($"Номер телефона не заполнен! \n\n*Должен содержать 11 цифр", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTb.BorderBrush = Brushes.Red;
            }
            else
            {
                try
                {
                    int maxId = AppData.DB.Clients.Count() + 1;
                    if (PhoneTb.Text.Length == 0)
                    {
                        Clients add = new Clients
                        {
                            Id = maxId,
                            FirstName = FirstNameTb.Text,
                            MiddleName = MiddleNameTb.Text,
                            LastName = LastNameTb.Text,
                            Phone = null,
                            Email = EmailTb.Text
                        };
                        AppData.DB.Clients.Add(add);
                    }
                    else if (PhoneTb.Text.Length == 11)
                    {
                        Clients add = new Clients
                        {
                            Id = maxId,
                            FirstName = FirstNameTb.Text,
                            MiddleName = MiddleNameTb.Text,
                            LastName = LastNameTb.Text,
                            Phone = (long)Convert.ToDouble(PhoneTb.Text),
                            Email = EmailTb.Text
                        };
                        AppData.DB.Clients.Add(add);
                    }
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                    ClientsDg.ItemsSource = null;
                    ReturnColorButton();
                    ClearAllFields();
                    GetData(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                AddBtn.IsEnabled = false;
                Clients ClientDg = ClientsDg.SelectedItem as Clients;
                LastNameTb.Text = ClientDg.LastName;
                FirstNameTb.Text = ClientDg.FirstName;
                MiddleNameTb.Text = ClientDg.MiddleName;
                PhoneTb.Text = ClientDg.Phone.ToString();
                EmailTb.Text = ClientDg.Email;
                changeTrigegr = 1;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (changeTrigegr == 0)
            {
                MessageBox.Show($"Нажмите на кнопку «Изменить»!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (LastNameTb.Text == "" || FirstNameTb.Text == "" || MiddleNameTb.Text == "")
                {
                    MessageBox.Show($"Не все обязательные поля заполнены!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (LastNameTb.Text == "") LastNameTb.BorderBrush = Brushes.Red;
                    else if (FirstNameTb.Text == "") FirstNameTb.BorderBrush = Brushes.Red;
                    else if (MiddleNameTb.Text == "") MiddleNameTb.BorderBrush= Brushes.Red;
                }
                else if (PhoneTb.Text == "" && EmailTb.Text == "")
                {
                    MessageBox.Show($"Заполните телефон или почту!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (PhoneTb.Text != "" && PhoneTb.Text.Length != 11)
                {
                    MessageBox.Show($"Номер телефона не заполнен! \n\n*Должен содержать 11 цифр", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PhoneTb.BorderBrush = Brushes.Red;
                }
                else
                {
                    try
                    {
                        Clients ClientDg = ClientsDg.SelectedItem as Clients;
                        var edit = AppData.DB.Clients.Where(p => p.Id == ClientDg.Id).FirstOrDefault();
                        edit.LastName = LastNameTb.Text;
                        edit.FirstName = FirstNameTb.Text;
                        edit.MiddleName = MiddleNameTb.Text;
                        if (PhoneTb.Text.Length == 0)
                        {
                            edit.Phone = null;
                        }
                        else if (PhoneTb.Text.Length == 11)
                        {
                            edit.Phone = (long)Convert.ToDouble(PhoneTb.Text);
                        }
                        edit.Email = EmailTb.Text;
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Изменения были внесены");
                        ClientsDg.ItemsSource = null;
                        GetData(null);
                        ReturnColorButton();
                        AddBtn.IsEnabled = true;
                        ClearAllFields();
                        changeTrigegr = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ReturnColorButton()
        {
            LastNameTb.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            FirstNameTb.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            MiddleNameTb.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
            PhoneTb.BorderBrush = new SolidColorBrush(Color.FromRgb(171, 173, 179));
        }

        private void ClearAllFields()
        {
            FirstNameTb.Text = string.Empty;
            MiddleNameTb.Text = string.Empty;
            LastNameTb.Text = string.Empty;
            PhoneTb.Text = string.Empty;
            EmailTb.Text = string.Empty;
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClientsDg.ItemsSource = null;
            GetData(SearchTb.Text);
        }

        private void OnlyText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.Match(e.Text, @"[а-яА-Я]").Success)
            {
                e.Handled = true;
            }
        }

        private void BanSpace_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void OnlyNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }
    }
}
