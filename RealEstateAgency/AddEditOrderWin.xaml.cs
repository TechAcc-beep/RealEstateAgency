using RealEstateAgency.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для AddEditOrderWin.xaml
    /// </summary>
    public partial class AddEditOrderWin : Window
    {
        Orders Order;
        double minArea, maxArea, minRooms, maxRooms, minFloor, maxFloor, minAmountFloors, maxAmountFloors;
        int minPrice, maxPrice;

        public AddEditOrderWin(Orders orders)
        {
            InitializeComponent();
            Order = orders;
            ClientCb.ItemsSource = AppData.DB.Clients.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            RealtorCb.ItemsSource = AppData.DB.Agents.Select(c => c.LastName + " " + c.FirstName + " " + c.MiddleName).ToList();
            TypeCb.ItemsSource = AppData.DB.TypeRealEstateObjects.Select(c => c.Title).ToList();
            TypeOrderCb.ItemsSource = AppData.DB.TypeOrders.Select(c => c.Title).ToList();
            if (orders == null)
            {
                this.Title = "Добавление";
            }
            else
            {
                this.Title = "Изменение";
                ClientCb.SelectedIndex = orders.Client - 1;
                RealtorCb.SelectedIndex = orders.Agent - 1;
                TypeOrderCb.SelectedIndex = orders.TypeOrder - 1;
                TypeOrderCb.IsEnabled = false;
                TypeCb.SelectedIndex = orders.TypeRealEstate - 1;
                TypeCb.IsEnabled = false;
                AddressTb.Text = orders.Address.Title;
                MinPriceTb.Text = orders.MinPrice.ToString();
                MaxPriceTb.Text = orders.MaxPrice.ToString();
                if (TypeOrderCb.SelectedIndex == 1 || TypeOrderCb.SelectedIndex == 2)
                {
                    switch (TypeCb.SelectedIndex)
                    {
                        case 0:
                            AreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 1).Value.ToString();
                            RoomsTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 2).Value.ToString();
                            FloorTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 3).Value.ToString();
                            break;
                        case 1:
                            AreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 1).Value.ToString();
                            RoomsTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 2).Value.ToString();
                            NumOfStoreysTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 4).Value.ToString();
                            break;
                        case 2:
                            AreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 1).Value.ToString();
                            break;
                    }
                }
                else
                {
                    switch (TypeCb.SelectedIndex)
                    {
                        case 0:
                            MinAreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 5).Value.ToString();
                            MaxAreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 6).Value.ToString();
                            MinRoomsTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 7).Value.ToString();
                            MaxRoomsTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 8).Value.ToString();
                            MinFloorTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 9).Value.ToString();
                            MaxFloorTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 10).Value.ToString();
                            break;
                        case 1:
                            MinAreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 5).Value.ToString();
                            MaxAreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 6).Value.ToString();
                            MinRoomsTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 7).Value.ToString();
                            MaxRoomsTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 8).Value.ToString();
                            MinNumOfStoreysTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 11).Value.ToString();
                            MaxNumOfStoreysTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 12).Value.ToString();
                            break;
                        case 2:
                            MinAreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 5).Value.ToString();
                            MaxAreaTb.Text = AppData.DB.AttributesOrders.First(c => c.IdOrder == orders.Id && c.IdAttributesName == 6).Value.ToString();
                            break;
                    }
                }
            }
        }

        private void TypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeOrderCb.SelectedIndex == 0 || TypeOrderCb.SelectedIndex == 3)
            {
                if (TypeCb.SelectedIndex == 0)
                {
                    MinAreaTb.IsEnabled = true;
                    MaxAreaTb.IsEnabled = true;
                    MinRoomsTb.IsEnabled = true;
                    MaxRoomsTb.IsEnabled = true;
                    MinFloorTb.IsEnabled = true;
                    MaxFloorTb.IsEnabled = true;
                    MinNumOfStoreysTb.IsEnabled = false;
                    MaxNumOfStoreysTb.IsEnabled = false;
                    MinNumOfStoreysTb.Text = string.Empty;
                    MaxNumOfStoreysTb.Text = string.Empty;
                }
                else if (TypeCb.SelectedIndex == 1)
                {
                    MinAreaTb.IsEnabled = true;
                    MaxAreaTb.IsEnabled = true;
                    MinRoomsTb.IsEnabled = true;
                    MaxRoomsTb.IsEnabled = true;
                    MinNumOfStoreysTb.IsEnabled = true;
                    MaxNumOfStoreysTb.IsEnabled = true;
                    MinFloorTb.IsEnabled = false;
                    MaxFloorTb.IsEnabled = false;
                    MinFloorTb.Text = string.Empty;
                    MaxFloorTb.Text = string.Empty;
                }
                else
                {
                    MinAreaTb.IsEnabled = true;
                    MaxAreaTb.IsEnabled = true;
                    MinRoomsTb.IsEnabled = false;
                    MaxRoomsTb.IsEnabled = false;
                    MinFloorTb.IsEnabled = false;
                    MaxFloorTb.IsEnabled = false;
                    MinNumOfStoreysTb.IsEnabled = false;
                    MaxNumOfStoreysTb.IsEnabled = false;
                    MinRoomsTb.Text = string.Empty;
                    MaxRoomsTb.Text = string.Empty;
                    MinFloorTb.Text = string.Empty;
                    MaxFloorTb.Text = string.Empty;
                    MinNumOfStoreysTb.Text = string.Empty;
                    MaxNumOfStoreysTb.Text = string.Empty;
                }
            }
            else
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
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ClientCb.SelectedValue == null)
            {
                MessageBox.Show($"Не выбран клиент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (RealtorCb.SelectedValue == null)
            {
                MessageBox.Show($"Не выбран риелтор!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (TypeCb.SelectedValue == null)
            {
                MessageBox.Show($"Не выбран тип объекта!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (TypeOrderCb.SelectedValue == null)
            {
                MessageBox.Show($"Не выбран тип заявки!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (AddressTb.Text == "")
            {
                MessageBox.Show($"Адрес не заполнен!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                AddressTb.BorderBrush = Brushes.Red;
            }
            else
            {
                if (TypeOrderCb.SelectedIndex == 1 || TypeOrderCb.SelectedIndex == 2)
                {
                    if (MinPriceTb.Text == "") minPrice = 0; else minPrice = int.Parse(MinPriceTb.Text);
                    if (MaxPriceTb.Text == "") maxPrice = 0; else maxPrice = int.Parse(MaxPriceTb.Text);
                    if (AreaTb.Text == "")
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
                        if (Order == null)
                        {
                            int maxIdObj = AppData.DB.RealEstateObjects.Count() + 1;
                            int maxIdAdr = AppData.DB.Address.Count() + 1;
                            int maxIdOrd = AppData.DB.Orders.Count() + 1;
                            Address add = new Address
                            {
                                Id = maxIdAdr,
                                Title = AddressTb.Text
                            };
                            AppData.DB.Address.Add(add);
                            RealEstateObjects addObj = new RealEstateObjects
                            {
                                Id = maxIdObj,
                                IdAddress = maxIdAdr,
                                TypeEstate = TypeCb.SelectedIndex + 1,
                                Owner = ClientCb.SelectedIndex + 1,
                                Description = ""
                            };
                            AppData.DB.RealEstateObjects.Add(addObj);
                            int maxIdAtt = AppData.DB.AttributesRealEstateObjects.Count() + 1;
                            int maxIdAtt2 = AppData.DB.AttributesOrders.Count() + 1;
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
                                    AttributesOrders addApart1 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 1,
                                        Value = double.Parse(AreaTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(addApart1);
                                    AttributesOrders addApart2 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2 + 1,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 2,
                                        Value = double.Parse(RoomsTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(addApart2);
                                    AttributesOrders addApart3 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2 + 2,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 3,
                                        Value = double.Parse(FloorTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(addApart3);
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
                                    AttributesOrders addHome1 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 1,
                                        Value = double.Parse(AreaTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(addHome1);
                                    AttributesOrders addHome2 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2 + 1,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 2,
                                        Value = double.Parse(RoomsTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(addHome2);
                                    AttributesOrders addHome3 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2 + 2,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 4,
                                        Value = double.Parse(NumOfStoreysTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(addHome3);
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
                                    AttributesOrders add2 = new AttributesOrders
                                    {
                                        Id = maxIdAtt2,
                                        IdOrder = maxIdOrd,
                                        IdAttributesName = 1,
                                        Value = double.Parse(AreaTb.Text)
                                    };
                                    AppData.DB.AttributesOrders.Add(add2);
                                    break;
                            }
                            Orders addOrd = new Orders
                            {
                                Id = maxIdOrd,
                                StatusOrder = 1,
                                Client = ClientCb.SelectedIndex + 1,
                                Agent = RealtorCb.SelectedIndex + 1,
                                TypeRealEstate = TypeCb.SelectedIndex + 1,
                                IdAddress = maxIdAdr,
                                MinPrice = minPrice,
                                MaxPrice = maxPrice,
                                TypeOrder = TypeOrderCb.SelectedIndex + 1
                            };
                            AppData.DB.Orders.Add(addOrd);
                            AppData.DB.SaveChanges();
                            MessageBox.Show("Запись добавлена");
                            Close();
                        }
                        else
                        {
                            var editOrd = AppData.DB.Orders.Where(p => p.Id == Order.Id).FirstOrDefault();
                            editOrd.Client = ClientCb.SelectedIndex + 1;
                            editOrd.Agent = RealtorCb.SelectedIndex + 1;
                            editOrd.MinPrice = minPrice;
                            editOrd.MaxPrice = maxPrice;
                            editOrd.TypeOrder = TypeOrderCb.SelectedIndex + 1;
                            var editAdd = AppData.DB.Address.Where(p => p.Id == Order.IdAddress).FirstOrDefault();
                            editAdd.Title = AddressTb.Text;
                            var editRealEstate = AppData.DB.RealEstateObjects.Where(p => p.IdAddress == Order.IdAddress).FirstOrDefault();
                            editRealEstate.Owner = ClientCb.SelectedIndex + 1;
                            switch (TypeCb.SelectedIndex)
                            {
                                case 0:
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 2).Value = int.Parse(RoomsTb.Text);
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 3).Value = int.Parse(FloorTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress 
                                    && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress && 
                                    c.IdAttributesName == 2).Value = int.Parse(RoomsTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress && 
                                    c.IdAttributesName == 3).Value = int.Parse(FloorTb.Text);
                                    break;
                                case 1:
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 2).Value = int.Parse(RoomsTb.Text);
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 4).Value = int.Parse(NumOfStoreysTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress && 
                                    c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress && 
                                    c.IdAttributesName == 2).Value = int.Parse(RoomsTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress && 
                                    c.IdAttributesName == 4).Value = int.Parse(NumOfStoreysTb.Text);
                                    break;
                                case 2:
                                    AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                    AppData.DB.AttributesRealEstateObjects.First(c => c.RealEstateObjects.IdAddress == Order.IdAddress && 
                                    c.IdAttributesName == 1).Value = double.Parse(AreaTb.Text);
                                    break;
                            }
                            AppData.DB.SaveChanges();
                            MessageBox.Show("Изменения были внесены");
                            Close();
                        }
                    }
                }
                else
                {
                    if (MinPriceTb.Text == "") minPrice = 0; else minPrice = int.Parse(MinPriceTb.Text);
                    if (MaxPriceTb.Text == "") maxPrice = 0; else maxPrice = int.Parse(MaxPriceTb.Text);
                    if (MinAreaTb.Text == "") minArea = 0; else minArea = double.Parse(MinAreaTb.Text);
                    if (MaxAreaTb.Text == "") maxArea = 0; else maxArea = double.Parse(MaxAreaTb.Text);
                    if (MinRoomsTb.Text == "") minRooms = 0; else minRooms = double.Parse(MinRoomsTb.Text);
                    if (MaxRoomsTb.Text == "") maxRooms = 0; else maxRooms = double.Parse(MaxRoomsTb.Text);
                    if (MinFloorTb.Text == "") minFloor = 0; else minFloor = double.Parse(MinFloorTb.Text);
                    if (MaxFloorTb.Text == "") maxFloor = 0; else maxFloor = double.Parse(MaxFloorTb.Text);
                    if (MinNumOfStoreysTb.Text == "") minAmountFloors = 0; else minAmountFloors = double.Parse(MinNumOfStoreysTb.Text);
                    if (MaxNumOfStoreysTb.Text == "") maxAmountFloors = 0; else maxAmountFloors = double.Parse(MaxNumOfStoreysTb.Text);
                    if (Order == null)
                    {
                        int maxIdOrd = AppData.DB.Orders.Count() + 1;
                        int maxIdAdr = AppData.DB.Address.Count() + 1;
                        Address add = new Address
                        {
                            Id = maxIdAdr,
                            Title = AddressTb.Text
                        };
                        AppData.DB.Address.Add(add);
                        Orders addOrd = new Orders
                        {
                            Id = maxIdOrd,
                            StatusOrder = 1,
                            Client = ClientCb.SelectedIndex + 1,
                            Agent = RealtorCb.SelectedIndex + 1,
                            TypeRealEstate = TypeCb.SelectedIndex + 1,
                            IdAddress = maxIdAdr,
                            MinPrice = minPrice,
                            MaxPrice = maxPrice,
                            TypeOrder = TypeOrderCb.SelectedIndex + 1
                        };
                        AppData.DB.Orders.Add(addOrd);
                        int maxIdAtt = AppData.DB.AttributesOrders.Count() + 1;
                        switch (TypeCb.SelectedIndex)
                        {
                            case 0:
                                AttributesOrders addApartment1 = new AttributesOrders
                                {
                                    Id = maxIdAtt,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 5,
                                    Value = minArea
                                };
                                AppData.DB.AttributesOrders.Add(addApartment1);
                                AttributesOrders addApartment2 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 1,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 6,
                                    Value = maxArea
                                };
                                AppData.DB.AttributesOrders.Add(addApartment2);
                                AttributesOrders addApartment3 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 2,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 7,
                                    Value = minRooms
                                };
                                AppData.DB.AttributesOrders.Add(addApartment3);
                                AttributesOrders addApartment4 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 3,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 8,
                                    Value = maxRooms
                                };
                                AppData.DB.AttributesOrders.Add(addApartment4);
                                AttributesOrders addApartment5 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 4,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 9,
                                    Value = minFloor
                                };
                                AppData.DB.AttributesOrders.Add(addApartment5);
                                AttributesOrders addApartment6 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 5,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 10,
                                    Value = maxFloor
                                };
                                AppData.DB.AttributesOrders.Add(addApartment6);
                                break;
                            case 1:
                                AttributesOrders addHouse1 = new AttributesOrders
                                {
                                    Id = maxIdAtt,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 5,
                                    Value = minArea
                                };
                                AppData.DB.AttributesOrders.Add(addHouse1);
                                AttributesOrders addHouse2 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 1,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 6,
                                    Value = maxArea
                                };
                                AppData.DB.AttributesOrders.Add(addHouse2);
                                AttributesOrders addHouse3 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 2,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 7,
                                    Value = minRooms
                                };
                                AppData.DB.AttributesOrders.Add(addHouse3);
                                AttributesOrders addHouse4 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 3,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 8,
                                    Value = maxArea
                                };
                                AppData.DB.AttributesOrders.Add(addHouse4);
                                AttributesOrders addHouse5 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 4,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 11,
                                    Value = minAmountFloors
                                };
                                AppData.DB.AttributesOrders.Add(addHouse5);
                                AttributesOrders addHouse6 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 5,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 12,
                                    Value = maxAmountFloors
                                };
                                AppData.DB.AttributesOrders.Add(addHouse6);
                                break;
                            case 2:
                                AttributesOrders add1 = new AttributesOrders
                                {
                                    Id = maxIdAtt,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 5,
                                    Value = minArea
                                };
                                AppData.DB.AttributesOrders.Add(add1);
                                AttributesOrders add2 = new AttributesOrders
                                {
                                    Id = maxIdAtt + 1,
                                    IdOrder = maxIdOrd,
                                    IdAttributesName = 6,
                                    Value = maxArea
                                };
                                AppData.DB.AttributesOrders.Add(add2);
                                break;
                        }
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Запись добавлена");
                        Close();
                    }
                    else
                    {
                        var editOrd = AppData.DB.Orders.Where(p => p.Id == Order.Id).FirstOrDefault();
                        editOrd.Client = ClientCb.SelectedIndex + 1;
                        editOrd.Agent = RealtorCb.SelectedIndex + 1;
                        editOrd.MinPrice = minPrice;
                        editOrd.MaxPrice = maxPrice;
                        editOrd.TypeOrder = TypeOrderCb.SelectedIndex + 1;
                        var editAdd = AppData.DB.Address.Where(p => p.Id == Order.IdAddress).FirstOrDefault();
                        editAdd.Title = AddressTb.Text;
                        switch (TypeCb.SelectedIndex)
                        {
                            case 0:
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 5).Value = minArea;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 6).Value = maxArea;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 7).Value = minRooms;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 8).Value = maxRooms;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 9).Value = minFloor;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 10).Value = maxFloor;
                                break;
                            case 1:
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 5).Value = minArea;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 6).Value = maxArea;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 7).Value = minRooms;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 8).Value = maxRooms;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 11).Value = minAmountFloors;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 12).Value = maxAmountFloors;
                                break;
                            case 2:
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 5).Value = minArea;
                                AppData.DB.AttributesOrders.First(c => c.IdOrder == Order.Id && c.IdAttributesName == 6).Value = maxArea;
                                break;
                        }
                        AppData.DB.SaveChanges();
                        MessageBox.Show("Изменения были внесены");
                        Close();
                    }
                }
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
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ",") && (!MinAreaTb.Text.Contains(",") && MinAreaTb.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }

        private void TypeOrderCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeOrderCb.SelectedIndex == 0 || TypeOrderCb.SelectedIndex == 3)
            {
                Attributs1.Visibility = Visibility.Visible;
                Attributs2.Visibility = Visibility.Collapsed;
            }
            else
            {
                Attributs1.Visibility = Visibility.Collapsed;
                Attributs2.Visibility = Visibility.Visible;
            }
            TypeCb.SelectedItem = null;
        }
    }
}
