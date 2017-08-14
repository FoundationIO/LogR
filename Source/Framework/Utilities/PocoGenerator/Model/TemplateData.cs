using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Utilities.PocoGenerator
{
    public class TemplateData
    {
        public Config Config { get; set; }
        public DatabaseSchema DatabaseSchema { get; set; }
    }
}
