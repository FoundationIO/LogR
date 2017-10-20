using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Models.Identity
{
    public class LogRIdentityRole
    {
        private readonly List<LogRUserClaim> _claims;

        public LogRIdentityRole()
        {
            _claims = new List<LogRUserClaim>();
        }

        public string Id { get; internal set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public IEnumerable<LogRUserClaim> Claims
        {
            get => _claims;
            internal set
            {
                if (value != null)
                    _claims.AddRange(value);
            }
        }

        public static implicit operator LogRIdentityRole(string input) =>
            input == null ? null : new LogRIdentityRole { Name = input };

        public void AddClaim(LogRUserClaim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            _claims.Add(claim);
        }

        public void RemoveClaim(LogRUserClaim claim)
        {
            _claims.Remove(claim);
        }
    }
}
