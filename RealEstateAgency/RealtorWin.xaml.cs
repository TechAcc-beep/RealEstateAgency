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
    /// Логика взаимодействия для RealtorWin.xaml
    /// </summary>
    public partial class RealtorWin : Window
    {
        int changeTrigegr;
        int IdUser;
        public RealtorWin(int idUser)
        {
            InitializeComponent();
            GetData(null);
            changeTrigegr = 0;
            IdUser = idUser;
            if (AppData.DB.Agents.First(c => c.Id == idUser).IdRole == 2)
            {
                LastNameTb.IsEnabled = false;
                FirstNameTb.IsEnabled = false;
                MiddleNameTb.IsEnabled = false;
                PhoneTb.IsEnabled = false;
                ShareTb.IsEnabled = false;
                ShareSl.IsEnabled = false;
                AddBtn.IsEnabled = false;
                EditBtn.IsEnabled = false;
                SaveBtn.IsEnabled = false;
                DeleteBtn.IsEnabled = false;
            }
        }

        private void GetData(string textbox)
        {
            AppData.DB.Agents.Load();
            if (textbox == "")
            {
                var data = AppData.DB.Agents.Local.ToBindingList();
                RealtorsDg.ItemsSource = data;
            }
            else
            {
                var data = AppData.DB.Agents.Local.ToList();
                data = data.Where(c => c.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) ||
                        c.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                RealtorsDg.ItemsSource = data;
            }
        }

        private void OnlyNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RealtorsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (RealtorsDg.SelectedIndex + 1 == IdUser)
            {
                MessageBox.Show($"Вы не можете удалить сами себя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Orders.Any(c => c.Agent == RealtorsDg.SelectedIndex + 1) == true || AppData.DB.Contracts.Any(c => c.Realtor == RealtorsDg.SelectedIndex + 1) == true)
            {
                MessageBox.Show($"Этого риелтор нельзя удалить. \nОн задействован в других таблицах!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Agents cId = RealtorsDg.SelectedItem as Agents;
                if (MessageBox.Show($"Вы точно хотите удалить элемент {RealtorsDg.SelectedIndex + 1}?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.DB.Agents.RemoveRange(AppData.DB.Agents.Where(p => p.Id == cId.Id));
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    RealtorsDg.ItemsSource = null;
                    GetData(null);
                }
            }
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            RealtorsDg.ItemsSource = null;
            GetData(SearchTb.Text);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LastNameTb.Text == "" || FirstNameTb.Text == "" || MiddleNameTb.Text == "" || PhoneTb.Text == "" || PhoneTb.Text.Length != 11 || ShareTb.Text == "")
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
                else if (PhoneTb.Text == "" || PhoneTb.Text.Length != 11)
                {
                    MessageBox.Show($"Номер телефона не заполнен! \n\n*Должен содержать 11 цифр", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    PhoneTb.BorderBrush = Brushes.Red;
                }
                else if (ShareTb.Text == "")
                {
                    MessageBox.Show($"Доля от сделок не заполнена!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                try
                {
                    int maxId = AppData.DB.Agents.Count() + 1;
                    Agents add = new Agents
                    {
                        Id = maxId,
                        FirstName = FirstNameTb.Text,
                        MiddleName = MiddleNameTb.Text,
                        LastName = LastNameTb.Text,
                        Phone = (long)Convert.ToDouble(PhoneTb.Text),
                        Share = (int)ShareSl.Value,
                        Password = maxId.ToString(),
                        IdRole = 2
                    };
                    AppData.DB.Agents.Add(add);
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                    RealtorsDg.ItemsSource = null;
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
            if (RealtorsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                AddBtn.IsEnabled = false;
                if (RealtorsDg.SelectedIndex + 1 != IdUser)
                {
                    AddBtn.Visibility = Visibility.Collapsed;
                    RolesCb.Visibility = Visibility.Visible;
                }
                else
                {
                    AddBtn.Visibility = Visibility.Visible;
                    RolesCb.Visibility = Visibility.Collapsed;
                }
                Agents AgentDg = RealtorsDg.SelectedItem as Agents;
                LastNameTb.Text = AgentDg.LastName;
                FirstNameTb.Text = AgentDg.FirstName;
                MiddleNameTb.Text = AgentDg.MiddleName;
                PhoneTb.Text = AgentDg.Phone.ToString();
                ShareTb.Text = AgentDg.Share.ToString();
                RolesCb.SelectedIndex = AgentDg.IdRole - 1;
                changeTrigegr = 1;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (changeTrigegr == 0)
            {
                MessageBox.Show($"Нажмите на кнопку «Изменить»!", "Внимание", MessageBoxButton.OK);
            }
            else
            {
                if (LastNameTb.Text == "" || FirstNameTb.Text == "" || MiddleNameTb.Text == "" || PhoneTb.Text == "" || PhoneTb.Text.Length != 11 || ShareTb.Text == "")
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
                    else if (PhoneTb.Text == "" || PhoneTb.Text.Length != 11)
                    {
                        MessageBox.Show($"Номер телефона не заполнен! \n\n*Должен содержать 11 цифр", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PhoneTb.BorderBrush = Brushes.Red;
                    }
                    else if (ShareTb.Text == "")
                    {
                        MessageBox.Show($"Доля от сделок не заполнена!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    try
                    {
                        Agents AgentDg = RealtorsDg.SelectedItem as Agents;
                        var edit = AppData.DB.Agents.Where(p => p.Id == AgentDg.Id).FirstOrDefault();
                        edit.LastName = LastNameTb.Text;
                        edit.FirstName = FirstNameTb.Text;
                        edit.MiddleName = MiddleNameTb.Text;
                        edit.Phone = (long)Convert.ToDouble(PhoneTb.Text);
                        edit.Share = (int)ShareSl.Value;
                        edit.IdRole = RolesCb.SelectedIndex + 1;
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Изменения были внесены");
                        RealtorsDg.ItemsSource = null;
                        GetData(null);
                        ReturnColorButton();
                        AddBtn.IsEnabled = true;
                        AddBtn.Visibility = Visibility.Visible;
                        RolesCb.Visibility = Visibility.Collapsed;
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
            ShareTb.Text = "0";
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
    }
}
