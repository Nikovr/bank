using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banks.Exceptions
{
    public class DepositBankingException : BankingException
    {
        public DepositBankingException(string message)
            : base(message)
        {
        }
    }
}
