using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Tables.Config;
using Owin;
using PayMe.Services.WebApi.Config;
using PayMe.Services.WebApi.Helpers;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace PayMe.Services.WebApi
{

    public partial class Startup
    {

        public static void ConfigureMobileApp(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.IgnoreRoute("IgnoreFavicon", "*.ico");
            config.Routes.MapHttpRoute(
                name: "Home",
                routeTemplate: string.Empty,
                defaults: new { controller = "Home", action = "Index" });

            config.Formatters.JsonFormatter.SerializerSettings = SerializationHelper.GetJsonSerializerSettings();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"));

            config.Services.Replace(typeof(IExceptionHandler), new ContentNegotiatedExceptionHandler());

            new MobileAppConfiguration()
                .MapApiControllers()
                .AddTables(
                    new MobileAppTableConfiguration()
                        .MapTableControllers()
                        .AddEntityFramework()
                    )
                .ApplyTo(config);

            #region Dependency Injection

            var builder = new ContainerBuilder();

            builder.RegisterModule<DependencyConfigurationFrameworkModule>();
            builder.RegisterModule<DependencyConfigurationWebApiModule>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            var container = builder.Build();
            app.UseAutofacMiddleware(container);
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            #endregion

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseOwinExceptionHandler();
            app.Use<CurrentUserOwinHandler>(container.Resolve<Framework.Services.IAuthManagerService>());
            app.UseWebApi(config);
        }

        internal const string AppName = "Services.WebApi";

    }

}

