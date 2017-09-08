
using Autofac;
using Microsoft.Owin.Logging;
using PayMe.Framework.Data.Context;
using PayMe.Services.WebApi.Services;
using System.Configuration;

namespace PayMe.Services.WebApi.Config
{
    public class DependencyConfigurationWebApiModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            var connString = ConfigurationManager.ConnectionStrings["StoreConnection"].ConnectionString;
            builder.RegisterType<DataContext>().As<IDataContext>().WithParameter("connectionString", connString).SingleInstance();

            builder.RegisterType<TraceLoggerService>().As<ILogger>().WithParameter("component", "Services.WebApi").InstancePerRequest();

            builder.RegisterModule<DependencyConfigurationFrameworkModule>();
        }

    }
}
