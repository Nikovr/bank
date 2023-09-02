namespace Banks.Exceptions
{
    public class SuspiciousBankingException : BankingException
    {
        public SuspiciousBankingException(string message)
            : base(message)
        {
        }
    }
}
