using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace LogR.Common.Models.Search
{
    public class AppLogSearchCriteria : BaseSearchCriteria
    {
        public List<SearchTerm> SearchTerms { get; set; }

        public int LogType { get; set; }

        public List<string> LogIdList { get; set; }
    }
}
