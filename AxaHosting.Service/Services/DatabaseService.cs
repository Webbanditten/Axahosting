using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using AxaHosting.Service.util;

namespace AxaHosting.Service
{
    public interface IDatabaseService
    {

        Database GetDatabase(int databaseId);
        IEnumerable<Database> GetDatabasesByOwner(string owner);
        Database Create(Product product, string owner, string domainName = null);
        void Save();
        Database Get(int id);
        void Update(Database database);
        void Cancel(Database database);

    }

    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServerRepository _serverRepository;

        public DatabaseService(IDatabaseRepository repository, IServerRepository serverRepository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._serverRepository = serverRepository;
            this._unitOfWork = unitOfWork;
        }

        #region Members

        public Database GetDatabase(int databaseId)
        {
            return _repository.GetById(databaseId);
        }

        public IEnumerable<Database> GetDatabasesByOwner(string owner)
        {
            return _repository.GetDatabasesWithOwner(owner);
        }


        public Database Create(Product product, string owner, string domainName = null)
        {
            var database = new Database();
            var server = _serverRepository.GetServerByType(product.ServerType).OrderBy(c => c.LastUpdated).First();
            if (server != null)
            {
               
                var randNameExt = Guid.NewGuid().ToString().Substring(0, 4);

                if (domainName != null)
                {
                    database.Name = domainName.Split('.')[0] + randNameExt;
                }
                else
                {
                    database.Name = owner + randNameExt;
                }

                database.Username = owner + randNameExt;
                database.Password = RandomPassword.Generate(8, 8);
                database.ServerId = server.Id;
                database.Server = server;
                database.Owner = owner;
                database.Product = product;
                database.ProductId = product.Id;

                switch (product.ServerType)
                {
                    case ServerType.MsSql:
                        database.DatabaseType = DatabaseType.MsSql;
                        _repository.CreateMsSqlDatabase(database);
                        break;
                    case ServerType.MySql:
                        database.DatabaseType = DatabaseType.MySql;
                        _repository.CreateMySqlDatabase(database);
                        break;
                }

                _repository.Add(database);
                return database;
            }
            return null;


        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Database Get(int id)
        {
            return _repository.GetById(id);
        }

        public void Update(Database database)
        {
            _repository.Update(database);
            if (database.DatabaseType == DatabaseType.MsSql)
            {
                _repository.EditPasswordForMsSqlAccount(database);
            } else if (database.DatabaseType == DatabaseType.MySql)
            {
                _repository.EditPasswordForMySqlAccount(database);
            }
        }

        public void Cancel(Database database)
        {
            if (database.DatabaseType == DatabaseType.MsSql)
            {
                _repository.DeleteMsSqlDatabase(database);
            }else if (database.DatabaseType == DatabaseType.MySql)
            {
                _repository.DeleteMySqlDatabase(database);
            }
            _repository.Delete(database);
        }

        #endregion
    }
}
