namespace Banks.Exceptions
{
    public class WithdrawalBankingException : BankingException
    {
        public WithdrawalBankingException(string message)
            : base(message)
        {
        }
    }
}
