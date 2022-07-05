using System.Collections.Generic;
using BankingConsoleApp.Enum;

namespace BankingConsoleApp.Models
{
    public class AccountDetails
    {
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public AccountStatus AccountStatus { get; set; }
    }
}