using Banks.Accounts;

namespace Banks.Transactions
{
    public abstract class Transaction
    {
        public Transaction(decimal money, int id, Account account)
        {
            Money = money;
            Id = id;
            Account = account;
        }

        public bool IsCancelled { get; private set; } = false;

        public int Id { get; private set; }
        public decimal Money { get; private set; }
        public Account Account { get; private set; }
        public void SetCancelled()
        {
            IsCancelled = true;
        }

        public abstract void Cancel();
        public abstract void Make();
    }
}
