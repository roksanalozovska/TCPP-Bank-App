using System.Collections.Generic;

namespace BankClients
{
    class SelectData
    {
        public List<Client> clientList = new List<Client>();
        public List<Client> selectedNameList = new List<Client>();    // Результат для X
        public List<Client> selectedXYList = new List<Client>();      // Результат для XY

        // Реалізація Критерію Х (Відбір за іменем)
        public void SelectX(string clientName)
        {
            selectedNameList.Clear();
            foreach (Client client in clientList)
            {
                if (client.name == clientName)
                {
                    selectedNameList.Add(client);
                }
            }
        }

        // Реалізація Критерію XY (фільтрація за сумою)
        public void SelectXY(double minBalance)
        {
            selectedXYList.Clear();
            foreach (Client client in clientList)
            {
                if (client.balance > minBalance)
                {
                    selectedXYList.Add(client);
                }
            }
        }
    }
}