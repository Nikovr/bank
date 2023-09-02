using Banks.Accounts;

namespace Banks.Transactions
{
    public class TransferTransaction : Transaction
    {
        private readonly Account _receiver;
        public TransferTransaction(decimal money, int id, Account sender, Account receiver)
            : base(money, id, sender)
        {
            _receiver = receiver;
        }

        public override void Cancel()
        {
            Account.AddMoney(Money);
            _receiver.SubtractMoney(Money);
            SetCancelled();
        }

        public override void Make()
        {
            Account.Transfer(Money, _receiver);
        }
    }
}
