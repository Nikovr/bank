namespace Banks
{
    public sealed class CentralBank : ITimeTravel
    {
        private static CentralBank? _instance;
        private List<Bank> _banks = new List<Bank>();
        private CentralBank()
        {
            CurrentDate = new DateTime(1970, 1, 1);
        }

        public DateTime CurrentDate { get; private set; }
        public IReadOnlyCollection<Bank> Banks => _banks;

        public static CentralBank GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CentralBank();
            }

            return _instance;
        }

        public Bank RegisterBank(string name, decimal suspiciousLimitation, decimal commision, double yearlyPercentage, double[] depositPercentages)
        {
            var bank = new Bank(name, suspiciousLimitation, commision, yearlyPercentage, depositPercentages, _banks.Count);
            _banks.Add(bank);
            return bank;
        }

        public Bank GetBankById(int id)
        {
            return _banks[id];
        }

        public void AddDays(int days)
        {
            DateTime oldDate = CurrentDate;
            CurrentDate = CurrentDate.AddDays(days);
            NotifyDateBanks(oldDate);
        }

        public void AddMonths(int months)
        {
            DateTime oldDate = CurrentDate;
            CurrentDate = CurrentDate.AddMonths(months);
            NotifyDateBanks(oldDate);
        }

        public void AddYears(int years)
        {
            DateTime oldDate = CurrentDate;
            CurrentDate = CurrentDate.AddYears(years);
            NotifyDateBanks(oldDate);
        }

        private void NotifyDateBanks(DateTime oldDate)
        {
            foreach (Bank bank in _banks)
            {
                bank.NotifyDate(oldDate);
            }
        }
    }
}
