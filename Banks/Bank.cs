using Banks.Accounts;
using Banks.Clients;
using Banks.Exceptions;
using Banks.Transactions;

namespace Banks
{
    public class Bank
    {
        private List<Account> _accounts = new List<Account>();
        private List<Client> _clients = new List<Client>();
        public Bank(string bankName, decimal suspiciousLimitation, decimal comission, double yearlyPercentage, double[] depositPercentages, int id)
        {
            BankName = bankName;
            SuspiciousLimitation = suspiciousLimitation;
            Comission = comission;
            YearlyPercentage = yearlyPercentage;
            DepositPercentages = depositPercentages;
            Id = id;
        }

        public string BankName { get; set; }
        public int Id { get; private set; }
        public decimal SuspiciousLimitation { get; private set; }
        public decimal Comission { get; private set; }
        public double YearlyPercentage { get; private set; }
        public double[] DepositPercentages { get; private set; }
        public IReadOnlyCollection<Account> Accounts => _accounts;
        public IReadOnlyCollection<Client> Clients => _clients;

        public string Withdraw(Account account, decimal money)
        {
            string status = "Success!";
            try
            {
                var transaction = new WithdrawalTransaction(money, account.TransactionHistory.Count, account);
                transaction.Make();
                account.AddTransaction(transaction);
            }
            catch (BankingException e)
            {
                status = "Withdrawal Failed. Reason: " + e.Message;
                return status;
            }

            return status;
        }

        public string Deposit(Account account, decimal money)
        {
            string status = "Success!";
            try
            {
                var transaction = new DepositTransaction(money, account.TransactionHistory.Count, account);
                transaction.Make();
                account.AddTransaction(transaction);
            }
            catch (BankingException e)
            {
                status = "Deposit Failed. Reason: " + e.Message;
                return status;
            }

            return status;
        }

        public string Transfer(Account sender, Account reciever, decimal money)
        {
            string status = "Success!";
            try
            {
                var transaction = new TransferTransaction(money, sender.TransactionHistory.Count, sender, reciever);
                transaction.Make();
                sender.AddTransaction(transaction);
            }
            catch (BankingException e)
            {
                status = "Transfer Failed. Reason: " + e.Message;
                return status;
            }

            return status;
        }

        public string TransferToAnotherBank(decimal money, Account sender, int bankReceiverId, int accountReceiverId)
        {
            Bank bankReceiver = CentralBank.GetInstance().GetBankById(bankReceiverId);
            Account accountReceiver = bankReceiver.GetAccountByAccountId(accountReceiverId);

            string status = Transfer(sender, accountReceiver, money);
            return status;
        }

        public void UndoTransaction(Account account, int transactionId)
        {
            Transaction transaction = account.GetTransactionById(transactionId);

            if (!transaction.IsCancelled)
            {
                transaction.Cancel();
                transaction.SetCancelled();
            }
        }

        public Client RegisterClient(string firstName, string lastName, string homeAddress, string passportId)
        {
            var builder = new Client.ClientConcreteBuilder();
            var director = new ClientDirector();
            director.Builder = builder;

            director.BuildFullInfoClient(firstName, lastName, homeAddress, passportId, _clients.Count);
            Client client = builder.GetClient();
            AddClientToCollection(client);
            return client;
        }

        public Client RegisterClient(string firstName, string lastName)
        {
            var builder = new Client.ClientConcreteBuilder();
            var director = new ClientDirector();
            director.Builder = builder;

            director.BuildMinInfoClient(firstName, lastName, _clients.Count);
            Client client = builder.GetClient();
            AddClientToCollection(client);
            return client;
        }

        public void UpdateClientAddress(Client client, string homeAddress)
        {
            client.SetAddress(homeAddress);
        }

        public void UpdateClientPassport(Client client, string passport)
        {
            client.SetPassport(passport);
        }

        public CreditAccount CreateCreditAccount(Client client, decimal creditLimit)
        {
            var creditAccount = (CreditAccount)new CommissionFactory().CreateCreditAccount(client, this, creditLimit, _accounts.Count);
            AddAccountToCollections(creditAccount);
            return creditAccount;
        }

        public DebitAccount CreateDebitAccount(Client client)
        {
            var debitAccount = (DebitAccount)new PercentageFactory().CreateDebitAccount(client, this, _accounts.Count);
            AddAccountToCollections(debitAccount);
            return debitAccount;
        }

        public DepositAccount CreateDepositAccount(Client client, decimal money, DateTime limitDate)
        {
            int depositPercentageIndex;

            if (money < 50000)
            {
                depositPercentageIndex = 0;
            }
            else if (money < 100000)
            {
                depositPercentageIndex = 1;
            }
            else
            {
                depositPercentageIndex = 2;
            }

            var depositAccount = (DepositAccount)new PercentageFactory().CreateDepositAccount(client, money, this, limitDate, depositPercentageIndex, _accounts.Count);
            AddAccountToCollections(depositAccount);
            return depositAccount;
        }

        public Account GetAccountByAccountId(int id)
        {
            Account? account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                throw new ArgumentException("Error: no account with this id");
            }

            return account;
        }

        public IReadOnlyCollection<Account> GetAccountsByClient(Client client)
        {
            return client.Accounts;
        }

        public void UpdateSuspiciousLimitation(decimal suspiciousLimitation)
        {
            SuspiciousLimitation = suspiciousLimitation;
        }

        public void UpdateComission(decimal comission)
        {
            Comission = comission;
        }

        public void UpdateYearlyPercentage(double yearlyPercentage)
        {
            YearlyPercentage = yearlyPercentage;
        }

        public void UpdateDepositPercentages(double[] depositPercentages)
        {
            DepositPercentages = depositPercentages;
        }

        public Client GetClientByClientId(int id)
        {
            Client? client = _clients.FirstOrDefault(a => a.Id == id);
            if (client == null)
            {
                throw new ArgumentException("Error: no client with this id");
            }

            return client;
        }

        public void NotifyDate(DateTime oldDate)
        {
            Commission(oldDate);
        }

        private void Commission(DateTime oldDate)
        {
            while (DateTime.Compare(oldDate, CentralBank.GetInstance().CurrentDate) < 0)
            {
                if (oldDate.Day == 1)
                {
                    foreach (Account account in Accounts)
                    {
                        if (account is ICommissionGain)
                        {
                            ((ICommissionGain)account).GetCommissionedMoney();
                        }

                        if (account is IPercentagePayment)
                        {
                            ((IPercentagePayment)account).MonthlyPayment();
                        }
                    }
                }

                foreach (Account account in Accounts)
                {
                    if (account is IPercentagePayment)
                    {
                        double dailyPercentage;
                        if (account is DepositAccount)
                        {
                            dailyPercentage = DepositPercentages[((DepositAccount)account).DepositPercentageIndex] / 365;
                        }
                        else
                        {
                            dailyPercentage = YearlyPercentage / 365;
                        }

                        ((IPercentagePayment)account).CountDailyPercentage(dailyPercentage);
                    }
                }

                oldDate = oldDate.AddDays(1);
            }
        }

        private void AddClientToCollection(Client client)
        {
            _clients.Add(client);
        }

        private void AddAccountToCollections(Account account)
        {
            _accounts.Add(account);
            account.Owner.AddAccount(account);
        }
    }
}
