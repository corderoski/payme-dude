using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PayMe.Tests.Helpers
{
    public static class SerializationHelper
    {

        public static JToken ToJToken<T>(this T model) where T : class
        {
            var json = JsonConvert.SerializeObject(model);
            return JToken.Parse(json);
        }

    }
}
