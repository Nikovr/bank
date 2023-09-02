using Banks.Accounts;
using Banks.Clients;
using Banks.Exceptions;

namespace Banks.Console
{
    public class Program
    {
        private static void Main()
        {
            var centralBank = CentralBank.GetInstance();
            string? input = null;

            while (input != "exit")
            {
                System.Console.WriteLine("Welcome to the Banks app");
                System.Console.WriteLine("Choose wisely:");
                System.Console.WriteLine("1 - Create a bank");
                System.Console.WriteLine("2 - Register client in a bank");
                System.Console.WriteLine("3 - Log into account managment");
                System.Console.WriteLine("4 - Time Travel");
                System.Console.WriteLine("exit - exit the app");

                input = System.Console.ReadLine()?.ToLower().Trim();

                if (input == "1")
                {
                    System.Console.WriteLine("Welcome to the Banks Creation");
                    System.Console.WriteLine("Enter the name of your future bank:");
                    string? bankName = System.Console.ReadLine()?.Trim();
                    if (bankName == null)
                    {
                        bankName = "I am a silly goose";
                    }

                    System.Console.WriteLine("Enter the Limitation for suspicious accounts:");
                    string? input2 = System.Console.ReadLine();
                    if (!decimal.TryParse(input2, out decimal suslim))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    System.Console.WriteLine("Enter the comission:");
                    string? input3 = System.Console.ReadLine();
                    if (!decimal.TryParse(input3, out decimal commission))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    System.Console.WriteLine("Enter the yearly percentage");
                    string? input4 = System.Console.ReadLine();
                    if (!double.TryParse(input4, out double yearlyPercentage))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    System.Console.WriteLine("Enter the first deposit percentage");
                    string? input5 = System.Console.ReadLine();
                    if (!double.TryParse(input5, out double depositPercentage1))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    System.Console.WriteLine("Enter the second deposit percentage");
                    string? input6 = System.Console.ReadLine();
                    if (!double.TryParse(input6, out double depositPercentage2))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    System.Console.WriteLine("Enter the third deposit percentage");
                    string? input7 = System.Console.ReadLine();
                    if (!double.TryParse(input7, out double depositPercentage3))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    Bank bank = centralBank.RegisterBank(bankName, suslim, commission, yearlyPercentage, new double[] { depositPercentage1, depositPercentage2, depositPercentage3 });
                    System.Console.WriteLine("Congratulations!!! You've created your bank");
                    System.Console.WriteLine("Don't forget it's id " + bank.Id + " or else you'll lose access to it forever");
                }

                if (input == "2")
                {
                    System.Console.WriteLine("Welcome to client profile creation!");
                    System.Console.WriteLine("Here's a list of all available banks:");
                    foreach (Bank bank in centralBank.Banks)
                    {
                        System.Console.WriteLine("Id: " + bank.Id + " Name:" + bank.BankName);
                    }

                    System.Console.WriteLine("Please enter the id of a bank where you'd like to make a client profile:");
                    string? input1 = System.Console.ReadLine();
                    if (!int.TryParse(input1, out int id))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    Bank yourBank = centralBank.GetBankById(id);
                    System.Console.WriteLine("Please enter your first name");
                    string? firstName = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter your last name");
                    string? lastName = System.Console.ReadLine();
                    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                    {
                        throw new ArgumentException("Your name can't be empty");
                    }

                    System.Console.WriteLine("Please enter your home address (optional)");
                    string? address = System.Console.ReadLine();
                    System.Console.WriteLine("Please enter your passport number (optional)");
                    string? passport = System.Console.ReadLine();
                    Client client = yourBank.RegisterClient(firstName, lastName);
                    if (!string.IsNullOrEmpty(address))
                    {
                        client.SetAddress(address);
                    }

                    if (!string.IsNullOrEmpty(passport))
                    {
                        client.SetPassport(passport);
                    }

                    System.Console.WriteLine("Congratulations! You've successfully made a profile");
                    System.Console.WriteLine("Don't forget:");
                    System.Console.WriteLine("Your personal id: " + client.Id);
                    System.Console.WriteLine("Your bank's id: " + id);
                }

                if (input == "3")
                {
                    System.Console.WriteLine("Welcome to account management!");
                    System.Console.WriteLine("Please enter your bank's id");
                    string? input1 = System.Console.ReadLine();
                    if (!int.TryParse(input1, out int bankId))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    Bank bank = centralBank.GetBankById(bankId);

                    System.Console.WriteLine("Please enter your personal client id");
                    string? input2 = System.Console.ReadLine();
                    if (!int.TryParse(input2, out int clientId))
                    {
                        throw new ArgumentException("This is not a number");
                    }

                    Client client = bank.GetClientByClientId(clientId);
                    System.Console.WriteLine("You've successfully logged into your client profile!");

                    if (client.Accounts.Count == 0)
                    {
                        System.Console.WriteLine("Looks like you don't have an account yet, you should create one!");
                    }
                    else
                    {
                        System.Console.WriteLine("Here are all your accounts:");
                        foreach (Account acc in client.Accounts)
                        {
                            System.Console.WriteLine("Id: " + acc.Id);
                        }
                    }

                    string? input3 = null;

                    while (input3 != "exit")
                    {
                        System.Console.WriteLine("1 - create an account");
                        System.Console.WriteLine("2 - manage your account");
                        System.Console.WriteLine("exit - exit this menu");

                        input3 = System.Console.ReadLine()?.Trim().ToLower();

                        if (input3 == "1")
                        {
                            System.Console.WriteLine("1 - create a credit account");
                            System.Console.WriteLine("2 - create a debit account");
                            System.Console.WriteLine("3 - create a deposit account");

                            string? input4 = System.Console.ReadLine();
                            if (string.IsNullOrEmpty(input4))
                            {
                                throw new ArgumentNullException("No input");
                            }

                            if (input4 == "1")
                            {
                                Account account = bank.CreateCreditAccount(client, bank.SuspiciousLimitation);
                                System.Console.WriteLine("Success!");
                                System.Console.WriteLine("Don't forget your account id: " + account.Id);
                            }

                            if (input4 == "2")
                            {
                                Account account = bank.CreateDebitAccount(client);
                                System.Console.WriteLine("Success!");
                                System.Console.WriteLine("Don't forget your account id: " + account.Id);
                            }

                            if (input4 == "3")
                            {
                                System.Console.WriteLine("enter the desired amount of money: ");
                                if (!decimal.TryParse(System.Console.ReadLine(), out decimal money))
                                {
                                    throw new ArgumentNullException("This is not a number");
                                }

                                Account account = bank.CreateDepositAccount(client, money, centralBank.CurrentDate.AddYears(2));
                                System.Console.WriteLine("Success!");
                                System.Console.WriteLine("Don't forget your account id: " + account.Id);
                            }
                        }

                        if (input3 == "2")
                        {
                            System.Console.WriteLine("Please enter your account ID:");
                            if (!int.TryParse(System.Console.ReadLine(), out int accountId))
                            {
                                throw new ArgumentException("This is not a number");
                            }

                            Account account = bank.GetAccountByAccountId(accountId);
                            if (account.Owner != client)
                            {
                                throw new SuspiciousBankingException("This is not your account!");
                            }

                            string? input5 = null;
                            while (input5 != "exit")
                            {
                                System.Console.WriteLine("Welcome to your account!");
                                System.Console.WriteLine("Money: " + account.Money);
                                System.Console.WriteLine("1 - withdraw");
                                System.Console.WriteLine("2 - deposit");
                                System.Console.WriteLine("3 - transfer");
                                System.Console.WriteLine("exit - exit your accout");
                                input5 = System.Console.ReadLine();
                                if (input5 == "1")
                                {
                                    System.Console.WriteLine("enter the desired amount of money: ");
                                    if (!decimal.TryParse(System.Console.ReadLine(), out decimal money))
                                    {
                                        throw new ArgumentNullException("This is not a number");
                                    }

                                    account.Withdraw(money);
                                }

                                if (input5 == "2")
                                {
                                    System.Console.WriteLine("enter the desired amount of money: ");
                                    if (!decimal.TryParse(System.Console.ReadLine(), out decimal money))
                                    {
                                        throw new ArgumentNullException("This is not a number");
                                    }

                                    account.Deposit(money);
                                }

                                if (input5 == "3")
                                {
                                    System.Console.WriteLine("enter the id of account-receiver: ");
                                    if (!int.TryParse(System.Console.ReadLine(), out int receiverId))
                                    {
                                        throw new ArgumentNullException("This is not a number");
                                    }

                                    System.Console.WriteLine("enter the desired amount of money: ");
                                    if (!decimal.TryParse(System.Console.ReadLine(), out decimal money))
                                    {
                                        throw new ArgumentNullException("This is not a number");
                                    }

                                    account.Transfer(money, bank.GetAccountByAccountId(receiverId));
                                }
                            }
                        }
                    }
                }

                if (input == "4")
                {
                    string? input5 = null;
                    while (input5 != "exit")
                    {
                        System.Console.WriteLine("Welcome to time travel!");
                        System.Console.WriteLine("1 - add days");
                        System.Console.WriteLine("2 - add months");
                        System.Console.WriteLine("3 - add years");
                        System.Console.WriteLine("exit - exit your accout");
                        input5 = System.Console.ReadLine();
                        if (input5 == "1")
                        {
                            System.Console.WriteLine("enter the desired amount: ");
                            if (!int.TryParse(System.Console.ReadLine(), out int amount))
                            {
                                throw new ArgumentNullException("This is not a number");
                            }

                            centralBank.AddDays(amount);
                        }

                        if (input5 == "2")
                        {
                            System.Console.WriteLine("enter the desired amount: ");
                            if (!int.TryParse(System.Console.ReadLine(), out int amount))
                            {
                                throw new ArgumentNullException("This is not a number");
                            }

                            centralBank.AddMonths(amount);
                        }

                        if (input5 == "3")
                        {
                            System.Console.WriteLine("enter the desired amount: ");
                            if (!int.TryParse(System.Console.ReadLine(), out int amount))
                            {
                                throw new ArgumentNullException("This is not a number");
                            }

                            centralBank.AddYears(amount);
                        }
                    }
                }
            }
        }
    }
}