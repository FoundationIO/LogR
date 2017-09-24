using System.Diagnostics;
using NLog;
using NLog.Targets;

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
