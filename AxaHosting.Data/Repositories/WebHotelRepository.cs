using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;

namespace AxaHosting.Data.Repositories
{
    public interface IWebHotelRepository : IRepository<WebHotel>
    {
        // Here we define all the custom actions a repository might have. For example filtering for properties.
        
    }

    public class WebHotelRepository : RepositoryBase<WebHotel>, IWebHotelRepository
    {
        public WebHotelRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


    }

   
}
