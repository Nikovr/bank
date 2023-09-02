namespace Banks.Exceptions
{
    public class BankingException : Exception
    {
        public BankingException(string message)
            : base(message)
        {
        }
    }
}
