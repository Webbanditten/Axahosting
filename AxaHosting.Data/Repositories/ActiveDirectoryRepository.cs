using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;

namespace AxaHosting.Data.Repositories
{
    public interface IActiveDirectoryRepository : IRepository<User>
    {
        // Here we define all the custom actions a repository might have. For example filtering for properties.
        bool IsValid(User user);
        bool UsernameExist(User user);
        User GetUser(User user);

        IEnumerable<User> GetUsersInOu();
        bool IsUserInRole(User user, string role);

    }

    public class ActiveDirectoryRepository : IActiveDirectoryRepository, IDisposable
    {
        private readonly PrincipalContext _ctx;
        public ActiveDirectoryRepository()
        {
            var domainController = ConfigurationManager.AppSettings["domain"];
            var ldap = ConfigurationManager.AppSettings["ldap"];
            var serviceAccountUsername = ConfigurationManager.AppSettings["debugServiceAccountUsername"];
            var serviceAccountPassword = ConfigurationManager.AppSettings["debugServiceAccountPassword"];
            try
            {
                
                this._ctx = new PrincipalContext(ContextType.Domain, domainController, ldap);
                /*#if DEBUG
                this._ctx = new PrincipalContext(ContextType.Domain, domainController, ldap, serviceAccountUsername,
                    serviceAccountPassword);
                #endif*/
            }
            catch
            {
                this._ctx = null;
            }
        }

        public bool IsValid(User user)
        {
            return _ctx != null && _ctx.ValidateCredentials(user.Username, user.Password);
        }

        public bool UsernameExist(User user)
        {
            if (_ctx != null)
            {
                var findUser = UserPrincipal.FindByIdentity(_ctx, IdentityType.SamAccountName, user.Username);
                if (findUser != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        

        public void Add(User user)
        {
            if (_ctx != null)
            {

                if (!UsernameExist(user))
                {
                    var adUser = new UserPrincipal(_ctx, user.Username, user.Password, true)
                    {
                        GivenName = user.Name,
                        Surname = user.Surname,
                        PasswordNeverExpires = true,
                        EmailAddress = user.Email,
                        VoiceTelephoneNumber = user.PhoneNumber
                    };
                    adUser.Save();

                    // Fix the company prop on AD object... Thanks Microsoft :-/

                    var up = UserPrincipal.FindByIdentity(_ctx, IdentityType.SamAccountName, user.Username);
                    var upDe = (DirectoryEntry)up?.GetUnderlyingObject();

                    SetProperty(upDe, "company", user.Company);
                    SetProperty(upDe, "streetAddress", user.Street);
                    SetProperty(upDe, "l", user.City); // Yes "l" is city for some reason... 
                    SetProperty(upDe, "postalCode", user.PostalCode);

                    // End fix

                  
                }
            }
        }

        public void Update(User user)
        {
            if (_ctx != null)
            {
                var up = UserPrincipal.FindByIdentity(_ctx, IdentityType.SamAccountName, user.Username);
                var upDe = (DirectoryEntry)up?.GetUnderlyingObject();

                if (up != null)
                {
                    up.GivenName = user.Name;
                    up.Surname = user.Surname;
                    up.EmailAddress = user.Email;
                    up.VoiceTelephoneNumber = user.PhoneNumber;
                    if (user.Password != null)
                    {
                        up.SetPassword(user.Password);
                    }

                    up.Save();

                    SetProperty(upDe, "company", user.Company);
                    SetProperty(upDe, "streetAddress", user.Street);
                    SetProperty(upDe, "l", user.City);
                    SetProperty(upDe, "postalCode", user.PostalCode);
                }
                else
                {
                    throw new ActiveDirectoryObjectNotFoundException();
                }
            }
            
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<User, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User GetUser(User user)
        {
            if (_ctx != null)
            {
                var adUser = UserPrincipal.FindByIdentity(_ctx, user.Username);
                if (adUser != null)
                {
                    var adUserObjects = (DirectoryEntry)adUser?.GetUnderlyingObject();
                    user.Company = GetProperty(adUserObjects, "company");
                    user.Street = GetProperty(adUserObjects, "streetAddress");
                    user.City = GetProperty(adUserObjects, "l");
                    user.PostalCode = GetProperty(adUserObjects, "postalCode");
                    user.Email = adUser.EmailAddress;
                    user.PhoneNumber = adUser.VoiceTelephoneNumber;
                    user.Name = adUser.GivenName;
                    user.Surname = adUser.Surname;

                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<User> GetUsersInOu()
        {
            var qbeUser = new UserPrincipal(_ctx);

            // create your principal searcher passing in the QBE principal    
            var srch = new PrincipalSearcher(qbeUser);

            // find all matches
            return srch.FindAll().Select(found => GetUser(new User {Username = found.SamAccountName})).ToList();
        }

        public User Get(Expression<Func<User, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetMany(Expression<Func<User, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInRole(User user, string roleName)
        {
            if (_ctx != null)
            {
                var aduser = UserPrincipal.FindByIdentity(_ctx, IdentityType.SamAccountName, user.Username);
                return aduser != null && aduser.GetGroups().Any(result => roleName == result.SamAccountName);
            }else
            {
                return false;
            }
        }

        private string GetProperty(DirectoryEntry oDe, string sPropertyName)
        {
            if (oDe.Properties.Contains(sPropertyName))
            {
                return oDe.Properties[sPropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private void SetProperty(DirectoryEntry oDe, string sPropertyName, string sPropertyValue)
        {
            //check if the value is valid, otherwise dont update
            if (sPropertyValue != string.Empty)
            {
                //check if the property exists before adding it to the list
                if (oDe.Properties.Contains(sPropertyName))
                {
                    oDe.Properties[sPropertyName].Value = sPropertyValue;
                    oDe.CommitChanges();
                    oDe.Close();
                }
                else
                {
                    oDe.Properties[sPropertyName].Add(sPropertyValue);
                    oDe.CommitChanges();
                    oDe.Close();
                }
            }
        }
        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }

   
}
