using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Code.Models.Logs
{
    public class PerformanceLog : BaseLog
    {
        [JsonProperty("perf-module")]
        public String Module { get; set; }
        [JsonProperty("perf-function-name")]
        public String FunctionName { get; set; }
        [JsonProperty("perf-start-time")]
        public String StartTime { get; set; }
        [JsonProperty("elapsed-time")]
        public String ElapsedTime { get; set; }
        [JsonProperty("perf-status")]
        public String Status { get; set; }
        [JsonProperty("perf-additional-message")]
        public String AdditionalMessage { get; set; }

        [JsonProperty("request")]
        public String Request { get; set; }

        [JsonProperty("response")]
        public String Response { get; set; }
    }
}
