using Banks.Clients;
using Banks.Exceptions;

namespace Banks.Accounts
{
    public class DepositAccount : Account, IPercentagePayment
    {
        private decimal _profitFromPercentages = 0;

        public DepositAccount(Client owner, decimal money, Bank issuingBank, DateTime limitDate, int depositPercentageIndex, int id)
            : base(owner, money, issuingBank, id)
        {
            LimitDate = limitDate;
            DepositPercentageIndex = depositPercentageIndex;
        }

        public DateTime LimitDate { get; private set; }
        public int DepositPercentageIndex { get; private set; }
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
            if (DateTime.Compare(CentralBank.GetInstance().CurrentDate, LimitDate) < 0)
            {
                throw new WithdrawalBankingException("Withdrawals not allowed until " + LimitDate.ToString());
            }

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
