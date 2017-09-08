using Newtonsoft.Json;
using PayMe.Framework.Data.DTO;

namespace PayMe.Services.WebApi.Helpers
{
    public static class SerializationHelper
    {


        public static string ToJson(this AuthResult authResult)
        {
            return JsonConvert.SerializeObject(authResult);
        }

        public static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            };
            return settings;
        }

    }
}
