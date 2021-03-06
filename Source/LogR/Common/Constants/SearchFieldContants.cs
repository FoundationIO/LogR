﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Constants
{
    public class SearchFieldContants
    {
        public static class Operators
        {
            public const string Is = "is";
            public const string IsNot = "is not";
            public const string Contains = "contains";
            public const string NotContains = "not contains";
            public const string StartsWith = "starts with";
            public const string EndsWith = "ends with";

            public const string GreaterThan = ">";
            public const string LessThan = "<";
            public const string GreaterThanOrEqualTo = ">=";
            public const string LessThanOrEqualTo = "<=";
            public const string EqualTo = "==";
            public const string NotEqualTo = "!=";
        }

        public static class AppLogs
        {
            public const string Date = "LongDate";
            public const string LogLevel = "LogLevel";
            public const string Machine = "MachineName";
            public const string App = "App";
            public const string Ip = "Ip";
            public const string Username = "Username";
            public const string ElapsedTime = "ElapsedTime";
            public const string ThreadId = "ThreadId";
            public const string ProcessId = "ProcessId";
            public const string FileName = "FileName";
            public const string FunctionName = "FunctionName";
            public const string IpAddress = "IpAddress";
        }
    }
}
