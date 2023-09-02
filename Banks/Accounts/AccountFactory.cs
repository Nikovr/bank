using Banks.Clients;

namespace Banks.Accounts
{
    public abstract class AccountFactory
    {
        public abstract Account CreateDebitAccount(Client owner, Bank issuingBank, int id);
        public abstract Account CreateDepositAccount(Client owner, decimal money, Bank issuingBank, DateTime limitDate, int depositPercentageIndex, int id);
        public abstract Account CreateCreditAccount(Client owner, Bank issuingBank, decimal limit, int id);
    }
}
