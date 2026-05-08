namespace BankClients
{
    public class Authorization
    {
        // Змінна, яка зберігає роль: 0 - Гість, 2 - Редактор
        public static int logUser { get; set; } = 0;

        public int LogCheck(string logText, string pswText)
        {
            if (logText == "Редактор" && pswText == "222")
            {
                logUser = 2; // Роль редактора
            }
            else
            {
                logUser = 0; // Роль гостя
            }
            return logUser;
        }
    }
}