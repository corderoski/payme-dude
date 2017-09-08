using Autofac;
using PayMe.Framework.Data.Context;
using PayMe.Services.WebApi.Config;
using System.Configuration;

namespace PayMe.Tests
{
    public class CompositionRoot
    {

        private static IContainer container;

        internal static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = GetContainer();
                }
                return container;
            }
        }

        public static IContainer GetContainer()
        {
            var container = new Autofac.ContainerBuilder();

            container.RegisterModule<DependencyConfigurationFrameworkModule>();
            container.RegisterModule<DependencyConfigurationWebApiModule>();

            return container.Build();
        }

    }
}
