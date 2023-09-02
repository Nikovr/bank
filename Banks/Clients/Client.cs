using System.Text.RegularExpressions;
using Banks.Accounts;

namespace Banks.Clients
{
    public class Client
    {
        private List<Account> _accounts = new List<Account>();

        private Client(int id, string firstName, string lastName, string? address, string? passport)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            HomeAddress = address;
            PassportId = passport;

            if (HomeAddress != null && PassportId != null)
            {
                IsFullInfo = true;
            }
        }

        public bool IsFullInfo { get; private set; } = false;
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? HomeAddress { get; private set; }
        public string? PassportId { get; private set; }
        public int Id { get; private set; }
        public IReadOnlyCollection<Account> Accounts => _accounts;

        public void SetAddress(string address)
        {
            HomeAddress = address;

            if (!IsFullInfo && PassportId != null)
            {
                IsFullInfo = true;
            }
        }

        public void SetPassport(string passport)
        {
            PassportId = passport;

            if (!IsFullInfo && HomeAddress != null)
            {
                IsFullInfo = true;
            }
        }

        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }

        public class ClientConcreteBuilder : IClientBuilder
        {
            private int? _id;
            private string? _firstName;
            private string? _lastName;
            private string? _homeAddress;
            private string? _passportId;
            public ClientConcreteBuilder()
            {
                Reset();
            }

            public void BuildId(int id)
            {
                _id = id;
            }

            public void BuildAddress(string address)
            {
                _homeAddress = address;
            }

            public void BuildName(string firstName, string lastName)
            {
                ValidateName(lastName);
                ValidateName(firstName);
                _firstName = firstName;
                _lastName = lastName;
            }

            public void BuildPassport(string passport)
            {
                _passportId = passport;
            }

            public void Reset()
            {
                _id = null;
                _passportId = null;
                _firstName = null;
                _lastName = null;
                _homeAddress = null;
            }

            public Client GetClient()
            {
                if (_id == null || _firstName == null || _lastName == null)
                {
                    throw new ArgumentNullException("error: id or name are not built yet");
                }

                var result = new Client((int)_id, _firstName, _lastName, _homeAddress, _passportId);
                Reset();
                return result;
            }

            private void ValidateName(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("error: name can't be null or empty");
                }

                string pattern = @"^\s*[A-ZА-Я -,'.]+\s*$";
                var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Match match = regex.Match(name);

                if (!match.Success)
                {
                    throw new ArgumentException(name + " is an invalid name");
                }
            }
        }
    }
}
