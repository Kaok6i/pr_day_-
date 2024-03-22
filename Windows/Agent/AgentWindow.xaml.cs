using RealEstateProject.Database;
using RealEstateProject.Windows.Editing;
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

namespace RealEstateProject.Windows.Agent
{
    /// <summary>
    /// Логика взаимодействия для AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow : Window
    {
        public AgentWindow()
        {
            InitializeComponent();
        }
        Entities entity = new Entities();
        public List<Agents> agents;
        public int maxi;
        public int currentPage = 1, countPage = 20;
        public int page;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            agentsDG.ItemsSource = entity.Agents.ToList();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var add = new EditAgentWindows(false, null);
            add.Show();
            this.Close();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Вы точно хотите удалить данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message.Equals(MessageBoxResult.Yes))
            {
                var selected = agentsDG.SelectedItem as Agents;
                if (selected != null)
                {
                    if (entity.HouseDemands.Any(x => x.AgentID == selected.Id)
                    || entity.ApartmentDemands.Any(x => x.AgentID == selected.Id)
                    || entity.LandDemands.Any(x => x.AgentID == selected.Id)
                    )
                    {
                        MessageBox.Show("Нельзя удалить риэлтора, связанного с потребностью или предложением", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        entity.Agents.Remove(selected);
                        entity.SaveChanges();
                        MessageBox.Show("Данные были успешно удалены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshTable();
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали строку для удаления!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        /// <summary>
        /// Обновление DataGrid, свежими данными из базы данных
        /// </summary>
        private void RefreshTable()
        {
            agentsDG.ItemsSource = entity.Agents.ToList();
        }

        private void FindLineTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                List<Agents> product = agents;
                List<Agents> filteredRecords = new List<Agents>();
                if (FindLineTextBox.Text.Length == 0)
                {
                    filteredRecords = entity.Agents.ToList();
                }
                else
                {
                    string searchText = FindLineTextBox.Text;
                    filteredRecords =
                    product.Where(r => r.FirstName.ToLower().StartsWith(searchText.ToLower()) |
                    r.MiddleName.ToString().ToLower().StartsWith(searchText.ToLower()) |
                    r.LastName.ToString().ToLower().StartsWith(searchText.ToLower()) |
                    r.DealShare.ToString().ToLower().StartsWith(searchText.ToLower())).ToList();
                }
                agents = filteredRecords.ToList();
                if (agents.Count == 0)
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

        private void ChangeButton_Click_1(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("Вы точно хотите редактировать данные?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (message.Equals(MessageBoxResult.Yes))
            {
                var selected = agentsDG.SelectedItem as Agents;
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

        public void AutoLoad()
        {
            List<Agents> List = agents;
            List<Agents> currentPagesList;
            maxi = agents.Count() / countPage;
            if (page == 0)
            {
                currentPagesList = List.Skip(0).Take(countPage).ToList();
            }
            else
            {
                currentPagesList = List.Skip(page * countPage - countPage).Take(countPage).ToList();
            }
            agentsDG.ItemsSource = currentPagesList;
        }

    }
}
