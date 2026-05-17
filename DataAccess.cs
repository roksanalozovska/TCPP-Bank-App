using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace BankClients
{
    public class DataAccess
    {
        private string connectionString = "Server=localhost;Database=bank_db;Uid=root;Pwd=2251;";

        public List<Client> fList(string query)
        {
            List<Client> resultList = new List<Client>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(new Client(
                                Convert.ToInt32(reader["id"]),
                                reader["surname"].ToString(),
                                reader["name"].ToString(),
                                reader["patronymic"].ToString(),
                                reader["address"].ToString(),
                                Convert.ToInt32(reader["age"]),
                                Convert.ToDouble(reader["balance"])
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка бази даних: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return resultList;
        }

        // Метод для виконання SQL-запитів
        public void ExecuteNonQuery(string query)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}