using System;
using System.Globalization;
using System.Windows;

namespace BankClients
{
    public partial class AddClientForm : Window
    {
        public AddClientForm()
        {
            InitializeComponent();
        }
        // 
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(surnameTB.Text) ||
                string.IsNullOrWhiteSpace(nameTB.Text) ||
                string.IsNullOrWhiteSpace(patronymicTB.Text) ||
                string.IsNullOrWhiteSpace(addressTB.Text) ||
                string.IsNullOrWhiteSpace(ageTB.Text) ||
                string.IsNullOrWhiteSpace(balanceTB.Text))
            {
                MessageBox.Show("Введіть дані у всі поля.");
                return;
            }

            try
            {
                string query =
                    $"INSERT INTO clients(surname,name,patronymic,address,age,balance) VALUES(" +
                    $"'{surnameTB.Text.Replace("'", "''")}'," +
                    $"'{nameTB.Text.Replace("'", "''")}'," +
                    $"'{patronymicTB.Text.Replace("'", "''")}'," +
                    $"'{addressTB.Text.Replace("'", "''")}'," +
                    $"{ageTB.Text}," +
                    $"{Convert.ToDecimal(balanceTB.Text).ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                    $");";

                MainWindow.DataConnection.ExecuteNonQuery(query);

                MessageBox.Show("Клієнта додано.");

                this.Close();
            }
            catch
            {
                MessageBox.Show("Невірний формат введених даних.");
            }
        }
    }
}