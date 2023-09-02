using Banks.Clients;
using Banks.Exceptions;

namespace Banks.Accounts
{
    public class CreditAccount : Account, ICommissionGain
    {
        private decimal _limit;
        private decimal _commissionedMoney;

        public CreditAccount(Client owner, Bank issuingBank, decimal limit, int id)
            : base(owner, 0, issuingBank, id)
        {
            UpdateLimit(limit);
        }

        public void CommissionCount(decimal commission)
        {
            if (Money < 0)
            {
                _commissionedMoney += commission;
                SubtractMoney(commission);
            }
        }

        public override void Deposit(decimal amount)
        {
            if (Money + amount > _limit)
            {
                throw new DepositBankingException("Deposit leads to reaching the limit");
            }

            CheckSuspicious(amount);

            CommissionCount(IssuingBank.Comission);
            AddMoney(amount);
        }

        public decimal GetCommissionedMoney()
        {
            decimal gain = _commissionedMoney;
            _commissionedMoney = 0;
            return gain;
        }

        public override decimal Withdraw(decimal amount)
        {
            if (Math.Abs(Money - amount) > _limit)
            {
                throw new WithdrawalBankingException("Withdrawal leads to reaching the limit");
            }

            CheckSuspicious(amount);

            CommissionCount(IssuingBank.Comission);
            SubtractMoney(amount);
            return amount;
        }

        public void UpdateLimit(decimal limit)
        {
            _limit = limit;
        }
    }
}
