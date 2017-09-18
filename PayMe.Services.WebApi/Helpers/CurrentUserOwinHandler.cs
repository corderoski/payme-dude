using Microsoft.Owin;
using PayMe.Framework.Data;
using PayMe.Framework.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PayMe.Services.WebApi.Helpers
{

    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// Intercepts the request and manages User-specific data
    /// </summary>
    public class CurrentUserOwinHandler : OwinRequest
    {

        private readonly IAuthManagerService _authManagerService;
        private readonly AppFunc _next;

        public CurrentUserOwinHandler(AppFunc next, IAuthManagerService authManagerService)
        {
            if (next == null)
                throw new ArgumentNullException("next");

            _authManagerService = authManagerService;

            _next = next;
        }

        // given some entry points, the userId is required so request with no Id, should return 401
        public async Task Invoke(IDictionary<string, object> environment)
        {

            var serverUser = environment["server.User"];
            
            var claimsPrincipal = (ClaimsPrincipal)serverUser;
            if (null != claimsPrincipal)
            {
                var requestingUser_sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

                if (string.IsNullOrEmpty(requestingUser_sid))
                {
                    // TODO: report user not registered or authenticated
                }
                else
                {
                    var claim = claimsPrincipal.FindFirst(AppCommonConstant.USER_UNIQUE_ID_CLAIM_NAME);
                    if (claim == null)
                    {
                        
                        //claimsPrincipal.Identity.Name doesn't returns the same value
                        var identityProvider = claimsPrincipal.FindFirst(AppCommonConstant.USER_IDENTITY_PROVIDER_CLAIM);
                        var userSearchResult = await _authManagerService.VerifyAuthFromProviderAsync(identityProvider.Value, requestingUser_sid);
                        if (userSearchResult.Succeeded)
                        {
                            ((ClaimsIdentity)claimsPrincipal.Identity).AddClaim(new Claim(AppCommonConstant.USER_UNIQUE_ID_CLAIM_NAME, userSearchResult.UserId));
                        }
                        else
                        {
                            System.Diagnostics.Trace.TraceError($"Error | Auth verification launched an error: { userSearchResult.ToErrorString() }");
                        }
                    }
                }
            }

            await _next(environment);
        }

    }
}