using System;
using System.Collections.Generic;
using System.Globalization;
using Framework.Infrastructure.Utils;
using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class BaseLog
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("long-date")]
        [JsonConverter(typeof(LogDateConverter))]
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

        [JsonProperty("additional-properties")]
        public KeyValuePair<string,object> AdditionalProperties { get; set; }
    }
}
