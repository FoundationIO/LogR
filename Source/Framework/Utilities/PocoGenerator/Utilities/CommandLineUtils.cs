using System;

namespace Framework.Utilities.PocoGenerator.Utilities
{
    public static class CommandLineUtils
    {
        public static bool IsParamAvailable(this string[] args, string paramName)
        {
            if (args == null)
            {
                return false;
            }

            for (var i = 0; i < args.Length; ++i)
            {
                if (args[i].ToLower().Trim() == paramName.ToLower().Trim())
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsParamValueAvailable(this string[] args, string paramName)
        {
            if (args == null)
            {
                return false;
            }

            for (var i = 0; i < args.Length; ++i)
            {
                if (args[i].ToLower() == paramName.ToLower())
                {
                    if (i + 1 <= args.Length)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string GetParamValueAsString(this string[] args, string paramName, string defaultValue = "")
        {
            if (args == null)
            {
                return defaultValue;
            }

            for (var i = 0; i < args.Length; ++i)
            {
                if (args[i].ToLower() == paramName.ToLower())
                {
                    if (i + 1 < args.Length)
                    {
                        var val = args[i + 1];
                        return val;
                    }
                }
            }

            return defaultValue;
        }

        public static T GetParamValueAs<T>(this string[] args, string paramName, T defaultValue)
        {
            var paramValue = GetParamValueAsString(args, paramName);
            try
            {
                var t = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                return (T)Convert.ChangeType(paramValue, t);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
