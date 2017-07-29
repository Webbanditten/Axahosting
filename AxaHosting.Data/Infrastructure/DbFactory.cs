using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxaHosting.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        AxaHostingEntities _dbContext;

        public AxaHostingEntities Init()
        {
            return _dbContext ?? (_dbContext = new AxaHostingEntities());
        }

        protected override void DisposeCore()
        {
            _dbContext?.Dispose();
        }
    }
}
