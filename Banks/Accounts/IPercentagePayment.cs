using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banks.Accounts
{
    public interface IPercentagePayment
    {
        void CountDailyPercentage(double dailyPercentage);
        void MonthlyPayment();
    }
}
