using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class PerformanceLog : BaseLog
    {
        [JsonProperty("perf-module")]
        public string Module { get; set; }

        [JsonProperty("perf-function-name")]
        public string FunctionName { get; set; }

        [JsonProperty("perf-start-time")]
        public string StartTime { get; set; }

        [JsonProperty("elapsed-time")]
        public string ElapsedTime { get; set; }

        [JsonProperty("perf-status")]
        public string Status { get; set; }

        [JsonProperty("perf-additional-message")]
        public string AdditionalMessage { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }
    }
}
