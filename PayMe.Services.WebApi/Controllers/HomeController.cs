using Microsoft.Azure.Mobile.Server.Content;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMe.Services.WebApi.Controllers
{
    // ref: https://github.com/Azure/azure-mobile-apps-net-server/blob/master/src/Microsoft.Azure.Mobile.Server.Home/Controller/HomeController.cs
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Index()
        {
            return new StaticHtmlActionResult("PayMe.Services.WebApi.Startup.html");
        }
    }
}