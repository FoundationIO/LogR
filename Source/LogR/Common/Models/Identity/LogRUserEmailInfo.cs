using System;
using System.Collections.Generic;
using System.Text;

namespace LogR.Common.Models.Identity
{
    public class LogRUserEmailInfo
    {
        public string Address { get; set; }

        public string NormalizedAddress { get; set; }

        public DateTime? ConfirmationTime { get; set; }

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
