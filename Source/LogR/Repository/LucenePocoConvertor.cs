using LogR.Common.Models.Logs;
using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Repository
{
    public class LucenePocoConvertor
    {

        public Document ToLuceneDocument(AppLog perfLog)
        {
            Document result = new Document();
            return result;
        }

        public AppLog ToAppLog(Document doc)
        {
            AppLog result = new AppLog();
            return result;
        }

        public Document ToLuceneDocument(PerformanceLog perfLog)
        {
            Document result = new Document();
            return result;
        }
        public PerformanceLog ToPerformanceLog(Document doc)
        {
            PerformanceLog result = new PerformanceLog();
            return result;
        }

    }
}
