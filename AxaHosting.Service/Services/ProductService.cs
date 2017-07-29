using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;

namespace AxaHosting.Service
{
    public interface IProductService
    {

        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        IEnumerable<Product> GetProductsByType(ProductType type);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        #region Members

        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetMany(c=>c.Visible);
        }

        public Product GetProduct(int productId)
        {
            return _repository.GetById(productId);
        }

        public IEnumerable<Product> GetProductsByType(ProductType type)
        {
            return _repository.GetMany(c => c.ProductType == type && c.Visible);
        } 

        

        #endregion
    }
}