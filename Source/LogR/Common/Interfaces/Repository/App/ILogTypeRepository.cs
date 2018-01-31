using System.Collections.Generic;
using LogR.Common.Models.DB;

namespace LogR.Common.Interfaces.Repository.App
{
    public interface ILogTypeRepository
    {
        List<LogTypeConfig> GetLogTypeConfig();
    }
}