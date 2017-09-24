using Framework.Infrastructure.Models.Search;

namespace LogR.Common.Models.Search
{
    public class PerformanceLogSearchCriteria : BaseSearchCriteria
    {
        public string AppName { get; set; }

        public double? TimeTaken { get; set; }
    }
}
