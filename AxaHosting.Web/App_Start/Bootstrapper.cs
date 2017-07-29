using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AxaHosting.Data.Infrastructure;
using AxaHosting.Data.Repositories;
using AxaHosting.Service;
using Hangfire;

namespace AxaHosting.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            

            // Repositories
           builder.RegisterAssemblyTypes(
                typeof(ServerRepository).Assembly,
               typeof(ActiveDirectoryRepository).Assembly,
                typeof(DatabaseRepository).Assembly,
                typeof(ProductRepository).Assembly,
                typeof(ServerRepository).Assembly,
                typeof(WebHotelRepository).Assembly,
                typeof(IisRepository).Assembly,
                typeof(FtpRepository).Assembly,
                typeof(MailRepository).Assembly,
                typeof(PreparedIpRepository).Assembly
                )
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest(); ;

            // Services
            builder.RegisterAssemblyTypes(
                typeof(ServerService).Assembly, 
                typeof(ActiveDirectoryService).Assembly,
                typeof(MailService).Assembly,
                typeof(DomainService).Assembly,
                typeof(WebHotelService).Assembly,
                typeof(ProductService).Assembly,
                typeof(DatabaseService).Assembly,
                typeof(ServerService).Assembly
                )
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            // Repositories

            /*
            builder.RegisterType<ServerRepository>().As<IServerRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<ActiveDirectoryRepository>().As<IActiveDirectoryRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<DatabaseRepository>().As<IDatabaseRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<ServerRepository>().As<IServerRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<WebHotelRepository>().As<IWebHotelRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<IisRepository>().As<IIisRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<FtpRepository>().As<IFtpRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<MailRepository>().As<IMailRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<PreparedIpRepository>().As<IPreparedIpRepository>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);

            // Services
            builder.RegisterType<ServerService>().As<IServerService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<ActiveDirectoryService>().As<IActiveDirectoryService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<MailService>().As<IMailService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<DomainService>().As<IDomainService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<WebHotelService>().As<IWebHotelService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<DatabaseService>().As<IDatabaseService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);
            builder.RegisterType<ServerService>().As<IServerService>().InstancePerRequest(AutofacJobActivator.LifetimeScopeTag);*/

            IContainer container = builder.Build();
            //GlobalConfiguration.Configuration.UseAutofacActivator(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}