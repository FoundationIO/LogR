using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogR.Common.Interfaces.Service.App;
using LogR.Common.Models.Identity;

namespace LogR.Service.App
{
    public class AccountService : IAccountService
    {
        public Task<TUser> GetUserByEmailAsync<TUser>(string normalizedEmail)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public Task<IList<TUser>> GetUsersWithClaimTypeAndValueAsync<TUser>(string type, string value)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public void CreateUserAsync<TUser>(TUser user)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public void UpdateUserAsync<TUser>(TUser user)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public Task<TUser> GetUserByIdAsync<TUser>(string userId)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public Task<TUser> GetUserByNameAsync<TUser>(string normalizedUserName)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public Task<TUser> GetUserByLoginProviderAndProviderKeyAsync<TUser>(string loginProvider, string providerKey)
            where TUser : LogRIdentityUser
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TRole> GetRoleByNameAsync<TRole>(string normalizedRoleName)
            where TRole : LogRIdentityRole
        {
            throw new NotImplementedException();
        }

        public Task<TRole> GetRoleByIdAsync<TRole>(string roleId)
            where TRole : LogRIdentityRole
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task CreateRoleAsync<TRole>(TRole role)
            where TRole : LogRIdentityRole
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteRoleAsync<TRole>(string id)
            where TRole : LogRIdentityRole
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task UpdateRoleAsync<TRole>(TRole role)
            where TRole : LogRIdentityRole
        {
            throw new NotImplementedException();
        }
    }
}
