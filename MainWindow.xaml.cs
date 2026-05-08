using System;
using System.Windows;

namespace BankClients
{
    public partial class MainWindow : Window
    {
        public static DataAccess DataConnection;
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
            ClientListDG.ItemsSource = DataConnection.fList("SELECT * FROM clients;");
        }
        // Керування меню залежно від ролі
        private void LoadDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientListDG.ItemsSource = null;
                DataConnection = new DataAccess();
                ClientListDG.ItemsSource = DataConnection.fList("SELECT * FROM clients;");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при оновленні: " + ex.Message);
            }
        }

    }
}