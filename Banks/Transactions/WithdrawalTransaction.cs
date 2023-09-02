using Banks.Accounts;
namespace Banks.Transactions
{
    public class WithdrawalTransaction : Transaction
    {
        public WithdrawalTransaction(decimal money, int id, Account account)
            : base(money, id, account)
        {
        }

        public override void Cancel()
        {
            Account.AddMoney(Money);
            SetCancelled();
        }

        public override void Make()
        {
            Account.Withdraw(Money);
        }
    }
}
