using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class AppLog
    {
        [JsonProperty("applog-id")]
        public string AppLogId { get; set; }

        [JsonProperty("log-type")]
        public int LogType { get; set; }

        [JsonProperty("corelation-id")]
        public string CorelationId { get; set; }

        [JsonProperty("long-date")]
        //[JsonConverter(typeof(LogDateConverter))]
        public DateTime Longdate { get; set; }

        [JsonProperty("longdate-as-ticks")]
        public long LongdateAsTicks { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("app")]
        public string App { get; set; }

        [JsonProperty("machine-name")]
        public string MachineName { get; set; }

        [JsonProperty("process-id")]
        public string ProcessId { get; set; }

        [JsonProperty("thread-id")]
        public string ThreadId { get; set; }

        [JsonProperty("current-tag")]
        public string CurrentTag { get; set; }

        [JsonProperty("current-function")]
        public string CurrentFunction { get; set; }

        [JsonProperty("current-source-file-name")]
        public string CurrentSourceFilename { get; set; }

        [JsonProperty("current-source-line-number")]
        public string CurrentSourceLineNumber { get; set; }

        [JsonProperty("user-identity")]
        public string UserIdentity { get; set; }

        [JsonProperty("remote-addrress")]
        public string RemoteAddress { get; set; }

        [JsonProperty("user-agent")]
        public string UserAgent { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("result-code")]
        public int ResultCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("perf-module")]
        public string PerfModule { get; set; }

        [JsonProperty("perf-function-name")]
        public string PerfFunctionName { get; set; }

        [JsonProperty("start-time")]
        public string StartTime { get; set; }

        [JsonProperty("elapsed-time")]
        public string ElapsedTime { get; set; }

        [JsonProperty("perf-status")]
        public string PerfStatus { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("additional-properties")]
        public List<KeyValuePair<string, string>> AdditionalProperties { get; set; }
    }
}
