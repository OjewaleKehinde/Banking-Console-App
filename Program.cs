using System;
using System.Collections.Generic;
using BankingConsoleApp.Enum;
using BankingConsoleApp.Models;

namespace BankingConsoleApp
{
    class Program
    {
        // internal static List<Customer> customers = new List<Customer>();

        internal static Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
        internal static List<Transactions> transactions = new List<Transactions>();
        internal static int customerId = 0;
        internal static int accountNumberSeed = 1234567890;

        static void Main(string[] args)
        {
            Console.WriteLine("===>Welcome to Wellahealth bank");
            Console.WriteLine("===>How can we help you?");
            
            while (true)
            {
                Console.WriteLine("===>Pick 1 to create an account");
                Console.WriteLine("===>Pick 2 to close an account");
                Console.WriteLine("===>Pick 3 to check balance");
                Console.WriteLine("===>Pick 4 to credit your account");
                Console.WriteLine("===>Pick 5 to make a transfer");
                Console.WriteLine("===>Pick 6 to view your transacton history");
                Console.WriteLine("===>Pick 7 to change your password");
                Console.WriteLine("===>Pick 8 to close");

                try
                {
                    int response = int.Parse(Console.ReadLine());

                    if (response == 1)
                    {
                        Console.Write("===>What is your name: ");
                        var nameFromUser = Console.ReadLine();
                        Console.Write("===>What is your address: ");
                        var addressFromUser = Console.ReadLine();
                        Console.Write("===>How much deposit do you want to start with: ");
                        var depositFromUser = decimal.Parse(Console.ReadLine());
                        Console.Write("===>Please input a password: ");
                        string password = Console.ReadLine();
                        

                        Console.WriteLine("===>Creating account...");
                        CreateAccount(nameFromUser,addressFromUser,depositFromUser,password);
                        Console.WriteLine("===>Account Created successfully...");
                        Console.WriteLine("===>Your account number is: " + accountNumberSeed);
                        continue;
                    } 
                    else if (response == 2)
                    {
                        Console.Write("What is your account Number: ");
                        var accountNumberFromUser = int.Parse(Console.ReadLine());
                        Console.Write("===>Please input your password: ");
                        var password = Console.ReadLine();
                        Console.Write("Are you sure you wish to close this account? yes or no ");
                        var areYouSure = Console.ReadLine().ToLower();
                        if (areYouSure == "yes")
                        {
                            CloseAccount(accountNumberFromUser, password);
                        }
                        continue;
                    } 
                    else if (response == 3)
                    {
                        Console.Write("What is your account Number: ");
                        var accountNumberFromUser = int.Parse(Console.ReadLine());
                        Console.Write("===>Please input your password: ");
                        string password = Console.ReadLine();
                        var balance = CheckBalance(accountNumberFromUser,password);
                        if (balance == null)
                        {
                            Console.Write("Account does not exist");
                        }
                        else
                        {
                            Console.WriteLine("===>Checking balance...");
                            Console.WriteLine("===>Your balance is: " + balance);
                        }
                        
                        continue;
                    }
                    else if (response == 4)
                    {
                        CreditAccount();
                        continue;
                    }
                    else if (response == 5)
                    {
                        DebitAccount();
                        continue;
                    }
                    else if (response == 6)
                    {
                        TransactionHistory();
                        continue;
                    }
                    else if (response == 7)
                    {
                        Console.WriteLine("===>Please enter your account number");
                        int accountNumber = int.Parse(Console.ReadLine());
                        ChangePassword(accountNumber);
                        continue;
                    }
                    else if (response == 8)
                    {
                        Console.WriteLine("===>Thank you for banking with us");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("===>You have not made a valid selection");
                        continue;
                    }
                }
                catch (System.Exception ex)
                {
                     // TODO
                    Console.WriteLine("===>Please check value. Message: " + ex.Message);
                    continue;
                }
            }
        }
    
