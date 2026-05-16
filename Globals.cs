
namespace BankClients
{
    public class Client
    {
        public Client(int id, string surname, string name, string patronymic, string address, int age, double balance)
        {
            this.id = id;
            this.surname = surname;
            this.name = name;
            this.patronymic = patronymic;
            this.address = address;
            this.age = age;
            this.balance = balance;
        }
        public int id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string address { get; set; }
        public int age { get; set; }
        public double balance { get; set; }
        public string fullName => $"{surname} {name} {patronymic}";
    }
}