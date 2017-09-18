using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using PayMe.Framework.Data;
using PayMe.Framework.Data.Context;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;

namespace PayMe.Services.WebApi.Controllers
{
    public static class ApiControllerExtensions
    {

        public static IDomainManager<TEntity> GetAzureDomainManager<TEntity>(IDataContext dataContext, HttpRequestMessage request) where TEntity : class, ITableData
        {
            var dbContext = dataContext as System.Data.Entity.DbContext;
            return new EntityDomainManager<TEntity>(dbContext, request);
        }

        public static string GetClientIp(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }

            var env_remoteIp = request.GetOwinEnvironment()["server.RemoteIpAddress"];
            return env_remoteIp?.ToString() ?? "";
        }

        public static string GetUserId(this ApiController controller)
        {
            ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal)controller.User;

            if (null != claimsPrincipal)
            {
                var claim = claimsPrincipal.FindFirst(AppCommonConstant.USER_UNIQUE_ID_CLAIM_NAME);
                return claim?.Value;
            }

            return null;
        }

    }
}