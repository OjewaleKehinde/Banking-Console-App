namespace BankingConsoleApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public string Address { get; set; }
        public AccountDetails AccountDetails { get; set; }
    }
}