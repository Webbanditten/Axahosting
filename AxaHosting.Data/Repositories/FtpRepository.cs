using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;
using Microsoft.Web.Administration;
using Miracle.FileZilla.Api;
using System.Configuration;

namespace AxaHosting.Data.Repositories
{
    public interface IFtpRepository : IRepository<WebHotel>
    {
        
    }

    public class FtpRepository : IFtpRepository
    {
        public void Add(WebHotel entity)
        {
            var ip = entity.Server.InternalIp;
            try
            {
                using (IFileZillaApi fileZillaApi = new FileZillaApi(System.Net.IPAddress.Parse(ip), 14147))
                {
                    fileZillaApi.Connect(entity.Server.Password);
                    var accountSettings = fileZillaApi.GetAccountSettings();
                    var user = new Miracle.FileZilla.Api.User
                    {
                        GroupName = "FTPUser", // Reference to existing group
                        UserName = entity.FtpUsername,
                        Enabled = TriState.Yes,
                        SharedFolders = new List<SharedFolder>()
                        {
                            new SharedFolder()
                            {
                                Directory = @"C:\inetpub\wwwroot\" + entity.Domain,
                                AccessRights = 
                                AccessRights.DirList |
                                AccessRights.DirSubdirs |
                                AccessRights.FileRead |
                                AccessRights.FileWrite |
                                AccessRights.IsHome |
                                AccessRights.DirCreate |
                                AccessRights.DirDelete |
                                AccessRights.FileDelete |
                                AccessRights.FileWrite |
                                AccessRights.FileAppend
                            }
                        }
                    };
                    user.AssignPassword(entity.FtpPassword, fileZillaApi.ProtocolVersion);
                    accountSettings.Users.Add(user);
                    fileZillaApi.SetAccountSettings(accountSettings);
                }
            }
            catch
            {
                // ignored
            }
        }

        

        public void Update(WebHotel entity)
        {
            var ip = entity.Server.InternalIp;
            try
            {
                using (IFileZillaApi fileZillaApi = new FileZillaApi(System.Net.IPAddress.Parse(ip), 14147))
                {
                    fileZillaApi.Connect(entity.Server.Password);
                    var accountSettings = fileZillaApi.GetAccountSettings();
                    var user = accountSettings.Users.FirstOrDefault(c => c.UserName == entity.FtpUsername);
                    user?.AssignPassword(entity.FtpPassword, fileZillaApi.ProtocolVersion);

                    fileZillaApi.SetAccountSettings(accountSettings);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void Delete(WebHotel entity)
        {
            var ip = entity.Server.InternalIp;
            using (IFileZillaApi fileZillaApi = new FileZillaApi(System.Net.IPAddress.Parse(ip), 14147))
            {

                fileZillaApi.Connect(entity.Server.Password);
                var accountSettings = fileZillaApi.GetAccountSettings();
                var user = accountSettings.Users.FirstOrDefault(c => c.UserName == entity.FtpUsername);
                accountSettings.Users.Remove(user);

                fileZillaApi.SetAccountSettings(accountSettings);
            }
        }

        public void Delete(Expression<Func<WebHotel, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public WebHotel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public WebHotel Get(Expression<Func<WebHotel, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebHotel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebHotel> GetMany(Expression<Func<WebHotel, bool>> @where)
        {
            throw new NotImplementedException();
        }

        public bool Exist(WebHotel entity)
        {
            var server = entity.Server;
            try
            {
                var name = entity.Domain;
                using (var serverMgr = ServerManager.OpenRemote(server.InternalIp))
                {
                    var bWebsite = serverMgr.Sites.FirstOrDefault(c => c.Name == name);
                    return bWebsite != null;
                }
            }
            catch
            {
                return false;
                // ignored
            }
        }
    }

   
}
