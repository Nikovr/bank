using Banks.Clients;

namespace Banks.Accounts
{
    public class PercentageFactory : AccountFactory
    {
        public override Account CreateCreditAccount(Client owner, Bank issuingBank, decimal limit, int id)
        {
            throw new NotImplementedException();
        }

        public override Account CreateDebitAccount(Client owner, Bank issuingBank, int id)
        {
            return new DebitAccount(owner, issuingBank, id);
        }

        public override Account CreateDepositAccount(Client owner, decimal money, Bank issuingBank, DateTime limitDate, int depositPercentageIndex, int id)
        {
            return new DepositAccount(owner, money, issuingBank, limitDate, depositPercentageIndex, id);
        }
    }
}
