using Banks.Clients;
using Banks.Exceptions;
using Banks.Transactions;

namespace Banks.Accounts
{
    public abstract class Account
    {
        private List<Transaction> _transactionHistory = new List<Transaction>();
        public Account(Client owner, decimal money, Bank issuingBank, int id)
        {
            Owner = owner;
            Money = money;
            IssuingBank = issuingBank;
            Id = id;
        }

        public Client Owner { get; set; }
        public decimal Money { get; private set; }
        public Bank IssuingBank { get; private set; }
        public int Id { get; private set; }
        public IReadOnlyCollection<Transaction> TransactionHistory => _transactionHistory;
        public abstract decimal Withdraw(decimal amount);
        public abstract void Deposit(decimal amount);

        public void Transfer(decimal amount, Account receiver)
        {
            receiver.Deposit(Withdraw(amount));
        }

        public void CheckSuspicious(decimal amount)
        {
            if (!Owner.IsFullInfo && IssuingBank.SuspiciousLimitation < amount)
            {
                throw new SuspiciousBankingException("The amount is over the limit (this account is suspicious)");
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactionHistory.Add(transaction);
        }

        public Transaction GetTransactionById(int id)
        {
            return _transactionHistory[id];
        }

        public void AddMoney(decimal amount)
        {
            Money += amount;
        }

        public void SubtractMoney(decimal amount)
        {
            Money -= amount;
        }
    }
}
