using Framework.Infrastructure.Models.Search;

namespace LogR.Common.Models.Search
{
    public class AppLogSearchCriteria : BaseSearchCriteria
    {
        public string AppName { get; set; }

        public string LogType { get; set; }
    }
}
