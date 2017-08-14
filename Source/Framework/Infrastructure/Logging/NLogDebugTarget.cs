using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;

namespace Framework.Infrastructure.Logging
{
    [Target("NLogDebugTarget")]
    public sealed class NLogDebugTarget : TargetWithLayout
    {
        public NLogDebugTarget()
        {
        }

        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);
            Debug.WriteLine(logMessage);
        }

    }
}
