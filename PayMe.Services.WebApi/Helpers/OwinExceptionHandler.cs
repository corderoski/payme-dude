using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using PayMe.Services.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PayMe.Services.WebApi.Helpers
{

    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinExceptionHandlerMiddleware
    {
        private readonly AppFunc _next;

        public OwinExceptionHandlerMiddleware(AppFunc next)
        {
            if (next == null)
                throw new ArgumentNullException("next");

            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            try
            {
                await _next(environment);
            }
            catch (Exception ex)
            {
                try
                {
                    var owinContext = new OwinContext(environment);
                    HandleException(ex, owinContext);

                    return;
                }
                catch (Exception)
                {
                    // If there's a Exception while generating the error page, re-throw the original exception.
                }
                throw;
            }
        }
        private void HandleException(Exception ex, IOwinContext context)
        {
            var request = context.Request;

            var model = TraceLoggerService.WriteAndFormat($"UnexpectedBehaviour", ex);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ReasonPhrase = "Internal Server Error";
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(model));
        }

    }

    public static class OwinExceptionHandlerMiddlewareAppBuilderExtensions
    {
        public static void UseOwinExceptionHandler(this IAppBuilder app)
        {
            app.Use<OwinExceptionHandlerMiddleware>();
        }
    }
}