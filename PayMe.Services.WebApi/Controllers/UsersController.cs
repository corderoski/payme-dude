using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Login;
using Microsoft.Azure.Mobile.Server.Tables;
using Microsoft.Owin.Logging;
using Newtonsoft.Json.Linq;
using PayMe.Framework.Data.Context;
using PayMe.Framework.Data.DTO;
using PayMe.Framework.Data.Entities;
using PayMe.Framework.Services;
using PayMe.Services.WebApi.Helpers;
using System;
using System.Configuration;
using System.Dynamic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PayMe.Services.WebApi.Controllers
{

    [MobileAppController]
    public class UsersController : ApiController
    {

        private readonly ILogger _logger;
        private readonly IDataContext _dataContext;
        private readonly IAuthManagerService _authManagerService;

        private IDomainManager<User> _userDomainManager;

        public UsersController(ILogger logger, IDataContext dataContext, IAuthManagerService authManagerService)
        {
            _logger = logger;
            _dataContext = dataContext;
            _authManagerService = authManagerService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checksum">Device Unique Id or UserId</param>
        /// <returns></returns>
        [HttpGet, Route("api/users/{checksum}/auth")]
        [ResponseType(typeof(AuthResult))]
        public async Task<IHttpActionResult> ValidateAccountAsync([FromUri]string checksum)
        {
            _logger.WriteInformation($"Verification for {checksum}.");

            try
            {
                var result = await _authManagerService.VerifyAuthAsync(checksum);
                if (result.Succeeded)
                {
                    result.Token = this.GetAuthenticationTokenForUser(result.UserId);
                    return Ok(result);
                }
                else
                {
                    if (result.ResultCode == AuthResultCode.NotFound)
                        return NotFound();
                    else
                        return BadRequest(result.ToErrorString());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("api/users")]
        [ResponseType(typeof(AuthResult))]
        public async Task<IHttpActionResult> CreateUserAsync([FromBody]JToken body)
        {
            var model = body.ToObject<RegisterAuth>();
            model.ClientIP = Request.GetClientIp();

            _userDomainManager = ApiControllerExtensions.GetAzureDomainManager<User>(_dataContext, Request);
            try
            {
                var result = await _authManagerService.RegisterAuthAsync(_userDomainManager, model);
                if (result.Succeeded)
                {
                    result.Token = this.GetAuthenticationTokenForUser(result.UserId);
                    return Ok(result);
                }
                else
                    return BadRequest(result.ToErrorString());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private string GetAuthenticationTokenForUser(string userId)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Actort, userId),
            };

            var signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            var audience = ConfigurationManager.AppSettings["ValidAudience"];
            var issuer = ConfigurationManager.AppSettings["ValidIssuer"];

            try
            {
                //JwtSecurityToken token = AppServiceLoginHandler.CreateToken(claims, signingKey, audience, issuer, TimeSpan.FromDays(5));
                //return token;
                return string.Empty;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
