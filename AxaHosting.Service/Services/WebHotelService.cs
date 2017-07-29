using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using AxaHosting.Service.util;
using System.Configuration;

namespace AxaHosting.Service
{
    public interface IWebHotelService
    {

        WebHotel GetWebHotel(int webHotelId);
        WebHotel Create(Product entity, string domainName, string owner);
        IEnumerable<WebHotel> GetWebHotelsByOwner(string owner);
        void Save();
        void ChangeFtp(WebHotel webHotel);
        void Update(WebHotel webHotel);
        void Cancel(WebHotel webHotel);
    }

    public class WebHotelService : IWebHotelService
    {
        private readonly IWebHotelRepository _repository;
        private readonly IIisRepository _iisRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServerRepository _serverRepository;
        private readonly IFtpRepository _ftpRepository;
        private readonly IMailRepository _mailRepository;

        public WebHotelService(IWebHotelRepository repository, IIisRepository iisRepository, IServerRepository serverRepository, IUnitOfWork unitOfWork, IFtpRepository ftpRepository, IMailRepository mailRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _ftpRepository = ftpRepository;
            _mailRepository = mailRepository;
            _iisRepository = iisRepository;
            _serverRepository = serverRepository;
        }

        #region Members

        public WebHotel GetWebHotel(int webHotelId)
        {
            return _repository.GetById(webHotelId);
        }

       
        public WebHotel Create(Product product, string domainName, string owner)
        {
            try
            {
                
                var server = _serverRepository.GetServerByType(product.ServerType).OrderBy(c => c.Load).First();
                //var hmailServer = _serverRepository.GetServerByType(ServerType.Hmail).FirstOrDefault();

                var webhotel = new WebHotel
                {
                    Owner = owner,
                    Server = server,
                    ServerId = server.Id,
                    Domain = domainName,
                    AppPoolName = domainName,
                    FtpUsername = domainName,
                    FtpPassword = RandomPassword.Generate(8, 8),
                    Product = product,
                    ProductId = product.Id
                };



                var hMail = _mailRepository.AddDomain(ConfigurationManager.AppSettings["mailServer"], domainName);

                // SetHmailDomainId
                webhotel.HMailDomainId = hMail;


                // Create IIS
                _iisRepository.Add(webhotel);

                var iisId = _iisRepository.GetIisId(webhotel);
                if (iisId != null)
                {
                    webhotel.IisId = (int)iisId;
                }
                else
                {
                    //TODO Do some magic to revert
                    return null;
                }
                   
                // Create FTP
                _ftpRepository.Add(webhotel);

                // Add to website Database
                _repository.Add(webhotel);

                return webhotel;
            }
            catch
            {
                // ignored
                return null;
            }
            

        }



        public IEnumerable<WebHotel> GetWebHotelsByOwner(string owner)
        {
            return _repository.GetMany(c => c.Owner == owner);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void ChangeFtp(WebHotel webHotel)
        {
            _ftpRepository.Update(webHotel);
        }

        public void Update(WebHotel webHotel)
        {
            _repository.Update(webHotel);
        }

        public void Cancel(WebHotel webHotel)
        {
            _ftpRepository.Delete(webHotel);
            _iisRepository.Delete(webHotel);
            _repository.Delete(webHotel);
        }

        #endregion
    }
}
