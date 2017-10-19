﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LogR.Web.Identity
{
    public class EmailInfo
    {
        public string Address { get; internal set; }

        public string NormalizedAddress { get; internal set; }

        public DateTime? ConfirmationTime { get; internal set; }
        public bool IsConfirmed => (ConfirmationTime != null);

        public static implicit operator EmailInfo(string input)
            => new EmailInfo {Address = input, NormalizedAddress = input};

        public bool AllPropertiesAreSetToDefaults =>
            Address == null &&
            NormalizedAddress == null &&
            ConfirmationTime == null;
    }
}
