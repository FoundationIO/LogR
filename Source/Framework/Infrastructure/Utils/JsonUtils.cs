using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            ITraceWriter traceWriter = new MemoryTraceWriter();

            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
                {
                    TraceWriter = traceWriter,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
