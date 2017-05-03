using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Code.Models.Logs
{
    public class AppLog : BaseLog
    {
        [JsonProperty("sqlresult")]
        public String SqlResult { get; set; }
        [JsonProperty("elapsed-time")]
        public String ElapsedTime { get; set; }
        [JsonProperty("message")]
        public String Message { get; set; }
    }
}
