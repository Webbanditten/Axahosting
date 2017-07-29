using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model.Models;
using Newtonsoft.Json;
using RestSharp;

namespace AxaHosting.Data.Repositories
{
    public interface IMailRepository : IRepository<MailAccount>
    {
        int AddDomain(string mailServer, string domain);
        IEnumerable<MailAccount> GetAccountsForDomain(string mailServer, int domainId);
        void ChangePassword(string mailServer, int domainId, int accountId, string password);
        void DeleteAccount(string mailServer, int domainId, int accountId);
        void DeleteDomain(string mailServer, int domainId);
        MailAccount GetAccount(string mailServer, int domainId, int accountId);
        void AccountCreate(string mailServer, int domainId, string address, string password);
    }

    public class MailRepository : IMailRepository
    {
        #region Standard members
        public void Add(MailAccount entity)
        {
            throw new NotImplementedException();
        }

        public void Update(MailAccount entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(MailAccount entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<MailAccount, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public MailAccount GetById(int id)
        {
            throw new NotImplementedException();
        }

        

        public MailAccount Get(Expression<Func<MailAccount, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MailAccount> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MailAccount> GetMany(Expression<Func<MailAccount, bool>> @where)
        {
            throw new NotImplementedException();
        }

        #endregion

        public int AddDomain(string mailServer, string domain)
        {
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/DomainCreate", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("domainName",  domain); 
            var response = client.Execute(request);
            var content = response.Content;
            return int.Parse(content);
        }

        public IEnumerable<MailAccount> GetAccountsForDomain(string mailServer, int domainId)
        {
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/DomainGetAccounts", Method.GET) { RequestFormat = DataFormat.Json };
            request.AddParameter("domainId", domainId);
            var response = client.Execute(request);
            var users = JsonConvert.DeserializeObject<IEnumerable<MailAccount>>(response.Content);
            return users;
        }

        public void ChangePassword(string mailServer, int domainId, int accountId, string password)
        {
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/AccountChangePassword", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("domainId", domainId);
            request.AddParameter("accountId", accountId);
            request.AddParameter("password", password);
            var response = client.Execute(request);

        }

        public void DeleteAccount(string mailServer, int domainId, int accountId)
        {
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/AccountDelete", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("domainId", domainId);
            request.AddParameter("accountId", accountId);
            var response = client.Execute(request);
        }

        public void DeleteDomain(string mailServer, int domainId)
        {
            // DomainDelete
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/DomainDelete", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("domainId", domainId);
            var response = client.Execute(request);
        }

        public MailAccount GetAccount(string mailServer, int domainId, int accountId)
        {
            // GetAccount
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/GetAccount", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddParameter("domainId", domainId);
            request.AddParameter("accountId", accountId);
            var response = client.Execute(request);
            var user = new MailAccount { Address = response.Content.Replace("\"", "") };
            
            return user;
        }

        public void AccountCreate(string mailServer, int domainId, string address, string password)
        {
            // GetAccount
            var client = new RestClient("http://" + mailServer);
            var request = new RestRequest("/Home/GetAccount", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddParameter("domainId", domainId);
            request.AddParameter("alias", address);
            request.AddParameter("password", password);
            var response = client.Execute(request);


        }
    }

   
}
