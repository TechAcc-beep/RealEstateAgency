using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using OfficeOpenXml;
using PdfSharp.Pdf;
using RealEstateAgency.model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using Paragraph = MigraDoc.DocumentObjectModel.Paragraph;
using Section = MigraDoc.DocumentObjectModel.Section;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;
using Table = MigraDoc.DocumentObjectModel.Tables.Table;
using Column = MigraDoc.DocumentObjectModel.Tables.Column;
using Row = MigraDoc.DocumentObjectModel.Tables.Row;
using Color = MigraDoc.DocumentObjectModel.Color;
using System.Security.Cryptography;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Drawing;
using static System.Net.Mime.MediaTypeNames;
using static System.Collections.Specialized.BitVector32;

namespace RealEstateAgency
{
    /// <summary>
    /// Логика взаимодействия для AgreementsWin.xaml
    /// </summary>
    public partial class AgreementsWin : Window
    {
        int IdUser;
        public AgreementsWin(int idUser)
        {
            InitializeComponent();
            GetData(null);
            IdUser = idUser;
        }

        private void GetData(string combobox)
        {
            AppData.DB.Contracts.Load();
            AppData.DB.Clients.Load();
            AppData.DB.Agents.Load();
            AppData.DB.RealEstateObjects.Load();
            AppData.DB.TypeContracts.Load();
            AppData.DB.TypeDeals.Load();
            if (combobox == null)
            {
                var data = AppData.DB.Contracts.Local.ToBindingList();
                ContractsDg.ItemsSource = data;
            }
            else
            {
                var data = AppData.DB.Contracts.Local.ToList();
                switch (combobox)
                {
                    case "По номуре договора":
                        data = data.Where(c => c.ContractNumber.ToString().Contains(SearchTb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                    case "По покупателю":
                        data = data.Where(c => c.Clients.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) ||
                        c.Clients.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.Clients.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                    case "По агенту":
                        data = data.Where(c => c.Agents.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) ||
                        c.Agents.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.Agents.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                    case "По недвижимости":
                        data = data.Where(c => c.RealEstateObjects.Address.Title.ToLower().Contains(SearchTb.Text.ToLower())
                        || c.RealEstateObjects.TypeRealEstateObjects.Title.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                    case "По владельцу":
                        data = data.Where(c => c.Clients1.FirstName.ToLower().Contains(SearchTb.Text.ToLower()) ||
                        c.Clients1.MiddleName.ToLower().Contains(SearchTb.Text.ToLower()) || c.Clients1.LastName.ToLower().Contains(SearchTb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                    case "По типу договора":
                        data = data.Where(c => c.TypeContracts.Title.ToLower().StartsWith(SearchCb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                    case "По типу сделки":
                        data = data.Where(c => c.TypeDeals.Title.ToLower().StartsWith(SearchCb.Text.ToLower())).ToList();
                        ContractsDg.ItemsSource = data;
                        break;
                }
            }
        }

        private void ReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditAgreementsWin addEditAgreementsWin = new AddEditAgreementsWin(null);
            addEditAgreementsWin.ShowDialog();
            ContractsDg.ItemsSource = null;
            GetData(null);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Contracts cId = ContractsDg.SelectedItem as Contracts;
            if (ContractsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (AppData.DB.Agents.First(c => c.Id == IdUser).IdRole == 2 && cId.TypeContract == 2)
            {
                MessageBox.Show($"Вы не можете удалить элемент {ContractsDg.SelectedIndex + 1}!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show($"Вы точно хотите удалить элемент {ContractsDg.SelectedIndex + 1}?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AppData.DB.Contracts.RemoveRange(AppData.DB.Contracts.Where(p => p.Id == cId.Id));
                    AppData.DB.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    ContractsDg.ItemsSource = null;
                    GetData(null);
                }
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ContractsDg.ItemsSource = null;
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
            if (ContractsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                AddEditAgreementsWin addEditAgreementsWin = new AddEditAgreementsWin(ContractsDg.SelectedItem as Contracts);
                addEditAgreementsWin.ShowDialog();
                ContractsDg.ItemsSource = null;
                GetData(null);
            }
        }

        private void SearchByCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchByCb.SelectedIndex == 5 || SearchByCb.SelectedIndex == 6)
            {
                SearchCb.Visibility = Visibility.Visible;
                SearchTb.Visibility = Visibility.Collapsed;
                switch (SearchByCb.SelectedIndex)
                {
                    case (5):
                        SearchCb.ItemsSource = AppData.DB.TypeContracts.Select(c => c.Title).ToList();
                        break;
                    case (6):
                        SearchCb.ItemsSource = AppData.DB.TypeDeals.Select(c => c.Title).ToList();
                        break;
                }
            }
            else
            {
                SearchCb.Visibility = Visibility.Collapsed;
                SearchTb.Visibility = Visibility.Visible;
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            if (ContractsDg.SelectedItem == null)
            {
                MessageBox.Show($"Выберите элемент!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Contracts cId = ContractsDg.SelectedItem as Contracts;
                if (cId.TypeDeal == 1) PurchaseAndSalePdf(cId);
                else RentPdf(cId);
                if (cId.TypeContract == 2)
                {
                    var change = AppData.DB.Orders.Where(c => c.Agent == cId.Agents.Id && (c.Client == cId.Clients.Id || c.Client == cId.Clients1.Id)).FirstOrDefault();
                    change.StatusOrder = 3;
                    AppData.DB.SaveChanges();
                }
            }
        }

        private void SavePdfFile(Document document)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файл PDF (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
#pragma warning disable CS0618
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always);
#pragma warning restore CS0618
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save($"{saveFileDialog.FileName}");
                MessageBoxResult result = MessageBox.Show("Договор успешно составлен. Открыть файл?", "Сохранено", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start($"{saveFileDialog.FileName}");
                }
            }
        }

        private void PurchaseAndSalePdf(Contracts cId)
        {
            Document document = new Document();
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;
            section.PageSetup.BottomMargin = "2cm";
            section.PageSetup.TopMargin = "2cm";
            section.PageSetup.LeftMargin = "3cm";
            section.PageSetup.RightMargin = "1.5cm";
            Paragraph title = section.AddParagraph();
            title.AddFormattedText($"Договор № {cId.ContractNumber}\nКупли-продажи недвижимости", TextFormat.Bold);
            title.Format.Alignment = ParagraphAlignment.Center;
            title.Format.Font.Size = 12;
            title.Format.Font.Name = "Times New Roman";
            Paragraph date = section.AddParagraph();
            date.AddText($"\n\n{cId.Date:D}\n\n\n");
            date.Format.Alignment = ParagraphAlignment.Right;
            date.Format.Font.Size = 12;
            date.Format.Font.Name = "Times New Roman";
            Paragraph parties = section.AddParagraph();
            parties.AddFormattedText($"{cId.Clients1.FullName}", TextFormat.Bold);
            parties.AddText(", именуемый(ая) в дальнейшем «Продавец», с одной стороны и ");
            parties.AddFormattedText($"{cId.Clients.FullName}", TextFormat.Bold);
            parties.AddText(", именуемый(ая) в дальнейшем «Покупатель», с другой стороны ");
            parties.AddFormattedText($"{cId.Agents.FullName}", TextFormat.Bold);
            parties.AddText(", именуемый(ая) в дальнейшем «Исполнитель», совместно именуемые «Стороны», заключили настоящий Договор о нижеследующем: \n");
            parties.Format.Alignment = ParagraphAlignment.Justify;
            parties.Format.FirstLineIndent = "1.25cm";
            parties.Format.Font.Size = 12;
            parties.Format.Font.Name = "Times New Roman";
            Paragraph title1 = section.AddParagraph();
            title1.AddText("\n\n1. Предмет Договора\n\n");
            title1.Format.Alignment = ParagraphAlignment.Center;
            title1.Format.Font.Size = 12;
            title1.Format.Font.Name = "Times New Roman";
            Paragraph subject1 = section.AddParagraph();
            subject1.AddText("1.1. Продавец продает, а Покупатель приобретает в собственность объект недвижимости ");
            subject1.AddFormattedText($"«{cId.RealEstateObjects.TypeRealEstateObjects.Title}»", TextFormat.Bold);
            subject1.AddText(", расположенный по адресу: ");
            subject1.AddFormattedText($"{cId.RealEstateObjects.Address.Title}", TextFormat.Bold);
            subject1.AddText(" (именуемую в дальнейшем «Недвижимость»).");
            subject1.Format.Alignment = ParagraphAlignment.Justify;
            subject1.Format.FirstLineIndent = "1.25cm";
            subject1.Format.Font.Size = 12;
            subject1.Format.Font.Name = "Times New Roman";
            switch (cId.RealEstateObjects.TypeRealEstateObjects.Title)
            {
                case "Квартира":
                    Paragraph subjectApartment2 = section.AddParagraph();
                    subjectApartment2.AddText("1.2. Указанная Недвижимость расположена на ");
                    subjectApartment2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 3).Value}", 
                        TextFormat.Bold);
                    subjectApartment2.AddText(" этаже и состоит из ");
                    subjectApartment2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 2).Value}", 
                        TextFormat.Bold);
                    subjectApartment2.AddText(" комнат(ы), имеет общую площадь ");
                    subjectApartment2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 1).Value}", 
                        TextFormat.Bold);
                    subjectApartment2.AddText(" кв. м.");
                    subjectApartment2.Format.Alignment = ParagraphAlignment.Justify;
                    subjectApartment2.Format.FirstLineIndent = "1.25cm";
                    subjectApartment2.Format.Font.Size = 12;
                    subjectApartment2.Format.Font.Name = "Times New Roman";
                    break;
                case "Дом":
                    Paragraph subjectHouse2 = section.AddParagraph();
                    subjectHouse2.AddText("1.2. Указанная Недвижимость ");
                    subjectHouse2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 4).Value}", TextFormat.Bold);
                    subjectHouse2.AddText(" этажная и состоит из ");
                    subjectHouse2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 2).Value}", TextFormat.Bold);
                    subjectHouse2.AddText(" комнат(ы), имеет общую площадь ");
                    subjectHouse2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 1).Value}", TextFormat.Bold);
                    subjectHouse2.AddText(" кв. м.");
                    subjectHouse2.Format.Alignment = ParagraphAlignment.Justify;
                    subjectHouse2.Format.FirstLineIndent = "1.25cm";
                    subjectHouse2.Format.Font.Size = 12;
                    subjectHouse2.Format.Font.Name = "Times New Roman";
                    break;
                case "Участок":
                    Paragraph subject2 = section.AddParagraph();
                    subject2.AddText("1.2. Указанная Недвижимость имеет общую площадь ");
                    subject2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 1).Value}", TextFormat.Bold);
                    subject2.AddText(" сот.");
                    subject2.Format.Alignment = ParagraphAlignment.Justify;
                    subject2.Format.FirstLineIndent = "1.25cm";
                    subject2.Format.Font.Size = 12;
                    subject2.Format.Font.Name = "Times New Roman";
                    break;
            }
            Paragraph subject3 = section.AddParagraph();
            subject3.AddText("1.3. Переход права собственности на Недвижимость подлежит государственной регистрации в соответствии со ст. 551 Гражданского кодекса Российской Федерации " +
                "и Федеральным законом от 13.07.2015 N 218-ФЗ «О государственной регистрации недвижимости».");
            subject3.Format.Alignment = ParagraphAlignment.Justify;
            subject3.Format.FirstLineIndent = "1.25cm";
            subject3.Format.Font.Size = 12;
            subject3.Format.Font.Name = "Times New Roman";
            Paragraph subject4 = section.AddParagraph();
            subject4.AddText("1.4. Продавец и Исполнитель гарантирует, что указанная в настоящем Договоре Недвижимость никому не продана, не заложена, " +
                "в споре, под арестом и запретом не состоит и свободна от законных прав третьих лиц.\n");
            subject4.Format.Alignment = ParagraphAlignment.Justify;
            subject4.Format.FirstLineIndent = "1.25cm";
            subject4.Format.Font.Size = 12;
            subject4.Format.Font.Name = "Times New Roman";
            Paragraph title2 = section.AddParagraph();
            title2.AddText("\n\n2. Стоимость Недвижимости и порядок оплаты\n\n");
            title2.Format.Alignment = ParagraphAlignment.Center;
            title2.Format.Font.Size = 12;
            title2.Format.Font.Name = "Times New Roman";
            Paragraph price1 = section.AddParagraph();
            price1.AddText("2.1. По согласованию Сторон цена продаваемой Недвижимости составляет сумму в размере ");
            price1.AddFormattedText($"{cId.Price}", TextFormat.Bold);
            price1.AddText(" рублей, также комиссия Исполнителя составляет ");
            price1.AddFormattedText($"{cId.Commission}", TextFormat.Bold);
            price1.AddText(" рублей.");
            price1.Format.Alignment = ParagraphAlignment.Justify;
            price1.Format.FirstLineIndent = "1.25cm";
            price1.Format.Font.Size = 12;
            price1.Format.Font.Name = "Times New Roman";
            Paragraph price2 = section.AddParagraph();
            price2.AddText("2.2. Оплата производится Покупателем в любой форме путем перечисления денежных средств на счет Продавца.");
            price2.Format.Alignment = ParagraphAlignment.Justify;
            price2.Format.FirstLineIndent = "1.25cm";
            price2.Format.Font.Size = 12;
            price2.Format.Font.Name = "Times New Roman";
            Paragraph price3 = section.AddParagraph();
            price3.AddText("2.3. Расходы, связанные с оформлением и регистрацией перехода права собственности, не включаются в стоимость Недвижимости и уплачиваются " +
                "Сторонами по мере необходимости и своевременно.");
            price3.Format.Alignment = ParagraphAlignment.Justify;
            price3.Format.FirstLineIndent = "1.25cm";
            price3.Format.Font.Size = 12;
            price3.Format.Font.Name = "Times New Roman";
            Paragraph title3 = section.AddParagraph();
            title3.AddText("\n\n3. Реквизиты Сторон\n\n");
            title3.Format.Alignment = ParagraphAlignment.Center;
            title3.Format.Font.Size = 12;
            title3.Format.Font.Name = "Times New Roman";
            Table requisites = section.AddTable();
            requisites.Borders.Width = 0;
            requisites.Format.Font.Size = 12;
            requisites.Format.Font.Name = "Times New Roman";
            Column column = requisites.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = requisites.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = requisites.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            Row row = requisites.AddRow();
            row.Cells[0].AddParagraph("Продавец:");
            row.Cells[1].AddParagraph("Покупатель:");
            row.Cells[2].AddParagraph("Исполнитель:");
            row.Format.SpaceAfter = 4;
            Row row1 = requisites.AddRow();
            row1.Cells[0].AddParagraph($"ФИО: {cId.Clients1.FullName}");
            row1.Cells[1].AddParagraph($"ФИО: {cId.Clients.FullName}");
            row1.Cells[2].AddParagraph($"ФИО: {cId.Agents.FullName}");
            row1.Format.SpaceAfter = 2;
            Row row2 = requisites.AddRow();
            row2.Cells[0].AddParagraph($"Телефон: {cId.Clients1.Phone}");
            row2.Cells[1].AddParagraph($"Телефон: {cId.Clients.Phone}");
            row2.Cells[2].AddParagraph($"Телефон: {cId.Agents.Phone}");
            row2.Format.SpaceAfter = 2;
            Row row3 = requisites.AddRow();
            row3.Cells[0].AddParagraph($"Эл. почта: {cId.Clients1.Email}");
            row3.Cells[1].AddParagraph($"Эл. почта: {cId.Clients.Email}");
            Row row4 = requisites.AddRow();
            row4.Cells[0].AddParagraph("\n\n____________ (подпись)");
            row4.Cells[1].AddParagraph("\n\n____________ (подпись)");
            row4.Cells[2].AddParagraph("\n\n____________ (подпись)");
            if (cId.TypeContract == 2)
            {
                AddImagePdf(section);
            }
            SavePdfFile(document);
        }

        private void RentPdf(Contracts cId)
        {
            Document document = new Document();
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Portrait;
            section.PageSetup.BottomMargin = "2cm";
            section.PageSetup.TopMargin = "2cm";
            section.PageSetup.LeftMargin = "3cm";
            section.PageSetup.RightMargin = "1.5cm";
            Paragraph title = section.AddParagraph();
            title.AddFormattedText($"Договор № {cId.ContractNumber}\nАренда недвижимости", TextFormat.Bold);
            title.Format.Alignment = ParagraphAlignment.Center;
            title.Format.Font.Size = 12;
            title.Format.Font.Name = "Times New Roman";
            Paragraph date = section.AddParagraph();
            date.AddText($"\n\n{cId.Date:D}\n\n\n");
            date.Format.Alignment = ParagraphAlignment.Right;
            date.Format.Font.Size = 12;
            date.Format.Font.Name = "Times New Roman";
            Paragraph parties = section.AddParagraph();
            parties.AddFormattedText($"{cId.Clients1.FullName}", TextFormat.Bold);
            parties.AddText(", именуемый(ая) в дальнейшем «Арендодатель», с одной стороны и ");
            parties.AddFormattedText($"{cId.Clients.FullName}", TextFormat.Bold);
            parties.AddText(", именуемый(ая) в дальнейшем «Арендатор», с другой стороны ");
            parties.AddFormattedText($"{cId.Agents.FullName}", TextFormat.Bold);
            parties.AddText(", именуемый(ая) в дальнейшем «Исполнитель», совместно именуемые «Стороны», заключили настоящий Договор о нижеследующем: \n");
            parties.Format.Alignment = ParagraphAlignment.Justify;
            parties.Format.FirstLineIndent = "1.25cm";
            parties.Format.Font.Size = 12;
            parties.Format.Font.Name = "Times New Roman";
            Paragraph title1 = section.AddParagraph();
            title1.AddText("\n\n1. Предмет Договора\n\n");
            title1.Format.Alignment = ParagraphAlignment.Center;
            title1.Format.Font.Size = 12;
            title1.Format.Font.Name = "Times New Roman";
            Paragraph subject1 = section.AddParagraph();
            subject1.AddText("1.1. Арендодатель сдает, а Арендатор арендует объект недвижимости ");
            subject1.AddFormattedText($"«{cId.RealEstateObjects.TypeRealEstateObjects.Title}»", TextFormat.Bold);
            subject1.AddText(", расположенный по адресу: ");
            subject1.AddFormattedText($"{cId.RealEstateObjects.Address.Title}", TextFormat.Bold);
            subject1.AddText(" (именуемую в дальнейшем «Недвижимость»).");
            subject1.Format.Alignment = ParagraphAlignment.Justify;
            subject1.Format.FirstLineIndent = "1.25cm";
            subject1.Format.Font.Size = 12;
            subject1.Format.Font.Name = "Times New Roman";
            switch (cId.RealEstateObjects.TypeRealEstateObjects.Title)
            {
                case "Квартира":
                    Paragraph subjectApartment2 = section.AddParagraph();
                    subjectApartment2.AddText("1.2. Указанная Недвижимость расположена на ");
                    subjectApartment2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 3).Value}", 
                        TextFormat.Bold);
                    subjectApartment2.AddText(" этаже и состоит из ");
                    subjectApartment2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 2).Value}", 
                        TextFormat.Bold);
                    subjectApartment2.AddText(" комнат(ы), имеет общую площадь ");
                    subjectApartment2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 1).Value}", 
                        TextFormat.Bold);
                    subjectApartment2.AddText(" кв. м.");
                    subjectApartment2.Format.Alignment = ParagraphAlignment.Justify;
                    subjectApartment2.Format.FirstLineIndent = "1.25cm";
                    subjectApartment2.Format.Font.Size = 12;
                    subjectApartment2.Format.Font.Name = "Times New Roman";
                    break;
                case "Дом":
                    Paragraph subjectHouse2 = section.AddParagraph();
                    subjectHouse2.AddText("1.2. Указанная Недвижимость ");
                    subjectHouse2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 4).Value}", TextFormat.Bold);
                    subjectHouse2.AddText(" этажная и состоит из ");
                    subjectHouse2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 2).Value}", TextFormat.Bold);
                    subjectHouse2.AddText(" комнат(ы), имеет общую площадь ");
                    subjectHouse2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 1).Value}", TextFormat.Bold);
                    subjectHouse2.AddText(" кв. м.");
                    subjectHouse2.Format.Alignment = ParagraphAlignment.Justify;
                    subjectHouse2.Format.FirstLineIndent = "1.25cm";
                    subjectHouse2.Format.Font.Size = 12;
                    subjectHouse2.Format.Font.Name = "Times New Roman";
                    break;
                case "Участок":
                    Paragraph subject2 = section.AddParagraph();
                    subject2.AddText("1.2. Указанная Недвижимость имеет общую площадь ");
                    subject2.AddFormattedText($"{AppData.DB.AttributesRealEstateObjects.First(c => c.IdObject == cId.RealEstateObject && c.IdAttributesName == 1).Value}", TextFormat.Bold);
                    subject2.AddText(" сот.");
                    subject2.Format.Alignment = ParagraphAlignment.Justify;
                    subject2.Format.FirstLineIndent = "1.25cm";
                    subject2.Format.Font.Size = 12;
                    subject2.Format.Font.Name = "Times New Roman";
                    break;
            }
            Paragraph subject3 = section.AddParagraph();
            subject3.AddText("1.3. Переход права собственности на Недвижимость подлежит государственной регистрации в соответствии со ст. 551 Гражданского кодекса Российской Федерации " +
                "и Федеральным законом от 13.07.2015 N 218-ФЗ «О государственной регистрации недвижимости».");
            subject3.Format.Alignment = ParagraphAlignment.Justify;
            subject3.Format.FirstLineIndent = "1.25cm";
            subject3.Format.Font.Size = 12;
            subject3.Format.Font.Name = "Times New Roman";
            Paragraph subject4 = section.AddParagraph();
            subject4.AddText("1.4. Арендодатель и Исполнитель гарантирует, что указанная в настоящем Договоре Недвижимость никому не продана, не заложена, " +
                "в споре, под арестом и запретом не состоит и свободна от законных прав третьих лиц.\n");
            subject4.Format.Alignment = ParagraphAlignment.Justify;
            subject4.Format.FirstLineIndent = "1.25cm";
            subject4.Format.Font.Size = 12;
            subject4.Format.Font.Name = "Times New Roman";
            Paragraph title2 = section.AddParagraph();
            title2.AddText("\n\n2. Стоимость Недвижимости и порядок оплаты\n\n");
            title2.Format.Alignment = ParagraphAlignment.Center;
            title2.Format.Font.Size = 12;
            title2.Format.Font.Name = "Times New Roman";
            Paragraph price1 = section.AddParagraph();
            price1.AddText("2.1. По согласованию Сторон цена продаваемой Недвижимости составляет сумму в размере ");
            price1.AddFormattedText($"{cId.Price}", TextFormat.Bold);
            price1.AddText(" рублей, также комиссия Исполнителя составляет ");
            price1.AddFormattedText($"{cId.Commission}", TextFormat.Bold);
            price1.AddText(" рублей.");
            price1.Format.Alignment = ParagraphAlignment.Justify;
            price1.Format.FirstLineIndent = "1.25cm";
            price1.Format.Font.Size = 12;
            price1.Format.Font.Name = "Times New Roman";
            Paragraph price2 = section.AddParagraph();
            price2.AddText("2.2. Оплата производится Арендатором в любой форме путем перечисления денежных средств на счет Арендодателя.");
            price2.Format.Alignment = ParagraphAlignment.Justify;
            price2.Format.FirstLineIndent = "1.25cm";
            price2.Format.Font.Size = 12;
            price2.Format.Font.Name = "Times New Roman";
            Paragraph price3 = section.AddParagraph();
            price3.AddText("2.3. Расходы, связанные с оформлением и регистрацией перехода права собственности, не включаются в стоимость Недвижимости и уплачиваются " +
                "Сторонами по мере необходимости и своевременно.");
            price3.Format.Alignment = ParagraphAlignment.Justify;
            price3.Format.FirstLineIndent = "1.25cm";
            price3.Format.Font.Size = 12;
            price3.Format.Font.Name = "Times New Roman";
            Paragraph title3 = section.AddParagraph();
            title3.AddText("\n\n3. Реквизиты Сторон\n\n");
            title3.Format.Alignment = ParagraphAlignment.Center;
            title3.Format.Font.Size = 12;
            title3.Format.Font.Name = "Times New Roman";
            Table requisites = section.AddTable();
            requisites.Borders.Width = 0;
            requisites.Format.Font.Size = 12;
            requisites.Format.Font.Name = "Times New Roman";
            Column column = requisites.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = requisites.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = requisites.AddColumn("5.5cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            Row row = requisites.AddRow();
            row.Cells[0].AddParagraph("Арендодатель:");
            row.Cells[1].AddParagraph("Арендатор:");
            row.Cells[2].AddParagraph("Исполнитель:");
            row.Format.SpaceAfter = 4;
            Row row1 = requisites.AddRow();
            row1.Cells[0].AddParagraph($"ФИО: {cId.Clients1.FullName}");
            row1.Cells[1].AddParagraph($"ФИО: {cId.Clients.FullName}");
            row1.Cells[2].AddParagraph($"ФИО: {cId.Agents.FullName}");
            row1.Format.SpaceAfter = 2;
            Row row2 = requisites.AddRow();
            row2.Cells[0].AddParagraph($"Телефон: {cId.Clients1.Phone}");
            row2.Cells[1].AddParagraph($"Телефон: {cId.Clients.Phone}");
            row2.Cells[2].AddParagraph($"Телефон: {cId.Agents.Phone}");
            row2.Format.SpaceAfter = 2;
            Row row3 = requisites.AddRow();
            row3.Cells[0].AddParagraph($"Эл. почта: {cId.Clients1.Email}");
            row3.Cells[1].AddParagraph($"Эл. почта: {cId.Clients.Email}");
            Row row4 = requisites.AddRow();
            row4.Cells[0].AddParagraph("\n\n____________ (подпись)");
            row4.Cells[1].AddParagraph("\n\n____________ (подпись)");
            row4.Cells[2].AddParagraph("\n\n____________ (подпись)");
            if (cId.TypeContract == 2)
            {
                AddImagePdf(section);
            }
            SavePdfFile(document);
        }

        private void AddImagePdf(Section section)
        {
            Image image = section.Footers.Primary.AddImage("../../icon/stamp.png");
            image.Height = "4cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.TopBottom;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
        }
    }
}
