using System;
using System.Collections.Generic;
using System.Globalization;
using Framework.Infrastructure.Utils;
using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class LogDateConverter : JsonConverter
    {
        private static string format = @"yyyy-MM-dd HH:mm:ss.ffff";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if ((existingValue != null) && DateUtils.IsValidDate((DateTime)existingValue))
                return existingValue;
            try
            {
                var dt = DateTime.ParseExact((string)reader.Value, format, CultureInfo.InvariantCulture);
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            catch
            {
                return existingValue;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
