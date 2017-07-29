using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;

namespace AxaHosting.Data.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
        // Here we define all the custom actions a repository might have. For example filtering for properties.
        Server GetServerByExternalIp(string categoryName);
        IEnumerable<Server> GetServerByType(ServerType serverType);

    }

    public class ServerRepository : RepositoryBase<Server>, IServerRepository
    {
        public ServerRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Server GetServerByExternalIp(string ip)
        {
            var server = this.DbContext.Servers.FirstOrDefault(c => c.ExternalIp == ip);

            return server;
        }

        public IEnumerable<Server> GetServerByType(ServerType serverType)
        {
            var servers = this.DbContext.Servers.Where(c => c.Type == serverType);
            return servers;
        }

        public override void Update(Server entity)
        {
            entity.LastUpdated = DateTime.Now;
            base.Update(entity);
        }
    }

   
}
