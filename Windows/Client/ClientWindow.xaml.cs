using RealEstateProject.Database;
using RealEstateProject.Windows.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RealEstateProject.Windows.Client
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        Entities entity = new Entities();
        public List<Clients> clients;
        public int maxi;
        public int currentPage = 1, countPage = 100;
        public int page;
        public ClientWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обновление DataGrid, свежими данными из базы данных
        /// </summary>
        private void RefreshTable()
        {
            clientsDG.ItemsSource = entity.Clients.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Вы точно хотите удалить данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message.Equals(MessageBoxResult.Yes))
            {
                var selected = clientsDG.SelectedItem as Clients;
                if (selected != null)
                {
                    if (entity.HouseDemands.Any(x => x.ClientId == selected.Id)
                    || entity.ApartmentDemands.Any(x => x.ClientId == selected.Id)
                    || entity.LandDemands.Any(x => x.ClientId == selected.Id)
                    )
                    {
                        MessageBox.Show("Нельзя удалить клиента, связанного с потребностью или предложением", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        entity.Clients.Remove(selected);
                        entity.SaveChanges();
                        MessageBox.Show("Данные были успешно удалены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshTable();
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали строку для удаления!","Уведомление",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Вы точно хотите редактировать данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message.Equals(MessageBoxResult.Yes))
            {
                var selected = clientsDG.SelectedItem as Clients;
                if (selected != null)
                {
                    var edit = new EditClientWindows(true, selected);
                    edit.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы не выбрали строку для редактирования!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var add = new EditClientWindows(false, null);
            add.Show();
            this.Close();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void FindLineTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<Clients> product = clients;
                List<Clients> filteredRecords = new List<Clients>();
                if (FindLineTextBox.Text.Length == 0)
                {
                    filteredRecords = entity.Clients.ToList();
                }
                else
                {
                    string searchText = FindLineTextBox.Text;
                    filteredRecords =
                    product.Where(r => r.FirstName.ToLower().StartsWith(searchText.ToLower()) |
                    r.MiddleName.ToString().ToLower().StartsWith(searchText.ToLower()) |
                    r.LastName.ToString().ToLower().StartsWith(searchText.ToLower()) |
                    r.Email.ToString().ToLower().StartsWith(searchText.ToLower()) |
                    r.Phone.ToString().ToLower().StartsWith(searchText.ToLower())).ToList();
                }
                clients = filteredRecords.ToList();
                if (clients.Count == 0)
                {
                    MessageBox.Show("По результату поиска ничего не найдено!");
                }
                else
                {
                    AutoLoad();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Функционал по редактированию данных
        /// </summary>
        private void ChangeButton_Click_1(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Вы точно хотите редактировать данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message.Equals(MessageBoxResult.Yes))
            {
                var selected = clientsDG.SelectedItem as Agents;
                if (selected != null)
                {
                    var edit = new EditAgentWindows(true, selected);
                    edit.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы не выбрали строку для редактирования!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        /// <summary>
        /// Функционал автозагрузке данных
        /// </summary>
        public void AutoLoad()
        {
            List<Clients> List = clients;
            List<Clients> currentPagesList;
            maxi = clients.Count() / countPage;
            if (page == 0)
            {
                currentPagesList = List.Skip(0).Take(countPage).ToList();
            }
            else
            {
                currentPagesList = List.Skip(page * countPage - countPage).Take(countPage).ToList();
            }
            clientsDG.ItemsSource = currentPagesList;
        }
    }
}
