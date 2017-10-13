using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LogR.Common.Models.Logs;

namespace LogR.Common.Models.Mapper
{
    public static class LogModelMapper
    {
        static LogModelMapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AppLog, PerfLog>();
                cfg.CreateMap<AppLog, WebLog>();
                cfg.CreateMap<AppLog, EventLog>();
            });
        }

        public static PerfLog ToPerfLog(this AppLog appLog)
        {
            return AutoMapper.Mapper.Map<PerfLog>(appLog);
        }

        public static WebLog ToWebLog(this AppLog appLog)
        {
            return AutoMapper.Mapper.Map<WebLog>(appLog);
        }

        public static EventLog ToEventLog(this AppLog appLog)
        {
            return AutoMapper.Mapper.Map<EventLog>(appLog);
        }

        public static List<PerfLog> ToPerfLogs(this List<AppLog> appLogList)
        {
            return AutoMapper.Mapper.Map<List<PerfLog>>(appLogList);
        }

        public static List<WebLog> ToWebLogs(this List<AppLog> appLogList)
        {
            return AutoMapper.Mapper.Map<List<WebLog>>(appLogList);
        }

        public static List<EventLog> ToEventLogs(this List<AppLog> appLogList)
        {
            return AutoMapper.Mapper.Map<List<EventLog>>(appLogList);
        }
    }
}
