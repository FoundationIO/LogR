using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Utils
{
    public static class StringUtils
    {
        public static string ToString(this List<string> obj, string sep = " ", bool useNewLine = true)
        {
            if (obj == null)
                return string.Empty;
            return ToString((IList<string>)obj.ToArray(), sep, useNewLine);
        }

        public static string ToString(this IList<string> obj, string sep = " ", bool useNewLine = true)
        {
            if (obj == null)
                return string.Empty;
            return ToString(obj.ToArray(), sep, useNewLine);
        }

        public static string ToString(this string[] obj, string sep = " ", bool useNewLine = true)
        {
            if (obj == null)
                return string.Empty;
            var sb = new StringBuilder();
            var pos = 0;
            foreach (var item in obj)
            {
                if (useNewLine)
                    sb.AppendLine(item + ((pos != obj.Length - 1) ? sep : string.Empty));
                else
                    sb.Append(item + ((pos != obj.Length - 1) ? sep : string.Empty));
                pos++;
            }

            return sb.ToString();
        }

        public static string RemoveLastCharAndAddFirstChar(this string str, char citem)
        {
            if (str.IsNullOrEmpty())
                return str;
            str = AddFirstChar(str, citem);
            str = RemoveLastChar(str, citem);
            return str;
        }

        public static string AddFirstChar(this string str, char citem)
        {
            if (str.IsNullOrEmpty())
                return str;
            if (str.FirstChar() != citem.ToString())
                str = citem.ToString() + str;
            return str;
        }

        public static string RemoveLastChar(this string str, char citem)
        {
            if (str.IsNullOrEmpty())
                return str;
            if (str.LastChar() == citem.ToString())
                str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static string RemoveFirstChar(this string str, char citem)
        {
            if (str.IsNullOrEmpty())
                return str;
            if (str.FirstChar() == citem.ToString())
                str = str.Substring(1);
            return str;
        }

        public static string FlattenString(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            str = str.Replace('\n', ' ');
            str = str.Replace('\t', ' ');
            str = str.Replace('\r', ' ');
            return str;
        }

        public static string FirstChar(this string str)
        {
            if (str.Length > 0)
                return str[0].ToString();
            return string.Empty;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string FormatString(this string fmt, params object[] args)
        {
            return string.Format(fmt, args);
        }

        public static string LastChar(this string str)
        {
            if (str.Length > 0)
                return str[str.Length - 1].ToString();
            return string.Empty;
        }

        public static bool IsTrimmedStringNullOrEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            return string.IsNullOrEmpty(str.Trim());
        }

        public static bool IsTrimmedStringNotNullOrEmpty(this string str)
        {
            return !IsTrimmedStringNullOrEmpty(str);
        }

        public static bool IsSameWithCaseIgnoreTrimmed(this string str, string anotherStr)
        {
            if ((str == null) && (anotherStr == null))
                return true;

            if ((str == null) || (anotherStr == null))
                return false;
            return str.Trim().ToLower() == anotherStr.Trim().ToLower();
        }
    }
}
