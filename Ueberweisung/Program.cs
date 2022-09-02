using System;
using System.Threading;

namespace Ueberweisung
{
    internal class Program
    {
        static BankAccount[] bankAccounts = new BankAccount[15];
        static Thread[] tr = new Thread[15];
        static void Main(string[] args)
        {
            decimal amount = 0m;
            Random srand = new Random();

            for(int i = 0; i < 15; i++)
            {
                bankAccounts[i] = new BankAccount(Convert.ToDecimal(srand.NextDouble() * srand.Next(100,10000)));
                bankAccounts[i].Print();
                tr[i] = new Thread(StartTask);
                tr[i].Start(bankAccounts[i]);
            }
            
            for(int i = 0; i < tr.Length; i++)
            {
                tr[i].Join();
            }

            Thread.Sleep(100);
            CountValues();
        }

        private static void CountValues()
        {
            decimal totalMoney = 0m;
            decimal currentMoney = 0m;

            foreach(BankAccount account in bankAccounts)
            {
                totalMoney += account.InitValue;
                currentMoney += account.Balance;
            }

            Console.WriteLine("Total money: " + Math.Round(totalMoney,2).ToString());
            Console.WriteLine("Current money: " + Math.Round(currentMoney,2).ToString());
        }

        private static void StartTask(object state)
        {
            if (!(state is BankAccount)) return;
            BankAccount bankAccount = (BankAccount)state;
            Random srand = new Random();
            int current = 0;
            int max = 100;
            int random;
            while(max >= current)
            {
                random = srand.Next(0,bankAccounts.Length);
                if (bankAccounts[random] is null)
                {
                    Thread.Sleep(20);
                    continue;
                }
                bankAccount.TransferTo(bankAccounts[random], Convert.ToDecimal(srand.NextDouble() + 0.5 * srand.Next(100,1000)));
                current++;
                Thread.Sleep(5);
            }
        }
    }
}
