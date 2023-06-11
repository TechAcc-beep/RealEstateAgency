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
    /// Логика взаимодействия для AddEditAgreementsWin.xaml
    /// </summary>
    public partial class AddEditAgreementsWin : Window
    {
        Contracts contr; 
        public AddEditAgreementsWin(Contracts contracts)
        {
            InitializeComponent();
            contr = contracts;
            OrdersCb.ItemsSource = AppData.DB.Orders.Select(c => c.Clients.LastName + " " + c.Clients.FirstName + ": " + c.TypeOrders.Title + ", " + c.TypeRealEstateObjects.Title).ToList();
            OwnerCb.ItemsSource = AppData.DB.Clients.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            CustomerCb.ItemsSource = AppData.DB.Clients.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            RealtorCb.ItemsSource = AppData.DB.Agents.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            ObjectCb.ItemsSource = AppData.DB.RealEstateObjects.Select(c => c.TypeRealEstateObjects.Title + ": " + c.Address.Title).ToList();
            TypeContractCb.ItemsSource = AppData.DB.TypeContracts.Select(c => c.Title).ToList();
            TypeDealCb.ItemsSource = AppData.DB.TypeDeals.Select(c => c.Title).ToList();
            RealtorCb.IsEnabled = false;
            TypeDealCb.IsEnabled = false;
            OwnerCb.IsEnabled = false;
            if (contracts == null)
            {
                this.Title = "Добавление";
            }
            else
            {
                this.Title = "Изменение";
                ObjectCb.IsEnabled = false;
                OrdersCb.SelectedIndex = AppData.DB.Orders.FirstOrDefault(c => c.Agent == contracts.Realtor).Id - 1;
                OrdersCb.IsEnabled = false;
                NumberContractTb.Text = contracts.ContractNumber.ToString();
                OwnerCb.SelectedIndex = contracts.Owner - 1;
                ObjectCb.SelectedIndex = contracts.RealEstateObject - 1;
                RealtorCb.SelectedIndex = contracts.Realtor - 1;
                CustomerCb.SelectedIndex = contracts.Customer - 1;
                TypeContractCb.SelectedIndex = contracts.TypeContract - 1;
                TypeDealCb.SelectedIndex = contracts.TypeDeal - 1;
                PriceTb.Text = contracts.Price.ToString();
                CommissionTb.Text = contracts.Commission.ToString();
                DateDp.Text = contracts.Date.ToString();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NumberContractTb.Text == "")
            {
                MessageBox.Show($"Введите номер договора!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                NumberContractTb.BorderBrush = Brushes.Red;
            }
            else if (OrdersCb.SelectedValue == null)
            {
                MessageBox.Show($"Выберите заявку!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ObjectCb.SelectedValue == null)
            {
                MessageBox.Show($"Выберите объект недвижимости!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (CustomerCb.SelectedValue == null)
            {
                MessageBox.Show($"Выберите покупателя!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (TypeContractCb.SelectedValue == null)
            {
                MessageBox.Show($"Выберите тип договора!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (PriceTb.Text == "")
            {
                MessageBox.Show($"Введите цену!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTb.BorderBrush = Brushes.Red;
            }
            else if (DateDp.Text == "")
            {
                MessageBox.Show($"Выберите дату!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                DateDp.BorderBrush = Brushes.Red;
            }
            else
            {
                if (contr == null)
                {
                    try
                    {
                        int maxId = AppData.DB.Contracts.Count() + 1;
                        Contracts add = new Contracts
                        {
                            Id = maxId,
                            ContractNumber = int.Parse(NumberContractTb.Text),
                            Customer = CustomerCb.SelectedIndex + 1,
                            Price = int.Parse(PriceTb.Text),
                            Commission = double.Parse(CommissionTb.Text),
                            Date = Convert.ToDateTime(DateDp.Text),
                            Realtor = RealtorCb.SelectedIndex + 1,
                            RealEstateObject = ObjectCb.SelectedIndex + 1,
                            Owner = OwnerCb.SelectedIndex + 1,
                            TypeContract = TypeContractCb.SelectedIndex + 1,
                            TypeDeal = TypeDealCb.SelectedIndex + 1
                        };
                        AppData.DB.Contracts.Add(add);
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Запись добавлена");
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        var edit = AppData.DB.Contracts.Where(p => p.Id == contr.Id).FirstOrDefault();
                        edit.ContractNumber = int.Parse(NumberContractTb.Text);
                        edit.Customer = CustomerCb.SelectedIndex + 1;
                        edit.Price = int.Parse(PriceTb.Text);
                        edit.Commission = double.Parse(CommissionTb.Text);
                        edit.Date = Convert.ToDateTime(DateDp.Text);
                        edit.Realtor = RealtorCb.SelectedIndex + 1;
                        edit.RealEstateObject = ObjectCb.SelectedIndex + 1;
                        edit.Owner = OwnerCb.SelectedIndex + 1;
                        edit.TypeContract = TypeContractCb.SelectedIndex + 1;
                        edit.TypeDeal = TypeDealCb.SelectedIndex + 1;
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Изменения были внесены");
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                var change = AppData.DB.Orders.Where(c => c.Agent == RealtorCb.SelectedIndex + 1 && (c.Clients.Id == CustomerCb.SelectedIndex + 1
                || c.Clients.Id == OwnerCb.SelectedIndex + 1)).FirstOrDefault();
                change.StatusOrder = 2;
                AppData.DB.SaveChanges();
            }
        }

        private void RealtorCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PriceTb.Text != "")
            {
                int com = (int.Parse(PriceTb.Text) * AppData.DB.Agents.First(c => c.Id == RealtorCb.SelectedIndex + 1).Share) / 100;
                CommissionTb.Text = $"{com}";
            }
        }

        private void PriceTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RealtorCb.SelectedItem != null && PriceTb.Text != "")
            {
                try
                {
                    int com = (int.Parse(PriceTb.Text) * AppData.DB.Agents.First(c => c.Id == RealtorCb.SelectedIndex + 1).Share) / 100;
                    CommissionTb.Text = $"{com}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnlyNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
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

        private void OrdersCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AppData.DB.Orders.FirstOrDefault(c => c.Id == OrdersCb.SelectedIndex + 1).TypeOrder == 2 || 
                AppData.DB.Orders.FirstOrDefault(c => c.Id == OrdersCb.SelectedIndex + 1).TypeOrder == 3)
            {
                ObjectCb.IsEnabled = false;
                ObjectCb.SelectedIndex = AppData.DB.RealEstateObjects.FirstOrDefault(c => c.IdAddress == AppData.DB.Orders.FirstOrDefault(d => d.Id 
                == OrdersCb.SelectedIndex + 1).IdAddress).Id - 1;
            }
            else
            {
                ObjectCb.IsEnabled = true;
                ObjectCb.SelectedItem = null;
            }
            RealtorCb.SelectedIndex = AppData.DB.Orders.FirstOrDefault(c => c.Agent == OrdersCb.SelectedIndex + 1).Id - 1;
            if (AppData.DB.Orders.FirstOrDefault(c => c.Id == OrdersCb.SelectedIndex + 1).TypeOrder == 3 ||
                AppData.DB.Orders.FirstOrDefault(c => c.Id == OrdersCb.SelectedIndex + 1).TypeOrder == 4) TypeDealCb.SelectedIndex = 1;
            else TypeDealCb.SelectedIndex = 0;
            if (AppData.DB.Orders.FirstOrDefault(c => c.Id == OrdersCb.SelectedIndex + 1).TypeOrder == 1 ||
                AppData.DB.Orders.FirstOrDefault(c => c.Id == OrdersCb.SelectedIndex + 1).TypeOrder == 4)
            {
                CustomerCb.IsEnabled = false;
                OwnerCb.SelectedItem = null;
                CustomerCb.SelectedIndex = AppData.DB.Orders.FirstOrDefault(c => c.Client == AppData.DB.Orders.FirstOrDefault(d => d.Id
                == OrdersCb.SelectedIndex + 1).Client).Client - 1;
            }
            else
            {
                CustomerCb.IsEnabled = true;
                CustomerCb.SelectedItem = null;
                OwnerCb.SelectedIndex = AppData.DB.Orders.FirstOrDefault(c => c.Client == AppData.DB.Orders.FirstOrDefault(d => d.Id
                == OrdersCb.SelectedIndex + 1).Client).Client - 1;
            }
        }

        private void ObjectCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OwnerCb.SelectedItem == null) 
            {
                OwnerCb.SelectedIndex = AppData.DB.RealEstateObjects.FirstOrDefault(c => c.Id == ObjectCb.SelectedIndex + 1).Owner - 1;
            }
        }
    }
}
