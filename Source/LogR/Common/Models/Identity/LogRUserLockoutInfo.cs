using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LogR.Common.Models.Identity
{
    public class LogRUserLockoutInfo
    {
        public DateTimeOffset? EndDate { get; set; }

        public bool Enabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool AllPropertiesAreSetToDefaults =>
            EndDate == null &&
            Enabled == false &&
            AccessFailedCount == 0;
    }
}
