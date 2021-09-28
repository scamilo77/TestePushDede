using System;
using System.Collections.Generic;
using System.Linq;

namespace Banking
{
    public interface IVault
    {
        List<Account> Accounts { get; set; }
        void Deposit(Guid accountId, decimal value);
        void Withdraw(Guid accountId, decimal value);
        void Add(Account account);
    }
    public class Vault
    {
        public List<Account> Accounts { get; set; }

        public Vault()
        {
            Accounts = new List<Account>(100);
        }

        public void Deposit(Guid accountId, decimal value)
        {
            var account = Accounts.FirstOrDefault(x => x.Id == accountId);

            if (account == null)
            {
                throw new Exception("Inexistent Account");
            }

            account.Balance += value;

        }

        public void Withdraw(Guid accountId, decimal value)
        {
            var account = Accounts.FirstOrDefault(x => x.Id == accountId);

            if (account == null)
            {
                throw new Exception("Inexistent Account");
            }

            if (account.Balance <= 0)
            {
                throw new Exception("Insufficient Balance");
            }

            account.Balance -= value;

        }

        public void Add(Account account)
        {
            if (Accounts.FirstOrDefault(x => x.Id == account.Id) != null)
            {
                return;
            }

            Accounts.Add(account);
        }
    }
}