        #region Create Account
        static void CreateAccount(string name, string address, decimal amount, string password)
        {
            customerId += 1;
            accountNumberSeed +=1;

            Console.WriteLine("===>Mapping Customer");
            Customer customer = new Customer();
            customer.Id = customerId;
            customer.Name = name;
            customer.Address = address;
            customer.password = password;

            Console.WriteLine("===>Mapping Account Details");
            AccountDetails accountDetails = new AccountDetails();
            accountDetails.AccountNumber = accountNumberSeed;
            accountDetails.Balance = amount;
            accountDetails.AccountStatus = AccountStatus.Active;
            customer.AccountDetails = accountDetails;

            Console.WriteLine("===>Mapping Transactions");
            Transactions transaction = new Transactions();
            transaction.Amount = amount;
            transaction.TransactionType = TransactionType.Credit;
            transaction.DateOfTransaction = DateTime.UtcNow;

            Console.WriteLine("===>Adding Transactions");
            customer.AccountDetails.Transactions.Add(transaction);
            Console.WriteLine("===>Adding to customer");
            customers.Add(customer.AccountDetails.AccountNumber, customer);
        }
        #endregion
    
        #region Close Account
        static void CloseAccount(int accountNumber, string password)
        {
           
            if (customers.ContainsKey(accountNumber) && customers[accountNumber].password==password)
            {
                Console.Write("===>Account found.");
                customers[accountNumber].AccountDetails.AccountStatus = AccountStatus.Disabled;
                Console.WriteLine($"Account of: {accountNumber} disabled");
                return;
            }
            Console.WriteLine("===>Account number or password incorrect");
        }
        #endregion
    
        #region Check Balance
        static decimal? CheckBalance(int accountNumber, string password)
        {
           
            if (customers.ContainsKey(accountNumber) && customers[accountNumber].password==password)
            {
                Console.WriteLine("===>Account found");
                return customers[accountNumber].AccountDetails.Balance;
            }
            Console.WriteLine("===>Account number or password incorrect");
            return null;
        }
        #endregion

        #region credit account

        static void CreditAccount()
        {
            Console.Write("===>===>Please input your account number: ");
            var accountNumber = int.Parse(Console.ReadLine());
            Console.Write("===>===>Please input the amount to be credited to your account: ");
            decimal creditAmount = decimal.Parse(Console.ReadLine());
            Console.Write("===>Please input your password: ");
            string password = Console.ReadLine();
            if (customers.ContainsKey(accountNumber) && customers[accountNumber].password==password)
            {
                while (true)
                {
                    Console.Write($"===>Your account with account number {accountNumber} will be credited with {creditAmount} naira. Type yes to confirm the transaction or no to cancel: ");
                    var answer = Console.ReadLine().ToLower();
                    if(answer=="yes")
                    {
                        customers[accountNumber].AccountDetails.Balance += creditAmount; 
                        Transactions transaction = new Transactions();
                        transaction.Amount = creditAmount;
                        transaction.TransactionType = TransactionType.Credit;
                        transaction.DateOfTransaction = DateTime.UtcNow;
                        customers[accountNumber].AccountDetails.Transactions.Add(transaction);
                        break;
                    }
                    else if(answer=="no")
                    {
                        Console.WriteLine("===>Transaction cancelled");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("===>You have not made a correct selection");
                        continue;
                    }
                }
            }
            else
                {
                    Console.WriteLine("===>Your account number or password is incorrect. If you will like to try again, enter yes. If not, press any other key to cancel the transaction: ");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "yes")
                    {
                        CreditAccount();
                        return;
                    }
                    Console.WriteLine("===>Tranacton cancelled"); 
                }
        }

        #endregion

        #region Debit account

