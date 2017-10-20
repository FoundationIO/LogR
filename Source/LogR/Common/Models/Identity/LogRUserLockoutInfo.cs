using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LogR.Common.Models.Identity
{
    public class LogRUserLockoutInfo
    {
        public DateTimeOffset? EndDate { get; internal set; }

        public bool Enabled { get; internal set; }

        public int AccessFailedCount { get; internal set; }

        public bool AllPropertiesAreSetToDefaults =>
            EndDate == null &&
            Enabled == false &&
            AccessFailedCount == 0;
    }
}
