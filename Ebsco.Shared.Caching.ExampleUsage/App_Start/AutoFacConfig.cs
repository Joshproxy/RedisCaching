
using System.Web.Mvc;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Integration.Mvc;
using Ebsco.Shared.Caching.ExampleUsage.ServiceAbstractions;
using Ebsco.Shared.Caching.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Moq;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.ExampleUsage.App_Start
{
    public class AutoFacConfig
    {

        public static void RegisterAutoFac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            // Similar to what would actually be used
            //builder.Register(c => ConnectionMultiplexer.Connect("localhost").GetDatabase())
            //    .As<IDatabase>()
            //    .SingleInstance();

            var mockRedisDatabase = new Mock<IDatabase>();
            builder.RegisterInstance(mockRedisDatabase.Object)
                .As<IDatabase>().SingleInstance();

            var mockLogger = new Mock<IErrorLogger>();
            builder.RegisterInstance(mockLogger.Object)
                .As<IErrorLogger>().SingleInstance();

            builder.RegisterType<ExampleServiceCaching>()
                .As<IExampleServiceCaching>()
                .SingleInstance();

            builder.RegisterType<ExampleService>()
                .As<IExampleService>()
                .SingleInstance();

            var container = builder.Build();
            var locator = new AutofacServiceLocator(container);

            ServiceLocator.SetLocatorProvider(() => locator);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}