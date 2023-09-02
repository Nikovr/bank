using Banks.Accounts;
using Banks.Clients;
using Xunit;

namespace Banks.Test
{
    public class CentralBankTest
    {
        [Fact]
        public void RegisterBank_BankIsInList()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 500, 200, 3.14, new double[] { 3.2, 5.6, 6.1 });
            Assert.Equal<Bank>(centralBank.GetBankById(bank.Id), bank);
        }

        [Fact]
        public void SkipDays_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            DateTime date1 = centralBank.CurrentDate.AddDays(2);
            centralBank.AddDays(2);
            Assert.Equal(centralBank.CurrentDate, date1);
        }

        [Fact]
        public void SkipMonths_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            DateTime date1 = centralBank.CurrentDate.AddMonths(2);
            centralBank.AddMonths(2);
            Assert.Equal(centralBank.CurrentDate, date1);
        }

        [Fact]
        public void SkipYears_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            DateTime date1 = centralBank.CurrentDate.AddYears(2);
            centralBank.AddYears(2);
            Assert.Equal(centralBank.CurrentDate, date1);
        }

        [Fact]
        public void CountPercentageGainFromDebitAccount_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 500, 200, 3.14, new double[] { 3.2, 5.6, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "qwerty", "1234");
            Account account = bank.CreateDebitAccount(client);
            bank.Deposit(account, 1000);
            centralBank.AddMonths(2);
            Assert.Equal(1571.9478476261964871352974291m, account.Money);
        }

        [Fact]
        public void CountPercentageGainFromDepositAccount_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 500, 200, 3.14, new double[] { 3.2, 5.6, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "qwerty", "1234");
            Account account = bank.CreateDepositAccount(client, 1000m, new DateTime(2000, 12, 19));
            centralBank.AddMonths(2);
            Assert.Equal(1595.1267404766378074637643085m, account.Money);
        }

        [Fact]
        public void CountComissionGainFromCreditAccount_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "qwerty", "1234");
            Account account = bank.CreateCreditAccount(client, 1000);
            bank.Withdraw(account, 200m);
            bank.Withdraw(account, 200m);
            centralBank.AddMonths(2);
            Assert.Equal(-500, account.Money);
        }

        [Fact]
        public void TransferBetweenBanks_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank1 = centralBank.RegisterBank("FFF", 500, 200, 3.14, new double[] { 3.2, 5.6, 6.1 });
            Client client1 = bank1.RegisterClient("John", "Smith", "qwerty", "1234");
            Account account1 = bank1.CreateDebitAccount(client1);
            bank1.Deposit(account1, 1000m);
            Bank bank2 = centralBank.RegisterBank("FFF", 500, 200, 3.14, new double[] { 3.2, 5.6, 6.1 });
            Client client2 = bank2.RegisterClient("John", "Smith", "qwerty", "1234");
            Account account2 = bank2.CreateDebitAccount(client2);
            bank1.TransferToAnotherBank(100, account1, bank2.Id, account2.Id);
            Assert.Equal(900, account1.Money);
            Assert.Equal(100, account2.Money);
        }

        [Fact]
        public void RegisterFullInfoClient_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "qwerty", "1234");
            Assert.Equal(bank.GetClientByClientId(0), client);
            Assert.True(client.IsFullInfo);
        }

        [Fact]
        public void RegisterMinInfoClient_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith");
            Assert.Equal(bank.GetClientByClientId(0), client);
            Assert.False(client.IsFullInfo);
        }

        [Fact]
        public void RegisterPartialInfoClient_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith");
            client.SetAddress("sdfsd");
            Assert.Equal(bank.GetClientByClientId(client.Id), client);
            Assert.False(client.IsFullInfo);
        }

        [Fact]
        public void DetectAndLimitSuspiciousAccounts_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith");
            DebitAccount debitAccount = bank.CreateDebitAccount(client);
            bank.Deposit(debitAccount, 100000000);
            Assert.Equal(0, debitAccount.Money);
        }

        [Fact]
        public void DetectAndLimitSuspiciousAccounts_UpdatedInfo()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith");
            DebitAccount debitAccount = bank.CreateDebitAccount(client);
            client.SetAddress("fdsfsd");
            client.SetPassport("fsdfds");
            bank.Deposit(debitAccount, 100000000);
            Assert.Equal(100000000, debitAccount.Money);
        }

        [Fact]
        public void WithdrawDebitAccount_Incorrect()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "sfdsdf", "asdfasdf");
            DebitAccount debitAccount = bank.CreateDebitAccount(client);
            bank.Withdraw(debitAccount, 1);
            Assert.Equal(0, debitAccount.Money);
        }

        [Fact]
        public void UndoTransaction_Correct()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "sfdsdf", "asdfasdf");
            DebitAccount debitAccount = bank.CreateDebitAccount(client);
            bank.Deposit(debitAccount, 1000);
            bank.Withdraw(debitAccount, 1);
            bank.UndoTransaction(debitAccount, debitAccount.TransactionHistory.Count - 1);
            Assert.Equal(1000, debitAccount.Money);
        }

        [Fact]
        public void UndoTransactionTwice_Incorrect()
        {
            var centralBank = CentralBank.GetInstance();
            Bank bank = centralBank.RegisterBank("FFF", 10000, 100, 3.14, new double[] { 3.2, 5.3, 6.1 });
            Client client = bank.RegisterClient("John", "Smith", "sfdsdf", "asdfasdf");
            DebitAccount debitAccount = bank.CreateDebitAccount(client);
            bank.Deposit(debitAccount, 1000);
            bank.Withdraw(debitAccount, 1);
            bank.UndoTransaction(debitAccount, debitAccount.TransactionHistory.Count - 1);
            bank.UndoTransaction(debitAccount, debitAccount.TransactionHistory.Count - 1);
            Assert.Equal(1000, debitAccount.Money);
        }
    }
}
