using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class AppLog
    {
        //[JsonProperty("log-id")]
        public virtual Guid LogId { get; set; }

        //[JsonProperty("log-type")]
        public int LogType { get; set; }

        public string ApplicationId { get; set; }

        //[JsonProperty("corelation-id")]
        public virtual string CorelationId { get; set; }

        //[JsonProperty("function-id")]
        public virtual string FunctionId { get; set; }

        //[JsonProperty("long-date")]
        //[JsonConverter(typeof(LogDateConverter))]
        public virtual DateTime Longdate { get; set; }

        //[JsonProperty("long-date")]
        //[JsonConverter(typeof(LogDateConverter))]
        public virtual DateTime ReceivedDate { get; set; }

        public virtual long ReceivedDateAsTicks { get; set; }

        //[JsonProperty("longdate-as-ticks")]
        public virtual long LongdateAsTicks { get; set; }

        //[JsonProperty("severity")]
        public virtual string Severity { get; set; }

        //[JsonProperty("app")]
        public virtual string App { get; set; }

        //[JsonProperty("module")]
        public virtual string Module { get; set; }

        //[JsonProperty("machine-name")]
        public virtual string MachineName { get; set; }

        public virtual string FunctionName { get; set; }

        //[JsonProperty("process-id")]
        public virtual int ProcessId { get; set; }

        //[JsonProperty("thread-id")]
        public virtual int ThreadId { get; set; }

        //[JsonProperty("current-function")]
        public virtual string CurrentFunction { get; set; }

        //[JsonProperty("current-source-file-name")]
        public virtual string CurrentSourceFilename { get; set; }

        //[JsonProperty("current-source-line-number")]
        public virtual int CurrentSourceLineNumber { get; set; }

        //[JsonProperty("current-tag")]
        public virtual string CurrentTag { get; set; }

        //[JsonProperty("user-identity")]
        public virtual string UserIdentity { get; set; }

        //[JsonProperty("remote-addrress")]
        public virtual string RemoteAddress { get; set; }

        //[JsonProperty("user-agent")]
        public virtual string UserAgent { get; set; }

        //[JsonProperty("result")]
        public virtual string Result { get; set; }

        //[JsonProperty("result-code")]
        public virtual int ResultCode { get; set; }

        //[JsonProperty("message")]
        public virtual string Message { get; set; }

        //[JsonProperty("start-time")]
        public virtual DateTime StartTime { get; set; }

        //[JsonProperty("elapsed-time")]
        public virtual double ElapsedTime { get; set; }

        //[JsonProperty("request")]
        public virtual string Request { get; set; }

        //[JsonProperty("response")]
        public virtual string Response { get; set; }

        //[JsonProperty("additional-properties")]
        //public List<> AdditionalProperties { get; set; }
    }
}