        static void DebitAccount()
        {
            Console.Write("===>===>Please input your account number: ");
            var senderAccountNumber = int.Parse(Console.ReadLine());
            Console.Write("===>===>Please input the account number of the recipient: ");
            int recipientAccountNumber = int.Parse(Console.ReadLine());
            Console.Write("===>===>Please input the amount you will like to send: ");
            decimal debitAmount = decimal.Parse(Console.ReadLine());
            Console.Write("===>Please input your password: ");
            string password = Console.ReadLine();

            if (customers.ContainsKey(recipientAccountNumber) && customers.ContainsKey(senderAccountNumber) && customers[senderAccountNumber].AccountDetails.Balance>=debitAmount && customers[senderAccountNumber].password==password)
            {
                while (true)
                {
                    Console.WriteLine($"===>The sum of {debitAmount} naira will be sent to {customers[recipientAccountNumber].Name} with account number {recipientAccountNumber}. Type yes to confirm the transaction or no to cancel.");
                    var answer = Console.ReadLine().ToLower();
                    if(answer=="yes")
                    {
                        customers[senderAccountNumber].AccountDetails.Balance -= debitAmount;
                        Transactions transaction = new Transactions();
                        transaction.Amount = debitAmount;
                        transaction.TransactionType = TransactionType.Debit;
                        transaction.DateOfTransaction = DateTime.UtcNow;
                        customers[senderAccountNumber].AccountDetails.Transactions.Add(transaction);

                        customers[recipientAccountNumber].AccountDetails.Balance += debitAmount;
                        Transactions transaction2 = new Transactions();
                        transaction2.Amount = debitAmount;
                        transaction2.TransactionType = TransactionType.Credit;
                        transaction2.DateOfTransaction = DateTime.UtcNow;
                        customers[recipientAccountNumber].AccountDetails.Transactions.Add(transaction2);
                        break;
                    }
                    else if(answer=="no")
                    {
                        Console.WriteLine("===>Transaction cancelled");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("===>You have not made a correct selection");
                        continue;
                    }
                }
            }
            else if (!customers.ContainsKey(senderAccountNumber)||customers[senderAccountNumber].password==password) 
            {
                Console.Write("===>Your account number or password is incorrect. If you will like to try again, enter yes. If not, press any other key to cancel the transaction: ");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes")
                {
                    DebitAccount();
                    return;
                }
                Console.WriteLine("===>Tranacton cancelled");  
            }
            else if(!customers.ContainsKey(recipientAccountNumber))
            {
                Console.Write("===>The recipient's account does not exist. Please check the account number. If you will like to try again, enter yes. If not, press any other key to cancel the transaction: ");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes")
                {
                    DebitAccount();
                    return;
                }
                Console.WriteLine("===>Tranacton cancelled"); 
            }
             else
            {
                Console.WriteLine("Insufficient funds");
            }
        }

        #endregion

        #region Transaction history

        static void TransactionHistory()
        {
            Console.Write("===>===>Please input your account number: ");
            var accountNumber = int.Parse(Console.ReadLine());
            Console.Write("===>Please input your password: ");
            string password = Console.ReadLine();

            if (customers.ContainsKey(accountNumber)&&customers[accountNumber].password==password)
            {
                Console.WriteLine("Your previous tranactions are listed below");
                Console.WriteLine("Date                Amount Transaction type");
                foreach (Transactions transaction in customers[accountNumber].AccountDetails.Transactions)
                {
                    Console.Write(transaction.DateOfTransaction + " ");
                    Console.Write(transaction.Amount + " ");
                    Console.WriteLine(transaction.TransactionType);
                }
                return;
            }
            else
            {
                Console.Write("===>Your account number or password is incorrect. If you will like to try again, enter yes. If not, press any other key to cancel the transaction: ");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes")
                {
                    TransactionHistory();
                    return;
                }
                Console.WriteLine("===>Tranacton cancelled");            
            }
        }

        #endregion

        #region Change Password

        static void ChangePassword(int accountNumber)
        {
            if (customers.ContainsKey(accountNumber))
            {
                Console.Write("===>Please input your current password: ");
                var password = Console.ReadLine();
                if (customers[accountNumber].password==password)
                {
                    Console.Write("===>Please input a new password: ");
                    var newPassword = Console.ReadLine();
                    customers[accountNumber].password=newPassword;

                }
                else
                {
                    Console.Write("===>Incorrect password. Type yes to try again or press any other key to cancel: ");
                    var answer = Console.ReadLine().ToLower();
                    if (answer=="yes")
                    {
                        ChangePassword(accountNumber);
                    }
                    else
                    {
                        return;
                    }
                }
                return;
            }
            Console.WriteLine("===>Account not found");
        }
        #endregion
    }
}
