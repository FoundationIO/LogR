using System;
using System.Reflection;

namespace Framework.Infrastructure.Utils
{
    public class ReflectionUtils
    {
        public static void SetPropertyValueFromString(object target, PropertyInfo prop, string value, object defaultValue)
        {
            if (value == null)
            {
                if (defaultValue != null)
                    prop.SetValue(target, defaultValue, null);
                return;
            }

            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(target, value, null);
            }
            else if (prop.PropertyType == typeof(short))
            {
                prop.SetValue(target, SafeUtils.Short(value), null);
            }
            else if (prop.PropertyType == typeof(ushort))
            {
                prop.SetValue(target, SafeUtils.UShort(value), null);
            }
            else if (prop.PropertyType == typeof(int))
            {
                prop.SetValue(target, SafeUtils.Int(value), null);
            }
            else if (prop.PropertyType == typeof(long))
            {
                prop.SetValue(target, SafeUtils.Long(value), null);
            }
            else if (prop.PropertyType == typeof(float))
            {
                prop.SetValue(target, SafeUtils.Float(value), null);
            }
            else if (prop.PropertyType == typeof(double))
            {
                prop.SetValue(target, SafeUtils.Double(value), null);
            }
            else if (prop.PropertyType == typeof(bool))
            {
                prop.SetValue(target, SafeUtils.Bool(value), null);
            }
            else if (prop.PropertyType == typeof(Guid))
            {
                prop.SetValue(target, SafeUtils.Guid(value), null);
            }
            else if (prop.PropertyType == typeof(Enum))
            {
                prop.SetValue(target, value, null);
            }
            else if (prop.PropertyType.GetTypeInfo().BaseType == typeof(Enum))
            {
                var propType = prop.PropertyType;
                var safeValue = SafeUtils.Enum(propType, value, null);
                if (safeValue != null)
                    prop.SetValue(target, safeValue, null);
            }
        }

        public static T GetPrivatePropertyValue<T>(object obj, string propName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            PropertyInfo pi = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)pi.GetValue(obj, null);
        }

        public static T GetPrivateFieldValue<T>(object obj, string propName)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }

            if (fi == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            return (T)fi.GetValue(obj);
        }

        public static void SetPrivatePropertyValue<T>(object obj, string propName, T val)
        {
            Type t = obj.GetType();
            if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
            t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
        }

        public static void SetPrivateFieldValue<T>(object obj, string propName, T val)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            Type t = obj.GetType();
            FieldInfo fi = null;
            while (fi == null && t != null)
            {
                fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                t = t.BaseType;
            }

            if (fi == null)
                throw new ArgumentOutOfRangeException("propName", string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
            fi.SetValue(obj, val);
        }
    }
}
