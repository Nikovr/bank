namespace Banks.Clients
{
    public class ClientDirector
    {
        private IClientBuilder? _builder;
        public IClientBuilder Builder
        {
            set { _builder = value;  }
        }

        public void BuildMinInfoClient(string firstName, string lastName, int id)
        {
            _builder?.BuildId(id);
            _builder?.BuildName(firstName, lastName);
        }

        public void BuildFullInfoClient(string firstName, string lastName, string address, string passport, int id)
        {
            _builder?.BuildId(id);
            _builder?.BuildName(firstName, lastName);
            _builder?.BuildPassport(passport);
            _builder?.BuildAddress(address);
        }
    }
}
