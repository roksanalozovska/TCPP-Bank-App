using System;
using System.Windows;

namespace BankClients
{
    public partial class LogInForm : Window
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        // Логіка для кнопки "Увійти"
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization auth = new Authorization();
            
            int role = auth.LogCheck(logTextBox.Text, passwordTextBox.Password);

            if (role == 2)
            {
                MessageBox.Show("Ви увійшли як Редактор.");

                // Оновлюємо рівень доступу в головному вікні
                MainWindow main = (MainWindow)Application.Current.MainWindow;
                main.UpdateAccessLevel();
                main.Visibility = Visibility.Visible;

                // Тільки при успішному вході закриваємо форму
                this.Close();
            }
            else
            {
                // Якщо дані невірні, виводимо помилку
                MessageBox.Show("Введіть правильні дані авторизації.", "Помилка!",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

        // Логіка для кнопки "Скасувати"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Просто повертаємося до головного вікна без жодних змін
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }

            this.Close();
        }
    }
}