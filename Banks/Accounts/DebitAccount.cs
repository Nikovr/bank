using Banks.Clients;
using Banks.Exceptions;

namespace Banks.Accounts
{
    public class DebitAccount : Account, IPercentagePayment
    {
        private decimal _profitFromPercentages = 0;

        public DebitAccount(Client owner, Bank issuingBank, int id)
            : base(owner, 0, issuingBank, id)
        {
        }

        public void CountDailyPercentage(double dailyPercentage)
        {
            _profitFromPercentages += Money * (decimal)dailyPercentage;
        }

        public override void Deposit(decimal amount)
        {
            CheckSuspicious(amount);

            AddMoney(amount);
        }

        public void MonthlyPayment()
        {
            AddMoney(_profitFromPercentages);
            _profitFromPercentages = 0;
        }

        public override decimal Withdraw(decimal amount)
        {
            if (Money < amount)
            {
                throw new WithdrawalBankingException("Not enough money for withdrawal");
            }

            CheckSuspicious(amount);

            SubtractMoney(amount);
            return amount;
        }
    }
}
