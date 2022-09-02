using System;
using System.Collections.Generic;
using System.Text;

namespace Ueberweisung
{
    internal class BankAccount
    {
        public object Lock = new object();
        public decimal Balance { get; private set; }
        public decimal InitValue { get; private set; }
        public int IBAN { get; private set; }
        private string Country { get; set; }
        public bool Active { get; private set; } = true;

        public BankAccount(decimal balance = 0.0m)
        {
            Balance = balance;
            InitValue = balance;
            GenerateIBAN();
            Country = "DE";
        }

        public string GetIBAN()
        {
            return Country + IBAN;
        }

        public void TransferTo(BankAccount otherAccount, decimal amount)
        {
            BankAccount lower, higher;
            if (otherAccount.IBAN < IBAN)
            {
                lower = this;
                higher = otherAccount;
            }
            else
            {
                higher = this;
                lower = otherAccount;
            }

            lock (lower.Lock)
            {
                lock (higher.Lock)
                {
                    otherAccount.Balance += amount;
                    Balance -= amount;
                }
            }
            PrintTransaction(otherAccount, amount);
        }

        public static void PrintHeader()
        {
            Console.WriteLine("{0,13} | {1,13} | {2,6}", "IBAN sender", "IBAN receiver", "Amount");
            Console.WriteLine("-------------------------------------------");
        }

        private void PrintTransaction(BankAccount otherAccount, decimal amount)
        {
            Console.WriteLine("{0,13} | {1,13} | {2,-8}EUR", this.IBAN, otherAccount.IBAN, amount);
        }

        public void Print()
        {
            Console.WriteLine("IBAN {0} - Balance {1,9} EUR", IBAN, Math.Round(Balance, 2));
        }

        private void GenerateIBAN()
        {
            Random srand = new Random();
            IBAN = srand.Next(int.MaxValue / 2, int.MaxValue);
        }
    }
}
