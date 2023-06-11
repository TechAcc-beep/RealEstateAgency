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
    /// Логика взаимодействия для AddEditRealEstateWin.xaml
    /// </summary>
    public partial class AddEditRealEstateWin : Window
    {
        RealEstateObjects REObjects;
        public AddEditRealEstateWin(RealEstateObjects realEstateObjects)
        {
            InitializeComponent();
            REObjects = realEstateObjects;
            TypeCb.ItemsSource = AppData.DB.TypeRealEstateObjects.Select(c => c.Title).ToList();
            OwnerCb.ItemsSource = AppData.DB.Clients.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            if (realEstateObjects == null)
            {
                this.Title = "Добавление";
            }
            else
            {
                this.Title = "Изменение";
                TypeCb.SelectedIndex = realEstateObjects.TypeEstate - 1;
                TypeCb.IsEnabled = false;
                AddressTb.Text = realEstateObjects.Address.Title;
                OwnerCb.SelectedIndex = realEstateObjects.Owner - 1;
                DescriptionTb.Text = realEstateObjects.Description;
                switch (TypeCb.SelectedIndex)
                {
                    case 0:
                        AreaTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 1).Value.ToString();
                        RoomsTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 2).Value.ToString();
                        FloorTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 3).Value.ToString();
                        break;
                    case 1:
                        AreaTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 1).Value.ToString();
                        RoomsTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 2).Value.ToString();
                        NumOfStoreysTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 4).Value.ToString();
                        break;
                    case 2:
                        AreaTb.Text = AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == realEstateObjects.Id && c.IdAttributesName == 1).Value.ToString();
                        break;
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TypeCb.SelectedValue == null)
            {
                MessageBox.Show($"Не выбран тип объекта!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (OwnerCb.SelectedValue == null)
            {
                MessageBox.Show($"Не выбран владелец!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (AddressTb.Text == "")
            {
                MessageBox.Show($"Адрес не заполнен!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                AddressTb.BorderBrush = Brushes.Red;
            }
            else if (AreaTb.Text == "")
            {
                MessageBox.Show($"Площадь объекта не заполнена!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                AreaTb.BorderBrush = Brushes.Red;
            }
            else if ((TypeCb.SelectedIndex == 0 || TypeCb.SelectedIndex == 1) && RoomsTb.Text == "")
            {
                MessageBox.Show($"Количество комнат не указано!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                RoomsTb.BorderBrush = Brushes.Red;
            }
            else if (TypeCb.SelectedIndex == 0 && FloorTb.Text == "")
            {
                MessageBox.Show($"Этаж не указан!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                FloorTb.BorderBrush = Brushes.Red;
            }
            else if (TypeCb.SelectedIndex == 1 && NumOfStoreysTb.Text == "")
            {
                MessageBox.Show($"Этажность не указана!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                NumOfStoreysTb.BorderBrush = Brushes.Red;
            }
            else
            {
                try
                {
                    if (REObjects == null)
                    {

                        int maxIdObj = AppData.DB.RealEstateObjects.Count() + 1;
                        int maxIdAdr = AppData.DB.Address.Count() + 1;
                        Address add = new Address
                        {
                            Id = maxIdAdr,
                            Title = AddressTb.Text
                        };
                        RealEstateObjects addObj = new RealEstateObjects
                        {
                            Id = maxIdObj,
                            IdAddress = maxIdAdr,
                            TypeEstate = TypeCb.SelectedIndex + 1,
                            Owner = OwnerCb.SelectedIndex + 1,
                            Description = DescriptionTb.Text,
                        };
                        int maxIdAtt = AppData.DB.AttributesRealEstateObjects.Count() + 1;
                        switch (TypeCb.SelectedIndex)
                        {
                            case 0:
                                AttributesRealEstateObjects addApartment1 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 1,
                                    Value = double.Parse(AreaTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(addApartment1);
                                AttributesRealEstateObjects addApartment2 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt + 1,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 2,
                                    Value = double.Parse(RoomsTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(addApartment2);
                                AttributesRealEstateObjects addApartment3 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt + 2,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 3,
                                    Value = double.Parse(FloorTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(addApartment3);
                                break;
                            case 1:
                                AttributesRealEstateObjects addHouse1 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 1,
                                    Value = double.Parse(AreaTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(addHouse1);
                                AttributesRealEstateObjects addHouse2 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt + 1,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 2,
                                    Value = double.Parse(RoomsTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(addHouse2);
                                AttributesRealEstateObjects addHouse3 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt + 2,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 4,
                                    Value = double.Parse(NumOfStoreysTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(addHouse3);
                                break;
                            case 2:
                                AttributesRealEstateObjects add1 = new AttributesRealEstateObjects
                                {
                                    Id = maxIdAtt,
                                    IdObject = maxIdObj,
                                    IdAttributesName = 1,
                                    Value = double.Parse(AreaTb.Text)
                                };
                                AppData.DB.AttributesRealEstateObjects.Add(add1);
                                break;
                        }
                        AppData.DB.Address.Add(add);
                        AppData.DB.RealEstateObjects.Add(addObj);
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Запись добавлена");
                        Close();
                    }
                    else
                    {
                        var editObj = AppData.DB.RealEstateObjects.Where(p => p.Id == REObjects.Id).FirstOrDefault();
                        editObj.Description = DescriptionTb.Text;
                        editObj.Owner = OwnerCb.SelectedIndex + 1;
                        var editAdd = AppData.DB.Address.Where(p => p.Id == REObjects.IdAddress).FirstOrDefault();
                        editAdd.Title = AddressTb.Text;
                        switch (TypeCb.SelectedIndex)
                        {
                            case 0:
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 2).Value = int.Parse(RoomsTb.Text);
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 3).Value = int.Parse(FloorTb.Text);
                                break;
                            case 1:
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 2).Value = int.Parse(RoomsTb.Text);
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 4).Value = int.Parse(NumOfStoreysTb.Text);
                                break;
                            case 2:
                                AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == REObjects.Id && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                break;
                        }
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Изменения были внесены");
                        Close();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeCb.SelectedIndex == 0)
            {
                AreaTb.IsEnabled = true;
                RoomsTb.IsEnabled = true;
                FloorTb.IsEnabled = true;
                NumOfStoreysTb.IsEnabled = false;
                NumOfStoreysTb.Text = string.Empty;
            }
            else if (TypeCb.SelectedIndex == 1)
            {
                AreaTb.IsEnabled = true;
                RoomsTb.IsEnabled = true;
                NumOfStoreysTb.IsEnabled = true;
                FloorTb.IsEnabled = false;
                FloorTb.Text = string.Empty;
            }
            else
            {
                AreaTb.IsEnabled = true;
                RoomsTb.IsEnabled = false;
                FloorTb.IsEnabled = false;
                NumOfStoreysTb.IsEnabled = false;
                RoomsTb.Text = string.Empty;
                FloorTb.Text = string.Empty;
                NumOfStoreysTb.Text = string.Empty;
            }
        }

        private void SpaceBan_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void NumberAndVirgule_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ",") && (!AreaTb.Text.Contains(",") && AreaTb.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }
    }
}
