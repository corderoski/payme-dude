using PayMe.Services.WebApi.Helpers;
using System.Linq;
using System.Text;
using System.Web.Http.ModelBinding;

namespace PayMe.Services.WebApi.Controllers
{
    public static class ModelStateExtensions
    {

        public static string GetModelErrors(this ModelStateDictionary modelState, object model)
        {
            string messages = modelState.Values.Aggregate(new StringBuilder(), (sb, a) =>
           {
               sb.Append(string.Join(" ", a.Errors.Select(p => p.ErrorMessage)));
               return sb;
           }, sb => sb.ToString());

            var r = new { messages, model };
            return Newtonsoft.Json.JsonConvert.SerializeObject(r, SerializationHelper.GetJsonSerializerSettings());
        }
    }
}
