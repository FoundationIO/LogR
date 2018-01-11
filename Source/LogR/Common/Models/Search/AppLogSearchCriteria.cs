using System.Collections.Generic;
using Framework.Infrastructure.Models.Search;

namespace LogR.Common.Models.Search
{
    public class AppLogSearchCriteria : BaseSearchCriteria
    {
        public List<SearchTerm> SearchTerms { get; set; }
        /*public int ApplicationId { get; set; }

        public virtual string CorelationId { get; set; }

        public virtual string FunctionId { get; set; }

        public virtual string Severity { get; set; }

        public virtual string App { get; set; }

        public virtual string MachineName { get; set; }

        public virtual int ProcessId { get; set; }

        public virtual int ThreadId { get; set; }

        public virtual string CurrentFunction { get; set; }

        public virtual string CurrentSourceFilename { get; set; }

        public virtual int CurrentSourceLineNumber { get; set; }

        public virtual string UserIdentity { get; set; }

        public virtual string RemoteAddress { get; set; }

        public virtual string UserAgent { get; set; }

        public virtual string Result { get; set; }

        public virtual int ResultCode { get; set; }*/
    }
}
