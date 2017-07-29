using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace AxaHosting.Service
{
    public interface IActiveDirectoryService
    {
       
        void Add(User user);
        void Update(User user);
        bool IsValid(User user);
        User Get(User user);
        bool UsernameExist(User user);
        bool IsUserInRole(User user, string role);
        IEnumerable<User> GetUsersInOu();
    }

    public class ActiveDirectoryService : IActiveDirectoryService
    {

        private readonly IActiveDirectoryRepository _repository;

        public ActiveDirectoryService(IActiveDirectoryRepository repository)
        {
            _repository = repository;
        }

        public void Add(User user)
        {
            _repository.Add(user);

        }

        public void Update(User user)
        {
           _repository.Update(user);
        }

        public bool IsValid(User user)
        {
            return _repository.IsValid(user);
        }

        public User Get(User user)
        {
            return _repository.GetUser(user);


        }

        public bool UsernameExist(User user)
        {
            return _repository.UsernameExist(user);
        }

        public bool IsUserInRole(User user, string role)
        {
            return _repository.IsUserInRole(user, role);
        }

        public IEnumerable<User> GetUsersInOu()
        {
            return _repository.GetUsersInOu();
        }
    }
}
