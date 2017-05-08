using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Utils
{
    public class SafeUtils
    {
        public static int Int(string obj, int defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                int result;
                if (int.TryParse((string)obj, out result))
                    return result;
                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static bool Bool(string obj, bool defaultValue = false)
        {
            if (String.IsNullOrEmpty(obj))
            {
                return defaultValue;
            }
            var bstr = obj.Trim().ToUpper();
            if ((bstr == "ON") || (bstr == "T") || (bstr == "TRUE") || (bstr == "Y") || (bstr == "YES") || (bstr == "1") || (Int(bstr) > 0))
                return true;
            return false;
        }

        public static bool Bool(object obj, bool defaultValue = false)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                var s = obj as string;
                if (s != null)
                    return Bool(s, defaultValue);
                return Convert.ToBoolean(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

    }
}
