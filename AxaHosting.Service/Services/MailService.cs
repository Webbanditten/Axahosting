using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using AxaHosting.Model.Models;

namespace AxaHosting.Service
{
    public interface IMailService
    {
        IEnumerable<MailAccount> GetAccountsForDomain(int domainId);
        void ChangePasswordForAccount(int domainId, int accountId, string password);
        void CreateAccount(int domainId, string address, string password);
        void DeleteAccount(int domainId, int accountId);
        MailAccount ViewAccount(int domainId, int accountId);
        void CreateDomain(string mailserver, string domain);
    }

    public class MailService : IMailService
    {
        private readonly IMailRepository _repository;
        private readonly string _mailServer;

        public MailService(IMailRepository repository)
        {
            _repository = repository;
            _mailServer = ConfigurationManager.AppSettings["mailServer"];
        }
        public IEnumerable<MailAccount> GetAccountsForDomain(int domainId)
        {
            return _repository.GetAccountsForDomain(_mailServer, domainId);
        }

        public void ChangePasswordForAccount(int domainId, int accountId, string password)
        {
           _repository.ChangePassword(_mailServer, domainId, accountId, password);
        }

        public void CreateAccount(int domainId, string address, string password)
        {
            _repository.AccountCreate(_mailServer, domainId, address, password);
        }

        public void DeleteAccount(int domainId, int accountId)
        {
            _repository.DeleteAccount(_mailServer, domainId, accountId);
        }

        public MailAccount ViewAccount(int domainId, int accountId)
        {
            return _repository.GetAccount(_mailServer, domainId, accountId);
        }

        public void CreateDomain(string mailServer, string domain)
        {
            _repository.AddDomain(mailServer, domain);
        }
    }
}
