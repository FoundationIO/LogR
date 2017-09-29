using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Enums
{
    public enum IndexStoreType
    {
        None = 0,

        Lucene = 10,

        Sqlite3 = 20,

        MySql = 21,

        SqlServer = 22,

        Postgresql = 23,

        MongoDB = 30,

        RaptorDB = 31,

        ElasticSearch = 40,

        EmbbededElasticSearch = 41,
    }
}
