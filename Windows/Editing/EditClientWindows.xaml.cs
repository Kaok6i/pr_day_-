using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using RealEstateProject.Database;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity.Migrations;
using RealEstateProject.Windows.Client;

namespace RealEstateProject.Windows.Editing
{
    /// <summary>
    /// Логика взаимодействия для EditClientWindows.xaml
    /// </summary>
    public partial class EditClientWindows : Window
    {
        Entities entity = new Entities();
        bool isEdit = false;
        Clients client;
        public EditClientWindows(bool isEditable, Clients clientInfo)
        {
            InitializeComponent();
            client = clientInfo;
            isEdit = isEditable;
            if (isEditable)
            {
                this.Title = 
                    "Eesoft | Агенство Недвижимости | Редактирование клиента";
                this.saveBtn.Content = "Изменить";
            }
            else
            {
                this.Title = 
                    "Eesoft | Агенство Недвижимости | Добавление клиента";
                this.saveBtn.Content = "Добавить";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                surnameTB.Text = client.FirstName;
                nameTB.Text = client.MiddleName;
                patronymicTB.Text = client.LastName;
                phoneTB.Text = client.Phone;
                emailTB.Text = client.Email;
            }
            else
            {
                client = new Clients();
                client.Id = entity.Clients.Max(x => x.Id)+ 1 ;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasPhone = false;
                bool hasEmail = false;
                if (emailTB.Text.Length >= 0 && emailTB.Text != "")
                {
                    hasEmail = true;
                }
                if (phoneTB.Text.Length >= 0 && phoneTB.Text != "") 
                { 
                    hasPhone = true;
                }
                if (hasPhone || hasEmail)
                {
                    client.FirstName = surnameTB.Text;
                    client.MiddleName = nameTB.Text;
                    client.LastName = patronymicTB.Text;
                    client.Phone = phoneTB.Text;
                    client.Email = emailTB.Text;
                    entity.Clients.AddOrUpdate(client);
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
                    MessageBox.Show($"Клиент успешно {action}", "Уведомление");
                    BackBttn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Нельзя создать клиента без электронной почты или телефона",
                        "Уведомление");
                    return;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show
               ($"Возникла ошибка при добавлений или изменений клиента\n{error}"
                    ,"Ошибка");
            }
        }

        private void BackBttn_Click(object sender, RoutedEventArgs e)
        {
            var client = new ClientWindow();
            client.Show();
            this.Close();
        }
    }
}
