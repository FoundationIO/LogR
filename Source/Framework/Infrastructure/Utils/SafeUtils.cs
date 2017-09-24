using System;

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
            if (string.IsNullOrEmpty(obj))
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

        public static ushort UShort(string obj, ushort defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                ushort result;
                if (ushort.TryParse((string)obj, out result))
                    return result;
                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static short Short(object obj, short defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt16(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static long Long(object obj, long defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToInt64(obj);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static decimal Decimal(string obj, decimal defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                decimal result;
                if (decimal.TryParse(obj, out result))
                    return result;
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static float Float(object obj, float defaultValue = 0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToSingle(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double Double(object obj, double defaultValue = 0.0)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T Enum<T>(string enumString, object defaultValue)
        {
            var enumValue = default(T);

            if (!string.IsNullOrEmpty(enumString))
            {
                try
                {
                    enumValue = (T)System.Enum.Parse(typeof(T), enumString);
                }
                catch (Exception)
                {
                    if (defaultValue != null)
                        enumValue = (T)System.Enum.ToObject(typeof(T), defaultValue);
                }
            }
            else
            {
                if (defaultValue != null)
                    enumValue = (T)System.Enum.ToObject(typeof(T), defaultValue);
            }

            return enumValue;
        }

        public static T Enum<T>(int enumValue, object defaultValue)
        {
            var enumRet = default(T);

            try
            {
                enumRet = (T)System.Enum.ToObject(typeof(T), enumValue);
            }
            catch (Exception)
            {
                enumRet = (T)System.Enum.ToObject(typeof(T), defaultValue);
            }

            return enumRet;
        }

        public static object Enum(Type enumType, string enumString, object defaultValue)
        {
            object enumValue = null;

            if (!string.IsNullOrEmpty(enumString))
            {
                try
                {
                    enumValue = System.Enum.Parse(enumType, enumString);
                }
                catch (Exception)
                {
                    if (defaultValue != null)
                        enumValue = System.Enum.ToObject(enumType, defaultValue);
                }
            }
            else
            {
                if (defaultValue != null)
                    enumValue = System.Enum.ToObject(enumType, defaultValue);
            }

            return enumValue;
        }

        public static object Enum(Type enumType, int enumValue, object defaultValue)
        {
            object enumRet = null;

            try
            {
                enumRet = System.Enum.ToObject(enumType, enumValue);
            }
            catch (Exception)
            {
                enumRet = System.Enum.ToObject(enumType, defaultValue);
            }

            return enumRet;
        }

        public static Guid Guid(string value, System.Guid defaultValue)
        {
            try
            {
                Guid result;
                if (System.Guid.TryParse(value, out result))
                    return result;
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static Guid Guid(string value)
        {
            return Guid(value, System.Guid.NewGuid());
        }
    }
}
