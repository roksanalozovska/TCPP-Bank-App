using System.Windows;

namespace BankClients
{
    public partial class EditClientForm : Window
    {
        private Client currentClient;

        public EditClientForm(Client client)
        {
            InitializeComponent();

            currentClient = client;

            surnameTB.Text = client.surname;
            nameTB.Text = client.name;
            patronymicTB.Text = client.patronymic;
            addressTB.Text = client.address;
            ageTB.Text = client.age.ToString();
            balanceTB.Text = client.balance.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string formattedBalance = balanceTB.Text.Replace(",", "."); // Заміна коми на крапку для правильного формату числа
            string query =
                $"UPDATE clients SET " +
                $"surname='{surnameTB.Text}', " +
                $"name='{nameTB.Text}', " +
                $"patronymic='{patronymicTB.Text}', " +
                $"address='{addressTB.Text}', " +
                $"age={ageTB.Text}, " +
                $"balance={formattedBalance} " +
                $"WHERE id={currentClient.id};";

            MainWindow.DataConnection.ExecuteNonQuery(query);

            MessageBox.Show("Дані змінено.");

            this.Close();
        }
    }
}