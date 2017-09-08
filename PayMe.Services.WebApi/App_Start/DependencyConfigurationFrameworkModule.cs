using Autofac;
using AutoMapper;
using PayMe.Framework.Services;
using System.Linq;

namespace PayMe.Services.WebApi.Config
{
    public class DependencyConfigurationFrameworkModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            var mappingConfiguration = CustomMapperInitializer.GetMappingConfigurations();
            builder.RegisterInstance<IMapper>(mappingConfiguration.CreateMapper());

            // implementations, contracts
            builder.RegisterAssemblyTypes(ThisAssembly, typeof(IAuthManagerService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

    }
}
