using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Model;
using System.Management;
using System.Security.Cryptography.X509Certificates;

namespace AxaHosting.Service
{
    public interface IDomainService
    {
        void CreateARecordsForDomain(string domain, string ip);
        void CreateMxRecordsForDomain(string domain, string mailServer);
        bool IsDomainTaken(string domain);
        void Connect();
        void CreateRecordsForDomain(WebHotel webhotel);
    }

    public class DomainService : IDomainService
    {
        #region Members
        private ManagementScope _session = null;
        public string Server = null;
        public string User = null;
        private readonly string _password = null;

        #endregion
        public DomainService(string serverName = null, string userName = null, string password = null)
        {
            if (serverName == null && userName == null && password == null)
            {
                this.Server = ConfigurationManager.AppSettings["dnsServer"];
                this.User = ConfigurationManager.AppSettings["dnsAccountUsername"];
                this._password = ConfigurationManager.AppSettings["dnsAccountPassword"];
            }
            //this.Connect();
        }

        public void Connect()
        {
            if(_session == null)
            {
                this.Logon();
                this.Initialize();
            }
        }
        private void Logon()
        {
            this.NameSpace = "\\\\" + this.Server + "\\root\\microsoftdns";
            var con = new ConnectionOptions
            {
                Username = this.User,
                Password = this._password,
                Impersonation = ImpersonationLevel.Impersonate
            };
            this._session = new ManagementScope(this.NameSpace) { Options = con };
            this._session.Connect();
        }
        private void Initialize()
        {
        }
        #region Methods
        public void Dispose()
        {
        }
        public void Dispose(ref ManagementClass x)
        {
            if (x == null) return;
            x.Dispose();
            x = null;
        }
        public void Dispose(ref ManagementBaseObject x)
        {
            if (x == null) return;
            x.Dispose();
            x = null;
        }
        public bool DomainExists(string domainName)
        {
            var retval = false;
            var wql = "";
            wql = "SELECT *";
            wql += " FROM MicrosoftDNS_ATYPE";
            wql += " WHERE OwnerName = '" + domainName + "'";
            var q = new ObjectQuery(wql);
            var s = new ManagementObjectSearcher(this._session, q);
            var col = s.Get();
            var total = col.Count;
            foreach (var o in col)
            {
                retval = true;
            }
            return retval;
        }
        public void AddDomain(string domainName, string ipDestination)
        {
            //check if domain already exists
            if (this.DomainExists(domainName))
            {
                throw new Exception("The domain you are trying to add already exists on this server!");
            }
            //generate zone
            var man = this.Manage("MicrosoftDNS_Zone");
            ManagementBaseObject ret = null;
            var obj = man.GetMethodParameters("CreateZone");
            obj["ZoneName"] = domainName;
            obj["ZoneType"] = 0;
            //invoke method, dispose unneccesary vars
            man.InvokeMethod("CreateZone", obj, null);
            this.Dispose(ref obj);
            this.Dispose(ref ret);
            this.Dispose(ref man);
            //add rr containing the ip destination
            this.AddARecord(domainName, null, ipDestination);
        }
        public void RemoveDomain(string domainName)
        {
            var wql = "";
            wql = "SELECT *";
            wql += " FROM MicrosoftDNS_Zone";
            wql += " WHERE Name = '" + domainName + "'";
            var q = new ObjectQuery(wql);
            var s = new ManagementObjectSearcher(this._session, q);
            var col = s.Get();
            var total = col.Count;
            foreach (var o in col.Cast<ManagementObject>())
            {
                o.Delete();
            }
        }
        public void AddARecord(string domain, string recordName, string ipDestination)
        {
            if (this.DomainExists(recordName + "." + domain))
            {
                throw new Exception("That record already exists!");
            }
            var man = new ManagementClass(this._session, new ManagementPath("MicrosoftDNS_ATYPE"), null);
            var vars = man.GetMethodParameters("CreateInstanceFromPropertyData");
            vars["DnsServerName"] = this.Server;
            vars["ContainerName"] = domain;
            if (recordName == null)
            {
                vars["OwnerName"] = domain;
            }
            else
            {
                vars["OwnerName"] = recordName + "." + domain;
            }
            vars["IPAddress"] = ipDestination;
            man.InvokeMethod("CreateInstanceFromPropertyData", vars, null);
        }
        public void AddMxRecord(string domain, string recordName, string mailServer)
        {
            if (this.DomainExists(recordName + "." + domain))
            {
                throw new Exception("That record already exists!");
            }
            var man = new ManagementClass(this._session, new ManagementPath("MicrosoftDNS_MXType"), null);
            var vars = man.GetMethodParameters("CreateInstanceFromPropertyData");
            vars["DnsServerName"] = this.Server;
            vars["ContainerName"] = domain;
            if (recordName == null)
            {
                vars["OwnerName"] = domain;
            }
            else
            {
                vars["OwnerName"] = recordName + "." + domain;
            }
            vars["MailExchange"] = mailServer;
            vars["Preference"] = 10;
            man.InvokeMethod("CreateInstanceFromPropertyData", vars, null);
        }
        public void RemoveARecord(string domain, string aRecord)
        {
            var wql = "";
            wql = "SELECT *";
            wql += " FROM MicrosoftDNS_ATYPE";
            wql += " WHERE OwnerName = '" + aRecord + "." + domain + "'";
            var q = new ObjectQuery(wql);
            var s = new ManagementObjectSearcher(this._session, q);
            var col = s.Get();
            var total = col.Count;
            foreach (var o in col.Cast<ManagementObject>())
            {
                o.Delete();
            }
        }
        public void RemoveMxRecord(string domain, string aRecord)
        {
            var wql = "";
            wql = "SELECT *";
            wql += " FROM MicrosoftDNS_ATYPE";
            wql += " WHERE OwnerName = '" + aRecord + "." + domain + "'";
            var q = new ObjectQuery(wql);
            var s = new ManagementObjectSearcher(this._session, q);
            var col = s.Get();
            var total = col.Count;
            foreach (var o in col.Cast<ManagementObject>())
            {
                o.Delete();
            }
        }
        #endregion
        #region Properties
        public string NameSpace { get; private set; } = null;

        public bool Enabled
        {
            get
            {
                const bool retval = false;
                try
                {
                    var wql = new SelectQuery { QueryString = "" };
                }
                catch
                {
                    // ignored
                }
                return retval;
            }
        }
        public ManagementClass Manage(string path)
        {
            //ManagementClass retval=new ManagementClass(path);
            var retval = new ManagementClass(this._session, new ManagementPath(path), null);
            return retval;
        }
        #endregion

        public void CreateARecordsForDomain(string domain, string ip)
        {
            AddDomain(domain, ip);
            AddARecord(domain, "www", ip);
        }

        public void CreateMxRecordsForDomain(string domain, string mailServer)
        {
            AddMxRecord(domain, null, mailServer);
        }

        public bool IsDomainTaken(string domain)
        {
            return DomainExists(domain);
        }

        public void CreateRecordsForDomain(WebHotel webhotel)
        {
            CreateARecordsForDomain(webhotel.Domain, webhotel.Server.ExternalIp);
            CreateMxRecordsForDomain(webhotel.Domain, ConfigurationManager.AppSettings["mailServer"]);
        }
    }
}
