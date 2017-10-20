using System;
using System.Collections.Generic;
using System.Text;

namespace LogR.Common.Models.Identity
{
    public class LogRUserEmailInfo
    {
        public string Address { get; internal set; }

        public string NormalizedAddress { get; internal set; }

        public DateTime? ConfirmationTime { get; internal set; }

        public bool IsConfirmed => ConfirmationTime != null;

        public bool AllPropertiesAreSetToDefaults
        {
            get
            {
                return Address == null && NormalizedAddress == null && ConfirmationTime == null;
            }
        }

        public static implicit operator LogRUserEmailInfo(string input)
            => new LogRUserEmailInfo { Address = input, NormalizedAddress = input };
    }
}
