using System;
using System.Security.Claims;

namespace LogR.Common.Models.Identity
{
    public class LogRUserClaim : IEquatable<LogRUserClaim>, IEquatable<Claim>
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public static implicit operator LogRUserClaim(Claim original) =>
            new LogRUserClaim { Type = original.Type, Value = original.Value };

        public static implicit operator Claim(LogRUserClaim simplified) =>
            new Claim(simplified.Type, simplified.Value);

        public bool Equals(LogRUserClaim other)
            => Type == other.Type && Value == other.Value;

        public bool Equals(Claim other)
            => Type == other.Type && Value == other.Value;
    }
}