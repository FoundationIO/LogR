using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Web.Identity
{
    public class LogRIdentityRole
    {
        private readonly List<SimplifiedClaim> _claims;

        public LogRIdentityRole()
        {
            _claims = new List<SimplifiedClaim>();
        }


        public string Id { get; internal set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public IEnumerable<SimplifiedClaim> Claims
        {
            get => _claims;
            internal set
            {
                if (value != null) _claims.AddRange(value);
            }
        }

        internal void AddClaim(SimplifiedClaim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            _claims.Add(claim);
        }

        internal void RemoveClaim(SimplifiedClaim claim)
        {
            _claims.Remove(claim);
        }

        public static implicit operator LogRIdentityRole(string input) => 
            input == null ? null : new LogRIdentityRole {Name = input};
    }
}
