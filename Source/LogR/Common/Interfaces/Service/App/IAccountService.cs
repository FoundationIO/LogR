using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogR.Common.Models.Identity;

namespace LogR.Common.Interfaces.Service.App
{
    public interface IAccountService
    {
        Task<TUser> GetUserByEmailAsync<TUser>(string normalizedEmail)
            where TUser : LogRIdentityUser;

        Task<IList<TUser>> GetUsersWithClaimTypeAndValueAsync<TUser>(string type, string value)
            where TUser : LogRIdentityUser;

        void CreateUserAsync<TUser>(TUser user)
            where TUser : LogRIdentityUser;

        void UpdateUserAsync<TUser>(TUser user)
            where TUser : LogRIdentityUser;

        Task<TUser> GetUserByIdAsync<TUser>(string userId)
            where TUser : LogRIdentityUser;

        Task<TUser> GetUserByNameAsync<TUser>(string normalizedUserName)
            where TUser : LogRIdentityUser;

        Task<TUser> GetUserByLoginProviderAndProviderKeyAsync<TUser>(string loginProvider, string providerKey)
            where TUser : LogRIdentityUser;

        Task<bool> DeleteUserAsync(string userId);

        Task<TRole> GetRoleByNameAsync<TRole>(string normalizedRoleName)
            where TRole : LogRIdentityRole;

        Task<TRole> GetRoleByIdAsync<TRole>(string roleId)
            where TRole : LogRIdentityRole;

        System.Threading.Tasks.Task CreateRoleAsync<TRole>(TRole role)
            where TRole : LogRIdentityRole;

        System.Threading.Tasks.Task DeleteRoleAsync<TRole>(string id)
            where TRole : LogRIdentityRole;

        System.Threading.Tasks.Task UpdateRoleAsync<TRole>(TRole role)
            where TRole : LogRIdentityRole;
    }
}
