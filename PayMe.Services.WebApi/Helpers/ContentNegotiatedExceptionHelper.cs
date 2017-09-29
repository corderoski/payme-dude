using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace PayMe.Services.WebApi.Helpers
{

    public class ContentNegotiatedExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var metadata = new
            {
                Timespan = DateTimeOffset.Now,
                Expected = context.Request.RequestUri,
                Received = "Your intention cannot be done.",
                ErrorHash = context.Request.IsLocal() ? context.Exception.Message : Guid.NewGuid().ToString(),
            };

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, metadata);
            context.Result = new ResponseMessageResult(response);
        }
    }
}