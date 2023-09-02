using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banks.Clients
{
    public interface IClientBuilder
    {
        void BuildName(string firstName, string lastName);
        void BuildAddress(string address);
        void BuildPassport(string passport);
        void BuildId(int id);
    }
}
