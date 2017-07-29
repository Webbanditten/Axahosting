using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AxaHosting.Web.App_Start.Startup))]

namespace AxaHosting.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("AxaHostingHangfire");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}
