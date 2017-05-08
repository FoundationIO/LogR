using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Framework.Infrastructure.Utils;

namespace LogR.Common.Models.Logs
{
    public class BaseLog
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("longdate")]
        [JsonConverter(typeof(LogDateConverter))]
        public DateTime Longdate { get; set; }
        public long LongdateAsTicks { get { return Longdate.Date.Ticks; } set { } }
        [JsonProperty("severity")]
        public String Severity { get; set; }
        [JsonProperty("app")]
        public String App { get; set; }

        [JsonProperty("machinename")]
        public String MachineName { get; set; }
        [JsonProperty("processid")]
        public String ProcessId { get; set; }
        [JsonProperty("threadid")]
        public String ThreadId { get; set; }
        [JsonProperty("current-tag")]
        public String CurrentTag { get; set; }

        [JsonProperty("current-function")]
        public String CurrentFunction { get; set; }
        [JsonProperty("current-source-file-name")]
        public String CurrentSourceFilename { get; set; }
        [JsonProperty("current-source-line-number")]
        public String CurrentSourceLineNumber { get; set; }

        [JsonProperty("aspnet-user-identity")]
        public String AspnetUserIdentity { get; set; }
        [JsonProperty("remote_addr")]
        public String RemoteAddress { get; set; }


    }

    public class LogDateConverter : JsonConverter
    {
        static string format = @"yyyy-MM-dd HH:mm:ss.ffff";

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if ((existingValue != null) && DateUtils.IsValidDate((DateTime)existingValue))
                return existingValue;
            try
            {
                var dt = DateTime.ParseExact((string)reader.Value, format, CultureInfo.InvariantCulture);
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            catch
            {
                return existingValue;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
