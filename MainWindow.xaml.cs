using RealEstateProject.Database;
using RealEstateProject.Windows.Agent;
using RealEstateProject.Windows.Client;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealEstateProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities entity = new Entities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientBtn_Click(object sender, RoutedEventArgs e)
        {
            var client = new ClientWindow();
            client.Show();
            this.Close();
        }

        private void AgentBtn_Click(object sender, RoutedEventArgs e)
        {
            var agent = new AgentWindow();
            agent.Show();
            this.Close();
        }
    }
}
