using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class AppLog : BaseLog
    {
        [JsonProperty("sql-result")]
        public string SqlResult { get; set; }

        [JsonProperty("elapsed-time")]
        public string ElapsedTime { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
