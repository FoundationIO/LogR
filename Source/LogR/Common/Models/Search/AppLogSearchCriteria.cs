using Framework.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Common.Models.Search
{
    public class AppLogSearchCriteria : BaseSearchCriteria
    {
        public String AppName { get; set; }
        public String LogType { get; set; }
    }
}
