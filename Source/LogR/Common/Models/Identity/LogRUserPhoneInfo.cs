using System;
using System.Collections.Generic;
using System.Text;

namespace LogR.Common.Models.Identity
{
    public class LogRUserPhoneInfo
    {
        public string Number { get; set; }

        public DateTime? ConfirmationTime { get; set; }

        public bool IsConfirmed => ConfirmationTime != null;

        public bool AllPropertiesAreSetToDefaults
        {
            get
            {
                return Number == null && ConfirmationTime == null;
            }
        }

        public static implicit operator LogRUserPhoneInfo(string input)
            => new LogRUserPhoneInfo { Number = input };
    }
}
