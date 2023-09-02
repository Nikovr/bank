using Banks.Clients;

namespace Banks.Accounts
{
    public class CommissionFactory : AccountFactory
    {
        public override Account CreateCreditAccount(Client owner, Bank issuingBank, decimal limit, int id)
        {
            return new CreditAccount(owner, issuingBank, limit, id);
        }

        public override Account CreateDebitAccount(Client owner, Bank issuingBank, int id)
        {
            throw new NotImplementedException();
        }

        public override Account CreateDepositAccount(Client owner, decimal money, Bank issuingBank, DateTime limitDate, int depositPercentageIndex, int id)
        {
            throw new NotImplementedException();
        }
    }
}
