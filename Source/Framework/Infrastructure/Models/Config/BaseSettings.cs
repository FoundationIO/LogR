using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class BaseSettings
    {
        public BaseSettings(IConfiguration configuration)
        {
            var propList = this.GetType().GetProperties();

            var valueList = configuration.GetChildren().AsEnumerable().ToList();

            foreach (var prop in propList)
            {
                var value = valueList.FirstOrDefault(x => x.Key.ToLower().Trim() == prop.Name.ToLower().Trim());
                if (value.Key.IsTrimmedStringNullOrEmpty() || value.Value.IsTrimmedStringNullOrEmpty())
                    continue;

                ReflectionUtils.SetPropertyValueFromString(this, prop, value.Value, null);
            }
        }
    }
}
