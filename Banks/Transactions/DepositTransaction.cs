using Banks.Accounts;

namespace Banks.Transactions
{
    public class DepositTransaction : Transaction
    {
        public DepositTransaction(decimal money, int id, Account account)
            : base(money, id, account)
        {
        }

        public override void Cancel()
        {
            Account.SubtractMoney(Money);
            SetCancelled();
        }

        public override void Make()
        {
            Account.Deposit(Money);
        }
    }
}
