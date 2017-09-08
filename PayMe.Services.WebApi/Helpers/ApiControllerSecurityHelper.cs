using System.IdentityModel.Tokens;
using System.Linq;
using System.Web.Http;

namespace PayMe.Services.WebApi.Helpers
{

    //from: https://github.com/martinnormark/ZuMo.API/blob/master/ZuMo.API/PresentationLogic/Security/ApiControllerSecurityHelpers.cs
    public static class ApiControllerSecurityHelper
    {
        public const string ZUMO_AUTH_TOKEN_KEY = "X-ZUMO-AUTH";
 
        public static string GetAuthToken(this ApiController controller)
        {
            return controller.Request.Headers.GetValues(ZUMO_AUTH_TOKEN_KEY).FirstOrDefault();
        }

        public static JwtSecurityToken GetSecurityToken(this ApiController controller)
        {
            string authToken = controller.GetAuthToken();

            if (string.IsNullOrWhiteSpace(authToken))
            {
                return null;
            }

            return new JwtSecurityToken(authToken);
        }
    }
}