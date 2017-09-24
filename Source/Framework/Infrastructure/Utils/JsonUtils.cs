using Newtonsoft.Json;

namespace Framework.Infrastructure.Utils
{
    public class JsonUtils
    {
        public static T Deserialize<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
