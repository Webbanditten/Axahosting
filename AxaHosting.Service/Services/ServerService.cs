using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using System.Management.Automation;
using System.Threading;

namespace AxaHosting.Service
{
    public interface IServerService
    {
       
        Server GetServerByExternalIp(string ip);
        void Create(Server sever);
        void Delete(Server server);
        void EvaluateLoadOfMachines();
        List<Server> ReCalculateLoadOnServers();
        void Save();
    }

    public class ServerService : IServerService
    {
        private readonly IServerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPreparedIpRepository _preparedIpRepository;
        private readonly IIisRepository _iisRepository;

        public ServerService()
        {
            var db = new DbFactory();
            _repository = new ServerRepository(db);
            _preparedIpRepository = new PreparedIpRepository(db);
            _unitOfWork = new UnitOfWork(db);
            _iisRepository = new IisRepository();
        }

        public ServerService(IServerRepository repository, IPreparedIpRepository preparedIpRepository, IIisRepository iisRepository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._preparedIpRepository = preparedIpRepository;
            this._unitOfWork = unitOfWork;
            this._iisRepository = iisRepository;
        }

        #region ServerService Members

        public Server GetServerByExternalIp(string ip = null)
        {
            return !string.IsNullOrEmpty(ip) ? _repository.GetServerByExternalIp(ip) : null;
        }

       
        public void Create(Server entity)
        {
            _repository.Add(entity);
        }

        public void Delete(Server server)
        {
            _repository.Delete(server);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<Server> ReCalculateLoadOnServers()
        {
            var updatedServers = new List<Server>();
            var servers = _repository.GetMany(c => c.Load < 100 && c.Type == ServerType.Iis);
            foreach(var s in servers)
            {

                var amountOfSites = _iisRepository.GetAmountOfSites(s);
                s.Load = (100 / 20) * amountOfSites;
                _repository.Update(s);
                updatedServers.Add(s);
            }
            Save();
            return updatedServers;
        }
        private void CreateNewMachine()
        {
            var preparedIp = _preparedIpRepository.GetAll().FirstOrDefault();
            if(preparedIp != null)
            {
                
                using (var powerShellInstance = PowerShell.Create())
                {
                    // this script has a sleep in it to simulate a long running script
                    powerShellInstance.AddScript(@"C:\Powershell\vm-deploy.ps1 -iip "+preparedIp.InternalIp+ " -eip " + preparedIp.ExternalIp);

                    // begin invoke execution on the pipeline
                    IAsyncResult result = powerShellInstance.BeginInvoke();

                    while (result.IsCompleted == false)
                    {
                        Console.WriteLine("Waiting for pipeline to finish...");
                        Thread.Sleep(1000);

                        // might want to place a timeout here...
                    }
                }
                // Wait for VM to start
                Thread.Sleep(90000);

                // Add server to database
                _repository.Add(new Server
                {
                    ExternalIp = preparedIp.ExternalIp,
                    Load = 0,
                    Password = "Pa$$w0rd",
                    Type = ServerType.Iis,
                    InternalIp = preparedIp.InternalIp,
                    LastUpdated = DateTime.Now
                });
                _preparedIpRepository.Delete(preparedIp);
                Save();
            }
        }
        public void EvaluateLoadOfMachines()
        {
            // Check not fully loaders server for amount of IIS Sites
            var servers = ReCalculateLoadOnServers();

            if(!servers.Any(c=>c.Load < 85))
            {
                 CreateNewMachine();
            }

        }
        

        #endregion
    }
}
