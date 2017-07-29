using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;

namespace AxaHosting.Data.Repositories
{
    public interface IPreparedIpRepository : IRepository<PreparedIp>
    {
        // Here we define all the custom actions a repository might have. For example filtering for properties.
    }

    public class PreparedIpRepository : RepositoryBase<PreparedIp>, IPreparedIpRepository
    {
        public PreparedIpRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


    }

   
}
