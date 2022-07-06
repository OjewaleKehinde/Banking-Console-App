using System;
using BankingConsoleApp.Enum;

namespace BankingConsoleApp.Models
{
    public class Transactions
    {
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}