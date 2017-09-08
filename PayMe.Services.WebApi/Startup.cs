using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PayMe.Services.WebApi.Startup))]

namespace PayMe.Services.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }

    }
}