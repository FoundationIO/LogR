using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
    }
}
