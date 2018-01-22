using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using LogR.Common.Enums;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Common.Interfaces.Repository.Log
{
    public interface ILogRepository : ILogReadRepository, ILogWriteRepository
    {
    }
}