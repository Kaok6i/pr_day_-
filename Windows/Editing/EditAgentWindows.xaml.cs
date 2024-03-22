using RealEstateProject.Database;
using RealEstateProject.Windows.Agent;
using RealEstateProject.Windows.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace RealEstateProject.Windows.Editing
{
    /// <summary>
    /// Логика взаимодействия для EditAgentWindows.xaml
    /// </summary>
    public partial class EditAgentWindows : Window
    {
        Entities entity = new Entities();
        bool isEdit = false;
        Agents agents;
        public EditAgentWindows(bool isEditable, Agents agentsInfo)
        {
            InitializeComponent();
            agents = agentsInfo;
            isEdit = isEditable;
            if (isEditable)
            {
                this.Title =
                    "Eesoft | Агенство Недвижимости | Редактирование клиента";
                this.saveBttn.Content = "Изменить";
            }
            else
            {
                this.Title =
                    "Eesoft | Агенство Недвижимости | Добавление клиента";
                this.saveBttn.Content = "Добавить";
            }
        }

        private void saveBttn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasDeal = false;
                if (DealShareTB.Text.Length >= 0 && DealShareTB.Text != "")
                {
                    hasDeal = true;
                }
                if (surnameTB.Text == "" && surnameTB.Text.Length > 0)
                {
                    MessageBox.Show("Нельзя создать риелтора без фамилий","Уведомление");
                    return;
                }
                if (nameTB.Text == "" && nameTB.Text.Length > 0)
                {
                    MessageBox.Show("Нельзя создать риелтора без имени", "Уведомление");
                    return;
                }
                if (patronymicTB.Text == "" && patronymicTB.Text.Length > 0)
                {
                    MessageBox.Show("Нельзя создать риелтора без отчества", "Уведомление");
                    return;
                }
                var dealShare = Convert.ToInt32(DealShareTB.Text);
                if (dealShare > 0 && dealShare <= 100)
                {
                    if (hasDeal)
                    {
                        agents.FirstName = surnameTB.Text;
                        agents.MiddleName = nameTB.Text;
                        agents.LastName = patronymicTB.Text;
                        agents.DealShare = dealShare;
                        entity.Agents.AddOrUpdate(agents);
                        entity.SaveChanges();
                        string action;
                        if (isEdit)
                        {
                            action = "изменен";
                        }
                        else
                        {
                            action = "добавлен";
                        }
                        MessageBox.Show($"Агент успешно {action}", "Уведомление");
                        backBttn_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Нельзя создать Агента без комиссионной доли",
                            "Уведомление");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Введена некорректная комиссионная доля","Уведомление");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show
               ($"Возникла ошибка при добавлений или изменений клиента\n{error}"
                    , "Ошибка");
            }
        }

        private void backBttn_Click(object sender, RoutedEventArgs e)
        {
            var agent = new AgentWindow();
            agent.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                surnameTB.Text = agents.FirstName;
                nameTB.Text = agents.MiddleName;
                patronymicTB.Text = agents.LastName;
                DealShareTB.Text = Convert.ToString(agents.DealShare);
            }
            else
            {
                agents = new Agents();
                agents.Id = entity.Agents.Max(x => x.Id) + 1;
            }
        }
        Regex regex = new Regex("^[0-9]+");
        private void DealShareTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
