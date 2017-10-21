using System;
using System.Collections.Generic;
using System.Linq;

namespace LogR.Common.Models.Identity
{
    public class LogRIdentityUser
    {
        private readonly List<LogRUserLoginInfo> _logins;
        private readonly List<LogRUserClaim> _claims;

        public LogRIdentityUser()
        {
            _logins = new List<LogRUserLoginInfo>();
            _claims = new List<LogRUserClaim>();
        }

        public LogRIdentityUser(string userName)
            : this()
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }

        public string Id { get; internal set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public LogRUserEmailInfo Email { get; set; }

        public string PasswordHash { get; set; }

        public bool UsesTwoFactorAuthentication { get; set; }

        public IEnumerable<LogRUserLoginInfo> Logins
        {
            get => _logins;
            internal set
            {
                if (value != null)
                    _logins.AddRange(value);
            }
        }

        public IEnumerable<LogRUserClaim> Claims
        {
            get => _claims;
            internal set
            {
                if (value != null)
                    _claims.AddRange(value);
            }
        }

        public string SecurityStamp { get; set; }

        public LogRUserLockoutInfo Lockout { get; set; }

        public LogRUserPhoneInfo Phone { get; set; }

        public void AddLogin(LogRUserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (_logins.Any(l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey))
            {
                throw new InvalidOperationException("There is a login with the same provider already exists.");
            }

            _logins.Add(login);
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            var loginToRemove = _logins.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);

            if (loginToRemove == null)
                return;

            _logins.Remove(loginToRemove);
        }

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

        public void CleanUp()
        {
            if (Lockout != null && Lockout.AllPropertiesAreSetToDefaults)
            {
                Lockout = null;
            }

            if (Email != null && Email.AllPropertiesAreSetToDefaults)
            {
                Email = null;
            }

            if (Phone != null && Phone.AllPropertiesAreSetToDefaults)
            {
                Phone = null;
            }
        }
    }
}
