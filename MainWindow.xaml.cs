using System;
using System.Windows;

namespace BankClients
{
    public partial class MainWindow : Window
    {
        public static DataAccess DataConnection;
        SelectData selectData = new SelectData();
        public MainWindow()
        {
            InitializeComponent();
        }
        // Метод для оновлення рівня доступу після авторизації
        public void UpdateAccessLevel()
        {
            if (Authorization.logUser == 2)
            {
                ClientsMenu.IsEnabled = true;
                SearchMenu.IsEnabled = true;
            }
            else
            {
                ClientsMenu.IsEnabled = false;
                SearchMenu.IsEnabled = false;
            }
        }
        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogInForm logWnd = new LogInForm();
            logWnd.Show();
            this.Visibility = Visibility.Collapsed;
        }
        // Автоматичне завантаження 
        private void InfoClientForm_Loaded(object sender, RoutedEventArgs e)
        {
            DataConnection = new DataAccess();
            var list = DataConnection.fList("SELECT * FROM clients;");
            ClientListDG.ItemsSource = list;
            selectData.clientList = list;
        }
        // Керування меню залежно від ролі
        private void LoadDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientListDG.ItemsSource = null;
                DataConnection = new DataAccess();
                var list = DataConnection.fList("SELECT * FROM clients;");

                ClientListDG.ItemsSource = list;
                // Оновлюємо дані для пошуку
                selectData.clientList = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при оновленні: " + ex.Message);
            }
        }
        // Редагування даних клієнта
        private void EditDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ClientListDG.SelectedItem == null)
            {
                MessageBox.Show("Виберіть клієнта.");
                return;
            }

            Client selectedClient = (Client)ClientListDG.SelectedItem;

            EditClientForm form = new EditClientForm(selectedClient);
            form.ShowDialog();

            var updatedList = DataConnection.fList("SELECT * FROM clients;");
            ClientListDG.ItemsSource = updatedList;
            selectData.clientList = updatedList; // синхронізація пошуку з актуальною БД
        }
        // Додавання нового клієнта
        private void AddDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddClientForm form = new AddClientForm();

            form.ShowDialog();

            var updatedList = DataConnection.fList("SELECT * FROM clients;");
            ClientListDG.ItemsSource = updatedList;
            selectData.clientList = updatedList; // синхронізація пошуку з актуальною БД
        }
        // Видалення клієнта
        private void DeleteDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ClientListDG.SelectedItem == null)
            {
                MessageBox.Show("Виберіть клієнта.");
                return;
            }

            Client selectedClient = (Client)ClientListDG.SelectedItem;

            MessageBoxResult result =
                MessageBox.Show(
                    "Видалити клієнта?",
                    "Підтвердження",
                    MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                string query =
                    $"DELETE FROM clients WHERE id={selectedClient.id};";

                DataConnection.ExecuteNonQuery(query);

                var updatedList = DataConnection.fList("SELECT * FROM clients;");
                ClientListDG.ItemsSource = updatedList;
                selectData.clientList = updatedList; // синхронізація пошуку з актуальною БД

                MessageBox.Show("Клієнта видалено.");
            }
        }
        // Панель пошуку за віком
        private void SelectYearsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SearchNameGB.Visibility = Visibility.Visible;
            SearchBalanceGB.Visibility = Visibility.Collapsed;
        }
        // Пошук за ім'ям
        private void SelectNameBtn_Click(object sender, RoutedEventArgs e)
        {
            selectData.SelectX(NameTB.Text);

            if (selectData.selectedNameList.Count > 0)
            {
                Client client = selectData.selectedNameList[0];

                int maxCreditYears = 65 - client.age;

                if (maxCreditYears < 1)
                {
                    MessageBox.Show($"Клієнту {client.fullName} кредит видати неможливо (вік: {client.age}).",
                                    "Результат", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show($"Для клієнта {client.fullName} максимальна кількість років кредиту: {maxCreditYears}",
                                    "Результат");
                }

                ClientListDG.ItemsSource = null;
                ClientListDG.ItemsSource = selectData.selectedNameList;
            }
            else
            {
                MessageBox.Show("Клієнта з таким ім'ям не знайдено.");
            }
        }
        // Панель пошуку за сумою
        private void SearchBalanceMI_Click(object sender, RoutedEventArgs e)
        {
            SearchBalanceGB.Visibility = Visibility.Visible;
            SearchNameGB.Visibility = Visibility.Collapsed;
        }
        // Пошук за сумою
        private void SelectBalanceBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double minSum = Convert.ToDouble(BalanceTB.Text);

                selectData.SelectXY(minSum);

                ClientListDG.ItemsSource = null;
                ClientListDG.ItemsSource = selectData.selectedXYList;

                if (selectData.selectedXYList.Count == 0)
                {
                    MessageBox.Show("Клієнтів з балансом більше " + minSum + " не знайдено.");
                }
            }
            catch
            {
                MessageBox.Show("Введіть коректну суму для фільтрації.");
            }
        }
        // Збереження результатів пошуку в шаблоні Word
        private void SaveSelBtn_Click(object sender, RoutedEventArgs e)
        {
            string nameCriteria = string.IsNullOrEmpty(NameTB.Text) ? "Не задано" : NameTB.Text;
            string sumCriteria = string.IsNullOrEmpty(BalanceTB.Text) ? "0" : BalanceTB.Text;

            string calculatedYears = "Кредит не розраховувався";

            if (selectData.selectedNameList.Count > 0)
            {
                int maxCreditYears = 65 - selectData.selectedNameList[0].age;
                calculatedYears = maxCreditYears < 1 ? "Кредит неможливий" : maxCreditYears.ToString() + " років";
            }
            selectData.WriteData(
                selectData.selectedNameList,
                selectData.selectedXYList,
                nameCriteria,
                calculatedYears,
                sumCriteria
            );
        }

    }
}