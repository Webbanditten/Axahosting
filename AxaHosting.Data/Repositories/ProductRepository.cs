using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Model;

namespace AxaHosting.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        // Here we define all the custom actions a repository might have. For example filtering for properties.
      
    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

      
    }

   
}
