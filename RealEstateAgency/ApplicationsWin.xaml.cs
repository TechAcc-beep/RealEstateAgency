using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RealEstateAgency.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для ApplicationsWin.xaml
    /// </summary>
    public partial class ApplicationsWin : Window
    {
        int closeTrigger;
        int IdUser;
        public ApplicationsWin(int idUser)
        {
            InitializeComponent();
            closeTrigger = 0;
            GetData(null);
            FullNameUserLbl.Content = AppData.DB.Agents.First(c => c.Id == idUser).FullName;
            IdUser = idUser;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (AppData.DB.Agents.First(c => c.Id == IdUser).IdRole == 1)
            {
                FullNameUserLbl.Content = $"{AppData.DB.Agents.First(c => c.Id == idUser).FullName} (admin)";
            }
        }

        private void GetData(string combobox)
        {
            AppData.DB.Orders.Load();
            AppData.DB.StatusOrders.Load();
            AppData.DB.Clients.Load();
            AppData.DB.Agents.Load();
            AppData.DB.TypeRealEstateObjects.Load();
            AppData.DB.Address.Load();
            AppData.DB.TypeOrders.Load();
            AppData.DB.AttributesName.Load();
            AppData.DB.AttributesOrders.Load();
            if (combobox == null)
            {
                var data = AppData.DB.Orders.Local.ToBindingList();
                ApplicationsDg.ItemsSource = data;
            }
            else
            {
                var data = AppData.DB.Orders.Local.ToList();
                switch (combobox)
                {
                    case "По статусу":
                        data = data.Where(c => c.StatusOrders.Title.ToLower().StartsWith(SearchCb.Text.ToLower())).ToList();
                        ApplicationsDg.ItemsSource = data;
                        break;
                    case "По адресу":
                        data = data.Where(c => c.Address.Title.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ApplicationsDg.ItemsSource = data;
                        break;
                    case "По клиенту":
                        data = data.Where(c => c.Clients.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) || 
                        c.Clients.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.Clients.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ApplicationsDg.ItemsSource = data;
                        break;
                    case "По риелтору":
                        data = data.Where(c => c.Agents.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) || 
                        c.Agents.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.Agents.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ApplicationsDg.ItemsSource = data;
                        break;
                    case "По типу недвижимости":
                        data = data.Where(c => c.TypeRealEstateObjects.Title.ToLower().StartsWith(SearchCb.Text.ToLower())).ToList();
                        ApplicationsDg.ItemsSource = data;
                        break;
                    case "По типу заявки":
                        data = data.Where(c => c.TypeOrders.Title.ToLower().StartsWith(SearchCb.Text.ToLower())).ToList();
                        ApplicationsDg.ItemsSource = data;
                        break;
                }
            }
        }

        private void UserChange_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите сменить пользователя?", "Смена пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question) ;
            if (result == MessageBoxResult.Yes)
            { 
                closeTrigger = 1;
                this.Close();
                Application.Current.MainWindow.Show();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Закрыть приложение?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closeTrigger == 0) Application.Current.MainWindow.Close();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWin changePasswordWin = new ChangePasswordWin(IdUser);
            changePasswordWin.ShowDialog();
        }

        private void ReferenceInfo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("UserGuide.chm");
        }

        private void Realtor_Click(object sender, RoutedEventArgs e)
        {
            RealtorWin realtorWin = new RealtorWin(IdUser);
            realtorWin.ShowDialog();
            ApplicationsDg.ItemsSource = null;
            GetData(null);
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            ClientWin clientWin = new ClientWin();
            clientWin.ShowDialog();
            ApplicationsDg.ItemsSource = null;
            GetData(null);
        }

        private void RealEstate_Click(object sender, RoutedEventArgs e)
        {
            RealEstateWin realEstateWin = new RealEstateWin();
            realEstateWin.ShowDialog();
            ApplicationsDg.ItemsSource = null;
            GetData(null);
        }

        private void Agreements_Click(object sender, RoutedEventArgs e)
        {
            AgreementsWin agreementsWin = new AgreementsWin(IdUser);
            agreementsWin.ShowDialog();
            ApplicationsDg.ItemsSource = null;
            GetData(null);
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            Orders OrderDg = ApplicationsDg.SelectedItem as Orders;
            if (ApplicationsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Agents.First(c => c.Id == IdUser).IdRole == 2 && OrderDg.StatusOrder == 3)
            {
                MessageBox.Show($"Вы не можете изменить элемент {ApplicationsDg.SelectedIndex + 1}!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                AddEditOrderWin addEditOrder = new AddEditOrderWin(ApplicationsDg.SelectedItem as Orders);
                addEditOrder.ShowDialog();
                ApplicationsDg.ItemsSource = null;
                GetData(null);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        { 
            Orders OrderDg = ApplicationsDg.SelectedItem as Orders;
            if (ApplicationsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Agents.First(c => c.Id == IdUser).IdRole == 2 && OrderDg.StatusOrder == 3)
            {
                MessageBox.Show($"Вы не можете удалить элемент {ApplicationsDg.SelectedIndex + 1}!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Contracts.Any(c => c.Realtor == OrderDg.Agent) == true && (AppData.DB.Contracts.Any(c => c.Clients.Id == OrderDg.Client) 
                || AppData.DB.Contracts.Any(c => c.Clients1.Id == OrderDg.Client)) == true)
            {
                MessageBox.Show($"Эту заявку нельзя удалить. \nОна задействован в других таблицах!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show($"Вы точно хотите удалить элемент {ApplicationsDg.SelectedIndex + 1}?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.DB.Orders.RemoveRange(AppData.DB.Orders.Where(p => p.Id == OrderDg.Id));
                    AppData.DB.Address.RemoveRange(AppData.DB.Address.Where(p => p.Id == OrderDg.IdAddress));
                    AppData.DB.AttributesOrders.RemoveRange(AppData.DB.AttributesOrders.Where(p => p.IdOrder == OrderDg.Id));
                    if (OrderDg.TypeOrder == 2 || OrderDg.TypeOrder == 3)
                    {
                        AppData.DB.RealEstateObjects.RemoveRange(AppData.DB.RealEstateObjects.Where(p => p.IdAddress == OrderDg.IdAddress));
                        AppData.DB.AttributesRealEstateObjects.RemoveRange(AppData.DB.AttributesRealEstateObjects.Where(p => p.RealEstateObjects.IdAddress == OrderDg.IdAddress));
                    }
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ApplicationsDg.ItemsSource = null;
                    GetData(null);
                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditOrderWin addEditOrder = new AddEditOrderWin(null);
            addEditOrder.ShowDialog();
            ApplicationsDg.ItemsSource = null;
            GetData(null);
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchTb.Text = string.Empty;
            SearchCb.Text = string.Empty;
            SearchByCb.Text = string.Empty;
            GetData(null);
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ApplicationsDg.ItemsSource = null;
            GetData(((ComboBoxItem)SearchByCb.SelectedValue).Content.ToString());    
        }

        private void SearchByCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchByCb.SelectedIndex == 0 || SearchByCb.SelectedIndex == 4 || SearchByCb.SelectedIndex == 5)
            {
                SearchCb.Visibility = Visibility.Visible;
                SearchTb.Visibility = Visibility.Collapsed;
                switch (SearchByCb.SelectedIndex)
                {
                    case (0):
                        SearchCb.ItemsSource = AppData.DB.StatusOrders.Select(c => c.Title).ToList();
                        break;
                    case (4):
                        SearchCb.ItemsSource = AppData.DB.TypeRealEstateObjects.Select(c => c.Title).ToList();
                        break;
                    case (5):
                        SearchCb.ItemsSource = AppData.DB.TypeOrders.Select(c => c.Title).ToList();
                        break;
                }
            }
            else
            {
                SearchCb.Visibility = Visibility.Collapsed;
                SearchTb.Visibility = Visibility.Visible;
            }
        }

        private void ListRealtors_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Риелторы");
            excelWorksheet.Cells["A1"].Value = "ФИО";
            excelWorksheet.Cells["B1"].Value = "Доля от сделок";
            excelWorksheet.Cells["C1"].Value = "Телефон";
            for(int i = 2; i <= AppData.DB.Agents.Count() + 1; i++)
            {
                excelWorksheet.Cells["A" + i].Value = AppData.DB.Agents.First(c => c.Id == i - 1).FullName;
                excelWorksheet.Cells["B" + i].Value = AppData.DB.Agents.First(c => c.Id == i - 1).Share;
                excelWorksheet.Cells["C" + i].Value = AppData.DB.Agents.First(c => c.Id == i - 1).Phone;
            }
            excelWorksheet.Cells["A1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 170, 170, 170));
            excelWorksheet.Cells[$"A1:C{AppData.DB.Agents.Count() + 1}"].AutoFitColumns();
            excelWorksheet.Cells[$"A1:C{AppData.DB.Agents.Count() + 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:C{AppData.DB.Agents.Count() + 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:C{AppData.DB.Agents.Count() + 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:C{AppData.DB.Agents.Count() + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Select("D1");
            SaveExcelFile(excelPackage);
        }

        private void ListClients_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Клиенты");
            excelWorksheet.Cells["A1"].Value = "ФИО";
            excelWorksheet.Cells["B1"].Value = "Телефон";
            excelWorksheet.Cells["C1"].Value = "Эл. почта";
            for (int i = 2; i <= AppData.DB.Clients.Count() + 1; i++)
            {
                excelWorksheet.Cells["A" + i].Value = AppData.DB.Clients.First(c => c.Id == i - 1).FullName;
                excelWorksheet.Cells["B" + i].Value = AppData.DB.Clients.First(c => c.Id == i - 1).Phone;
                excelWorksheet.Cells["C" + i].Value = AppData.DB.Clients.First(c => c.Id == i - 1).Email;
            }
            excelWorksheet.Cells["A1:C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 170, 170, 170));
            excelWorksheet.Cells[$"A1:C{AppData.DB.Clients.Count() + 1}"].AutoFitColumns();
            excelWorksheet.Cells[$"A1:C{AppData.DB.Clients.Count() + 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:C{AppData.DB.Clients.Count() + 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:C{AppData.DB.Clients.Count() + 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:C{AppData.DB.Clients.Count() + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Select("D1");
            SaveExcelFile(excelPackage);
        }

        private void ListObjects_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Объекты недвижимости");
            excelWorksheet.Cells["A1"].Value = "Адрес";
            excelWorksheet.Cells["B1"].Value = "Тип объекта";
            excelWorksheet.Cells["C1"].Value = "Описание";
            excelWorksheet.Cells["D1"].Value = "Владелец";
            for (int i = 2; i <= AppData.DB.RealEstateObjects.Count() + 1; i++)
            {
                excelWorksheet.Cells["A" + i].Value = AppData.DB.RealEstateObjects.First(c => c.Id == i - 1).Address.Title;
                excelWorksheet.Cells["B" + i].Value = AppData.DB.RealEstateObjects.First(c => c.Id == i - 1).TypeRealEstateObjects.Title;
                excelWorksheet.Cells["C" + i].Value = AppData.DB.RealEstateObjects.First(c => c.Id == i - 1).Description;
                excelWorksheet.Cells["D" + i].Value = AppData.DB.RealEstateObjects.First(c => c.Id == i - 1).Clients.FullName;
            }
            excelWorksheet.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 170, 170, 170));
            excelWorksheet.Cells[$"A1:D{AppData.DB.RealEstateObjects.Count() + 1}"].AutoFitColumns();
            excelWorksheet.Cells[$"A1:D{AppData.DB.RealEstateObjects.Count() + 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:D{AppData.DB.RealEstateObjects.Count() + 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:D{AppData.DB.RealEstateObjects.Count() + 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:D{AppData.DB.RealEstateObjects.Count() + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Select("E1");
            SaveExcelFile(excelPackage);
        }

        private void ListOrders_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Заявки");
            excelWorksheet.Cells["A1"].Value = "Статус";
            excelWorksheet.Cells["B1"].Value = "Адрес";
            excelWorksheet.Cells["C1"].Value = "Клиент";
            excelWorksheet.Cells["D1"].Value = "Риелтор";
            excelWorksheet.Cells["E1"].Value = "Тип объекта";
            excelWorksheet.Cells["F1"].Value = "Мин. цена";
            excelWorksheet.Cells["G1"].Value = "Макс. цена";
            excelWorksheet.Cells["H1"].Value = "Тип заявки";
            for (int i = 2; i <= AppData.DB.Orders.Count() + 1; i++)
            {
                excelWorksheet.Cells["A" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).StatusOrders.Title;
                excelWorksheet.Cells["B" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).Address.Title;
                excelWorksheet.Cells["C" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).Clients.FullName;
                excelWorksheet.Cells["D" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).Agents.FullName;
                excelWorksheet.Cells["E" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).TypeRealEstateObjects.Title;
                excelWorksheet.Cells["F" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).MinPrice;
                excelWorksheet.Cells["G" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).MaxPrice;
                excelWorksheet.Cells["H" + i].Value = AppData.DB.Orders.First(c => c.Id == i - 1).TypeOrders.Title;
            }
            excelWorksheet.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 170, 170, 170));
            excelWorksheet.Cells[$"A1:H{AppData.DB.Orders.Count() + 1}"].AutoFitColumns();
            excelWorksheet.Cells[$"A1:H{AppData.DB.Orders.Count() + 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:H{AppData.DB.Orders.Count() + 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:H{AppData.DB.Orders.Count() + 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:H{AppData.DB.Orders.Count() + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Select("I1");
            SaveExcelFile(excelPackage);
        }

        private void ListContracts_Click(object sender, RoutedEventArgs e)
        {
            ExcelPackage excelPackage = new ExcelPackage();
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Договоры");
            excelWorksheet.Cells["A1"].Value = "Номер договора";
            excelWorksheet.Cells["B1"].Value = "Покупатель";
            excelWorksheet.Cells["C1"].Value = "Цена";
            excelWorksheet.Cells["D1"].Value = "Комиссия";
            excelWorksheet.Cells["E1"].Value = "Дата";
            excelWorksheet.Cells["F1"].Value = "Риелтор";
            excelWorksheet.Cells["G1"].Value = "Объект недвижимости";
            excelWorksheet.Cells["H1"].Value = "Владелец";
            excelWorksheet.Cells["I1"].Value = "Тип договора";
            excelWorksheet.Cells["J1"].Value = "Тип сделки";
            for (int i = 2; i <= AppData.DB.Contracts.Count() + 1; i++)
            {
                excelWorksheet.Cells["A" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).ContractNumber;
                excelWorksheet.Cells["B" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).Clients.FullName;
                excelWorksheet.Cells["C" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).Price;
                excelWorksheet.Cells["D" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).Commission;
                excelWorksheet.Cells["E" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).Date.GetDateTimeFormats();
                excelWorksheet.Cells["F" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).Agents.FullName;
                excelWorksheet.Cells["G" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).RealEstateObjects.TypeAndAddress;
                excelWorksheet.Cells["H" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).Clients1.FullName;
                excelWorksheet.Cells["I" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).TypeContracts.Title;
                excelWorksheet.Cells["J" + i].Value = AppData.DB.Contracts.First(c => c.Id == i - 1).TypeDeals.Title;
            }
            excelWorksheet.Cells["A1:J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(255, 170, 170, 170));
            excelWorksheet.Cells[$"A1:J{AppData.DB.Contracts.Count() + 1}"].AutoFitColumns();
            excelWorksheet.Cells[$"A1:J{AppData.DB.Contracts.Count() + 1}"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:J{AppData.DB.Contracts.Count() + 1}"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:J{AppData.DB.Contracts.Count() + 1}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"A1:J{AppData.DB.Contracts.Count() + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            excelWorksheet.Cells[$"E2:E{AppData.DB.Contracts.Count() + 1}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            excelWorksheet.Select("K1");
            SaveExcelFile(excelPackage);
        }

        private void SaveExcelFile(ExcelPackage table)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файл Excel (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                table.SaveAs(new FileInfo($"{saveFileDialog.FileName}"));
                MessageBoxResult result = MessageBox.Show("Данные успешно записаны. Открыть файл?", "Сохранено", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start($"{saveFileDialog.FileName}");
                }
            }
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWin profileWin = new ProfileWin(IdUser);
            profileWin.ShowDialog();
        }
    }
}
